#pragma once

#include "Stdafx.h"
#include "Enumerations.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpExceptionInfo
			{
			private:
				ExceptionCode code;
				bool continuable;
				ULONG64 record;
				ULONG64 address;

			public:
				property ExceptionCode Code
				{
					ExceptionCode get()
					{
						return code;
					}

				private:
					void set(ExceptionCode value)
					{
						code = value;
					}
				}

				property bool Continuable
				{
					bool get()
					{
						return continuable;
					}

				private:
					void set(bool value)
					{
						continuable = value;
					}
				}

				property ULONG64 Record
				{
					ULONG64 get()
					{
						return record;
					}

				private:
					void set(ULONG64 value)
					{
						record = value;
					}
				}

				property ULONG64 Address
				{
					ULONG64 get()
					{
						return address;
					}

				private:
					void set(ULONG64 value)
					{
						address = value;
					}
				}

				DumpExceptionInfo(ExceptionCode code, bool continuable, ULONG64 record, ULONG64 address);
			};
		}
	}
}