#include "stdafx.h"
#include "EvalWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		void EvalWrapper2::NewParameterizedObject(FunctionWrapper^ constructorFunction, List<TypeWrapper^>^ typeArguments, List<ValueWrapper^>^ arguments)
		{
			ICorDebugType** typeArgs = new ICorDebugType*[typeArguments->Count];
			for(int index = 0; index < typeArguments->Count; index++)
			{
				typeArgs[index] = typeArguments[index]->Type;
			}

			ICorDebugValue** args = new ICorDebugValue*[arguments->Count];

			for(int index = 0; index < arguments->Count; index++)
			{
				args[index] = arguments[index]->Value;
			}

			HRESULT hResult = Eval2->NewParameterizedObject(constructorFunction->Function, typeArguments->Count, typeArgs, arguments->Count, args);

			delete [] args;
			delete [] typeArgs;

			CheckHResult(hResult);
		}

		void EvalWrapper2::NewParameterizedArray(TypeWrapper^ elementType, ULONG32 rank, List<ULONG32>^ dimensions, List<ULONG32>^ lowerBounds)
		{
			ULONG32 *dims = new ULONG32[rank];
			ULONG32 *lowBounds = new ULONG32[rank];

			for(ULONG32 index = 0; index < rank; index++)
			{
				dims[index] = dimensions[index];
				lowBounds[index] = lowerBounds[index];
			}

			HRESULT hResult = Eval2->NewParameterizedArray(elementType->Type, rank, dims, lowBounds);

			delete [] lowBounds;
			delete [] dims;

			CheckHResult(hResult);
		}
	}
}