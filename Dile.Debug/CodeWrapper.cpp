#include "stdafx.h"
#include "CodeWrapper.h"
#include "FunctionBreakpointWrapper.h"

namespace Dile
{
	namespace Debug
	{
		FunctionBreakpointWrapper^ CodeWrapper::CreateBreakpoint(ULONG32 offset)
		{
			ICorDebugFunctionBreakpoint* breakpoint;
			HRESULT hResult = Code->CreateBreakpoint(offset, &breakpoint);

			FunctionBreakpointWrapper^ result = nullptr;
			
			if (SUCCEEDED(hResult))
			{
			  result = gcnew FunctionBreakpointWrapper();
				result->FunctionBreakpoint = breakpoint;
			}
			else
			{
				if (hResult != CORDBG_E_UNABLE_TO_SET_BREAKPOINT)
				{
					CheckHResult(hResult);
				}
			}

			return result;
		}

		array<Byte>^ CodeWrapper::GetILCode()
		{
			ULONG32 codeSize;
			CheckHResult(Code->GetSize(&codeSize));

			BYTE *buffer = new BYTE[codeSize];
			ULONG32 bufferSize = codeSize;
			CheckHResult(Code->GetCode(0, codeSize, codeSize, buffer, &bufferSize));

			array<Byte>^ result = gcnew array<Byte>(bufferSize);
			
			for (unsigned int index = 0; index < bufferSize; index++)
			{
				result[index] = buffer[index];
			}

			delete [] buffer;

			return result;
		}

		CORDB_ADDRESS CodeWrapper::GetAddress()
		{
			CORDB_ADDRESS start;
			CheckHResult(Code->GetAddress(&start));

			return start;
		}
	}
}