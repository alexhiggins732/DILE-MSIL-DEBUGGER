#pragma once

#include "Stdafx.h"
#include <metahost.h>
#include "BaseWrapper.h"
#include "DumpFileReader.h"
#include "ProcessWrapper.h"
#include "DumpExceptionInfo.h"
#include "DumpSystemInfo.h"
#include "DumpThreadInfo.h"
#include "DumpUnloadedModuleInfo.h"
#include "DumpMiscInfo.h"

using namespace Dile::Debug;
using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpDebugger : BaseWrapper
			{
			private:
				ICLRDebugging* pDebugging;
				ICorDebugDataTarget* pDataTarget;
				ICLRDebuggingLibraryProvider* pLibraryProvider;
				DumpFileReader* pDumpFileReader;
				String^ dumpFilePath;

			public:
				DumpDebugger(String^ dumpFilePath);

				~DumpDebugger();

				!DumpDebugger();

				ProcessWrapper^ OpenDumpFile();

				String^ GetDumpFilePath();

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