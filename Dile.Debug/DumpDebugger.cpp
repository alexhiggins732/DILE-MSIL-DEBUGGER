#include "Stdafx.h"
#include "DumpDebugger.h"
#include "MemoryDumpDataTarget.h"
#include "ProcessLibraryProvider.h"

using namespace System;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpDebugger::DumpDebugger(String^ dumpFilePath)
			{
				this->dumpFilePath = dumpFilePath;

				pDumpFileReader = new DumpFileReader(dumpFilePath);
				pDataTarget = new MemoryDumpDataTarget(pDumpFileReader);
				pLibraryProvider = new ProcessLibraryProvider();

				ICLRDebugging *pDebuggingTemp = NULL;
				CheckHResult(CLRCreateInstance(CLSID_CLRDebugging, IID_ICLRDebugging, (LPVOID*)&pDebuggingTemp));
				pDebugging = pDebuggingTemp;

				CheckHResult(pDataTarget->AddRef());
				CheckHResult(pLibraryProvider->AddRef());
				CheckHResult(pDebugging->AddRef());
			}

			DumpDebugger::~DumpDebugger()
			{
				this->!DumpDebugger();
			}

			DumpDebugger::!DumpDebugger()
			{
				CheckHResult(pDataTarget->Release());
				CheckHResult(pLibraryProvider->Release());
				CheckHResult(pDebugging->Release());
				delete pDumpFileReader;
			}

			ProcessWrapper^ DumpDebugger::OpenDumpFile()
			{
				//I couldn't find the definition of this error code but I think the following is correct:
				const HRESULT COR_E_NOT_CLR = 0x80131c44;

				ProcessWrapper^ result = nullptr;
				std::vector<ULONG64> moduleBaseAddresses = pDumpFileReader->GetModuleBaseAddresses();
				unsigned int index = 0;
				bool clrModuleFound = false;
				IUnknown* pProcess = NULL;

				while(!clrModuleFound && index < moduleBaseAddresses.size())
				{
					ULONG64 moduleBaseAddress = moduleBaseAddresses.at(index);
					CLR_DEBUGGING_VERSION debuggingVersion;
					debuggingVersion.wStructVersion = 0;
					debuggingVersion.wMajor = 4;
					debuggingVersion.wMinor = 0;
					debuggingVersion.wBuild = 30319;
					debuggingVersion.wRevision = 65535;

					pProcess = NULL;
					CLR_DEBUGGING_VERSION* pVersion = NULL;
					CLR_DEBUGGING_PROCESS_FLAGS* pdwFlags = NULL;

					CheckHResult(pDataTarget->AddRef());
					CheckHResult(pLibraryProvider->AddRef());
					CheckHResult(pDebugging->AddRef());

					HRESULT hResult = pDebugging->OpenVirtualProcess(moduleBaseAddress, pDataTarget, pLibraryProvider, &debuggingVersion, IID_ICorDebugProcess3, &pProcess, pVersion, pdwFlags);
					switch(hResult)
					{
						case S_OK:
							clrModuleFound = true;

							CheckHResult(pDataTarget->Release());
							CheckHResult(pLibraryProvider->Release());
							CheckHResult(pDebugging->Release());
							break;

						case COR_E_NOT_CLR:
							index++;
							break;

						default:
							CheckHResult(hResult);
							break;
					}
				}

				if (!clrModuleFound)
				{
					throw gcnew InvalidOperationException("Failed to find any CLR module in the memory dump file.");
				}

				ICorDebugProcess* pDebugProcess;
				CheckHResult(pProcess->QueryInterface(&pDebugProcess));

				result = gcnew ProcessWrapper(pDebugProcess);

				return result;
			}

			String^ DumpDebugger::GetDumpFilePath()
			{
				return dumpFilePath;
			}

			array<DumpModuleInfo^>^ DumpDebugger::GetModules()
			{
				return pDumpFileReader->GetModules();
			}

			DumpExceptionInfo^ DumpDebugger::GetException()
			{
				return pDumpFileReader->GetException();
			}

			DumpSystemInfo^ DumpDebugger::GetSystemInfo()
			{
				return pDumpFileReader->GetSystemInfo();
			}

			array<DumpThreadInfo^>^ DumpDebugger::GetThreads()
			{
				return pDumpFileReader->GetThreads();
			}

			array<DumpUnloadedModuleInfo^>^ DumpDebugger::GetUnloadedModules()
			{
				return pDumpFileReader->GetUnloadedModules();
			}

			DumpMiscInfo^ DumpDebugger::GetMiscInfo()
			{
				return pDumpFileReader->GetMiscInfo();
			}
		}
	}
}