#include "stdafx.h"
#include "ValueWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		TypeWrapper^ ValueWrapper2::GetExactType()
		{
			TypeWrapper^ result = nullptr;

			ICorDebugType* exactType;
			CheckHResult(Value2->GetExactType(&exactType));

			result = gcnew TypeWrapper(exactType);

			return result;
		}
	}
}