using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using Dile.Disassemble;
using Dile.Debug;
using Dile.UI.Debug;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public partial class CallStackPanel : BasePanel
	{
		private ToolStripMenuItem addModuleMenuItem;
		private ToolStripMenuItem AddModuleMenuItem
		{
			get
			{
				return addModuleMenuItem;
			}
			set
			{
				addModuleMenuItem = value;
			}
		}

		private ToolStripMenuItem AddModuleFromDumpMenuItem
		{
			get;
			set;
		}

		private ToolStripMenuItem displayCodeMenuItem;
		private ToolStripMenuItem DisplayCodeMenuItem
		{
			get
			{
				return displayCodeMenuItem;
			}
			set
			{
				displayCodeMenuItem = value;
			}
		}

		private ToolStripMenuItem copyCallStackMenuItem;
		private ToolStripMenuItem CopyCallStackMenuItem
		{
			get
			{
				return copyCallStackMenuItem;
			}
			set
			{
				copyCallStackMenuItem = value;
			}
		}

		private ToolStripMenuItem displayCallStackMenuItem;
		private ToolStripMenuItem DisplayCallStackMenuItem
		{
			get
			{
				return displayCallStackMenuItem;
			}
			set
			{
				displayCallStackMenuItem = value;
			}
		}

		public CallStackPanel()
		{
			InitializeComponent();

			callStackView.Initialize();

			DisplayCallStackMenuItem = new ToolStripMenuItem("Display callstack as text");
			DisplayCallStackMenuItem.Click += new EventHandler(DisplayCallStackMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, DisplayCallStackMenuItem);

			CopyCallStackMenuItem = new ToolStripMenuItem("Copy callstack to clipboard");
			CopyCallStackMenuItem.Click += new EventHandler(CopyCallStackMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, CopyCallStackMenuItem);

			AddModuleFromDumpMenuItem = new ToolStripMenuItem("Add module to project from memory dump");
			AddModuleFromDumpMenuItem.Click += new EventHandler(AddModuleFromDumpMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, AddModuleFromDumpMenuItem);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, AddModuleMenuItem);

			DisplayCodeMenuItem = new ToolStripMenuItem("Display code");
			DisplayCodeMenuItem.Click += new EventHandler(DisplayCodeMenuItem_Click);
			callStackView.ItemContextMenu.Items.Insert(0, DisplayCodeMenuItem);

			callStackView.ItemContextMenu.Opening += new CancelEventHandler(ItemContextMenu_Opening);
		}

		protected override void RegisterToDebugEvents()
		{
			DebugEventHandler.Instance.ActiveThreadChanged += new ActiveThreadChangedDelegate(debugEventHandler_ActiveThreadChanged);

			base.RegisterToDebugEvents();
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		private void debugEventHandler_ActiveThreadChanged(ThreadWrapper newActiveThread)
		{
			FrameRefresher activeFrameRefresher = null;
			FrameWrapper activeFrame = null;

			try
			{
				if (newActiveThread != null)
				{
					activeFrame = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging ? newActiveThread.Version3.GetActiveFrame() : newActiveThread.GetActiveFrame());

					if (activeFrame != null && activeFrame.IsILFrame())
					{
						activeFrameRefresher = new FrameRefresher(newActiveThread, activeFrame.ChainIndex, activeFrame.FrameIndex, true);
					}
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}

			UIHandler.Instance.FrameChangedUpdate(activeFrameRefresher, activeFrame);
			ForceReinitializePanel();
		}

		private void ShowCodeObject()
		{
			if (callStackView.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];

				if (selectedItem.Tag is FrameInformation)
				{
					FrameInformation frameInformation = (FrameInformation)selectedItem.Tag;

					frameInformation.RefreshFrame();
					List<BaseLineDescriptor> specialLines = null;
					CodeObjectDisplayOptions displayOptions = new CodeObjectDisplayOptions();

					if (frameInformation.IsActiveFrame)
					{
						if (frameInformation.IsExactLocation)
						{
							displayOptions.CurrentLine = new CurrentLine(frameInformation.Offset);
						}
						else
						{
							displayOptions.CurrentLine = new CallerLine(frameInformation.Offset);
						}
					}
					else
					{
						BaseLineDescriptor lineDescriptor = null;
						specialLines = new List<BaseLineDescriptor>(1);

						if (frameInformation.IsExactLocation)
						{
							lineDescriptor = new ExactCallerLine(frameInformation.Offset);
						}
						else
						{
							lineDescriptor = new CallerLine(frameInformation.Offset);
						}

						specialLines.Add(lineDescriptor);
					}

					displayOptions.SpecialLinesToAdd = specialLines;

					UIHandler.Instance.ShowCodeObject(frameInformation.MethodDefinition, displayOptions);
					UIHandler.Instance.FrameChangedUpdate(frameInformation.Refresher, frameInformation.Frame);
				}
				else if (selectedItem.Tag is MissingModuleWithFrame)
				{
					MissingModuleWithFrame missingModule = (MissingModuleWithFrame)selectedItem.Tag;

					UIHandler.Instance.FrameChangedUpdate(missingModule.FrameRefresher, missingModule.Frame);
				}
				else
				{
					UIHandler.Instance.FrameChangedUpdate(null, null);
				}
			}
		}

		private void callStackView_Resize(object sender, EventArgs e)
		{
			methodColumn.Width = callStackView.ClientSize.Width;
		}

		public void DisplayCallStack(List<FrameWrapper> callStack)
		{
			if (callStack != null)
			{
				callStackView.BeginUpdate();
				callStackView.Items.Clear();

				for (int index = 0; index < callStack.Count; index++)
				{
					FrameWrapper frame = callStack[index];
					bool activeFrame = (index == 0);
					ListViewItem item = new ListViewItem();
					bool isCodeAvailable = false;
					FunctionWrapper function = null;

					try
					{
						function = frame.GetFunction();
						isCodeAvailable = true;
					}
					catch (COMException comException)
					{
						//0x80131309 == CORDBG_E_CODE_NOT_AVAILABLE
						if ((uint)comException.ErrorCode == 0x80131309)
						{
							isCodeAvailable = false;
						}
						else
						{
							throw;
						}
					}
					catch (NullReferenceException)
					{
						isCodeAvailable = false;
					}

					if (isCodeAvailable)
					{
						ModuleWrapper module = function.GetModule();
						uint functionToken = function.GetToken();

						TokenBase tokenObject = HelperFunctions.FindObjectByToken(functionToken, module);
						MethodDefinition methodDefinition = tokenObject as MethodDefinition;

						if (methodDefinition != null)
						{
							FrameInformation frameInformation = new FrameInformation(DebugEventHandler.Instance.EventObjects.Thread, methodDefinition, activeFrame, frame);
							item.Tag = frameInformation;
							item.Text = string.Format("{0}::{1}", methodDefinition.BaseTypeDefinition.FullName, methodDefinition.DisplayName);

							if (!frameInformation.IsExactLocation)
							{
								item.Text += " - not exact offset";
							}
						}
						else
						{
							item.Tag = new MissingModuleWithFrame(module, DebugEventHandler.Instance.EventObjects.Thread, activeFrame, frame);
							item.Text = "Unknown method (perhaps a reference is not loaded). Module name: " + module.GetName();
						}
					}

					if (!frame.IsILFrame())
					{
						if (isCodeAvailable)
						{
							item.Text = "Native frame, IP offset is not available (" + item.Text + ")";
						}
						else
						{
							item.Text = "Native frame, IP offset is not available (code is unavailable).";
						}
					}

					item.ToolTipText = item.Text;
					callStackView.Items.Add(item);
				}

				callStackView.EndUpdate();
			}
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			if (DebugEventHandler.Instance.EventObjects.Thread != null)
			{
				List<FrameWrapper> callStack = null;

				if (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging)
				{
					callStack = DebugEventHandler.Instance.EventObjects.Thread.Version3.StackWalk();
				}
				else
				{
					callStack = DebugEventHandler.Instance.EventObjects.Thread.GetCallStack();
				}
				DisplayCallStack(callStack);
			}
			else
			{
				callStackView.Items.Clear();
				UIHandler.Instance.DisplayUserWarning("Unable to retrieve the call stack because there is no current thread.");
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			callStackView.Items.Clear();
		}

		private void callStackView_DoubleClick(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void callStackView_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				ShowCodeObject();
			}
		}

		private string GetCallStackAsText()
		{
			StringBuilder result = new StringBuilder();

			for (int index = 0; index < callStackView.Items.Count; index++)
			{
				result.Append(callStackView.Items[index].Text);

				if (index < callStackView.Items.Count - 1)
				{
					result.Append("\r\n");
				}
			}

			return result.ToString();
		}

		private void DisplayCallStackMenuItem_Click(object sender, EventArgs e)
		{
			TextDisplayer.Instance.ShowText(GetCallStackAsText());
		}

		private void CopyCallStackMenuItem_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetCallStackAsText());
		}

		private void AddMissingModule(bool treatAsInMemory)
		{
			if (callStackView.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];
				MissingModule missingModule = selectedItem.Tag as MissingModule;

				if (missingModule != null)
				{
					missingModule.AddModuleToProject(treatAsInMemory);
				}
			}
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			AddMissingModule(false);
		}

		private void AddModuleFromDumpMenuItem_Click(object sender, EventArgs e)
		{
			AddMissingModule(true);
		}

		private void DisplayCodeMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void ItemContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool isOneItemSelected = (callStackView.SelectedItems.Count == 1);

			AddModuleMenuItem.Visible = false;
			AddModuleFromDumpMenuItem.Visible = false;
			DisplayCodeMenuItem.Enabled = isOneItemSelected;
			DisplayCodeMenuItem.Visible = true;

			if (isOneItemSelected)
			{
				ListViewItem selectedItem = callStackView.SelectedItems[0];

				if (selectedItem.Tag is MissingModule)
				{
					AddModuleMenuItem.Visible = true;
					AddModuleFromDumpMenuItem.Visible = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging);
					DisplayCodeMenuItem.Visible = false;
				}
			}
		}
	}
}