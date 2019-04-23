#include "stdafx.h"
#include "FunctionWrapper.h"
#include "ModuleWrapper.h"
#include "FunctionBreakpointWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		FunctionBreakpointWrapper^ FunctionWrapper::CreateBreakpoint(ULONG32 offset)
		{
			return Code->CreateBreakpoint(offset);
		}

		ModuleWrapper^ FunctionWrapper::GetModule()
		{
			ICorDebugModule* module;
			CheckHResult(Function->GetModule(&module));

			ModuleWrapper^ result = gcnew ModuleWrapper();
			result->Module = module;

			return result;
		}

		UInt32 FunctionWrapper::GetToken()
		{
			mdMethodDef result;
			CheckHResult(Function->GetToken(&result));

			return result;
		}

		array<Byte>^ FunctionWrapper::GetILCode()
		{
			mdSignature local;
			CheckHResult(Function->GetLocalVarSigToken(&local));
			return Code->GetILCode();
		}

		CORDB_ADDRESS FunctionWrapper::GetAddress()
		{
			return Code->GetAddress();
		}

		mdSignature FunctionWrapper::GetLocalVarSigToken()
		{
			mdSignature result;
			CheckHResult(Function->GetLocalVarSigToken(&result));

			return result;
		}
	}
}