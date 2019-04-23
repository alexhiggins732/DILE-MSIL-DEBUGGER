using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public class BasePanel : DockContent
	{
		private MenuItem menuItem;
		public MenuItem MenuItem
		{
			get
			{
				return menuItem;
			}
			set
			{
				menuItem = value;
			}
		}

		private bool isDebugPanelInitialized;
		private bool IsDebugPanelInitialized
		{
			get
			{
				return isDebugPanelInitialized;
			}
			set
			{
				isDebugPanelInitialized = value;
			}
		}

		private bool isPanelCleared = true;
		private bool IsPanelCleared
		{
			get
			{
				return isPanelCleared;
			}
			set
			{
				isPanelCleared = value;
			}
		}

		private NoArgumentsDelegate onInitializePanelMethod;
		private NoArgumentsDelegate OnInitializePanelMethod
		{
			get
			{
				return onInitializePanelMethod;
			}
			set
			{
				onInitializePanelMethod = value;
			}
		}

		private NoArgumentsDelegate onClearPanelMethod;
		private NoArgumentsDelegate OnClearPanelMethod
		{
			get
			{
				return onClearPanelMethod;
			}
			set
			{
				onClearPanelMethod = value;
			}
		}

		private FrameRefresher activeFrameRefresher;
		protected FrameRefresher ActiveFrameRefresher
		{
			get
			{
				return activeFrameRefresher;
			}
			private set
			{
				activeFrameRefresher = value;
			}
		}

		private FrameWrapper activeFrame;
		protected FrameWrapper ActiveFrame
		{
			get
			{
				if (!IsFrameValid && ActiveFrameRefresher != null)
				{
					try
					{
						activeFrame = ActiveFrameRefresher.GetRefreshedValue();
						IsFrameValid = true;
					}
					catch
					{
					}
				}

				return activeFrame;
			}
			set
			{
				activeFrame = value;
			}
		}

		private bool isFrameValid;
		private bool IsFrameValid
		{
			get
			{
				return isFrameValid;
			}
			set
			{
				isFrameValid = value;
			}
		}

		public BasePanel()
		{
			KeyPreview = true;
			HideOnClose = true;

			if (IsDebugPanel())
			{
				RegisterToDebugEvents();
				AssemblyLoader.Instance.AssembliesLoaded += new AssembliesLoadedDelegate(AssemblyLoader_AssembliesLoaded);
			}
		}

		protected override void OnDockStateChanged(EventArgs e)
		{
			base.OnDockStateChanged(e);

			if (MenuItem != null && DockState != DockState.Unknown)
			{
				MenuItem.Checked = (DockState != DockState.Hidden);
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;

			if (msg.Msg == Constants.WM_KEYDOWN || msg.Msg == Constants.WM_SYSKEYDOWN)
			{
				if (keyData == (Keys.Control | Keys.Tab))
				{
					UIHandler.Instance.DisplayDocumentSelector();
					result = true;
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		protected virtual bool IsDebugPanel()
		{
			return false;
		}

		protected virtual bool UpdateWhenActiveFrameChanges()
		{
			return false;
		}

		protected virtual void OnClearPanel()
		{
		}

		protected virtual void RegisterToDebugEvents()
		{
			DebugEventHandler.Instance.ActiveFrameChanged += new ActiveFrameChangedDelegate(debugEventHandler_ActiveFrameChanged);
			DebugEventHandler.Instance.EvaluationComplete += new EvaluationCompleteDelegate(debugEventHandler_EvaluationComplete);
			DebugEventHandler.Instance.InvalidateDebugInformation += new InvalidateDebugInformationDelegate(debugEventHandler_InvalidateDebugInformation);
			UIHandler.Instance.MainForm.UpdateDebugInformation += new UpdateDebugInformationDelegate(debugEventHandler_UpdateDebugInformation);
			DebugEventHandler.Instance.ProcessExited += new ProcessExitedDelegate(debugEventHandler_ProcessExited);

			OnInitializePanelMethod = new NoArgumentsDelegate(OnInitializePanel);
			OnClearPanelMethod = new NoArgumentsDelegate(OnClearPanel);
		}

		private void debugEventHandler_EvaluationComplete(EvalWrapper evalWrapper)
		{
			IsFrameValid = false;
		}

		private void debugEventHandler_ProcessExited()
		{
			if (!IsPanelCleared)
			{
				IsDebugPanelInitialized = false;
				ActiveFrameRefresher = null;
				ActiveFrame = null;
				IsFrameValid = false;

				ClearPanel();
			}
		}

		private bool IsVisible()
		{
			return (DockState == DockState.Float
				|| DockPanel.DockWindows[DockState].VisibleNestedPanes[0].ActiveContent == this);
		}

		private void AssemblyLoader_AssembliesLoaded(List<Assembly> assemblies, bool isProjectChanged)
		{
			if (!IsPanelCleared)
			{
				ClearPanel();
			}

			IsDebugPanelInitialized = false;

			if (IsVisible())
			{
				InitializePanel();
			}
		}

		private void debugEventHandler_ActiveFrameChanged(FrameRefresher newActiveFrameRefresher, FrameWrapper newActiveFrame)
		{
			ActiveFrameRefresher = newActiveFrameRefresher;
			ActiveFrame = newActiveFrame;
			IsFrameValid = true;

			if (UpdateWhenActiveFrameChanges())
			{
				if (IsVisible())
				{
					ClearPanel();
					InitializePanel();
				}
				else
				{
					IsPanelCleared = false;
					IsDebugPanelInitialized = false;
				}
			}
		}

		private void debugEventHandler_InvalidateDebugInformation()
		{
			if (!IsPanelCleared)
			{
				IsDebugPanelInitialized = false;
				ActiveFrameRefresher = null;
				ActiveFrame = null;
				IsFrameValid = false;

				ClearPanel();
			}
		}

		private void debugEventHandler_UpdateDebugInformation(FrameRefresher activeFrameRefresher, FrameWrapper activeFrame)
		{
			ActiveFrameRefresher = activeFrameRefresher;
			ActiveFrame = activeFrame;
			IsFrameValid = true;
			IsPanelCleared = false;

			if (IsVisible())
			{
				InitializePanel();
			}
			else
			{
				IsDebugPanelInitialized = false;
			}
		}

		public void ClearPanel()
		{
			if (!IsPanelCleared)
			{
				ForceClearPanel();
			}
		}

		public void ForceClearPanel()
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke(OnClearPanelMethod);
				}
				else
				{
					OnClearPanel();
				}

				IsDebugPanelInitialized = false;
				IsPanelCleared = true;
			}
			catch (Exception exception)
			{
				UIHandler.Instance.DisplayUserWarning(string.Format("An error occurred while trying to clear the {0}: {1}", Text, exception.Message));
			}
		}

		private void InitializePanel()
		{
			if (!IsDebugPanelInitialized)
			{
				ForceReinitializePanel();
			}
		}

		protected void ForceReinitializePanel()
		{
			try
			{
				switch (DebugEventHandler.Instance.State)
				{
					case DebuggerState.DebuggeePaused:
					case DebuggerState.DebuggeeSuspended:
					case DebuggerState.DebuggeeThrewException:
					case DebuggerState.DumpDebugging:
						if (InvokeRequired)
						{
							Invoke(OnInitializePanelMethod);
						}
						else
						{
							OnInitializePanel();
						}

						IsDebugPanelInitialized = true;
						IsPanelCleared = false;
						break;
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.DisplayUserWarning(string.Format("An error occurred while trying to initialize the {0}: {1}", Text, exception.Message));
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			base.OnEnter(e);

			InitializePanel();
		}

		protected virtual void OnInitializePanel()
		{
			if (!IsPanelCleared)
			{
				ClearPanel();
				IsPanelCleared = true;
			}
		}

		public override string ToString()
		{
			return TabText;
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// BasePanel
			// 
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "BasePanel";
			this.ResumeLayout(false);

		}
	}
}