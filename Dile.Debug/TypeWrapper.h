#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ref class ClassWrapper;

		public ref class TypeWrapper : BaseWrapper
		{
		private:
			ICorDebugType* type;
			CorElementType elementType;

		internal:
			property ICorDebugType* Type
			{
				ICorDebugType* get()
				{
					return type;
				}

				void set(ICorDebugType* value)
				{
					type = value;

					CorElementType typeType;
					CheckHResult(type->GetType(&typeType));
					elementType = typeType;
				}
			}

			TypeWrapper()
			{
			}

			TypeWrapper(ICorDebugType* type)
			{
				TypeWrapper();

				Type = type;
			}

		public:
			property int ElementType
			{
				int get()
				{
					return elementType;
				}
			}

			TypeWrapper^ GetBase();

			ClassWrapper^ GetClass();

			List<TypeWrapper^>^ EnumerateTypeParameters();

			List<TypeWrapper^>^ EnumerateAllTypeParameters();
		};
	}
}