using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using System.IO;
using System.Runtime.InteropServices;

namespace Dile.UI.Debug
{
	public class BreakpointHandler
	{
		private static BreakpointHandler instance = new BreakpointHandler();
		public static BreakpointHandler Instance
		{
			get
			{
				return instance;
			}
		}

		private BreakpointHandler()
		{
			DebugEventHandler.Instance.ModuleLoading += new ModuleLoadingDelegate(SetInitialBreakpoints);
			DebugEventHandler.Instance.ProcessExited += new ProcessExitedDelegate(ProcessExited);
			DebugEventHandler.Instance.BreakpointNotSetError += new BreakpointSetErrorDelegate(BreakpointNotSetError);
		}

		public void ClearBreakpoints()
		{
			Project.Instance.FunctionBreakpoints.Clear();
		}

		public void DeleteRemovedBreakpoints()
		{
			int index = 0;

			while (index < Project.Instance.FunctionBreakpoints.Count)
			{
				FunctionBreakpointInformation functionBreakpoint = Project.Instance.FunctionBreakpoints[index];

				if (functionBreakpoint.State == BreakpointState.Removed)
				{
					Project.Instance.FunctionBreakpoints.Remove(functionBreakpoint);
				}
				else
				{
					index++;
				}
			}
		}

		public void SetInitialBreakpoints(ModuleWrapper module)
		{
			string moduleName = string.Empty;
			bool isInMemoryModule = module.IsInMemory();

			if (isInMemoryModule)
			{
				moduleName = module.GetNameFromMetaData();
			}
			else
			{
				moduleName = module.GetName();
			}

			try
			{
				moduleName = Path.GetFileNameWithoutExtension(moduleName);
			}
			catch
			{
			}

			moduleName = moduleName.ToLower();

			foreach (FunctionBreakpointInformation functionBreakpoint in Project.Instance.FunctionBreakpoints)
			{
				string breakpointModuleName = string.Empty;
				bool isInMemoryBreakpointModule = functionBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.IsInMemory;

				if (isInMemoryBreakpointModule)
				{
					breakpointModuleName = functionBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Name;
				}
				else
				{
					breakpointModuleName = Path.GetFileNameWithoutExtension(functionBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.FullPath);
				}

				breakpointModuleName = breakpointModuleName.ToLower();

				if (isInMemoryModule == isInMemoryBreakpointModule && moduleName == breakpointModuleName)
				{
					FunctionWrapper function = module.GetFunction(functionBreakpoint.MethodDefinition.Token);
					FunctionBreakpointWrapper breakpointWrapper = null;

					try
					{
						breakpointWrapper = function.CreateBreakpoint(functionBreakpoint.Offset);
					}
					catch (COMException exception)
					{
						unchecked
						{
							//0x8013130A == "Attempt to get a ICorDebugFunction for a function that is not IL."
							//This error most likely means that the assembly has been recompiled and thus the token is now pointing at a different non-IL method.
							if (exception.ErrorCode != (int)0x8013130A)
							{
								throw;
							}
						}
					}

					if (breakpointWrapper == null)
					{
						functionBreakpoint.State = BreakpointState.Inactive;
					}
					else
					{
						breakpointWrapper.Activate(functionBreakpoint.State == BreakpointState.Active);
						functionBreakpoint.Breakpoints.Add(breakpointWrapper);
					}
				}
			}

			if (Project.Instance.RunToCursorBreakpoint != null)
			{
				string breakpointModuleName = string.Empty;
				bool isInMemoryBreakpointModule = Project.Instance.RunToCursorBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.IsInMemory;

				if (isInMemoryBreakpointModule)
				{
					breakpointModuleName = Project.Instance.RunToCursorBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Name;
				}
				else
				{
					breakpointModuleName = Path.GetFileNameWithoutExtension(Project.Instance.RunToCursorBreakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly.FullPath);
				}

				breakpointModuleName = breakpointModuleName.ToLower();

				if (breakpointModuleName != null && breakpointModuleName.Equals(moduleName, StringComparison.Ordinal))
				{
					FunctionWrapper function = module.GetFunction(Project.Instance.RunToCursorBreakpoint.MethodDefinition.Token);
					FunctionBreakpointWrapper breakpointWrapper = function.CreateBreakpoint(Project.Instance.RunToCursorBreakpoint.Offset);

					if (breakpointWrapper == null)
					{
						Project.Instance.RunToCursorBreakpoint.Remove();
					}
					else
					{
						breakpointWrapper.Activate(true);
					}
				}
			}
		}

		public void Step(StepType stepType)
		{
			if (DebugEventHandler.Instance.EventObjects.Controller != null && !DebugEventHandler.Instance.EventObjects.Controller.IsRunning())
			{
				if (DebugEventHandler.Instance.Step(stepType))
				{
					DebugEventHandler.Instance.ContinueProcess();
				}
				else
				{
					UIHandler.Instance.DisplayUserWarning("There's no current thread on which the step command could be executed.");
				}
			}
		}

		private void ResetBreakpoints()
		{
			foreach (BreakpointInformation breakpoint in Project.Instance.FunctionBreakpoints)
			{
				breakpoint.Reset();
			}
		}

