#include "Stdafx.h"
#include "DumpExceptionInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpExceptionInfo::DumpExceptionInfo(ExceptionCode code, bool continuable, ULONG64 record, ULONG64 address)
			{
				Code = code;
				Continuable = continuable;
				Record = record;
				Address = address;
			}
		}
	}
}