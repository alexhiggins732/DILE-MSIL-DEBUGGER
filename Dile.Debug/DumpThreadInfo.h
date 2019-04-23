#pragma once

#include "Stdafx.h"
#include "Enumerations.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class DumpThreadInfo
			{
			private:
				ULONG32 id;
				ULONG32 suspendCount;
				ThreadPriorityClass priorityClass;
				ThreadPriority priority;

			public:
				property ULONG32 Id
				{
					ULONG32 get()
					{
						return id;
					}

				private:
					void set(ULONG32 value)
					{
						id = value;
					}
				}

				property ULONG32 SuspendCount
				{
					ULONG32 get()
					{
						return suspendCount;
					}

				private:
					void set(ULONG32 value)
					{
						suspendCount = value;
					}
				}

				property ThreadPriorityClass PriorityClass
				{
					ThreadPriorityClass get()
					{
						return priorityClass;
					}

				private:
					void set(ThreadPriorityClass value)
					{
						priorityClass = value;
					}
				}

				property ThreadPriority Priority
				{
					ThreadPriority get()
					{
						return priority;
					}

				private:
					void set(ThreadPriority value)
					{
						priority = value;
					}
				}

				DumpThreadInfo(ULONG32 id, ULONG32 suspendCount, ThreadPriorityClass priorityClass, ThreadPriority priority);
			};
		}
	}
}