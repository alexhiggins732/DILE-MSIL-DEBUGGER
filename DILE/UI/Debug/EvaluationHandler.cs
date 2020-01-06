using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Debug;
using Dile.Disassemble;
using System.Runtime.InteropServices;
using System.Threading;

namespace Dile.UI.Debug
{
	public class EvaluationHandler
	{
		private FrameWrapper frame;
		public FrameWrapper Frame
		{
			get
			{
				if (frame == null)
				{
					frame = FrameRefresher.GetRefreshedValue();
				}

				return frame;
			}
			private set
			{
				frame = value;
			}
		}

		private FrameRefresher frameRefresher;
		private FrameRefresher FrameRefresher
		{
			get
			{
				return frameRefresher;
			}
			set
			{
				frameRefresher = value;
			}
		}

		private AutoResetEvent autoResetEvent;
		private AutoResetEvent AutoResetEvent
		{
			get
			{
				return autoResetEvent;
			}
			set
			{
				autoResetEvent = value;
			}
		}

		private EvaluationCompleteDelegate evaluationCompleteEventHandler;
		private EvaluationCompleteDelegate EvaluationCompleteEventHandler
		{
			get
			{
				return evaluationCompleteEventHandler;
			}
			set
			{
				evaluationCompleteEventHandler = value;
			}
		}

		private DebuggerState originalDebuggerState;
		private DebuggerState OriginalDebuggerState
		{
			get
			{
				return originalDebuggerState;
			}
			set
			{
				originalDebuggerState = value;
			}
		}

		private ValueWrapper evaluationResult;
		private ValueWrapper EvaluationResult
		{
			get
			{
				return evaluationResult;
			}
			set
			{
				evaluationResult = value;
			}
		}

		private bool isEvalAborted;
		private bool IsEvalAborted
		{
			get
			{
				return isEvalAborted;
			}
			set
			{
				isEvalAborted = value;
			}
		}

		public EvaluationHandler(FrameRefresher frameRefresher)
		{
			FrameRefresher = frameRefresher;

			EvaluationCompleteEventHandler = new EvaluationCompleteDelegate(EvaluationComplete);
		}

		public BaseEvaluationResult CallMethod(EvaluationContext context, MethodDefinition methodToCall, List<ValueWrapper> arguments)
		{
			return CallMethod(context, methodToCall, arguments, null);
		}

		public BaseEvaluationResult CallMethod(EvaluationContext context, MethodDefinition methodToCall, List<ValueWrapper> arguments, List<TypeWrapper> typeArguments)
		{
			BaseEvaluationResult result = null;

			if (methodToCall == null)
			{
				throw new ArgumentNullException("methodToCall");
			}

			ModuleWrapper module = HelperFunctions.FindModuleOfType(context, methodToCall.BaseTypeDefinition);
			FunctionWrapper functionToCall = module.GetFunction(methodToCall.Token);
			result = context.EvaluationHandler.CallFunction(functionToCall, arguments, typeArguments);

			return result;
		}

		public BaseEvaluationResult CallFunction(FunctionWrapper functionToCall, List<ValueWrapper> arguments)
		{
			return CallFunction(functionToCall, arguments, null);
		}

		public BaseEvaluationResult CallFunction(FunctionWrapper functionToCall, List<ValueWrapper> arguments, List<TypeWrapper> typeArguments)
		{
			BaseEvaluationResult result = null;

			try
			{
				result = InternalCallFunction(functionToCall, arguments, typeArguments);
			}
			catch (Exception exception)
			{
				UnregisterEvaluationCompleteHandler();
				result = new BaseEvaluationResult();
				result.Reason = EvaluationFinishedReason.Failed;
				result.Exception = exception;

				string errorMessage = "The following exception occurred while trying to evaluate a method call: " + exception.Message;
				UIHandler.Instance.DisplayUserWarning(errorMessage);
				UIHandler.Instance.DisplayLogMessage(errorMessage);
			}

			return result;
		}

