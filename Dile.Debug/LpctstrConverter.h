#pragma once

#include <vcclr.h>

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public class LpctstrConverterW
		{
		private:
			wchar_t* unicodeString;
			gcroot<String^> managedString;

		public:
			LpctstrConverterW(String^ managedString);

			~LpctstrConverterW();

			operator LPCWSTR() const;

			operator String^() const;
		};

		public class LpctstrConverterA
		{
		private:
			char* ansiString;
			gcroot<String^> managedString;

		public:
			LpctstrConverterA(String^ managedString);

			~LpctstrConverterA();

			operator LPCSTR() const;

			operator String^() const;
		};

#ifdef _UNICODE
#define LpctstrConverter LpctstrConverterW
#else
#define LpctstrConverter LpctstrConverterA
#endif

	}
}