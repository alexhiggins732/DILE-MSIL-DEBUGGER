#pragma once

#include "Enumerations.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpFileWriter
			{
			public:
				static void WriteDumpFile(String^ dumpFilePath, int processId, DumpType dumpType);
			};
		}
	}
}