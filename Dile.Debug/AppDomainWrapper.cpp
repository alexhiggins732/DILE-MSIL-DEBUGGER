#include "stdafx.h"
#include "AppDomainWrapper.h"
#include "ProcessWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		void AppDomainWrapper::Attach()
		{
			CheckHResult(AppDomain->Attach());
		}

		UInt32 AppDomainWrapper::GetID()
		{
			ULONG32 id;
			CheckHResult(AppDomain->GetID(&id));

			return id;
		}

		String^ AppDomainWrapper::GetName()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = AppDomain->GetName(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = AppDomain->GetName(expectedSize, &expectedSize, name);

				if (FAILED(hResult))
				{
					delete [] name;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(name);

			delete [] name;

			return result;
		}

		ProcessWrapper^ AppDomainWrapper::GetProcess()
		{
			ICorDebugProcess* process;
			CheckHResult(AppDomain->GetProcess(&process));

			ProcessWrapper^ result = gcnew ProcessWrapper();
			result->Process = process;

			return result;
		}

		void AppDomainWrapper::DeactivateSteppers()
		{
			ICorDebugStepperEnum* stepperEnum;
			CheckHResult(AppDomain->EnumerateSteppers(&stepperEnum));

			ULONG fetched;
			ICorDebugStepper *steppers[DefaultArrayCount];
			HRESULT hResult = stepperEnum->Next(DefaultArrayCount, steppers, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugStepper* stepper = steppers[index];
						stepper->Deactivate();
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(stepperEnum->Next(DefaultArrayCount, steppers, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}
		}

		void AppDomainWrapper::ActivateBreakpoints(bool active)
		{
			BOOL setActive = (active ? TRUE : FALSE);
			ICorDebugBreakpointEnum* breakpointEnum;
			CheckHResult(AppDomain->EnumerateBreakpoints(&breakpointEnum));

			ULONG fetched;
			ICorDebugBreakpoint *breakpoints[DefaultArrayCount];
			HRESULT hResult = breakpointEnum->Next(DefaultArrayCount, breakpoints, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugBreakpoint* breakpoint = breakpoints[index];
						breakpoint->Activate(setActive);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(breakpointEnum->Next(DefaultArrayCount, breakpoints, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}
		}

		void AppDomainWrapper::Detach()
		{
			DeactivateSteppers();
			ActivateBreakpoints(false);
			ControllerWrapper::Detach();
		}

		List<StepperWrapper^>^ AppDomainWrapper::EnumerateSteppers()
		{
			List<StepperWrapper^>^ result = nullptr;
			ICorDebugStepperEnum* stepperEnum;
			CheckHResult(AppDomain->EnumerateSteppers(&stepperEnum));

			ULONG fetched;
			ICorDebugStepper* steppers[DefaultArrayCount];
			CheckHResult(stepperEnum->Next(DefaultArrayCount, steppers, &fetched));

			result = gcnew List<StepperWrapper^>();

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugStepper* stepper = steppers[index];
					StepperWrapper^ stepperWrapper = gcnew StepperWrapper();
					stepperWrapper->Stepper = stepper;

					result->Add(stepperWrapper);
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(stepperEnum->Next(DefaultArrayCount, steppers, &fetched));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}

			return result;
		}
	}
}