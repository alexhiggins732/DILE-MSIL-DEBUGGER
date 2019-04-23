#pragma once

#include <metahost.h>
#include "BaseWrapper.h"
#include "RuntimeInfo.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class MetaHost : BaseWrapper
		{
		private:
			ICLRMetaHost *pMetaHost;

		public:
			MetaHost();

			~MetaHost();

			!MetaHost();

			String^ GetVersionFromFile(String^ filePath);

			RuntimeInfo^ GetRuntime(String^ frameworkVersion);

			array<RuntimeInfo^>^ EnumerateLoadedRuntimes(IntPtr hProcess);
		};
	}
}