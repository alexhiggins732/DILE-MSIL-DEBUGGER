#include "Stdafx.h"
#include "BstrConverter.h"

using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		BstrConverter::BstrConverter(String^ managedString)
		{
			this->managedString = managedString;
			bstrString = BSTR(Marshal::StringToBSTR(managedString).ToPointer());
		}

		BstrConverter::~BstrConverter()
		{
			SysFreeString(bstrString);
		}

		BstrConverter::operator BSTR() const
		{
			return bstrString;
		}

		BstrConverter::operator String^() const
		{
			return managedString;
		}
	}
}