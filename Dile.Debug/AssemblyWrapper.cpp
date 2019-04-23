#include "stdafx.h"
#include "AssemblyWrapper.h"
#include "AppDomainWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		String^ AssemblyWrapper::GetName()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Assembly->GetName(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Assembly->GetName(expectedSize, &expectedSize, name);

				if (FAILED(hResult))
				{
					delete [] name;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(name);

			delete [] name;

			return result;
		}

		String^ AssemblyWrapper::GetCodeBase()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Assembly->GetCodeBase(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Assembly->GetCodeBase(expectedSize, &expectedSize, name);

				if (FAILED(hResult))
				{
					delete [] name;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(name);

			delete [] name;

			return result;
		}

		AppDomainWrapper^ AssemblyWrapper::GetAppDomain()
		{
			ICorDebugAppDomain* appDomain;
			CheckHResult(Assembly->GetAppDomain(&appDomain));

			AppDomainWrapper^ result = gcnew AppDomainWrapper();
			result->AppDomain = appDomain;

			return result;
		}
	}
}