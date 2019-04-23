#include "Stdafx.h"
#include "RuntimeInfo.h"

namespace Dile
{
	namespace Debug
	{
		RuntimeInfo::RuntimeInfo(ICLRRuntimeInfo *pRuntimeInfo)
		{
			this->pRuntimeInfo = pRuntimeInfo;
		}

		RuntimeInfo::~RuntimeInfo()
		{
			this->!RuntimeInfo();
		}

		RuntimeInfo::!RuntimeInfo()
		{
			CheckHResult(pRuntimeInfo->Release());
		}

		String^ RuntimeInfo::GetVersionString()
		{
			String^ result = String::Empty;
			WCHAR *pBuffer = NULL;
			DWORD bufferSize = NULL;
			HRESULT hResult = pRuntimeInfo->GetVersionString(pBuffer, &bufferSize);

			if (hResult == HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER))
			{
				pBuffer = new WCHAR[bufferSize];
				hResult = pRuntimeInfo->GetVersionString(pBuffer, &bufferSize);
			}

			CheckHResult(hResult);
			result = gcnew String(pBuffer);
			delete pBuffer;

			return result;
		}

		Debugger^ RuntimeInfo::GetDebugger()
		{
			Debugger^ result = nullptr;
			ICorDebug *pDebug = NULL;
			CheckHResult(pRuntimeInfo->GetInterface(CLSID_CLRDebuggingLegacy, IID_ICorDebug, (LPVOID*)&pDebug));

			result = gcnew Debugger(pDebug);

			return result;
		}
	}
}