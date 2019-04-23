#include "stdafx.h"
#include "StepperWrapper.h"

namespace Dile
{
	namespace Debug
	{
		void StepperWrapper::Step(bool stepInto)
		{
			CheckHResult(Stepper->SetInterceptMask(INTERCEPT_ALL));
			Stepper->SetUnmappedStopMask(STOP_ALL);
			CheckHResult(Stepper->Step((stepInto ? TRUE : FALSE)));
		}

		void StepperWrapper::StepOut()
		{
			CheckHResult(Stepper->SetInterceptMask(INTERCEPT_ALL));
			Stepper->SetUnmappedStopMask(STOP_ALL);
			CheckHResult(Stepper->StepOut());
		}

		bool StepperWrapper::IsActive()
		{
			BOOL active;
			CheckHResult(Stepper->IsActive(&active));

			return (active == TRUE ? true : false);
		}

		void StepperWrapper::Deactivate()
		{
			CheckHResult(Stepper->Deactivate());
		}
	}
}