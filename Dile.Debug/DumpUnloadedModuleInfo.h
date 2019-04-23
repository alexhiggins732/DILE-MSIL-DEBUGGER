#pragma once

#include "Stdafx.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpUnloadedModuleInfo
			{
			private:
				ULONG64 baseAddress;
				ULONG32 size;
				ULONG32 checksum;
				DateTimeOffset timestamp;
				String^	name;

			public:
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

				DumpUnloadedModuleInfo(ULONG64 baseAddress, ULONG32 size, ULONG32 checksum, DateTimeOffset timestamp, String^ name);
			};
		}
	}
}