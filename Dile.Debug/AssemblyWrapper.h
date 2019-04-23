#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class AppDomainWrapper;

		public ref class AssemblyWrapper : BaseWrapper
		{
		private:
			ICorDebugAssembly* assembly;

		internal:
			property ICorDebugAssembly* Assembly
			{
				ICorDebugAssembly* get()
				{
					return assembly;
				}

				void set (ICorDebugAssembly* value)
				{
					assembly = value;
				}
			}

			AssemblyWrapper()
			{
			}

			AssemblyWrapper(ICorDebugAssembly* assembly)
			{
				AssemblyWrapper();

				Assembly = assembly;
			}

		public:
			String^ GetName();

			String^ GetCodeBase();

			AppDomainWrapper^ GetAppDomain();
		};
	}
}