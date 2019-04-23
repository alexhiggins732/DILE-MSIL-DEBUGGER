#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "EvalWrapper.h"
#include "ValueWrapper.h"
#include "TypeWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class ThreadWrapper2 : BaseWrapper
		{
		private:
			ICorDebugThread* thread;
			ICorDebugThread2* thread2;

			property ICorDebugThread* Thread
			{
				ICorDebugThread* get()
				{
					if (thread == NULL)
					{
						ICorDebugThread* threadPointer;
						CheckHResult(Thread2->QueryInterface(&threadPointer));
						thread = threadPointer;
					}

					return thread;
				}
			}

		internal:
			property ICorDebugThread2* Thread2
			{
				ICorDebugThread2* get()
				{
					return thread2;
				}

				void set(ICorDebugThread2* value)
				{
					thread2 = value;
				}
			}

			ThreadWrapper2()
			{
			}

			ThreadWrapper2(ICorDebugThread2* thread2)
			{
				ThreadWrapper2();

				Thread2 = thread2;
			}

		public:
			Int32 CallParameterizedFunction(FunctionWrapper^ function, List<TypeWrapper^>^ typeArguments, List<ValueWrapper^>^ arguments, interior_ptr<EvalWrapper^> evalWrapper);
		};
	}
}