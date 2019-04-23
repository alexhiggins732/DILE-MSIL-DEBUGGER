#include "Stdafx.h"
#include "MetaHost.h"
#include "Constants.h"

using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		MetaHost::MetaHost()
		{
			ICLRMetaHost *pMetaHostTemp = NULL;
			CheckHResult(CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost, (LPVOID*)&pMetaHostTemp));
			pMetaHost = pMetaHostTemp;
		}

		MetaHost::~MetaHost()
		{
			this->!MetaHost();
		}

		MetaHost::!MetaHost()
		{
			CheckHResult(pMetaHost->Release());
		}

		String^ MetaHost::GetVersionFromFile(String^ filePath)
		{
			String^ result = String::Empty;
			BSTR bstrFilePath = BSTR(Marshal::StringToBSTR(filePath).ToPointer());
			WCHAR *pBuffer = NULL;
			DWORD bufferSize = NULL;
			HRESULT hResult = pMetaHost->GetVersionFromFile(bstrFilePath, pBuffer, &bufferSize);

			if (hResult == HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER))
			{
				pBuffer = new WCHAR[bufferSize];
				hResult = pMetaHost->GetVersionFromFile(bstrFilePath, pBuffer, &bufferSize);
			}

			SysFreeString(bstrFilePath);
			CheckHResult(hResult);

			result = gcnew String(pBuffer);
			delete pBuffer;

			return result;
		}

		RuntimeInfo^ MetaHost::GetRuntime(String^ frameworkVersion)
		{
			RuntimeInfo^ result = nullptr;

			BSTR bstrFrameworkVersion = BSTR(Marshal::StringToBSTR(frameworkVersion).ToPointer());
			ICLRRuntimeInfo *pRuntimeInfo = NULL;
			HRESULT hResult = pMetaHost->GetRuntime(bstrFrameworkVersion, IID_ICLRRuntimeInfo, (LPVOID*)&pRuntimeInfo);

			SysFreeString(bstrFrameworkVersion);
			CheckHResult(hResult);

			result = gcnew RuntimeInfo(pRuntimeInfo);

			return result;
		}

		array<RuntimeInfo^>^ MetaHost::EnumerateLoadedRuntimes(IntPtr hProcess)
		{
			List<RuntimeInfo^>^ result = nullptr;
			IEnumUnknown *pEnum = NULL;
			CheckHResult(pMetaHost->EnumerateLoadedRuntimes((HANDLE)hProcess.ToPointer(), &pEnum));

			ICLRRuntimeInfo *pRuntimeInfos[DefaultArrayCount];
			ULONG fetched;
			CheckHResult(pEnum->Next(DefaultArrayCount, (IUnknown**)&pRuntimeInfos, &fetched));
			result = gcnew List<RuntimeInfo^>();

			while (fetched > 0)
			{
				for (ULONG index = 0; index < fetched; index++)
				{
					ICLRRuntimeInfo *pRuntimeInfo = pRuntimeInfos[index];
					RuntimeInfo^ runtimeInfo = gcnew RuntimeInfo(pRuntimeInfo);

					result->Add(runtimeInfo);
				}

				if (fetched < DefaultArrayCount)
				{
					fetched = 0;
				}
				else
				{
					CheckHResult(pEnum->Next(DefaultArrayCount, (IUnknown**)&pRuntimeInfos, &fetched));
				}
			}

			return result->ToArray();
		}
	}
}