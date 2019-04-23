using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public sealed class UIHandler
	{
		#region Singleton implementation
		private static readonly UIHandler instance = new UIHandler();
		public static UIHandler Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion

		private readonly DocumentSelectorForm documentSelector = new DocumentSelectorForm();
		private DocumentSelectorForm DocumentSelector
		{
			get
			{
				return documentSelector;
			}
		}

		private MainForm mainForm;
		internal MainForm MainForm
		{
			get
			{
				return mainForm;
			}
			set
			{
				mainForm = value;
			}
		}

		private UIHandler()
		{
		}

		internal void Initialize(MainForm mainForm, DockPanel dockPanel)
		{
			MainForm = mainForm;
			DocumentSelector.DockPanel = dockPanel;
		}

		public void StepProgressBar(int incrementValue)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.StepProgressBar(incrementValue);
			});
		}

		public void SetProgressBarMaximum(int maximum)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.SetProgressBarMaximum(maximum);
			});
		}

		public void SetProgressText(string text, bool addElapsedTime)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.SetProgressText(text, addElapsedTime);
			});
		}

		public void SetProgressText(string fileName, string text, bool addElapsedTime)
		{
			text = string.Format("{0}: {1} ", fileName, text);
			SetProgressText(text, addElapsedTime);
		}

		public void ResetProgressBar()
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ResetProgressBar();
			});
		}

		public void ShowException(Exception exception)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ShowException(exception);
			});
		}

		public void AssembliesLoaded(List<Assembly> loadedAssemblies, bool isProjectChanged)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.AssembliesLoaded(loadedAssemblies, isProjectChanged);
			});
		}

		public void SetProgressBarVisible(bool visible)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.SetProgressBarVisible(visible);
			});
		}

		public void DisplayOutputInformation(DebugEventDescriptor debugEventDescriptor)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.DisplayOutputInformation(debugEventDescriptor);
			});
		}

		public void DisplayLogMessage(string logMessage)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.DisplayLogMessage(logMessage);
			});
		}

		public void ShowCodeObject(IMultiLine codeObject, CodeObjectDisplayOptions options)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm.ProjectExplorer, delegate()
			{
				MainForm.ProjectExplorer.ShowCodeObject(codeObject, options);
			});
		}

		public void ClearDebugPanels()
		{
			ClearDebugPanels(false);
		}

		public void ClearDebugPanels(bool leaveThreads)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ClearDebugPanels(leaveThreads);
			});
		}

		public void ClearOutputPanel()
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ClearOutputPanel();
			});
		}

		public void ShowObjectInObjectViewer(FrameWrapper frame, BaseValueRefresher valueRefresher, string initialExpression)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ShowObjectInObjectViewer(frame, valueRefresher, initialExpression);
			});
		}

		public void ThreadChangedUpdate(ThreadWrapper thread)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, () => MainForm.ThreadChangedUpdate(thread));
		}

		public void FrameChangedUpdate(FrameRefresher frameRefresher, FrameWrapper frame)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.FrameChangedUpdate(frameRefresher, frame);
			});
		}

		public void ClearCodeDisplayers(bool refreshDisplayers)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.ClearCodeDisplayers(refreshDisplayers);
			});
		}

		public void DisplayUserWarning(string text)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.DisplayUserWarning(text);
			});
		}

		public void ClearUserWarning()
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.ClearUserWarning();
			});
		}

		public void ShowDebuggerState(DebuggerState newState)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ShowDebuggerState(newState);
			});
		}

		public void AddBreakpoint(BreakpointInformation breakpoint)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.AddBreakpoint(breakpoint);
			});
		}

		public void RemoveBreakpoint(BreakpointInformation breakpoint)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.RemoveBreakpoint(breakpoint);
			});
		}

		public void DeactivateBreakpoint(BreakpointInformation breakpoint)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.DeactivateBreakpoint(breakpoint);
			});
		}

		public void UpdateBreakpoint(IMultiLine codeObject, BreakpointInformation breakpointInformation)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm.ProjectExplorer, delegate()
			{
				MainForm.ProjectExplorer.UpdateBreakpoint(codeObject, breakpointInformation);
			});
		}

		public void ShowAssemblyMissingWarning(string assemblyPath)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.ShowAssemblyMissingWarning(assemblyPath);
			});
		}

		public void AddAssembly(string filePath)
		{
			AddAssembly(new AssemblyLoadRequest[] { new AssemblyLoadRequest(filePath) });
		}

		public void AddAssembly(ModuleWrapper module)
		{
			AddAssembly(new AssemblyLoadRequest[] { new AssemblyLoadRequest(module) });
		}

		public void AddAssembly(AssemblyLoadRequest[] requestedAssemblies)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.AddAssembly(requestedAssemblies);
			});
		}

		public void DisplayDocumentSelector()
		{
			if (!DocumentSelector.Visible)
			{
				DocumentSelector.Display(MainForm, MainForm.ActiveDocuments);
			}
		}

		public void DocumentActivated(DocumentContent documentContent)
		{
			InvokeHelper.InvokeFormMethod(MainForm, () => MainForm.DocumentActivated(documentContent));
		}

		public void ShowMessageBox(string caption, string text)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.ShowMessageBox(caption, text);
			});
		}

		public void RemoveUnnecessaryAssemblies()
		{
			InvokeHelper.InvokeFormMethod(MainForm.ProjectExplorer, delegate()
			{
				MainForm.ProjectExplorer.RemoveUnnecessaryAssemblies();
			});
		}

		public void CloseDynamicModuleDocuments()
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.CloseDynamicModuleDocuments();
			});
		}

		public void AddModulesToPanel(ModuleWrapper[] modules)
		{
			InvokeHelper.InvokeFormMethod(MainForm, delegate()
			{
				MainForm.AddModulesToPanel(modules);
			});
		}

		public void RaiseUpdateDebugInformation(FrameRefresher activeFrameRefresher, FrameWrapper activeFrame, DebuggerState newState)
		{
			InvokeHelper.BeginInvokeFormMethod(MainForm, delegate()
			{
				MainForm.RaiseUpdateDebugInformation(activeFrameRefresher, activeFrame, newState);
			});
		}

		public void OpenDumpFile(string dumpFilePath)
		{
			InvokeHelper.InvokeFormMethod(MainForm, () => MainForm.OpenDumpFile(dumpFilePath));
		}

		public void OpenFiles(StringCollection fileNames)
		{
			InvokeHelper.InvokeFormMethod(MainForm, () => MainForm.OpenFiles(fileNames));
		}

		public void DisplayOpCodeHelp(OpCodeItem opCodeItem)
		{
			InvokeHelper.InvokeFormMethod(mainForm, () => MainForm.DisplayOpCodeHelp(opCodeItem));
		}
	}
}