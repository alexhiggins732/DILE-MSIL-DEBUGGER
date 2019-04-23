#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class ThreadWrapper;

		public ref class ChainWrapper : BaseWrapper
		{
		private:
			ICorDebugChain* chain;

		internal:
			property ICorDebugChain* Chain
			{
				ICorDebugChain* get()
				{
					return chain;
				}

				void set(ICorDebugChain* value)
				{
					chain = value;
				}
			}

			ChainWrapper()
			{
			}

			ChainWrapper(ICorDebugChain* chain)
			{
				ChainWrapper();

				Chain = chain;
			}

		public:
			ThreadWrapper^ GetThread();
		};
	}
}