#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "TypeWrapper.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		public ref class ClassWrapper2 : BaseWrapper
		{
		private:
			ICorDebugClass2* class2Object;

		internal:
			property ICorDebugClass2* Class2Object
			{
				ICorDebugClass2* get()
				{
					return class2Object;
				}

				void set(ICorDebugClass2* value)
				{
					class2Object = value;
				}
			}

			ClassWrapper2()
			{
			}

			ClassWrapper2(ICorDebugClass2* class2Object)
			{
				ClassWrapper2();

				Class2Object = class2Object;
			}

		public:
			TypeWrapper^ GetParameterizedType(int elementType, List<TypeWrapper^>^ typeArguments);
		};
	}
}