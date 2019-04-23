#include "Stdafx.h"
#include "LpctstrConverter.h"

using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		LpctstrConverterW::LpctstrConverterW(String^ managedString)
		{
			this->managedString = managedString;
			unicodeString = static_cast<wchar_t*>(Marshal::StringToHGlobalUni(managedString).ToPointer());
		}

		LpctstrConverterW::~LpctstrConverterW()
		{
			Marshal::FreeHGlobal(IntPtr(unicodeString));
		}

		LpctstrConverterW::operator LPCWSTR() const
		{
			return unicodeString;
		}

		LpctstrConverterW::operator String^() const
		{
			return managedString;
		}

		LpctstrConverterA::LpctstrConverterA(String^ managedString)
		{
			this->managedString = managedString;
			ansiString = static_cast<char*>(Marshal::StringToHGlobalAnsi(managedString).ToPointer());
		}

		LpctstrConverterA::~LpctstrConverterA()
		{
			Marshal::FreeHGlobal(IntPtr(ansiString));
		}

		LpctstrConverterA::operator LPCSTR() const
		{
			return ansiString;
		}

		LpctstrConverterA::operator String^() const
		{
			return managedString;
		}
	}
}