		public BaseEvaluationResult InternalCallFunction(FunctionWrapper functionToCall, List<ValueWrapper> arguments, List<TypeWrapper> typeArguments)
		{
			IsEvalAborted = false;
			EvaluationResult = null;

			DebugEventHandler.Instance.EvaluationComplete += EvaluationCompleteEventHandler;
			OriginalDebuggerState = DebugEventHandler.Instance.State;

			if (OriginalDebuggerState == DebuggerState.DebuggeeRunning)
			{
				OriginalDebuggerState = DebuggerState.DebuggeeSuspended;
			}

			DebugEventHandler.Instance.State = DebuggerState.EvaluatingExpression;
			DebugEventHandler.Instance.ResetModulesLoadedDuringEval();

			if (AutoResetEvent == null)
			{
				AutoResetEvent = new AutoResetEvent(false);
			}
			else
			{
				AutoResetEvent.Reset();
			}

			ThreadWrapper thread = DebugEventHandler.Instance.EventObjects.Thread;

			if (thread == null)
			{
				ChainWrapper chain = Frame.GetChain();
				thread = chain.GetThread();
			}

			ProcessWrapper process = DebugEventHandler.Instance.EventObjects.Process;

			if (process == null)
			{
				process = thread.GetProcess();
			}

			if ((CorDebugThreadState)thread.GetDebugState() == CorDebugThreadState.Suspend)
			{
				thread.SetDebugState((uint)CorDebugThreadState.Run);
			}

			DeactivateSteppers(process, thread);

			EvalWrapper eval = null;
			BaseEvaluationResult result = new BaseEvaluationResult();
			int hResult;

			if (typeArguments == null)
			{
				hResult = thread.CallFunction(functionToCall, arguments, ref eval);
			}
			else
			{
				if (thread.IsVersion2)
				{
					hResult = thread.Version2.CallParameterizedFunction(functionToCall, typeArguments, arguments, ref eval);
				}
				else
				{
					throw new NotSupportedException("Type arguments are specified but the thread interface is v1.");
				}
			}

			if (hResult == 0)
			{
				process.Continue();

				if (!AutoResetEvent.WaitOne(Settings.Instance.FuncEvalTimeout * 1000, false))
				{
					try
					{
						IsEvalAborted = true;
						eval.Abort();

						if (!AutoResetEvent.WaitOne(Settings.Instance.FuncEvalAbortTimeout * 1000, false))
						{
							process.Pause();

							UnregisterEvaluationCompleteHandler();
							result.Reason = EvaluationFinishedReason.AbortTimeout;
						}
						else
						{
							UnregisterEvaluationCompleteHandler();

							result.Result = EvaluationResult;
							result.Reason = EvaluationFinishedReason.AbortSucceeded;
						}
					}
					catch (Exception exception)
					{
						UnregisterEvaluationCompleteHandler();
						result.Reason = EvaluationFinishedReason.AbortFailed;
						result.Exception = exception;

						string errorMessage = "The following exception occurred while trying to abort a method call: " + exception.Message;
						UIHandler.Instance.DisplayUserWarning(errorMessage);
						UIHandler.Instance.DisplayLogMessage(errorMessage);
					}
				}
				else
				{
					result.Result = EvaluationResult;
					result.Reason = EvaluationFinishedReason.Succeeded;
				}
			}
			else
			{
				UnregisterEvaluationCompleteHandler();
				result.Reason = EvaluationFinishedReason.Failed;
				result.Exception = Marshal.GetExceptionForHR(hResult);
			}

			return result;
		}

		private static void DeactivateSteppers(ProcessWrapper process, ThreadWrapper thread)
		{
			foreach (ThreadWrapper debuggeeThread in process.EnumerateThreads())
			{
				try
				{
					AppDomainWrapper appDomain = debuggeeThread.GetAppDomain();
					appDomain.SetAllThreadsDebugState(true, thread);

					foreach (StepperWrapper stepper in appDomain.EnumerateSteppers())
					{
						if (stepper.IsActive())
						{
							stepper.Deactivate();
						}
					}
				}
				catch (Exception exception)
				{
					string errorMessage = "The following exception occurred while trying to disable code steppers in a thread: " + exception.Message;
					UIHandler.Instance.ShowException(exception);
				}
			}
		}

