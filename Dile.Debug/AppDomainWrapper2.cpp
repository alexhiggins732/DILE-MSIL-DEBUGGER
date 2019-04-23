#include "stdafx.h"
#include "AppDomainWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		TypeWrapper^ AppDomainWrapper2::GetArrayOrPointerType(int elementType, ULONG32 rank, TypeWrapper^ typeArgument)
		{
			TypeWrapper^ result = nullptr;
			ICorDebugType* resultType = NULL;

			CheckHResult(AppDomain2->GetArrayOrPointerType((CorElementType)elementType, rank, typeArgument->Type, &resultType));

			result = gcnew TypeWrapper();
			result->Type = resultType;

			return result;
		}
	}
}