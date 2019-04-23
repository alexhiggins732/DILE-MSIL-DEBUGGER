#pragma once

#include "Enumerations.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpSystemInfo
			{
			private:
				ProcessorArchitecture processorArchitecture;
				ProcessorLevel processorLevel;
				byte modelNumber;
				byte stepping;
				short numberOfProcessors;
				OperatingSystemType operatingSystemType;
				int majorVersion;
				int minorVersion;
				int buildNumber;
				Platform platform;
				String^ servicePack;
				OperatingSystemSuite operatingSystemSuite;

			public:
				property Dile::Debug::Dump::ProcessorArchitecture ProcessorArchitecture
				{
					Dile::Debug::Dump::ProcessorArchitecture get()
					{
						return processorArchitecture;
					}

				private:
					void set(Dile::Debug::Dump::ProcessorArchitecture value)
					{
						processorArchitecture = value;
					}
				}

				property Dile::Debug::Dump::ProcessorLevel ProcessorLevel
				{
					Dile::Debug::Dump::ProcessorLevel get()
					{
						return processorLevel;
					}

				private:
					void set(Dile::Debug::Dump::ProcessorLevel value)
					{
						processorLevel = value;
					}
				}

				property byte ModelNumber
				{
					byte get()
					{
						return modelNumber;
					}

				private:
					void set(byte value)
					{
						modelNumber = value;
					}
				}

				property byte Stepping
				{
					byte get()
					{
						return stepping;
					}

				private:
					void set(byte value)
					{
						stepping = value;
					}
				}

				property short NumberOfProcessors
				{
					short get()
					{
						return numberOfProcessors;
					}

				private:
					void set(short value)
					{
						numberOfProcessors = value;
					}
				}

				property Dile::Debug::Dump::OperatingSystemType OperatingSystemType
				{
					Dile::Debug::Dump::OperatingSystemType get()
					{
						return operatingSystemType;
					}

				private:
					void set(Dile::Debug::Dump::OperatingSystemType value)
					{
						operatingSystemType = value;
					}
				}

				property int MajorVersion
				{
					int get()
					{
						return majorVersion;
					}

				private:
					void set(int value)
					{
						majorVersion = value;
					}
				}

				property int MinorVersion
				{
					int get()
					{
						return minorVersion;
					}

				private:
					void set(int value)
					{
						minorVersion = value;
					}
				}

				property int BuildNumber
				{
					int get()
					{
						return buildNumber;
					}

				private:
					void set(int value)
					{
						buildNumber = value;
					}
				}

				property Dile::Debug::Dump::Platform Platform
				{
					Dile::Debug::Dump::Platform get()
					{
						return platform;
					}

				private:
					void set(Dile::Debug::Dump::Platform value)
					{
						platform = value;
					}
				}

				property String^ ServicePack
				{
					String^ get()
					{
						return servicePack;
					}

				private:
					void set(String^ value)
					{
						servicePack = value;
					}
				}

				property Dile::Debug::Dump::OperatingSystemSuite OperatingSystemSuite
				{
					Dile::Debug::Dump::OperatingSystemSuite get()
					{
						return operatingSystemSuite;
					}

				private:
					void set(Dile::Debug::Dump::OperatingSystemSuite value)
					{
						operatingSystemSuite = value;
					}
				}

				DumpSystemInfo(Dile::Debug::Dump::ProcessorArchitecture processorArchitecture,
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
					Dile::Debug::Dump::OperatingSystemSuite operatingSystemSuite);
			};
		}
	}
}