#include "stdafx.h"
#include "ControllerWrapper.h"
#include "Constants.h"
#include "ThreadWrapper.h"

using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		void ControllerWrapper::Continue()
		{
			BOOL isRunning;

			CheckHResult(Controller->IsRunning(&isRunning));

			if (isRunning == FALSE)
			{
				CheckHResult(Controller->Continue(FALSE));
			}
		}

		bool ControllerWrapper::IsRunning()
		{
			BOOL isRunning;

			CheckHResult(Controller->IsRunning(&isRunning));

			return (isRunning == TRUE ? true : false);
		}

		void ControllerWrapper::Detach()
		{
			CheckHResult(Controller->Detach());
		}

		void ControllerWrapper::Pause()
		{
			CheckHResult(Controller->Stop(INFINITE));
		}

		void ControllerWrapper::Stop()
		{
			CheckHResult(Controller->Terminate(0));
		}

		void ControllerWrapper::SetAllThreadsDebugState(bool suspend, ThreadWrapper^ exceptThisThread)
		{
			CorDebugThreadState threadState = (suspend ? THREAD_SUSPEND : THREAD_RUN);

			if (exceptThisThread == nullptr)
			{
				CheckHResult(Controller->SetAllThreadsDebugState(threadState, NULL));
			}
			else
			{
				CheckHResult(Controller->SetAllThreadsDebugState(threadState, exceptThisThread->Thread));
			}
		}

		bool ControllerWrapper::HasQueuedCallbacks(ThreadWrapper^ thread)
		{
			BOOL hasQueuedCallbacks;
			ICorDebugThread* threadPointer = thread->Thread;
			CheckHResult(Controller->HasQueuedCallbacks(threadPointer, &hasQueuedCallbacks));

			return (hasQueuedCallbacks == TRUE ? true : false);
		}

		bool ControllerWrapper::HasAnyThreadQueuedCallbacks()
		{
			BOOL hasQueuedCallbacks = FALSE;
			List<ValueWrapper^>^ result = gcnew List<ValueWrapper^>();
			ICorDebugThreadEnum* threadEnum;
			CheckHResult(Controller->EnumerateThreads(&threadEnum));

			ULONG fetched;
			ICorDebugThread *threads[DefaultArrayCount];
			HRESULT hResult = threadEnum->Next(DefaultArrayCount, threads, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount && hasQueuedCallbacks == FALSE)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugThread* thread = threads[index];
						CheckHResult(Controller->HasQueuedCallbacks(thread, &hasQueuedCallbacks));
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(threadEnum->Next(DefaultArrayCount, threads, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}

			return (hasQueuedCallbacks == TRUE ? true : false);
		}

		List<ThreadWrapper^>^ ControllerWrapper::EnumerateThreads()
		{
			List<ThreadWrapper^>^ result = gcnew List<ThreadWrapper^>();
			ICorDebugThreadEnum* threadEnum;
			CheckHResult(Controller->EnumerateThreads(&threadEnum));

			ULONG fetched;
			ICorDebugThread *threads[DefaultArrayCount];
			HRESULT hResult = threadEnum->Next(DefaultArrayCount, threads, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugThread* thread = threads[index];
						ThreadWrapper^ threadWrapper = gcnew ThreadWrapper();
						threadWrapper->Thread = thread;
						result->Add(threadWrapper);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(threadEnum->Next(DefaultArrayCount, threads, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}

			return result;
		}
	}
}