#pragma once

#include "stdafx.h"
#include "BreakpointWrapper.h"
#include "FunctionWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class FunctionBreakpointWrapper : BreakpointWrapper
		{
		private:
			ICorDebugFunctionBreakpoint* functionBreakpoint;

		internal:
			property ICorDebugFunctionBreakpoint* FunctionBreakpoint
			{
				ICorDebugFunctionBreakpoint* get()
				{
					return functionBreakpoint;
				}

				void set(ICorDebugFunctionBreakpoint* value)
				{
					functionBreakpoint = value;
					Breakpoint = functionBreakpoint;
				}
			}

			FunctionBreakpointWrapper()
			{
			}

			FunctionBreakpointWrapper(ICorDebugFunctionBreakpoint* functionBreakpoint)
			{
				FunctionBreakpointWrapper();

				FunctionBreakpoint = functionBreakpoint;
			}

		public:
			FunctionWrapper^ GetFunction();

			UInt32 GetOffset();
		};
	}
}