#pragma once
#include <DbgHelp.h>

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public class ThreadDump
			{
			private:
				ULONG32 threadId;
				RVA contextRva;
				ULONG64 contextSize;

			public:
				ThreadDump(ULONG32 threadId, RVA contextRva, ULONG64 contextSize);

				ULONG32 GetThreadId();

				RVA GetContextRva();

				ULONG64 GetContextSize();
			};
		}
	}
}