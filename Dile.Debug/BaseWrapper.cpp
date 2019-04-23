#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		void BaseWrapper::CheckHResult(HRESULT hResult)
		{
			if (hResult != CORDBG_E_PROCESS_TERMINATED && FAILED(hResult))
			{
				Marshal::ThrowExceptionForHR(hResult);
			}
		}
	}
}