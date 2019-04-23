#include "Stdafx.h"
#include "ProcessLibraryProvider.h"
#include "LpctstrConverter.h"

using namespace System;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		ProcessLibraryProvider::ProcessLibraryProvider()
		{
			referenceCount = 0;
		}

		HRESULT ProcessLibraryProvider::QueryInterface(const IID& iid, void** obj)
		{
			HRESULT result = S_OK;

			if (iid == IID_ICLRDebuggingLibraryProvider)
			{
				*obj = static_cast<ICLRDebuggingLibraryProvider*>(this);
			}
			else
			{
				result = E_NOINTERFACE;
			}

			return result;
		}

		ULONG ProcessLibraryProvider::AddRef()
		{
			return ++referenceCount;
		}

		ULONG ProcessLibraryProvider::Release()
		{
			ULONG temp = --referenceCount;
			if (referenceCount == 0)
			{
				delete this;
			}

			return temp;
		}

		HRESULT ProcessLibraryProvider::ProvideLibrary(const WCHAR* pwszFileName, DWORD dwTimestamp, DWORD dwSizeOfImage, HMODULE* hModule)
		{
			HRESULT result = S_OK;

			//TODO Validate file size and timestamp
			//TODO Fix the following call. It returns the runtime directory of DILE and not the debugee's. In the future it would be nice to load all modules from the dump file.
			String^ clrFolderPath = RuntimeEnvironment::GetRuntimeDirectory();
			IntPtr fileNamePtr((PVOID)pwszFileName);
			String^ managedFileName = Marshal::PtrToStringAuto(fileNamePtr);
			String^ filePath = Path::Combine(clrFolderPath, managedFileName);
			LpctstrConverter filePathConverter(filePath);
				
			HMODULE tempHModule = LoadLibrary(filePathConverter);
			if (tempHModule == NULL)
			{
				result = E_FAIL;
			}
			else
			{
				*hModule = tempHModule;
			}

			return result;
		}
	}
}