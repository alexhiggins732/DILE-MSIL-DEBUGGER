#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ref class ThreadWrapper;

		public ref class ControllerWrapper : BaseWrapper
		{
		private:
			ICorDebugController* controller;

		internal:
			property ICorDebugController* Controller
			{
				ICorDebugController* get()
				{
					return controller;
				}

				void set(ICorDebugController* value)
				{
					controller = value;
				}
			}

			ControllerWrapper()
			{
			}

			ControllerWrapper(ICorDebugController* controller)
			{
				Controller = controller;
			}

		public:
			void Continue();

			bool IsRunning();

			virtual void Detach();

			void Pause();

			void Stop();

			void SetAllThreadsDebugState(bool suspend, ThreadWrapper^ exceptThisThread);

			bool HasQueuedCallbacks(ThreadWrapper^ thread);

			bool HasAnyThreadQueuedCallbacks();

			List<ThreadWrapper^>^ EnumerateThreads();
		};
	}
}