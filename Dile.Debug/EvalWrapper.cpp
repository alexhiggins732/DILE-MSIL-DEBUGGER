#include "stdafx.h"
#include "EvalWrapper.h"
#include "FunctionWrapper.h"
#include "BstrConverter.h"

using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		ValueWrapper^ EvalWrapper::GetResult()
		{
			ICorDebugValue* value;
			CheckHResult(Eval->GetResult(&value));

			ValueWrapper^ result = nullptr;

			if (value != NULL)
			{
				result = gcnew ValueWrapper();
				result->Value = value;
			}

			return result;
		}

		void EvalWrapper::Abort()
		{
			CheckHResult(Eval->Abort());
		}

		ValueWrapper^ EvalWrapper::CreateValue(int elementType, ClassWrapper^ classWrapper)
		{
			ICorDebugValue* createdValue;
			ICorDebugClass* elementClass = NULL;

			if (classWrapper != nullptr)
			{
				elementClass = classWrapper->ClassObject;
			}

			CheckHResult(Eval->CreateValue((CorElementType)elementType, elementClass, &createdValue));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = createdValue;

			return result;
		}

		void EvalWrapper::NewString(String^ string)
		{
			BstrConverter stringBstr(string);

			CheckHResult(Eval->NewString(stringBstr));
		}

		void EvalWrapper::NewObjectNoConstructor(ClassWrapper^ classWrapper)
		{
			CheckHResult(Eval->NewObjectNoConstructor(classWrapper->ClassObject));
		}

		void EvalWrapper::NewArray(int elementType, ClassWrapper^ elementClass, ULONG32 length)
		{
			ICorDebugClass* elementClassPointer = (elementClass == nullptr ? NULL : elementClass->ClassObject);
			ULONG32 dimensions[1];
			dimensions[0] = length;

			CheckHResult(Eval->NewArray((CorElementType)elementType, elementClassPointer, 1, dimensions, NULL));
		}

		void EvalWrapper::NewObject(FunctionWrapper^ constructorFunction, List<ValueWrapper^>^ arguments)
		{
			ICorDebugValue** args = new ICorDebugValue*[arguments->Count];

			for(int index = 0; index < arguments->Count; index++)
			{
				args[index] = arguments[index]->Value;
			}

			HRESULT hResult = eval->NewObject(constructorFunction->Function, arguments->Count, args);

			delete [] args;

			CheckHResult(hResult);
		}
	}
}