#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "FunctionWrapper.h"
#include "FrameWrapper.h"
#include "StepperWrapper.h"
#include "ValueWrapper.h"
#include "Constants.h"
#include "ThreadWrapper2.h"
#include "ThreadWrapper3.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;

namespace Dile
{
	namespace Debug
	{
		ref class AppDomainWrapper;
		ref class EvalWrapper;
		ref class ProcessWrapper;

		public ref class ThreadWrapper : BaseWrapper
		{
		private:
			ICorDebugThread* thread;
			ThreadWrapper2^ thread2;
			ThreadWrapper3^ thread3;
			bool^ isVersion2;
			bool^ isVersion3;

		internal:
			property ICorDebugThread* Thread
			{
				ICorDebugThread* get()
				{
					return thread;
				}

				void set(ICorDebugThread* value)
				{
					thread = value;
				}
			}

			ThreadWrapper()
			{
				isVersion2 = nullptr;
				isVersion3 = nullptr;
			}

			ThreadWrapper(ICorDebugThread* thread)
			{
				ThreadWrapper();

				Thread = thread;
			}

		public:
			property ThreadWrapper2^ Version2
			{
				ThreadWrapper2^ get()
				{
					if (thread2 == nullptr)
					{
						ICorDebugThread2* thread2Pointer;

						if (SUCCEEDED(thread->QueryInterface(&thread2Pointer)))
						{
							thread2 = gcnew ThreadWrapper2(thread2Pointer);
						}
					}

					return thread2;
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

			property ThreadWrapper3^ Version3
			{
				ThreadWrapper3^ get()
				{
					if (thread3 == nullptr)
					{
						ICorDebugThread3* thread3Pointer;

						if (SUCCEEDED(thread->QueryInterface(&thread3Pointer)))
						{
							thread3 = gcnew ThreadWrapper3(thread3Pointer);
						}
					}

					return thread3;
				}
			}

			property bool IsVersion3
			{
				virtual bool get()
				{
					bool result = false;

					if (isVersion3 == nullptr)
					{
						isVersion3 = (Version3 != nullptr);
						result = *isVersion3;
					}
					else
					{
						result = *isVersion3;
					}

					return result;
				}
			}

			FrameWrapper^ GetActiveFrame();

			UInt32 GetID();

			List<FrameWrapper^>^ GetCallStack();

			StepperWrapper^ CreateStepper();

			ValueWrapper^ GetCurrentException();

			Int32 CallFunction(FunctionWrapper^ function, List<ValueWrapper^>^ arguments, interior_ptr<EvalWrapper^> evalWrapper);

			FrameWrapper^ FindFrame(int chainIndex, int frameIndex);

			void ClearCurrentException();

			void InterceptCurrentException();

			ProcessWrapper^ GetProcess();

			ValueWrapper^ GetObject();

			void EnumerateModules(String^ moduleName, bool verifyExtension, ICorDebugAssembly* assembly, List<ModuleWrapper^>^ foundModules);

			void EnumerateAssemblies(String^ moduleName, bool verifyExtension, ICorDebugAppDomain* appDomain, List<ModuleWrapper^>^ foundModules);

			List<ModuleWrapper^>^ FindModulesByName(String^ moduleName);

			List<ModuleWrapper^>^ FindModulesByNameWithoutExtension(String^ moduleName);

			UInt32 GetDebugState();

			void SetDebugState(UInt32 debugState);

			AppDomainWrapper^ GetAppDomain();

			EvalWrapper^ CreateEval();
		};
	}
}