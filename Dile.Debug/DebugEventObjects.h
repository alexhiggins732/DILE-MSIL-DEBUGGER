#pragma once

#include "stdafx.h"
#include "AssemblyWrapper.h"
#include "AppDomainWrapper.h"
#include "BreakpointWrapper.h"
#include "ClassWrapper.h"
#include "ControllerWrapper.h"
#include "EvalWrapper.h"
#include "FrameWrapper.h"
#include "FunctionWrapper.h"
#include "MdaWrapper.h"
#include "ModuleWrapper.h"
#include "ProcessWrapper.h"
#include "StepperWrapper.h"
#include "ThreadWrapper.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		public ref class DebugEventObjects
		{
		private:
			AssemblyWrapper^ assembly;
			AppDomainWrapper^ appDomain;
			BreakpointWrapper^ breakpoint;
			ClassWrapper^ classObject;
			ControllerWrapper^ controller;
			EvalWrapper^ eval;
			FrameWrapper^ frame;
			FunctionWrapper^ function;
			MdaWrapper^ mda;
			ModuleWrapper^ module;
			FunctionWrapper^ newFunction;
			ProcessWrapper^ process;
			StepperWrapper^ stepper;
			ThreadWrapper^ thread;
			Nullable<bool> accurate;
			Nullable<UInt32> errorCode;
			Nullable<UInt32> errorHResult;
			Nullable<int> logLevel;
			String^ message;
			String^ logParentName;
			Nullable<UInt32> logReason;
			Nullable<UInt32> stepReason;
			String^ logSwitchName;
			Nullable<bool> isUnhandledException;
			Nullable<UInt32> offset;
			Nullable<UInt32> connectionID;
			String^ connectionName;
			Nullable<UInt32> eventType;
			Nullable<UInt32> flags;
			//IStream* pSymbolStream

		public:
			property AssemblyWrapper^ Assembly
			{
				AssemblyWrapper^ get()
				{
					return assembly;
				}
				void set(AssemblyWrapper^ value)
				{
					assembly = value;
				}
			}

			property AppDomainWrapper^ AppDomain
			{
				AppDomainWrapper^ get()
				{
					return appDomain;
				}
				void set(AppDomainWrapper^ value)
				{
					appDomain = value;

					if (Controller == nullptr)
					{
						Controller = appDomain;
					}
				}
			}

			property BreakpointWrapper^ Breakpoint
			{
				BreakpointWrapper^ get()
				{
					return breakpoint;
				}
				void set(BreakpointWrapper^ value)
				{
					breakpoint = value;
				}
			}

			property ClassWrapper^ ClassObject
			{
				ClassWrapper^ get()
				{
					return classObject;
				}
				void set(ClassWrapper^ value)
				{
					classObject = value;
				}
			}

			property ControllerWrapper^ Controller
			{
				ControllerWrapper^ get()
				{
					return controller;
				}
				void set(ControllerWrapper^ value)
				{
					controller = value;
				}
			}

			property EvalWrapper^ Eval
			{
				EvalWrapper^ get()
				{
					return eval;
				}
				void set(EvalWrapper^ value)
				{
					eval = value;
				}
			}

			property FrameWrapper^ Frame
			{
				FrameWrapper^ get()
				{
					return frame;
				}
				void set(FrameWrapper^ value)
				{
					frame = value;
				}
			}

			property FunctionWrapper^ Function
			{
				FunctionWrapper^ get()
				{
					return function;
				}
				void set(FunctionWrapper^ value)
				{
					function = value;
				}
			}

			property MdaWrapper^ Mda
			{
				MdaWrapper^ get()
				{
					return mda;
				}
				void set(MdaWrapper^ value)
				{
					mda = value;
				}
			}

			property ModuleWrapper^ Module
			{
				ModuleWrapper^ get()
				{
					return module;
				}
				void set(ModuleWrapper^ value)
				{
					module = value;
				}
			}

			property FunctionWrapper^ NewFunction
			{
				FunctionWrapper^ get()
				{
					return newFunction;
				}
				void set(FunctionWrapper^ value)
				{
					newFunction = value;
				}
			}

			property ProcessWrapper^ Process
			{
				ProcessWrapper^ get()
				{
					return process;
				}
				void set(ProcessWrapper^ value)
				{
					process = value;
					Controller = process;
				}
			}

			property StepperWrapper^ Stepper
			{
				StepperWrapper^ get()
				{
					return stepper;
				}
				void set(StepperWrapper^ value)
				{
					stepper = value;
				}
			}

			property ThreadWrapper^ Thread
			{
				ThreadWrapper^ get()
				{
					return thread;
				}
				void set(ThreadWrapper^ value)
				{
					thread = value;
				}
			}

			property Nullable<bool> Accurate
			{
				Nullable<bool> get()
				{
					return accurate;
				}
				void set(Nullable<bool> value)
				{
					accurate = value;
				}
			}

			property Nullable<UInt32> ErrorCode
			{
				Nullable<UInt32> get()
				{
					return errorCode;
				}
				void set(Nullable<UInt32> value)
				{
					errorCode = value;
				}
			}

			property Nullable<UInt32> ErrorHResult
			{
				Nullable<UInt32> get()
				{
					return errorHResult;
				}
				void set(Nullable<UInt32> value)
				{
					errorHResult = value;
				}
			}

			property Nullable<int> LogLevel
			{
				Nullable<int> get()
				{
					return logLevel;
				}
				void set(Nullable<int> value)
				{
					logLevel = value;
				}
			}

			property String^ Message
			{
				String^ get()
				{
					return message;
				}
				void set(String^ value)
				{
					message = value;
				}
			}

			property String^ LogParentName
			{
				String^ get()
				{
					return logParentName;
				}
				void set(String^ value)
				{
					logParentName = value;
				}
			}

			property Nullable<UInt32> LogReason
			{
				Nullable<UInt32> get()
				{
					return logReason;
				}
				void set(Nullable<UInt32> value)
				{
					logReason = value;
				}
			}

			property Nullable<UInt32> StepReason
			{
				Nullable<UInt32> get()
				{
					return stepReason;
				}
				void set(Nullable<UInt32> value)
				{
					stepReason = value;
				}
			}

			property String^ LogSwitchName
			{
				String^ get()
				{
					return logSwitchName;
				}
				void set(String^ value)
				{
					logSwitchName = value;
				}
			}

			property Nullable<bool> IsUnhandledException
			{
				Nullable<bool> get()
				{
					return isUnhandledException;
				}
				void set(Nullable<bool> value)
				{
					isUnhandledException = value;
				}
			}

			property Nullable<UInt32> Offset
			{
				Nullable<UInt32> get()
				{
					return offset;
				}
				void set(Nullable<UInt32> value)
				{
					offset = value;
				}
			}

			property Nullable<UInt32> ConnectionID
			{
				Nullable<UInt32> get()
				{
					return connectionID;
				}
				void set(Nullable<UInt32> value)
				{
					connectionID = value;
				}
			}

			property String^ ConnectionName
			{
				String^ get()
				{
					return connectionName;
				}
				void set(String^ value)
				{
					connectionName = value;
				}
			}

			property Nullable<UInt32> EventType
			{
				Nullable<UInt32> get()
				{
					return eventType;
				}
				void set(Nullable<UInt32> value)
				{
					eventType = value;
				}
			}

			property Nullable<UInt32> Flags
			{
				Nullable<UInt32> get()
				{
					return flags;
				}
				void set(Nullable<UInt32> value)
				{
					flags = value;
				}
			}

			void Reset();
		};
	}
}