#pragma once

#include "stdafx.h"
#include "ControllerWrapper.h"
#include "ThreadWrapper.h"
#include "AppDomainWrapper2.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class ProcessWrapper;

		public ref class AppDomainWrapper : ControllerWrapper
		{
		private:
			ICorDebugAppDomain* appDomain;
			AppDomainWrapper2^ appDomain2;
			bool^ isVersion2;

		internal:
			property ICorDebugAppDomain* AppDomain
			{
				ICorDebugAppDomain* get()
				{
					return appDomain;
				}

				void set (ICorDebugAppDomain* value)
				{
					appDomain = value;
					Controller = value;
				}
			}

			AppDomainWrapper()
			{
				isVersion2 = nullptr;
			}

			AppDomainWrapper(ICorDebugAppDomain* appDomain)
			{
				AppDomainWrapper();

				AppDomain = appDomain;
			}

		public:
			property AppDomainWrapper2^ Version2
			{
				AppDomainWrapper2^ get()
				{
					if (appDomain2 == nullptr)
					{
						ICorDebugAppDomain2* appDomain2Pointer;

						if (SUCCEEDED(appDomain->QueryInterface(&appDomain2Pointer)))
						{
							appDomain2 = gcnew AppDomainWrapper2(appDomain2Pointer);
						}
					}

					return appDomain2;
				}
			}

			property bool IsVersion2
			{
				virtual bool get()
				{
					bool result = false;

					if (isVersion2 == nullptr)
					{
						isVersion2 = (Version2 != nullptr);
						result = *isVersion2;
					}
					else
					{
						result = *isVersion2;
					}

					return result;
				}
			}

			void Attach();

			UInt32 GetID();

			String^ GetName();

			ProcessWrapper^ GetProcess();

			void DeactivateSteppers();

			void ActivateBreakpoints(bool active);

			void virtual Detach() override;

			List<StepperWrapper^>^ EnumerateSteppers();
		};
	}
}