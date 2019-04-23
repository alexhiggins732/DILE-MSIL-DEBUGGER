#include "stdafx.h"
#include "ClassWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		TypeWrapper^ ClassWrapper2::GetParameterizedType(int elementType, List<TypeWrapper^>^ typeArguments)
		{
			TypeWrapper^ result = nullptr;
			ICorDebugType** typeArgs = new ICorDebugType*[typeArguments->Capacity];

			for(int index = 0; index < typeArguments->Count; index++)
			{
				typeArgs[index] = typeArguments[index]->Type;
			}

			ICorDebugType* resultType;

			HRESULT hResult = Class2Object->GetParameterizedType((CorElementType)elementType, typeArguments->Count, typeArgs, &resultType);

			delete[] typeArgs;

			CheckHResult(hResult);

			result = gcnew TypeWrapper(resultType);

			return result;
		}
	}
}