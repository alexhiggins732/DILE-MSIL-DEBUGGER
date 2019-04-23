#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ref class ValueWrapper;

		public ref class ArrayValueWrapper : BaseWrapper
		{
		private:
			ICorDebugArrayValue* arrayValue;
			ULONG32 rank;

		internal:
			property ICorDebugArrayValue* ArrayValue
			{
				ICorDebugArrayValue* get()
				{
					return arrayValue;
				}

				void set(ICorDebugArrayValue* value)
				{
					arrayValue = value;
				}
			}

			ArrayValueWrapper()
			{
				rank = -1;
			}

			ArrayValueWrapper(ICorDebugArrayValue* arrayValue)
			{
				ArrayValueWrapper();

				ArrayValue = arrayValue;
			}

		public:
			ValueWrapper^ GetElementAtPosition(UInt32 position);

			ValueWrapper^ GetElement(List<UInt32>^ indices);

			List<UInt32>^ GetBaseIndicies();

			UInt32 GetCount();

			List<UInt32>^ GetDimensions();

			UInt32 GetRank();

			bool HasBaseIndicies();

			int GetElementType();
		};
	}
}