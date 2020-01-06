using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Controls;
using Dile.Disassemble;
using Dile.Disassemble.ILCodes;
using Dile.UI;
using Dile.UI.Debug;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Dile.Debug
{
	public class DebugEventHandler : IDebugEventHandler
	{
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		public event ModuleLoadingDelegate ModuleLoading;
		public event ProcessExitedDelegate ProcessExited;
		public event EvaluationCompleteDelegate EvaluationComplete;
		public event DebuggerStateChanged StateChanged;
		public event BreakpointSetErrorDelegate BreakpointNotSetError;
		public event InvalidateDebugInformationDelegate InvalidateDebugInformation;
		public event ActiveThreadChangedDelegate ActiveThreadChanged;
		public event ActiveFrameChangedDelegate ActiveFrameChanged;

		private static DebugEventHandler instance = new DebugEventHandler();
		public static DebugEventHandler Instance
		{
			get
			{
				return instance;
			}
		}

		private Debugger debugger;
		public Debugger Debugger
		{
			get
			{
				return debugger;
			}
			set
			{
				if (debugger != null)
				{
					debugger.Dispose();
				}

				debugger = value;
			}
		}

		private uint debugeeProcessID = 0;
		public uint DebugeeProcessID
		{
			get
			{
				return debugeeProcessID;
			}
			set
			{
				debugeeProcessID = value;

				if (debugeeProcessID > 0)
				{
					DebugeeProcess = Process.GetProcessById(Convert.ToInt32(debugeeProcessID));
				}
				else
				{
					DebugeeProcess = null;
				}
			}
		}

		private Process debugeeProcess;
		public Process DebugeeProcess
		{
			get
			{
				return debugeeProcess;
			}
			private set
			{
				debugeeProcess = value;
			}
		}

		private StepType lastStepType = StepType.StepOver;
		public StepType LastStepType
		{
			get
			{
				return lastStepType;
			}
			set
			{
				lastStepType = value;
			}
		}

		private uint lastIP;
		public uint LastIP
		{
			get
			{
				return lastIP;
			}
			set
			{
				lastIP = value;
			}
		}

		private uint lastFunctionToken;
		public uint LastFunctionToken
		{
			get
			{
				return lastFunctionToken;
			}
			set
			{
				lastFunctionToken = value;
			}
		}

		private DebuggerState state = DebuggerState.DebuggeeStopped;
		public DebuggerState State
		{
			get
			{
				return state;
			}
			set
			{
				if (StateChanged != null)
				{
					StateChanged(state, value);
				}

				state = value;
			}
		}

		private DebuggerState lastState;
		private DebuggerState LastState
		{
			get
			{
				return lastState;
			}
			set
			{
				lastState = value;
			}
		}

		private System.Timers.Timer activateDebugeeTimer = new System.Timers.Timer();
		private System.Timers.Timer ActivateDebugeeTimer
		{
			get
			{
				return activateDebugeeTimer;
			}
			set
			{
				activateDebugeeTimer = value;
			}
		}

		private List<ModuleWrapper> loadedModulesDuringEval = new List<ModuleWrapper>();
		private List<ModuleWrapper> LoadedModulesDuringEval
		{
			get
			{
				return loadedModulesDuringEval;
			}
			set
			{
				loadedModulesDuringEval = value;
			}
		}

		private DebugEventObjects eventObjects;
		public DebugEventObjects EventObjects
		{
			get
			{
				if (eventObjects == null)
				{
					eventObjects = new DebugEventObjects();
				}

				return eventObjects;
			}
			private set
			{
				eventObjects = value;
			}
		}

		public bool AreEventObjectsAvailable
		{
			get
			{
				return (EventObjects != null);
			}
		}

		private ReaderWriterLock lockObject = new ReaderWriterLock();
		private ReaderWriterLock LockObject
		{
			get
			{
				return lockObject;
			}
		}

		private DebugEventHandler()
		{
			ActivateDebugeeTimer.AutoReset = false;
			ActivateDebugeeTimer.Interval = 500;
			ActivateDebugeeTimer.Elapsed += new ElapsedEventHandler(ActivateDebugeeTimer_Elapsed);
		}

		private void ActivateDebugeeTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			ActivateDebugeeTimer.Stop();
			ActivateDebugee();
		}

		private void TryDetach()
		{
			try
			{
				EventObjects.Process.Detach();
				EventObjects.Reset();
				DebugeeProcessID = 0;
				State = DebuggerState.DebuggeeStopped;
			}
			catch (COMException comException)
			{
				if ((uint)comException.ErrorCode == 0x80131C18)
				{
					ContinueProcess();
					State = DebuggerState.AbortingEvalsForDetach;
				}
				else
				{
					UIHandler.Instance.ShowMessageBox("Detach failed", string.Format("An unexpected error occurred while trying to detach:\n\nMessage: {0}", comException.Message));
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowMessageBox("Detach failed", string.Format("An unexpected error occurred while trying to detach:\n\tMessage: {0}", exception.Message));
			}
		}

		public void Detach()
		{
			if (State != DebuggerState.DebuggeeStopped && State != DebuggerState.DumpDebugging)
			{
				if (EventObjects.Controller != null)
				{
					EventObjects.Controller.SetAllThreadsDebugState(false, null);
				}

				RefreshAndSuspendProcess();
				TryDetach();

				if (State != DebuggerState.AbortingEvalsForDetach)
				{
					OnProcessExited();
				}
			}
		}

		public void ResumeProcess()
		{
			EventObjects.Process.Continue();
			State = LastState;
			EventObjects.Reset();
		}

		public void RefreshAndSuspendProcess()
		{
			if (EventObjects.Process == null)
			{
				EventObjects.Process = Debugger.GetProcess(DebugeeProcessID);
			}

			EventObjects.Process.Pause();
			LastState = State;
			State = DebuggerState.DebuggeePaused;
		}

		private void UpdateEventObjects(DebugEventObjects eventObjects)
		{
			EventObjects = eventObjects;

			if (DebugeeProcessID > 0 && EventObjects.Process == null)
			{
				EventObjects.Process = Debugger.GetProcess(DebugeeProcessID);
			}
		}

		private void ActivateDebugee()
		{
			if (DebugeeProcess != null && !DebugeeProcess.HasExited)
			{
				if (DebugeeProcess.MainWindowHandle == null || DebugeeProcess.MainWindowHandle == IntPtr.Zero)
				{
					DebugeeProcess.Refresh();
				}

				if (DebugeeProcess.MainWindowHandle != null && DebugeeProcess.MainWindowHandle != IntPtr.Zero)
				{
					SetForegroundWindow(DebugeeProcess.MainWindowHandle);
				}
			}
		}

		private void InternalContinueProcess()
		{
			InternalContinueProcess(SuspendableDebugEvent.None);
		}

		private void InternalContinueProcess(SuspendableDebugEvent debugEvent)
		{
			bool evaluatingExpression = (State == DebuggerState.EvaluatingExpression);

			if (debugEvent == SuspendableDebugEvent.LoadAssembly)
			{
				if (Project.Instance.SuspendOnDebugEvent(debugEvent) && !evaluatingExpression)
				{
					UIHandler.Instance.DisplayUserWarning(string.Format("The debuggee is paused because \"{0}\" event happened.", debugEvent));
					RefreshAndSuspendProcess();
					DisplayAllInformation(DebuggerState.DebuggeeSuspended);
				}
				else
				{
					EventObjects.Reset();
				}
			}
			else if (debugEvent == SuspendableDebugEvent.None || !Project.Instance.SuspendOnDebugEvent(debugEvent) || evaluatingExpression)
			{
				ContinueProcess(true);
			}
			else
			{
				UIHandler.Instance.DisplayUserWarning(string.Format("The debuggee is paused because \"{0}\" event happened.", debugEvent));
				DisplayAllInformation(DebuggerState.DebuggeeSuspended);
			}
		}

		public void OnInvalidateDebugInformation()
		{
			if (InvalidateDebugInformation != null)
			{
				InvalidateDebugInformation();
			}
		}

		public void ContinueProcess()
		{
			OnInvalidateDebugInformation();
			ContinueProcess(false);
		}

		private void ContinueProcess(bool internalCall)
		{
			bool paused = (State == DebuggerState.DebuggeePaused);

			if ((!internalCall || !paused) && EventObjects != null && EventObjects.Controller != null)
			{
				if (State != DebuggerState.EvaluatingExpression)
				{
					EventObjects.Controller.SetAllThreadsDebugState(false, null);

					if (State != DebuggerState.DebuggeeStopped && State != DebuggerState.DebuggeeRunning)
					{
						ActivateDebugeeTimer.Start();
					}

					State = DebuggerState.DebuggeeRunning;
				}

				ControllerWrapper controller = EventObjects.Controller;
				ProcessWrapper process = EventObjects.Process;
				EventObjects.Reset();

				if (!internalCall && paused && process != null)
				{
					foreach (AppDomainWrapper appDomain in process.GetAppDomains())
					{
						appDomain.Continue();
					}

					controller.Continue();
				}
				else
				{
					controller.Continue();
				}
			}
		}

		public bool Step(StepType stepType)
		{
			bool result = true;

			if (EventObjects.Stepper == null)
			{
				if (EventObjects.Thread == null)
				{
					result = false;
				}
				else
				{
					AppDomainWrapper appDomain = EventObjects.Thread.GetAppDomain();
					List<StepperWrapper> steppers = appDomain.EnumerateSteppers();

					if (steppers == null || steppers.Count == 0)
					{
						EventObjects.Stepper = EventObjects.Thread.CreateStepper();
					}
					else if (steppers.Count == 1)
					{
						EventObjects.Stepper = steppers[0];
					}
					else
					{
						throw new InvalidOperationException("More than 1 stepper exists are associated with the thread.");
					}
				}
			}

			if (EventObjects.Stepper != null)
			{
				LastStepType = stepType;

				switch (stepType)
				{
					case StepType.StepIn:
						EventObjects.Stepper.Step(true);
						break;

					case StepType.StepOver:
						EventObjects.Stepper.Step(false);
						break;

					case StepType.StepOut:
						EventObjects.Stepper.StepOut();
						break;
				}
			}

			if (result)
			{
				OnInvalidateDebugInformation();
			}

			return result;
		}

		public void ChangeThread(ThreadWrapper newActiveThread)
		{
			if (ActiveThreadChanged != null)
			{
				ActiveThreadChanged(newActiveThread);
			}
		}

		public void ChangeFrame(FrameRefresher newActiveFrameRefresher, FrameWrapper newActiveFrame)
		{
			if (ActiveFrameChanged != null)
			{
				ActiveFrameChanged(newActiveFrameRefresher, newActiveFrame);
			}
		}

		public void DisplayAllInformation(DebuggerState newState)
		{
			ActivateDebugeeTimer.Stop();

			FrameRefresher frameRefresher = null;
			FrameWrapper activeFrame = null;

			try
			{
				if (EventObjects.Thread != null)
				{
					activeFrame = (State == DebuggerState.DumpDebugging ? EventObjects.Thread.Version3.GetActiveFrame() : EventObjects.Thread.GetActiveFrame());

					if (activeFrame != null && activeFrame.IsILFrame())
					{
						frameRefresher = new FrameRefresher(EventObjects.Thread, activeFrame.ChainIndex, activeFrame.FrameIndex, true);
					}
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}

			EventObjects.Frame = activeFrame;

			try
			{
				DisplayCurrentCodeLocation(activeFrame);
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}

			/*
			 * Start a new thread to avoid the following deadlock problem:
			 * When the Watch panel is displayed and it has an expression that has to call a method the following problem will occur.
			 * The thread that handles the debug events (using this class) will call raise an UpdateDebugInformation event -> the Watch panel will try to update its values -> the Watch panel will a method evaluation -> the EvaluationHandler class will "block" the current thread to simulate synchronized method call -> the debuggee will be allowed to run the method call -> this class should handle the debugger events (but at this point it will be blocked because the EvaluationHandler has "suspended" it earlier).
			 */

			state = newState;

			UIHandler.Instance.RaiseUpdateDebugInformation(frameRefresher, activeFrame, newState);
		}

		public void DisplayCurrentCodeLocation()
		{
			DisplayCurrentCodeLocation(EventObjects.Frame);
		}

		private void DisplayCurrentCodeLocation(FrameWrapper activeFrame)
		{
			if (activeFrame != null && activeFrame.IsILFrame())
			{
				FunctionWrapper currentFunction = activeFrame.GetFunction();
				ModuleWrapper currentModule = currentFunction.GetModule();
				uint currentFunctionToken = currentFunction.GetToken();
				IMultiLine currentMethod = HelperFunctions.FindObjectByToken(currentFunctionToken, currentModule) as IMultiLine;

				if (currentMethod != null)
				{
					try
					{
						bool exactLocation = false;
						LastIP = activeFrame.GetIP(ref exactLocation);
						LastFunctionToken = activeFrame.GetFunctionToken();

						if ((Project.Instance.RunToCursorBreakpoint != null) && (Project.Instance.RunToCursorBreakpoint.MethodDefinition == currentMethod) && (Project.Instance.RunToCursorBreakpoint.Offset == LastIP))
						{
							Project.Instance.RunToCursorBreakpoint.Remove();
							Project.Instance.RunToCursorBreakpoint = null;
						}

						CodeObjectDisplayOptions displayOptions = new CodeObjectDisplayOptions(Convert.ToInt32(LastIP), exactLocation);
						UIHandler.Instance.ShowCodeObject(currentMethod, displayOptions);
					}
					catch (Exception exception)
					{
						UIHandler.Instance.ShowException(exception);
					}
				}
			}
		}

		private void DisplayOutputInformation(DebugEventType debugEventType)
		{
			DebugEventDescriptor eventDescriptor = new DebugEventDescriptor(debugEventType);
			UIHandler.Instance.DisplayOutputInformation(eventDescriptor);
		}

		public ModuleWrapper[] GetModulesLoadedDuringEval()
		{
			return LoadedModulesDuringEval.ToArray();
		}

		public void ResetModulesLoadedDuringEval()
		{
			LoadedModulesDuringEval.Clear();
		}

		#region IDebugEventHandler Members

		public void Break(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.Break);
			InternalContinueProcess(SuspendableDebugEvent.Break);
		}

		public void Breakpoint(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.Breakpoint);

			if (State != DebuggerState.EvaluatingExpression)
			{
				UIHandler.Instance.ClearDebugPanels();
				DisplayAllInformation(DebuggerState.DebuggeePaused);
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void BreakpointSetError(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.BreakpointSetError);

			FunctionBreakpointWrapper breakpoint = eventObjects.Breakpoint.TryConvertFunctionBreakpoint();

			string errorMessage = "A breakpoint could not be set at the given location.";

			if (eventObjects.ErrorCode != null && eventObjects.ErrorCode.HasValue)
			{
				errorMessage += " Error code: " + HelperFunctions.FormatAsHexNumber(eventObjects.ErrorCode.Value, 8);
			}

			UIHandler.Instance.DisplayUserWarning(errorMessage);

			if (breakpoint != null && BreakpointNotSetError != null)
			{
				FunctionWrapper function = breakpoint.GetFunction();
				ModuleWrapper module = function.GetModule();
				uint functionToken = function.GetToken();

				TokenBase tokenObject = HelperFunctions.FindObjectByToken(functionToken, module);
				MethodDefinition methodDefinition = tokenObject as MethodDefinition;
				uint offset = breakpoint.GetOffset();

				BreakpointNotSetError(methodDefinition, offset);
			}

			if (State != DebuggerState.EvaluatingExpression)
			{
				DisplayAllInformation(DebuggerState.DebuggeePaused);
			}
			else
			{
				InternalContinueProcess(SuspendableDebugEvent.BreakpointSetError);
			}
		}

		public void ChangeConnection(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ChangeConnection);
			InternalContinueProcess();
		}

		public void ControlCTrap(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ControlCTrap);
			InternalContinueProcess(SuspendableDebugEvent.ControlCTrap);
		}

		public void CreateAppDomain(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.CreateAppDomain);
			eventObjects.AppDomain.Attach();
			InternalContinueProcess();
		}

		public void CreateConnection(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.CreateConnection);
			InternalContinueProcess();
		}

		public void CreateProcessA(DebugEventObjects eventObjects)
		{
			if (DebugeeProcessID == 0)
			{
				DebugeeProcessID = eventObjects.Process.GetID();
			}

			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.CreateProcess);

			if (eventObjects.Process.IsVersion2)
			{
				eventObjects.Process.Version2.SetDesiredNGENCompilerFlags((uint)JitCompilerFlags.CORDEBUG_JIT_DISABLE_OPTIMIZATION);
			}

			InternalContinueProcess();
		}

		public void CreateThread(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.CreateThread);
			InternalContinueProcess(SuspendableDebugEvent.CreateThread);
		}

		public void DebuggerError(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.DebuggerError);
			InternalContinueProcess(SuspendableDebugEvent.DebuggerError);
		}

		public void DestroyConnection(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.DestroyConnection);
			InternalContinueProcess();
		}

		public void EditAndContinueRemap(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.EditAndContinueRemap);
			InternalContinueProcess();
		}

		public void EvalComplete(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.EvalComplete);

			if (State == DebuggerState.AbortingEvalsForDetach)
			{
				Detach();
			}
			else
			{
				eventObjects.Controller.SetAllThreadsDebugState(true, eventObjects.Thread);

				if (EvaluationComplete != null)
				{
					EvaluationComplete(eventObjects.Eval);
				}
			}
		}

		public void EvalException(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.EvalException);
			eventObjects.Thread.ClearCurrentException();
			eventObjects.Thread.InterceptCurrentException();

			if (EvaluationComplete != null)
			{
				eventObjects.Controller.SetAllThreadsDebugState(true, eventObjects.Thread);

				EvaluationComplete(eventObjects.Eval);
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void Exception(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.Exception);

			if (State == DebuggerState.EvaluatingExpression)
			{
				eventObjects.Controller.SetAllThreadsDebugState(true, eventObjects.Thread);
				//eventObjects.Thread.ClearCurrentException();
				//eventObjects.Thread.InterceptCurrentException();
				eventObjects.Controller.Continue();
			}
			else if (Settings.Instance.StopOnException && (!Settings.Instance.StopOnlyOnUnhandledException || (eventObjects.IsUnhandledException.HasValue && eventObjects.IsUnhandledException == true)))
			{
				eventObjects.Controller.SetAllThreadsDebugState(true, eventObjects.Thread);
				FrameWrapper activeFrame = eventObjects.Thread.GetActiveFrame();
				bool isActiveFrameIL = (activeFrame != null && activeFrame.IsILFrame());
				ValueWrapper exceptionObject = eventObjects.Thread.GetCurrentException();
				bool skipException = false;

				ValueWrapper dereferencedException = exceptionObject.DereferenceValue();

				if (dereferencedException != null)
				{
					ClassWrapper exceptionClass = dereferencedException.GetClassInformation();
					ModuleWrapper module = exceptionClass.GetModule();
					AssemblyWrapper assembly = module.GetAssembly();
					string assemblyPath = assembly.GetName();
					uint exceptionClassToken = exceptionClass.GetToken();
					bool exactLocation = false;
					uint? currentIP = null;
					uint throwingMethodToken = 0;

					if (isActiveFrameIL && !exceptionObject.IsNull())
					{
						currentIP = activeFrame.GetIP(ref exactLocation);
						throwingMethodToken = activeFrame.GetFunctionToken();
					}

					skipException = Project.Instance.SkipException(assemblyPath, exceptionClassToken, throwingMethodToken, currentIP);
				}

				if (skipException)
				{
					InternalContinueProcess();
				}
				else
				{
					UIHandler.Instance.ClearDebugPanels();
					UIHandler.Instance.DisplayUserWarning("Exception thrown by debuggee.");
					DisplayAllInformation(DebuggerState.DebuggeeThrewException);
				}
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void Exception2(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.Exception);

			if (State == DebuggerState.EvaluatingExpression)
			{
				//eventObjects.Thread.ClearCurrentException();
				//eventObjects.Thread.InterceptCurrentException();
				eventObjects.Controller.Continue();
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void ExceptionUnwind(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ExceptionUnwind);

			if (State == DebuggerState.EvaluatingExpression && EvaluationComplete != null)
			{
				EvaluationComplete(null);
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void ExitAppDomain(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ExitAppDomain);
			InternalContinueProcess();
		}

		public void OnProcessExited()
		{
			if (ProcessExited != null)
			{
				ProcessExited();
			}

			DebugeeProcessID = 0;
			EventObjects.Reset();

			State = DebuggerState.DebuggeeStopped;
		}

		public void ExitProcess(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ExitProcess);

			if (eventObjects.Process.GetID() == DebugeeProcessID)
			{
				OnProcessExited();
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void ExitThread(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.ExitThread);
			try
			{
				InternalContinueProcess(SuspendableDebugEvent.ExitThread);
			}
			catch (COMException comException)
			{
				if ((uint)comException.ErrorCode == 0x80131301)
				{
					State = DebuggerState.DebuggeeStopped;
				}
				else
				{
					UIHandler.Instance.ShowException(comException);
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}
		}

		public void FunctionRemapComplete(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.FunctionRemapComplete);
			InternalContinueProcess();
		}

		public void FunctionRemapOpportunity(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.FunctionRemapOpportunity);
			InternalContinueProcess();
		}

		public void LoadAssembly(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.LoadAssembly);

			if (Settings.Instance.WarnUnloadedAssembly)
			{
				string assemblyPath = eventObjects.Assembly.GetName();

				if (System.IO.File.Exists(assemblyPath) && !Project.Instance.IsAssemblyLoaded(assemblyPath))
				{
					UIHandler.Instance.ShowAssemblyMissingWarning(assemblyPath);
				}
			}

			InternalContinueProcess(SuspendableDebugEvent.LoadAssembly);
			//RefreshAndSuspendProcess(eventObjects);
			//DisplayAllInformation(eventObjects);
		}

		public void LoadClass(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			if (Settings.Instance.IsLoadClassEnabled)
			{
				DisplayOutputInformation(DebugEventType.LoadClass);
			}

			InternalContinueProcess(SuspendableDebugEvent.LoadClass);
		}

		public void LoadModule(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.LoadModule);
			eventObjects.Module.EnableClassLoadBacks(Settings.Instance.IsLoadClassEnabled);
			eventObjects.Module.EnableJitDebugging(true, false);

			if (ModuleLoading != null)
			{
				ModuleLoading(eventObjects.Module);
			}

			if (State == DebuggerState.EvaluatingExpression && eventObjects.Module != null)
			{
				LoadedModulesDuringEval.Add(eventObjects.Module);
			}

			InternalContinueProcess(SuspendableDebugEvent.LoadModule);
		}

		public void LogMessage(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.LogMessage);
			UIHandler.Instance.DisplayLogMessage(eventObjects.Message);
			InternalContinueProcess(SuspendableDebugEvent.LogMessage);
		}

		public void LogSwitch(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.LogSwitch);
			InternalContinueProcess(SuspendableDebugEvent.LogSwitch);
		}

		public void MDANotification(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.MdaNotificiation);

			if (State != DebuggerState.EvaluatingExpression && Settings.Instance.StopOnMdaNotification)
			{
				UIHandler.Instance.ClearDebugPanels();
				DisplayAllInformation(DebuggerState.DebuggeePaused);
				UIHandler.Instance.ShowMessageBox(eventObjects.Mda.GetName(), eventObjects.Mda.GetDescription());
			}
			else
			{
				InternalContinueProcess();
			}
		}

		public void NameChange(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.NameChange);
			InternalContinueProcess(SuspendableDebugEvent.NameChange);
		}

		public void StepComplete(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);

			if (eventObjects.Stepper == null && State == DebuggerState.EvaluatingExpression)
			{
				//Unfortunately, in case of .NET FW 1.1 debuggee, seems like the debugger API raises a StepComplete event despite a stepper is deactivated. In this case the stepper parameter is null. If this happens during evaluation then ignore this event and let the debuggee run further to complete the evaluation. This problem doesn't seem to happen with 2.0 debuggees.
				InternalContinueProcess();
			}
			else
			{
				FrameWrapper activeFrame = eventObjects.Thread.GetActiveFrame();
				uint currentIP = LastIP;
				uint currentFunctionToken = LastFunctionToken;
				bool displayLocation = false;
				bool exactLocation = false;

				if (activeFrame != null && activeFrame.IsILFrame())
				{
					currentIP = activeFrame.GetIP(ref exactLocation);
					currentFunctionToken = activeFrame.GetFunctionToken();
				}

				if (currentIP == LastIP && currentFunctionToken == LastFunctionToken)
				{
					Step(LastStepType);
					InternalContinueProcess();
				}
				else
				{
					eventObjects.Controller.SetAllThreadsDebugState(true, eventObjects.Thread);
					displayLocation = true;
				}

				if (displayLocation)
				{
					DisplayOutputInformation(DebugEventType.StepComplete);
					UIHandler.Instance.ClearDebugPanels();
					DisplayAllInformation(DebuggerState.DebuggeePaused);
				}
			}
		}

		public void UnloadAssembly(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.UnloadAssembly);
			InternalContinueProcess(SuspendableDebugEvent.UnloadAssembly);
		}

		public void UnloadClass(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			if (Settings.Instance.IsLoadClassEnabled)
			{
				DisplayOutputInformation(DebugEventType.UnloadClass);
			}

			InternalContinueProcess(SuspendableDebugEvent.UnloadClass);
		}

		public void UnloadModule(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.UnloadModule);
			InternalContinueProcess(SuspendableDebugEvent.UnloadModule);
		}

		public void UpdateModuleSymbols(DebugEventObjects eventObjects)
		{
			UpdateEventObjects(eventObjects);
			DisplayOutputInformation(DebugEventType.UpdateModuleSymbols);
			InternalContinueProcess();
		}

		#endregion
	}
}