#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class FunctionBreakpointWrapper;

		public ref class BreakpointWrapper : BaseWrapper
		{
		private:
			ICorDebugBreakpoint* breakpoint;

		internal:
			property ICorDebugBreakpoint* Breakpoint
			{
				ICorDebugBreakpoint* get()
				{
					return breakpoint;
				}

				void set(ICorDebugBreakpoint* value)
				{
					breakpoint = value;
				}
			}

			BreakpointWrapper()
			{
			}

			BreakpointWrapper(ICorDebugBreakpoint* breakpoint)
			{
				BreakpointWrapper();

				Breakpoint = breakpoint;
			}

		public:
			void Activate(bool active);

			bool IsActive();

			FunctionBreakpointWrapper^ TryConvertFunctionBreakpoint();
		};
	}
}