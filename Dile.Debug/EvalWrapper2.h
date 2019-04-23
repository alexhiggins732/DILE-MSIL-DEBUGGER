#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "FunctionWrapper.h"
#include "TypeWrapper.h"
#include "ValueWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class EvalWrapper2 : BaseWrapper
		{
		private:
			ICorDebugEval2* eval2;

		internal:
			property ICorDebugEval2* Eval2
			{
				ICorDebugEval2* get()
				{
					return eval2;
				}

				void set(ICorDebugEval2* value)
				{
					eval2 = value;
				}
			}

			EvalWrapper2()
			{
			}

			EvalWrapper2(ICorDebugEval2* eval2)
			{
				EvalWrapper2();

				Eval2 = eval2;
			}

		public:
			void NewParameterizedObject(FunctionWrapper^ constructorFunction, List<TypeWrapper^>^ typeArguments, List<ValueWrapper^>^ arguments);

			void NewParameterizedArray(TypeWrapper^ elementType, ULONG32 rank, List<ULONG32>^ dimensions, List<ULONG32>^ lowerBounds);
		};
	}
}