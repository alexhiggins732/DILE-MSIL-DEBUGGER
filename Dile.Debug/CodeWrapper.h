#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class FunctionBreakpointWrapper;

		public ref class CodeWrapper : BaseWrapper
		{
		private:
			ICorDebugCode* code;

		internal:
			property ICorDebugCode* Code
			{
				ICorDebugCode* get()
				{
					return code;
				}

				void set(ICorDebugCode* value)
				{
					code = value;
				}
			}

			CodeWrapper()
			{
			}

			CodeWrapper(ICorDebugCode* code)
			{
				CodeWrapper();

				Code = code;
			}

		public:
			FunctionBreakpointWrapper^ CreateBreakpoint(ULONG32 offset);

			array<Byte>^ GetILCode();

			CORDB_ADDRESS GetAddress();
		};
	}
}