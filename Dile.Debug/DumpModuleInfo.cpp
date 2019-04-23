#include "Stdafx.h"
#include "DumpModuleInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpModuleInfo::DumpModuleInfo(String^ name,
				Version^ productVersion,
				Version^ fileVersion,
				DateTimeOffset timestamp,
				ULONG32 size,
				ULONG32 checksum,
				ULONG64 baseAddress,
				Dile::Debug::Dump::FileType fileType)
			{
				Name = name;
				ProductVersion = productVersion;
				FileVersion = fileVersion;
				Timestamp = timestamp;
				Size = size;
				Checksum = checksum;
				BaseAddress = baseAddress;
				FileType = fileType;
			}
		}
	}
}