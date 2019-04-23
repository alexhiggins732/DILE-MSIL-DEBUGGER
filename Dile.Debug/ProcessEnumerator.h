#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "ProcessInformation.h"
#include <corpub.h>

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		public ref class ProcessEnumerator : BaseWrapper
		{
		private:
			String^ GetProcessName(ICorPublishProcess* process);

		public:
			List<ProcessInformation^>^ GetManagedProcesses();
		};
	}
}