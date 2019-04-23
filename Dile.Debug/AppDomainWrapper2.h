#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "TypeWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class AppDomainWrapper2 : BaseWrapper
		{
		private:
			ICorDebugAppDomain2* appDomain2;

		internal:
			property ICorDebugAppDomain2* AppDomain2
			{
				ICorDebugAppDomain2* get()
				{
					return appDomain2;
				}

				void set (ICorDebugAppDomain2* value)
				{
					appDomain2 = value;
				}
			}

			AppDomainWrapper2()
			{
			}

			AppDomainWrapper2(ICorDebugAppDomain2* appDomain2)
			{
				AppDomainWrapper2();

				AppDomain2 = appDomain2;
			}

		public:
			TypeWrapper^ GetArrayOrPointerType(int elementType, ULONG32 rank, TypeWrapper^ typeArgument);
		};
	}
}