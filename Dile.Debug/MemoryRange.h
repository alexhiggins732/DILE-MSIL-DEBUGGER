#pragma once

#include <DbgHelp.h>

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public class MemoryRange
			{
			private:
				ULONG64 start;
				ULONG64 size;
				RVA64 rva;

			public:
				MemoryRange(ULONG64 start, ULONG64 size, RVA64 rva);

				ULONG64 GetStartAddress();

				ULONG64 GetEndAddress();

				ULONG64 GetSize();

				RVA64 GetRva();

				bool WithinRange(ULONG64 address);
			};
		}
	}
}