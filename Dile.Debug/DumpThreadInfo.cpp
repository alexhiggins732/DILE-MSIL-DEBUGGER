#include "Stdafx.h"
#include "DumpThreadInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpThreadInfo::DumpThreadInfo(ULONG32 id, ULONG32 suspendCount, ThreadPriorityClass priorityClass, ThreadPriority priority)
			{
				Id = id;
				SuspendCount = suspendCount;
				PriorityClass = priorityClass;
				Priority = priority;
			}
		}
	}
}