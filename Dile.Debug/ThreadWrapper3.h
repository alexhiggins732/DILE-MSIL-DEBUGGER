#pragma once

#include "Stdafx.h"
#include "BaseWrapper.h"
#include "FrameWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class ThreadWrapper3 : BaseWrapper
		{
		private:
			ICorDebugThread3* thread3;

		internal:
			property ICorDebugThread3* Thread3
			{
				ICorDebugThread3* get()
				{
					return thread3;
				}

				void set(ICorDebugThread3* value)
				{
					thread3 = value;
				}
			}

			ThreadWrapper3();

			ThreadWrapper3(ICorDebugThread3* thread3);

		public:
			List<FrameWrapper^>^ StackWalk();

			FrameWrapper^ GetActiveFrame();
		};
	}
}