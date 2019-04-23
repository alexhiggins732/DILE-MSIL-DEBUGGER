#include "Stdafx.h"
#include "Version.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			Version::Version(short major, short minor, short revision, short build)
			{
				Major = major;
				Minor = minor;
				Revision = revision;
				Build = build;
			}

			Version::Version(DWORD mostSignificantBits, DWORD leastSignificantBits)
			{
				Major = mostSignificantBits >> 16;
				Minor = (short)(mostSignificantBits - ((DWORD)Major << 16));
				Revision = leastSignificantBits >> 16;
				Build = (short)(leastSignificantBits - ((DWORD)Revision << 16));
			}

			String^ Version::ToString()
			{
				return String::Format("{0}.{1}.{2}.{3}", Major, Minor, Revision, Build);
			}
		}
	}
}