#include "Stdafx.h"
#include "DumpFileReader.h"
#include <memory.h>
#include "LpctstrConverter.h"
#include <Windows.h>

using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpFileReader::DumpFileReader(String^ dumpFilePath)
			{
				LpctstrConverter lpctstrDumpFilePath(dumpFilePath);

				hDumpFile = CheckHandle(CreateFile(lpctstrDumpFilePath, GENERIC_READ, 0, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL),
					INVALID_HANDLE_VALUE);

				hDumpFileMapping = CheckHandle(CreateFileMapping(hDumpFile, NULL, PAGE_READONLY, 0, 0, NULL),
					NULL);

				lpBaseAddress = MapViewOfFile(hDumpFileMapping, FILE_MAP_READ, 0, 0, 0);
				if (lpBaseAddress == NULL)
				{
					throw gcnew InvalidOperationException("Failed to map the dump file in memory.");
				}

				CollectMemoryRanges();
				CollectThreadDumps();
				CollectModuleBaseAddresses();
			}

			DumpFileReader::~DumpFileReader()
			{
				UnmapViewOfFile(lpBaseAddress);
				CloseHandle(hDumpFileMapping);
				CloseHandle(hDumpFile);

				while (memoryRanges.size() > 0)
				{
					MemoryRange* pMemoryRange = memoryRanges.back();
					delete pMemoryRange;

					memoryRanges.pop_back();
				}

				while (threadDumps.size() > 0)
				{
					ThreadDump* pThreadDump = threadDumps.back();
					delete pThreadDump;

					threadDumps.pop_back();
				}
			}

			HANDLE DumpFileReader::CheckHandle(HANDLE handle, HANDLE invalidValue)
			{
				HANDLE result = handle;

				if (handle == invalidValue)
				{
					throw gcnew InvalidOperationException("The returned handle is invalid.");
				}

				return result;
			}

			void DumpFileReader::ReadStream(ULONG streamNumber, PVOID* ppStream, ULONG* pStreamSize)
			{
				PMINIDUMP_DIRECTORY pMiniDumpDirectory = NULL;
				if (MiniDumpReadDumpStream(lpBaseAddress,
					streamNumber,
					&pMiniDumpDirectory,
					ppStream,
					pStreamSize))
				{
					if (ppStream == NULL || pStreamSize == 0)
					{
						throw gcnew InvalidOperationException("Empty stream was returned.");
					}
					else if (IsBadStringPtr((LPCSTR)*ppStream, *pStreamSize))
					{
						throw gcnew InvalidOperationException("The pointer to the requested stream is invalid.");
					}
				}
				else
				{
					throw gcnew InvalidOperationException("Failed to open the requested stream of the memory dump file.");
				}
			}

			void DumpFileReader::CollectMemoryRanges()
			{
				ULONG streamSize = 0;
				ReadStream(Memory64ListStream, &pMemory64Stream, &streamSize);

				PMINIDUMP_MEMORY64_LIST pMemoryList = (PMINIDUMP_MEMORY64_LIST)pMemory64Stream;
				RVA64 rva = pMemoryList->BaseRva;

				for(int index = 0; index < pMemoryList->NumberOfMemoryRanges; index++)
				{
					MINIDUMP_MEMORY_DESCRIPTOR64 memoryDescriptor = pMemoryList->MemoryRanges[index];
					MemoryRange *pMemoryRange = new MemoryRange(memoryDescriptor.StartOfMemoryRange, memoryDescriptor.DataSize, rva);

					memoryRanges.push_back(pMemoryRange);

					rva += memoryDescriptor.DataSize;
				}
			}

			void DumpFileReader::CollectThreadDumps()
			{
				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(ThreadListStream, &pStream, &streamSize);

				PMINIDUMP_THREAD_LIST pThreadList = (PMINIDUMP_THREAD_LIST)pStream;

				for(unsigned int index = 0; index < pThreadList->NumberOfThreads; index++)
				{
					MINIDUMP_THREAD thread = pThreadList->Threads[index];
					ThreadDump *pThreadDump = new ThreadDump(thread.ThreadId, thread.ThreadContext.Rva, thread.ThreadContext.DataSize);

					threadDumps.push_back(pThreadDump);
				}
			}

			void DumpFileReader::CollectModuleBaseAddresses()
			{
				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(ModuleListStream, &pStream, &streamSize);

				PMINIDUMP_MODULE_LIST pModules = (PMINIDUMP_MODULE_LIST)pStream;

				for(unsigned int index = 0; index < pModules->NumberOfModules; index++)
				{
					MINIDUMP_MODULE module = pModules->Modules[index];
					moduleBaseAddresses.push_back(module.BaseOfImage);
				}
			}

			CString DumpFileReader::ReadString(RVA rva)
			{
				PMINIDUMP_STRING pString = (PMINIDUMP_STRING)((LPBYTE)lpBaseAddress + rva);

				return CString(pString->Buffer, pString->Length);
			}

			std::vector<ULONG64> DumpFileReader::GetModuleBaseAddresses()
			{
				return moduleBaseAddresses;
			}

			void DumpFileReader::ReadMemory(CORDB_ADDRESS address, BYTE* pBuffer, ULONG32 bytesRequested, ULONG32* pBytesRead)
			{
				MemoryRange* pMemoryRange = NULL;
				unsigned int index = 0;

				while (pMemoryRange == NULL && index < memoryRanges.size())
				{
					MemoryRange* pCurrentMemoryRange = memoryRanges[index];

					if (pCurrentMemoryRange->WithinRange(address))
					{
						pMemoryRange = pCurrentMemoryRange;
					}
					else
					{
						index++;
					}
				}

				if (pMemoryRange == NULL)
				{
					throw gcnew InvalidOperationException("Could not find the requested memory address in the memory dump file.");
				}

				DWORD64 dwOffset = address - pMemoryRange->GetStartAddress();
				ULONG64 bytesRead = (pMemoryRange->GetSize() - dwOffset > bytesRequested ? bytesRequested : pMemoryRange->GetSize() - dwOffset);

				if (bytesRead <= 0)
				{
					throw gcnew InvalidOperationException("Failed to read the requested memory chunk from the memory dump file.");
				}

				memcpy_s(pBuffer,
					bytesRequested,
					(LPBYTE)lpBaseAddress + pMemoryRange->GetRva() + dwOffset,
					(size_t)bytesRead);
				*pBytesRead = (ULONG32)bytesRead;
			}

			CorDebugPlatform DumpFileReader::ReadPlatform()
			{
				CorDebugPlatform result = CORDB_PLATFORM_WINDOWS_X86;
				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(SystemInfoStream, &pStream, &streamSize);

				PMINIDUMP_SYSTEM_INFO pSystemInfo = (PMINIDUMP_SYSTEM_INFO)pStream;
				switch(pSystemInfo->ProcessorArchitecture)
				{
				case PROCESSOR_ARCHITECTURE_AMD64:
					result = CORDB_PLATFORM_WINDOWS_AMD64;
					break;

				case PROCESSOR_ARCHITECTURE_IA64:
					result = CORDB_PLATFORM_WINDOWS_IA64;
					break;

				case PROCESSOR_ARCHITECTURE_INTEL:
					result = CORDB_PLATFORM_WINDOWS_X86;
					break;

				default:
					//todo throw exception
					throw gcnew NotSupportedException("The following processor architecture is not supported: " + pSystemInfo->ProcessorArchitecture);
				}

				return result;
			}

			void DumpFileReader::ReadThreadContext(DWORD threadId, BYTE* pContext, ULONG32 contextSize)
			{
				ThreadDump* pThreadDump = NULL;
				unsigned int index = 0;

				while (pThreadDump == NULL && index < threadDumps.size())
				{
					ThreadDump* pCurrentThreadDump = threadDumps[index];

					if (pCurrentThreadDump->GetThreadId() == threadId)
					{
						pThreadDump = pCurrentThreadDump;
					}
					else
					{
						index++;
					}
				}

				if (pThreadDump == NULL)
				{
					throw gcnew InvalidOperationException("Could not find the context of the requested thread: " + threadId);
				}

				memcpy_s(pContext,
					contextSize,
					(LPBYTE)lpBaseAddress + pThreadDump->GetContextRva(),
					(size_t)pThreadDump->GetContextSize());
			}

			array<DumpModuleInfo^>^ DumpFileReader::GetModules()
			{
				array<DumpModuleInfo^>^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(ModuleListStream, &pStream, &streamSize);

				PMINIDUMP_MODULE_LIST pModules = (PMINIDUMP_MODULE_LIST)pStream;
				result = gcnew array<DumpModuleInfo^>(pModules->NumberOfModules);

				for(unsigned int index = 0; index < pModules->NumberOfModules; index++)
				{
					MINIDUMP_MODULE module = pModules->Modules[index];
					
					String^ name = gcnew String(ReadString(module.ModuleNameRva));
					Version^ productVersion = gcnew Version(module.VersionInfo.dwProductVersionMS, module.VersionInfo.dwProductVersionLS);
					Version^ fileVersion = gcnew Version(module.VersionInfo.dwFileVersionMS, module.VersionInfo.dwFileVersionLS);
					DateTimeOffset timestamp = DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan::Zero)
						.AddSeconds((Double)module.TimeDateStamp);
					ULONG32 size = module.SizeOfImage;
					ULONG32 checksum = module.CheckSum;
					ULONG64 baseAddress = module.BaseOfImage;
					FileType fileType = (FileType)module.VersionInfo.dwFileType;

					DumpModuleInfo^ moduleInfo = gcnew DumpModuleInfo(name,
						productVersion,
						fileVersion,
						timestamp,
						size,
						checksum,
						baseAddress,
						fileType);
					result[index] = moduleInfo;
				}

				return result;
			}

			DumpExceptionInfo^ DumpFileReader::GetException()
			{
				DumpExceptionInfo^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(ExceptionStream, &pStream, &streamSize);

				PMINIDUMP_EXCEPTION pException = (PMINIDUMP_EXCEPTION)pStream;

				if (pException != NULL)
				{
					result = gcnew DumpExceptionInfo((ExceptionCode)pException->ExceptionCode,
						((pException->ExceptionFlags & EXCEPTION_NONCONTINUABLE) != EXCEPTION_NONCONTINUABLE),
						pException->ExceptionRecord,
						pException->ExceptionAddress);
				}

				return result;
			}

			DumpSystemInfo^ DumpFileReader::GetSystemInfo()
			{
				DumpSystemInfo^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(SystemInfoStream, &pStream, &streamSize);

				PMINIDUMP_SYSTEM_INFO pSystemInfo = (PMINIDUMP_SYSTEM_INFO)pStream;

				ProcessorLevel processorLevel = (ProcessorLevel)pSystemInfo->ProcessorLevel;
				byte modelNumber;
				byte stepping;

				switch(processorLevel)
				{
				case ProcessorLevel::Intel386:
				case ProcessorLevel::Intel486:
					{
						byte xx = pSystemInfo->ProcessorRevision / 0x0100;

						if (xx == 0xFF)
						{
							modelNumber = ((pSystemInfo->ProcessorRevision / 0x10) % 0x10) - 0x0A;
							stepping = pSystemInfo->ProcessorRevision % 0x10;
						}
						else
						{
							modelNumber = xx + 'A';
							stepping = pSystemInfo->ProcessorRevision %0x0100;
						}
					}
					break;

				case ProcessorLevel::IntelPentium:
				case ProcessorLevel::IntelPentiumProOrPentiumII:
					modelNumber = pSystemInfo->ProcessorRevision / 0x0100;
					stepping = pSystemInfo->ProcessorRevision % 0x0100;
					break;

				default:
					throw gcnew NotSupportedException("The following processor level is not supported: " + Convert::ToString(processorLevel));
				}

				String^ servicePack = gcnew String(ReadString(pSystemInfo->CSDVersionRva));

				result = gcnew DumpSystemInfo((ProcessorArchitecture)pSystemInfo->ProcessorArchitecture,
					(ProcessorLevel)pSystemInfo->ProcessorLevel,
					modelNumber,
					stepping,
					pSystemInfo->NumberOfProcessors,
					(OperatingSystemType)pSystemInfo->ProductType,
					pSystemInfo->MajorVersion,
					pSystemInfo->MinorVersion,
					pSystemInfo->BuildNumber,
					(Platform)pSystemInfo->PlatformId,
					servicePack,
					(OperatingSystemSuite)pSystemInfo->SuiteMask);

				return result;
			}

			array<DumpThreadInfo^>^ DumpFileReader::GetThreads()
			{
				array<DumpThreadInfo^>^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(ThreadListStream, &pStream, &streamSize);

				PMINIDUMP_THREAD_LIST pThreads = (PMINIDUMP_THREAD_LIST)pStream;
				result = gcnew array<DumpThreadInfo^>(pThreads->NumberOfThreads);

				for(unsigned int index = 0; index < pThreads->NumberOfThreads; index++)
				{
					MINIDUMP_THREAD thread = pThreads->Threads[index];
					
					ULONG32 id = thread.ThreadId;
					ULONG32 suspendCount = thread.SuspendCount;
					ThreadPriorityClass priorityClass = (ThreadPriorityClass)thread.PriorityClass;
					ThreadPriority priority = (ThreadPriority)thread.Priority;

					DumpThreadInfo^ threadInfo = gcnew DumpThreadInfo(id, suspendCount, priorityClass, priority);
					result[index] = threadInfo;
				}

				return result;
			}

			array<DumpUnloadedModuleInfo^>^ DumpFileReader::GetUnloadedModules()
			{
				array<DumpUnloadedModuleInfo^>^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(UnloadedModuleListStream, &pStream, &streamSize);

				PMINIDUMP_UNLOADED_MODULE_LIST pUnloadedModules = (PMINIDUMP_UNLOADED_MODULE_LIST)pStream;
				result = gcnew array<DumpUnloadedModuleInfo^>(pUnloadedModules->NumberOfEntries);

				for(unsigned int index = 0; index < pUnloadedModules->NumberOfEntries; index++)
				{
					PMINIDUMP_UNLOADED_MODULE pUnloadedModule = (PMINIDUMP_UNLOADED_MODULE)((LPBYTE)(pStream) + pUnloadedModules->SizeOfEntry * index);
					
					ULONG64 baseAddress = pUnloadedModule->BaseOfImage;
					ULONG32 size = pUnloadedModule->SizeOfImage;
					ULONG32 checksum = pUnloadedModule->CheckSum;
					DateTimeOffset timestamp = DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan::Zero)
						.AddSeconds((Double)pUnloadedModule->TimeDateStamp);
					String^ name = gcnew String(ReadString(pUnloadedModule->ModuleNameRva));
					
					DumpUnloadedModuleInfo^ unloadedModuleInfo = gcnew DumpUnloadedModuleInfo(baseAddress, size, checksum, timestamp, name);
					result[index] = unloadedModuleInfo;
				}

				return result;
			}

			DumpMiscInfo^ DumpFileReader::GetMiscInfo()
			{
				DumpMiscInfo^ result = nullptr;

				PVOID pStream = NULL;
				ULONG streamSize = 0;
				ReadStream(MiscInfoStream, &pStream, &streamSize);

				MiscInfoType type = MiscInfoType::None;
				int processId = 0;
				DateTimeOffset processCreateTime = DateTimeOffset::MinValue;
				TimeSpan processUserTime = TimeSpan::Zero;
				TimeSpan processKernelTime = TimeSpan::Zero;
				int processorMaxMhz = 0;
				int processorCurrentMhz = 0;
				int processorMhzLimit = 0;
				int processorMaxIdleState = 0;
				int processorCurrentIdleState = 0;

				PMINIDUMP_MISC_INFO_2 pMiscInfo = (PMINIDUMP_MISC_INFO_2)pStream;
				type = (MiscInfoType)pMiscInfo->Flags1;

				if ((type & MiscInfoType::ProcessId) == MiscInfoType::ProcessId)
				{
					processId = pMiscInfo->ProcessId;
				}

				if ((type & MiscInfoType::ProcessTimes) == MiscInfoType::ProcessTimes)
				{
					processCreateTime = DateTimeOffset(1970, 1, 1, 0, 0, 0, 0, TimeZoneInfo::Utc->BaseUtcOffset)
						+ TimeSpan::FromSeconds(pMiscInfo->ProcessCreateTime);
					processUserTime = TimeSpan::FromSeconds(pMiscInfo->ProcessUserTime);
					processKernelTime = TimeSpan::FromSeconds(pMiscInfo->ProcessKernelTime);
				}

				if ((type & MiscInfoType::ProcessorPowerInfo) == MiscInfoType::ProcessorPowerInfo)
				{
					processorMaxMhz = pMiscInfo->ProcessorMaxMhz;
					processorCurrentMhz = pMiscInfo->ProcessorCurrentMhz;
					processorMhzLimit = pMiscInfo->ProcessorMhzLimit;
					processorMaxIdleState = pMiscInfo->ProcessorMaxIdleState;
					processorCurrentIdleState = pMiscInfo->ProcessorCurrentIdleState;
				}

				result = gcnew DumpMiscInfo(type,
					processId,
					processCreateTime,
					processUserTime,
					processKernelTime,
					processorMaxMhz,
					processorCurrentMhz,
					processorMhzLimit,
					processorMaxIdleState,
					processorCurrentIdleState);

				return result;
			}
		}
	}
}