using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Debug
{
	public enum CorDebugStepReason
	{
		Normal = 0,
		Return = Normal + 1,
		Call = Return + 1,
		ExceptionFilter = Call + 1,
		ExceptionHandler = ExceptionFilter + 1,
		Intercept = ExceptionHandler + 1,
		Exit = Intercept + 1
	}

	public enum CorDebugThreadState : uint
	{
		Run = 0,
		Suspend = Run + 1
	}

	public enum StepType
	{
		StepIn,
		StepOver,
		StepOut
	}

	public enum DebuggerState
	{
		DebuggeeStopped,
		DebuggeeRunning,
		DebuggeePaused,
		DebuggeeSuspended,
		EvaluatingExpression,
		DebuggeeThrewException,
		AbortingEvalsForDetach,
		DumpDebugging
	}

	public enum SuspendableDebugEvent
	{
		None = 0,
		Break = 1,
		BreakpointSetError = 2,
		ControlCTrap = 4,
		CreateThread = 32,
		DebuggerError = 64,
		ExitThread = 512,
		LoadAssembly = 1024,
		LoadClass = 2048,
		LoadModule = 4096,
		LogMessage = 8192,
		LogSwitch = 16384,
		NameChange = 32768,
		UnloadAssembly = 65536,
		UnloadClass = 131072,
		UnloadModule = 262144
	}

	public enum ProjectStartMode
	{
		StartAssembly = 0,
		StartProgram = 1,
		StartBrowser = 2
	}

	public enum JitCompilerFlags
	{
		CORDEBUG_JIT_DEFAULT = 1,
		CORDEBUG_JIT_DISABLE_OPTIMIZATION = 3,
		CORDEBUG_JIT_ENABLE_ENC = 7
	}
}