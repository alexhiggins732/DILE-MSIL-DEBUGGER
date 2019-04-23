#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "ValueWrapper.h"
#include "EvalWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		public ref class EvalWrapper : BaseWrapper
		{
		private:
			ICorDebugEval* eval;
			EvalWrapper2^ eval2;
			bool^ isVersion2;

		internal:
			property ICorDebugEval* Eval
			{
				ICorDebugEval* get()
				{
					return eval;
				}

				void set (ICorDebugEval* value)
				{
					eval = value;
				}
			}

			EvalWrapper()
			{
				isVersion2 = nullptr;
			}

			EvalWrapper(ICorDebugEval* eval)
			{
				EvalWrapper();

				Eval = eval;
			}

		public:
			property EvalWrapper2^ Version2
			{
				EvalWrapper2^ get()
				{
					if(eval2 == nullptr)
					{
						ICorDebugEval2* eval2Pointer;

						if (SUCCEEDED(eval->QueryInterface(&eval2Pointer)))
						{
							eval2 = gcnew EvalWrapper2(eval2Pointer);
						}
					}

					return eval2;
				}
			}

			property bool IsVersion2
			{
				virtual bool get()
				{
					bool result = false;

					if (isVersion2 == nullptr)
					{
						isVersion2 = (Version2 != nullptr);
						result = *isVersion2;
					}
					else
					{
						result = *isVersion2;
					}

					return result;
				}
			}

			ValueWrapper^ GetResult();

			void Abort();

			ValueWrapper^ CreateValue(int elementType, ClassWrapper^ classWrapper);

			void NewString(String^ string);

			void NewObjectNoConstructor(ClassWrapper^ classWrapper);

			void NewArray(int elementType, ClassWrapper^ elementClass, ULONG32 length);

			void NewObject(FunctionWrapper^ constructorFunction, List<ValueWrapper^>^ arguments);
		};
	}
}