#include "stdafx.h"
#include "ChainWrapper.h"
#include "ThreadWrapper.h"

namespace Dile
{
	namespace Debug
	{
		ThreadWrapper^ ChainWrapper::GetThread()
		{
			ICorDebugThread* thread;
			CheckHResult(Chain->GetThread(&thread));

			ThreadWrapper^ result = gcnew ThreadWrapper();
			result->Thread = thread;

			return result;
		}
	}
}