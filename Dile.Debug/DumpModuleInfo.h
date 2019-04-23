#pragma once

#include "Stdafx.h"
#include "Enumerations.h"
#include "Version.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpModuleInfo
			{
			private:
				String^ name;
				Version^ productVersion;
				Version^ fileVersion;
				DateTimeOffset timestamp;
				ULONG32 size;
				ULONG32 checksum;
				ULONG64 baseAddress;
				FileType fileType;
				FileFlags fileFlags;

			public:
				property String^ Name
				{
					String^ get()
					{
						return name;
					}

				private:
					void set(String^ value)
					{
						name = value;
					}
				}

				property Version^ ProductVersion
				{
					Version^ get()
					{
						return productVersion;
					}

				private:
					void set(Version^ value)
					{
						productVersion = value;
					}
				}

				property Version^ FileVersion
				{
					Version^ get()
					{
						return fileVersion;
					}

				private:
					void set(Version^ value)
					{
						fileVersion = value;
					}
				}

				property DateTimeOffset Timestamp
				{
					DateTimeOffset get()
					{
						return timestamp;
					}

				private:
					void set(DateTimeOffset value)
					{
						timestamp = value;
					}
				}

				property ULONG32 Size
				{
					ULONG32 get()
					{
						return size;
					}

				private:
					void set(ULONG32 value)
					{
						size = value;
					}
				}

				property ULONG32 Checksum
				{
					ULONG32 get()
					{
						return checksum;
					}

				private:
					void set(ULONG32 value)
					{
						checksum = value;
					}
				}

				property ULONG64 BaseAddress
				{
					ULONG64 get()
					{
						return baseAddress;
					}

				private:
					void set(ULONG64 value)
					{
						baseAddress = value;
					}
				}

				property Dile::Debug::Dump::FileType FileType
				{
					Dile::Debug::Dump::FileType get()
					{
						return fileType;
					}

				private:
					void set(Dile::Debug::Dump::FileType value)
					{
						fileType = value;
					}
				}

				DumpModuleInfo(String^ name,
					Version^ productVersion,
					Version^ fileVersion,
					DateTimeOffset timestamp,
					ULONG32 size,
					ULONG32 checksum,
					ULONG64 baseAddress,
					Dile::Debug::Dump::FileType fileType);
			};
		}
	}
}