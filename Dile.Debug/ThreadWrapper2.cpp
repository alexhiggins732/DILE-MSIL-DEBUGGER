#include "stdafx.h"
#include "ThreadWrapper.h"

namespace Dile
{
	namespace Debug
	{
		Int32 ThreadWrapper2::CallParameterizedFunction(FunctionWrapper^ function, List<TypeWrapper^>^ typeArguments, List<ValueWrapper^>^ arguments, interior_ptr<EvalWrapper^> evalWrapper)
		{
			ICorDebugEval* eval;
			CheckHResult(Thread->CreateEval(&eval));

			*evalWrapper = gcnew EvalWrapper(eval);
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

			HRESULT hResult = (*evalWrapper)->Version2->Eval2->CallParameterizedFunction(function->Function, typeArguments->Count, typeArgs, arguments->Count, args);

			delete [] typeArgs;
			delete [] args;

			return hResult;
		}
	}
}