#include "Stdafx.h"
#include "MemoryRange.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			MemoryRange::MemoryRange(ULONG64 start, ULONG64 size, RVA64 rva)
			{
				this->start = start;
				this->size = size;
				this->rva = rva;
			}

			ULONG64 MemoryRange::GetStartAddress()
			{
				return start;
			}

			ULONG64 MemoryRange::GetEndAddress()
			{
				return start + size;
			}

			ULONG64 MemoryRange::GetSize()
			{
				return size;
			}

			RVA64 MemoryRange::GetRva()
			{
				return rva;
			}

			bool MemoryRange::WithinRange(ULONG64 address)
			{
				return (address >= start && address < (start + size));
			}
		}
	}
}