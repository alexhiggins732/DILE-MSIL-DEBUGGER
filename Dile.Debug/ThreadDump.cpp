#include "Stdafx.h"
#include "ThreadDump.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			ThreadDump::ThreadDump(ULONG32 threadId, RVA contextRva, ULONG64 contextSize)
			{
				this->threadId = threadId;
				this->contextRva = contextRva;
				this->contextSize = contextSize;
			}

			ULONG32 ThreadDump::GetThreadId()
			{
				return threadId;
			}

			RVA ThreadDump::GetContextRva()
			{
				return contextRva;
			}

			ULONG64 ThreadDump::GetContextSize()
			{
				return contextSize;
			}
		}
	}
}