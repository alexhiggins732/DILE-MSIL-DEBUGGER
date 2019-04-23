#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "TypeWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class ValueWrapper2 : BaseWrapper
		{
		private:
			ICorDebugValue2* value2;

		internal:
			property ICorDebugValue2* Value2
			{
				ICorDebugValue2* get()
				{
					return value2;
				}

				void set(ICorDebugValue2* value)
				{
					value2 = value;
				}
			}

			ValueWrapper2()
			{
			}

			ValueWrapper2(ICorDebugValue2* value2)
			{
				ValueWrapper2();

				Value2 = value2;
			}

		public:
			TypeWrapper^ GetExactType();
		};
	}
}