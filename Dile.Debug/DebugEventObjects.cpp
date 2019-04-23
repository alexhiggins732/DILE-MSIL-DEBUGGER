#include "stdafx.h"
#include "DebugEventObjects.h"

using namespace System::Threading;

namespace Dile
{
	namespace Debug
	{
		void DebugEventObjects::Reset()
		{
			Assembly = nullptr;
			AppDomain = nullptr;
			Breakpoint = nullptr;
			ClassObject = nullptr;
			Controller = nullptr;
			Eval = nullptr;
			Frame = nullptr;
			Function = nullptr;
			Mda = nullptr;
			Module = nullptr;
			NewFunction = nullptr;
			Process = nullptr;
			Stepper = nullptr;
			Thread = nullptr;
			Accurate = Nullable<bool>();
			ErrorCode = Nullable<UInt32>();
			ErrorHResult = Nullable<UInt32>();
			LogLevel = Nullable<int>();
			Message = nullptr;
			LogParentName = nullptr;
			LogReason = Nullable<UInt32>();
			StepReason = Nullable<UInt32>();
			LogSwitchName = nullptr;
			IsUnhandledException = Nullable<bool>();
			Offset = Nullable<UInt32>();
			ConnectionID = Nullable<UInt32>();
			ConnectionName = nullptr;
			EventType = Nullable<UInt32>();
			Flags = Nullable<UInt32>();
		}
	}
}