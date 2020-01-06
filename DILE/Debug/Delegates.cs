using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;

namespace Dile.Debug
{
	public delegate void ModuleLoadingDelegate(ModuleWrapper module);
	public delegate void ProcessExitedDelegate();
	public delegate void EvaluationCompleteDelegate(EvalWrapper evalWrapper);
	public delegate void DebuggerStateChanged(DebuggerState oldState, DebuggerState newState);
	public delegate void BreakpointSetErrorDelegate(MethodDefinition methodDefinition, uint offset);
	public delegate void InvalidateDebugInformationDelegate();
	public delegate void UpdateDebugInformationDelegate(FrameRefresher activeFrameRefresher, FrameWrapper activeFrame);
	public delegate void ActiveThreadChangedDelegate(ThreadWrapper newActiveThread);
	public delegate void ActiveFrameChangedDelegate(FrameRefresher newActiveFrameRefresher, FrameWrapper newActiveFrame);
}