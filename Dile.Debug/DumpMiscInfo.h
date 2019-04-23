#pragma once

#include "Enumerations.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpMiscInfo
			{
			private:
				MiscInfoType type;
				bool isPowerInfoAvailable;
				int processId;
				DateTimeOffset processCreateTime;
				TimeSpan processUserTime;
				TimeSpan processKernelTime;
				int processorMaxMhz;
				int processorCurrentMhz;
				int processorMhzLimit;
				int processorMaxIdleState;
				int processorCurrentIdleState;

			public:
				property MiscInfoType Type
				{
					MiscInfoType get()
					{
						return type;
					}

				private:
					void set(MiscInfoType value)
					{
						type = value;
					}
				}

				property int ProcessId
				{
					int get()
					{
						return processId;
					}

				private:
					void set(int value)
					{
						processId = value;
					}
				}

				property DateTimeOffset ProcessCreateTime
				{
					DateTimeOffset get()
					{
						return processCreateTime;
					}

				private:
					void set(DateTimeOffset value)
					{
						processCreateTime = value;
					}
				}

				property TimeSpan ProcessUserTime
				{
					TimeSpan get()
					{
						return processUserTime;
					}

				private:
					void set(TimeSpan value)
					{
						processUserTime = value;
					}
				}

				property TimeSpan ProcessKernelTime
				{
					TimeSpan get()
					{
						return processKernelTime;
					}

				private:
					void set(TimeSpan value)
					{
						processKernelTime = value;
					}
				}

				property int ProcessorMaxMhz
				{
					int get()
					{
						return processorMaxMhz;
					}

				private:
					void set(int value)
					{
						processorMaxMhz = value;
					}
				}

				property int ProcessorCurrentMhz
				{
					int get()
					{
						return processorCurrentMhz;
					}

				private:
					void set(int value)
					{
						processorCurrentMhz = value;
					}
				}

				property int ProcessorMhzLimit
				{
					int get()
					{
						return processorMhzLimit;
					}

				private:
					void set(int value)
					{
						processorMhzLimit = value;
					}
				}

				property int ProcessorMaxIdleState
				{
					int get()
					{
						return processorMaxIdleState;
					}

				private:
					void set(int value)
					{
						processorMaxIdleState = value;
					}
				}

				property int ProcessorCurrentIdleState
				{
					int get()
					{
						return processorCurrentIdleState;
					}

				private:
					void set(int value)
					{
						processorCurrentIdleState = value;
					}
				}

				DumpMiscInfo(MiscInfoType type,
					int processId,
					DateTimeOffset processCreateTime,
					TimeSpan processUserTime,
					TimeSpan processKernelTime,
					int processorMaxMhz,
					int processorCurrentMhz,
					int processorMhzLimit,
					int processorMaxIdleState,
					int processorCurrentIdleState);
			};
		}
	}
}