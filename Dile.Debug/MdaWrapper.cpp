#include "stdafx.h"
#include "MdaWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		String^ MdaWrapper::GetName()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Mda->GetName(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Mda->GetName(expectedSize, &expectedSize, name);

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

		String^ MdaWrapper::GetDescription()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Mda->GetDescription(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Mda->GetDescription(expectedSize, &expectedSize, name);

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

		String^ MdaWrapper::GetXml()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Mda->GetXML(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Mda->GetXML(expectedSize, &expectedSize, name);

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

		UInt32 MdaWrapper::GetFlags()
		{
			CorDebugMDAFlags flags;
			CheckHResult(Mda->GetFlags(&flags));

			return flags;
		}

		UInt32 MdaWrapper::GetOSThreadID()
		{
			DWORD threadID;
			CheckHResult(Mda->GetOSThreadId(&threadID));

			return threadID;
		}
	}
}