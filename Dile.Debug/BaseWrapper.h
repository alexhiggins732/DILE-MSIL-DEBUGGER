#pragma once

#include "stdafx.h"

namespace Dile
{
	namespace Debug
	{
		public ref class BaseWrapper
		{
		protected:
			static ULONG32 DefaultCharArraySize = 255;

			void CheckHResult(HRESULT hResult);
		};
	}
}