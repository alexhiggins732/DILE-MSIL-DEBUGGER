#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class MdaWrapper : BaseWrapper
		{
		private:
			ICorDebugMDA* mda;

		internal:
			property ICorDebugMDA* Mda
			{
				ICorDebugMDA* get()
				{
					return mda;
				}
				void set(ICorDebugMDA* value)
				{
					mda = value;
				}
			}

			MdaWrapper()
			{
			}

			MdaWrapper(ICorDebugMDA* mda)
			{
				MdaWrapper();
				Mda = mda;
			}

		public:

			String^ GetName();

			String^ GetDescription();

			String^ GetXml();

			UInt32 GetFlags();

			UInt32 GetOSThreadID();
		};
	}
}