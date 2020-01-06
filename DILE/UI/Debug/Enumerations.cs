using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public enum BreakpointState
	{
		Active,
		Inactive,
		Removed
	}

	[Flags()]
	public enum DebugEventType : long
	{
		None = 0,
		Break = 0x1,
		Breakpoint = 0x2,
		BreakpointSetError = 0x4,
		ChangeConnection = 0x8,
		ControlCTrap = 0x10,
		CreateAppDomain = 0x20,
		CreateConnection = 0x40,
		CreateProcess = 0x80,
		CreateThread = 0x100,
		DebuggerError = 0x200,
		DestroyConnection = 0x400,
		EditAndContinueRemap = 0x800,
		EvalComplete = 0x1000,
		EvalException = 0x2000,
		Exception = 0x4000,
		ExceptionUnwind = 0x8000,
		ExitAppDomain = 0x10000,
		ExitProcess = 0x20000,
		ExitThread = 0x40000,
		FunctionRemapComplete = 0x80000,
		FunctionRemapOpportunity = 0x100000,
		LoadAssembly = 0x200000,
		LoadClass = 0x400000,
		LoadModule = 0x800000,
		LogMessage = 0x1000000,
		LogSwitch = 0x2000000,
		MdaNotificiation = 0x4000000,
		NameChange = 0x8000000,
		StepComplete = 0x10000000,
		UnloadAssembly = 0x20000000,
		UnloadClass = 0x40000000,
		UnloadModule = 0x80000000,
		UpdateModuleSymbols = 0x100000000,
		AllSet = 0x1FFFFFFFF
	}

	public enum ValueDisplayerState
	{
		Initialize,
		StartThread,
		CollectTypeInformation,
		EvaluateArrayElements,
		EvaluateStringValue,
		EvaluateFields,
		EvaluateProperties,
		EvaluateToString,
		Finish,
		Interrupted,
		MethodCallAbortFailed
	}

	public enum EvaluationFinishedReason
	{
		Succeeded,
		Failed,
		AbortSucceeded,
		AbortFailed,
		AbortTimeout
	}

	public enum ValueDisplayerCancelReason
	{
		None,
		Interrupted,
		MethodCallAbortFailed
	}
}