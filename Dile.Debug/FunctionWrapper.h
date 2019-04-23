#pragma once

#include "stdafx.h"
#include "CodeWrapper.h"
#include "ModuleWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class ModuleWrapper;
		ref class FunctionBreakpointWrapper;

		public ref class FunctionWrapper : BaseWrapper
		{
		private:
			ICorDebugFunction* function;
			CodeWrapper^ code;

			property CodeWrapper^ Code
			{
				CodeWrapper^ get()
				{
					if (Function != NULL && code == nullptr)
					{
						ICorDebugCode* tempCode;
						CheckHResult(Function->GetILCode(&tempCode));

						code = gcnew CodeWrapper();
						code->Code = tempCode;
					}

					return code;
				}
			}

		internal:
			property ICorDebugFunction* Function
			{
				ICorDebugFunction* get()
				{
					return function;
				}

				void set(ICorDebugFunction* value)
				{
					function = value;
					code = nullptr;
				}
			}

			FunctionWrapper()
			{
			}

			FunctionWrapper(ICorDebugFunction* function)
			{
				FunctionWrapper();

				Function = function;
			}

		public:
			FunctionBreakpointWrapper^ CreateBreakpoint(ULONG32 offset);

			ModuleWrapper^ GetModule();

			UInt32 GetToken();

			array<Byte>^ GetILCode();

			CORDB_ADDRESS GetAddress();

			mdSignature GetLocalVarSigToken();
		};
	}
}