		private void ProcessExited()
		{
			UIHandler.Instance.ClearDebugPanels();
			UIHandler.Instance.ClearCodeDisplayers(true);
			DeleteRemovedBreakpoints();
			ResetBreakpoints();
			Project.Instance.RemoveInMemoryAssemblies();
			UIHandler.Instance.RemoveUnnecessaryAssemblies();
		}

		public FunctionBreakpointInformation AddRemoveBreakpoint(MethodDefinition methodDefinition, int offset, bool removeOnly)
		{
			FunctionBreakpointInformation result = null;
			result = Project.Instance.FindFunctionBreakpoint(methodDefinition, Convert.ToUInt32(offset));

			bool isProcessSuspended = false;

			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeRunning)
			{
				DebugEventHandler.Instance.RefreshAndSuspendProcess();
				isProcessSuspended = true;
			}

			if (result != null)
			{
				result.Remove();
				Project.Instance.FunctionBreakpoints.Remove(result);
				UIHandler.Instance.RemoveBreakpoint(result);
			}
			else if (!removeOnly)
			{
				result = new FunctionBreakpointInformation(methodDefinition, Convert.ToUInt32(offset));

				if (DebugEventHandler.Instance.EventObjects.Process != null)
				{
					List<ModuleWrapper> modules = DebugEventHandler.Instance.EventObjects.Process.FindModulesByName(methodDefinition.BaseTypeDefinition.ModuleScope.Name);
					int index = 0;

					while (result != null && index < modules.Count)
					{
						ModuleWrapper module = modules[index++];
						FunctionWrapper function = module.GetFunction(methodDefinition.Token);
						FunctionBreakpointWrapper functionBreakpoint = function.CreateBreakpoint(Convert.ToUInt32(offset));

						if (functionBreakpoint == null)
						{
							foreach (FunctionBreakpointWrapper existingFunctionBreakpoint in result.Breakpoints)
							{
								existingFunctionBreakpoint.Activate(false);
							}

							result = null;
						}
						else
						{
							functionBreakpoint.Activate(true);
							result.Breakpoints.Add(functionBreakpoint);
						}
					}
				}

				if (result == null)
				{
					UIHandler.Instance.DisplayUserWarning("The breakpoint could not be set at the given location.");
				}
				else
				{
					Project.Instance.FunctionBreakpoints.Add(result);
					UIHandler.Instance.AddBreakpoint(result);
				}
			}

			if (isProcessSuspended)
			{
				DebugEventHandler.Instance.ResumeProcess();
			}

			return result;
		}

		public void RunToCursor(MethodDefinition methodDefinition, int offset, bool removeOnly)
		{
			bool isProcessSuspended = false;

			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeRunning)
			{
				DebugEventHandler.Instance.RefreshAndSuspendProcess();
				isProcessSuspended = true;
			}

			if (Project.Instance.RunToCursorBreakpoint != null)
			{
				Project.Instance.RunToCursorBreakpoint.Remove();

				if (removeOnly)
				{
					UIHandler.Instance.DisplayUserWarning("The breakpoint could not be set at the given location.");
				}
			}

			if (!removeOnly)
			{
				Project.Instance.RunToCursorBreakpoint = new FunctionBreakpointInformation(methodDefinition, Convert.ToUInt32(offset));

				if (DebugEventHandler.Instance.State != DebuggerState.DebuggeeStopped)
				{
					List<ModuleWrapper> modules = DebugEventHandler.Instance.EventObjects.Process.FindModulesByName(methodDefinition.BaseTypeDefinition.ModuleScope.Name);
					int index = 0;

					while (Project.Instance.RunToCursorBreakpoint != null && index < modules.Count)
					{
						ModuleWrapper module = modules[index++];
						FunctionWrapper function = module.GetFunction(methodDefinition.Token);
						FunctionBreakpointWrapper functionBreakpoint = function.CreateBreakpoint(Convert.ToUInt32(offset));

						if (functionBreakpoint == null)
						{
							foreach (FunctionBreakpointWrapper existingFunctionBreakpoint in Project.Instance.RunToCursorBreakpoint.Breakpoints)
							{
								existingFunctionBreakpoint.Activate(false);
							}

							Project.Instance.RunToCursorBreakpoint = null;
						}
						else
						{
							functionBreakpoint.Activate(true);
							Project.Instance.RunToCursorBreakpoint.Breakpoints.Add(functionBreakpoint);
						}
					}

					if (Project.Instance.RunToCursorBreakpoint == null)
					{
						UIHandler.Instance.DisplayUserWarning("The breakpoint could not be set at the given location.");
					}
				}
			}

			if (isProcessSuspended)
			{
				DebugEventHandler.Instance.ResumeProcess();
			}
		}

		private void BreakpointNotSetError(MethodDefinition methodDefinition, uint offset)
		{
			int offsetAsInt = Convert.ToInt32(offset);
			FunctionBreakpointInformation breakpoint = Project.Instance.FindFunctionBreakpoint(methodDefinition, Convert.ToUInt32(offset));

			if (breakpoint != null)
			{
				UIHandler.Instance.DeactivateBreakpoint(breakpoint);
				breakpoint.NavigateTo();
			}

			if ((Project.Instance.RunToCursorBreakpoint != null) && (Project.Instance.RunToCursorBreakpoint.MethodDefinition == methodDefinition) && (Project.Instance.RunToCursorBreakpoint.Offset == offset))
			{
				RunToCursor(methodDefinition, offsetAsInt, true);
			}
		}
	}
}