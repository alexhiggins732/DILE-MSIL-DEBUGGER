#include "Stdafx.h"
#include "DumpUnloadedModuleInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpUnloadedModuleInfo::DumpUnloadedModuleInfo(ULONG64 baseAddress,
				ULONG32 size,
				ULONG32 checksum,
				DateTimeOffset timestamp,
				String^ name)
			{
				BaseAddress = baseAddress;
				Size = size;
				Checksum = checksum;
				Timestamp = timestamp;
				Name = name;
			}
		}
	}
}