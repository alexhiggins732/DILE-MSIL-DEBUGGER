#include "stdafx.h"
#include "FunctionBreakpointWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		FunctionWrapper^ FunctionBreakpointWrapper::GetFunction()
		{
			ICorDebugFunction* function;
			CheckHResult(FunctionBreakpoint->GetFunction(&function));

			FunctionWrapper^ result = gcnew FunctionWrapper();
			result->Function = function;

			return result;
		}

		UInt32 FunctionBreakpointWrapper::GetOffset()
		{
			ULONG32 offset;
			CheckHResult(FunctionBreakpoint->GetOffset(&offset));

			return offset;
		}
	}
}