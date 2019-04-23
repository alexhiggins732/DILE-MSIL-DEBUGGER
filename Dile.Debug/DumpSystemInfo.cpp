#include "Stdafx.h"
#include "DumpSystemInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpSystemInfo::DumpSystemInfo(Dile::Debug::Dump::ProcessorArchitecture processorArchitecture,
					Dile::Debug::Dump::ProcessorLevel processorLevel,
					byte modelNumber,
					byte stepping,
					short numberOfProcessors,
					Dile::Debug::Dump::OperatingSystemType operatingSystemType,
					int majorVersion,
					int minorVersion,
					int buildNumber,
					Dile::Debug::Dump::Platform platform,
					String^ servicePack,
					Dile::Debug::Dump::OperatingSystemSuite operatingSystemSuite)
			{
				ProcessorArchitecture = processorArchitecture;
				ProcessorLevel = processorLevel;
				ModelNumber = modelNumber;
				Stepping = stepping;
				NumberOfProcessors = numberOfProcessors;
				OperatingSystemType = operatingSystemType;
				MajorVersion = majorVersion;
				MinorVersion = minorVersion;
				BuildNumber = buildNumber;
				Platform = platform;
				ServicePack = servicePack;
				OperatingSystemSuite = operatingSystemSuite;
			}
		}
	}
}