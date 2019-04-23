#include "stdafx.h"
#include "ProcessWrapper2.h"

namespace Dile
{
	namespace Debug
	{
		void ProcessWrapper2::SetDesiredNGENCompilerFlags(DWORD flags)
		{
			Process2Object->SetDesiredNGENCompilerFlags(flags);
		}
	}
}