		public BaseEvaluationResult RunEvaluation(EvalWrapper evalWrapper)
		{
			IsEvalAborted = false;
			EvaluationResult = null;

			DebugEventHandler.Instance.EvaluationComplete += EvaluationCompleteEventHandler;
			OriginalDebuggerState = DebugEventHandler.Instance.State;

			if (OriginalDebuggerState == DebuggerState.DebuggeeRunning)
			{
				OriginalDebuggerState = DebuggerState.DebuggeeSuspended;
			}

			DebugEventHandler.Instance.State = DebuggerState.EvaluatingExpression;
			DebugEventHandler.Instance.ResetModulesLoadedDuringEval();

			if (AutoResetEvent == null)
			{
				AutoResetEvent = new AutoResetEvent(false);
			}
			else
			{
				AutoResetEvent.Reset();
			}

			ThreadWrapper thread = DebugEventHandler.Instance.EventObjects.Thread;

			if (thread == null)
			{
				ChainWrapper chain = Frame.GetChain();
				thread = chain.GetThread();
			}

			ProcessWrapper process = DebugEventHandler.Instance.EventObjects.Process;

			if (process == null)
			{
				process = thread.GetProcess();
			}

			DeactivateSteppers(process, thread);

			if ((CorDebugThreadState)thread.GetDebugState() == CorDebugThreadState.Suspend)
			{
				thread.SetDebugState((uint)CorDebugThreadState.Run);
			}

			process.Continue();
			BaseEvaluationResult result = new BaseEvaluationResult();

			if (!AutoResetEvent.WaitOne(Settings.Instance.FuncEvalTimeout * 1000, false))
			{
				try
				{
					IsEvalAborted = true;
					evalWrapper.Abort();

					if (!AutoResetEvent.WaitOne(Settings.Instance.FuncEvalAbortTimeout * 1000, false))
					{
						process.Pause();

						UnregisterEvaluationCompleteHandler();
						result.Reason = EvaluationFinishedReason.AbortTimeout;
					}
					else
					{
						UnregisterEvaluationCompleteHandler();

						result.Result = EvaluationResult;
						result.Reason = EvaluationFinishedReason.AbortSucceeded;
					}
				}
				catch (Exception exception)
				{
					UnregisterEvaluationCompleteHandler();
					result.Reason = EvaluationFinishedReason.AbortFailed;
					result.Exception = exception;

					string errorMessage = "The following exception occurred while trying to run an evaluation: " + exception.Message;

					UIHandler.Instance.DisplayUserWarning(errorMessage);
					UIHandler.Instance.DisplayLogMessage(errorMessage);
				}
			}
			else
			{
				result.Result = EvaluationResult;
				result.Reason = EvaluationFinishedReason.Succeeded;
			}

			return result;
		}

		private void UnregisterEvaluationCompleteHandler()
		{
			DebugEventHandler.Instance.State = OriginalDebuggerState;
			DebugEventHandler.Instance.EvaluationComplete -= EvaluationCompleteEventHandler;
		}

		private void EvaluationComplete(EvalWrapper evalWrapper)
		{
			UnregisterEvaluationCompleteHandler();
			string errorMessage = string.Empty;

			if (!IsEvalAborted)
			{
				try
				{
					EvaluationResult = evalWrapper.GetResult();
				}
				catch (Exception exception)
				{
					errorMessage = exception.Message;
				}
			}

			DebugEventHandler.Instance.EventObjects.Frame = FrameRefresher.GetRefreshedValue();
			Frame = null;

			AutoResetEvent.Set();

			ModuleWrapper[] loadedModules = DebugEventHandler.Instance.GetModulesLoadedDuringEval();

			if (loadedModules.Length > 0)
			{
				UIHandler.Instance.AddModulesToPanel(loadedModules);
			}

			if (!string.IsNullOrEmpty(errorMessage))
			{
				errorMessage = "An error occurred while trying to get the result of an evaluation: " + errorMessage;
				UIHandler.Instance.DisplayUserWarning(errorMessage);
				UIHandler.Instance.DisplayLogMessage(errorMessage);
			}
		}
	}
}