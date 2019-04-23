#include "stdafx.h"
#include "BreakpointWrapper.h"
#include "FunctionBreakpointWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		void BreakpointWrapper::Activate(bool active)
		{
			CheckHResult(Breakpoint->Activate((active ? TRUE : FALSE)));
		}

		bool BreakpointWrapper::IsActive()
		{
			BOOL isActive;
			CheckHResult(Breakpoint->IsActive(&isActive));

			return (isActive == TRUE ? true : false);
		}

		FunctionBreakpointWrapper^ BreakpointWrapper::TryConvertFunctionBreakpoint()
		{
			ICorDebugFunctionBreakpoint* functionBreakpoint;
			HRESULT hResult = Breakpoint->QueryInterface(&functionBreakpoint);

			FunctionBreakpointWrapper^ result = nullptr;

			if (SUCCEEDED(hResult))
			{
				result = gcnew FunctionBreakpointWrapper();
				result->FunctionBreakpoint = functionBreakpoint;
			}

			return result;
		}
	}
}