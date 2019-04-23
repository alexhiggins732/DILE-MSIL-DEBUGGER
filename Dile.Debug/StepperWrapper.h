#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

namespace Dile
{
	namespace Debug
	{
		public ref class StepperWrapper : BaseWrapper
		{
		private:
			ICorDebugStepper* stepper;

		internal:
			property ICorDebugStepper* Stepper
			{
				ICorDebugStepper* get()
				{
					return stepper;
				}
				
				void set(ICorDebugStepper* value)
				{
					stepper = value;
				}
			}

			StepperWrapper()
			{
			}

			StepperWrapper(ICorDebugStepper* stepper)
			{
				StepperWrapper();

				Stepper = stepper;
			}

		public:
			void Step(bool stepInto);

			void StepOut();

			bool IsActive();

			void Deactivate();
		};
	}
}