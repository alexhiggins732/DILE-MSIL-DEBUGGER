#pragma once

#include <metahost.h>
#include "BaseWrapper.h"
#include "Debugger.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class RuntimeInfo : BaseWrapper
		{
		private:
			ICLRRuntimeInfo *pRuntimeInfo;

		internal:
			RuntimeInfo(ICLRRuntimeInfo *pRuntimeInfo);

		public:
			~RuntimeInfo();

			!RuntimeInfo();

			String^ GetVersionString();

			Debugger^ GetDebugger();
		};
	}
}