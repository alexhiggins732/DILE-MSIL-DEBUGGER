#include "stdafx.h"
#include "ClassWrapper.h"
#include "FrameWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ValueWrapper^ ClassWrapper::GetStaticFieldValue(mdFieldDef fieldToken, FrameWrapper^ frame)
		{
			ICorDebugValue* value;
			CheckHResult(ClassObject->GetStaticFieldValue(fieldToken, frame->Frame, &value));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = value;

			return result;
		}

		UInt32 ClassWrapper::GetToken()
		{
			mdTypeDef token;
			CheckHResult(ClassObject->GetToken(&token));

			return token;
		}

		ModuleWrapper^ ClassWrapper::GetModule()
		{
			ICorDebugModule* module;

			CheckHResult(ClassObject->GetModule(&module));

			ModuleWrapper^ result = gcnew ModuleWrapper();
			result->Module = module;

			return result;
		}
	}
}