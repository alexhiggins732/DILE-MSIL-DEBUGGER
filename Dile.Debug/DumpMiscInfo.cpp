#include "Stdafx.h"
#include "DumpMiscInfo.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			DumpMiscInfo::DumpMiscInfo(MiscInfoType type,
				int processId,
				DateTimeOffset processCreateTime,
				TimeSpan processUserTime,
				TimeSpan processKernelTime,
				int processorMaxMhz,
				int processorCurrentMhz,
				int processorMhzLimit,
				int processorMaxIdleState,
				int processorCurrentIdleState)
			{
				Type = type;
				ProcessId = processId;
				ProcessCreateTime = processCreateTime;
				ProcessUserTime = processUserTime;
				ProcessKernelTime = processKernelTime;
				ProcessorMaxMhz = processorMaxMhz;
				ProcessorCurrentMhz = processorCurrentMhz;
				ProcessorMhzLimit = processorMhzLimit;
				ProcessorMaxIdleState = processorMaxIdleState;
				ProcessorCurrentIdleState = processorCurrentIdleState;
			}
		}
	}
}