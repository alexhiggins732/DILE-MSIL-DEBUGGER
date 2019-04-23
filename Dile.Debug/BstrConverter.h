#pragma once

#include <vcclr.h>

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public class BstrConverter
		{
		private:
			BSTR bstrString;
			gcroot<String^> managedString;

		public:
			BstrConverter(String^ managedString);

			~BstrConverter();

			operator BSTR() const;

			operator String^() const;
		};
	}
}