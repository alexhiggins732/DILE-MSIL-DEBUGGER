#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class ProcessWrapper2 : BaseWrapper
		{
		private:
			ICorDebugProcess2* process2Object;

		internal:
			property ICorDebugProcess2* Process2Object
			{
				ICorDebugProcess2* get()
				{
					return process2Object;
				}

				void set(ICorDebugProcess2* value)
				{
					process2Object = value;
				}
			}

			ProcessWrapper2()
			{
			}

			ProcessWrapper2(ICorDebugProcess2* process2Object)
			{
				ProcessWrapper2();

				Process2Object = process2Object;
			}

		public:
			void SetDesiredNGENCompilerFlags(DWORD flags);
		};
	}
}