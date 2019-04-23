#pragma once

#include <DbgHelp.h>
#include <atlstr.h>
#include <vector>
#include "MemoryRange.h"
#include "ThreadDump.h"
#include "DumpModuleInfo.h"
#include "DumpExceptionInfo.h"
#include "DumpSystemInfo.h"
#include "DumpThreadInfo.h"
#include "DumpUnloadedModuleInfo.h"
#include "DumpMiscInfo.h"

//using namespace std;
using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public class DumpFileReader
			{
			private:
				HANDLE hDumpFile;
				HANDLE hDumpFileMapping;
				LPVOID lpBaseAddress;
				std::vector<MemoryRange*> memoryRanges;
				std::vector<ThreadDump*> threadDumps;
				std::vector<ULONG64> moduleBaseAddresses;
				PVOID pMemory64Stream;

				HANDLE CheckHandle(HANDLE handle, HANDLE invalidValue);
				void ReadStream(ULONG streamNumber, PVOID* ppStream, ULONG* pStreamSize);

			public:
				DumpFileReader(String^ dumpFilePath);

				~DumpFileReader();

				void CollectMemoryRanges();

				void CollectThreadDumps();

				void CollectModuleBaseAddresses();

				CString ReadString(RVA rva);

				std::vector<ULONG64> GetModuleBaseAddresses();

				void ReadMemory(CORDB_ADDRESS address, BYTE* pBuffer, ULONG32 bytesRequested, ULONG32* pBytesRead);

				CorDebugPlatform ReadPlatform();

				void ReadThreadContext(DWORD threadId, BYTE* pContext, ULONG32 contextSize);

				array<DumpModuleInfo^>^ GetModules();

				DumpExceptionInfo^ GetException();

				DumpSystemInfo^ GetSystemInfo();

				array<DumpThreadInfo^>^ GetThreads();

				array<DumpUnloadedModuleInfo^>^ GetUnloadedModules();

				DumpMiscInfo^ GetMiscInfo();
			};
		}
	}
}