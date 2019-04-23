using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.Disassemble.ILCodes;
using Dile.UI.Debug;
using System.IO;
using System.Runtime.InteropServices;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public partial class CodeEditorForm : DocumentContent
	{
		public IMultiLine CodeObject
		{
			get
			{
				return ilEditor.CodeObject;
			}
		}

		public BaseLineDescriptor CurrentLine
		{
			get
			{
				return ilEditor.CurrentLine;
			}
			set
			{
				ilEditor.CurrentLine = value;
			}
		}

		public CodeEditorForm()
		{
			InitializeComponent();

#if DEBUG
			ilEditor.ReadOnly = false;
#endif
		}

		public void AddSpecialLines(List<BaseLineDescriptor> specialLines)
		{
			if (specialLines != null)
			{
				ilEditor.SpecialLines.AddRange(specialLines);
			}
		}

		public void CopySettings(CodeEditorForm destinationForm)
		{
			destinationForm.ilEditor.SpecialLines.AddRange(ilEditor.SpecialLines);
			destinationForm.CurrentLine = CurrentLine;
		}

		public void ShowCodeObject(IMultiLine codeObject)
		{
			Text = codeObject.HeaderText;
			TabText = codeObject.HeaderText;
			ilEditor.ShowCodeObject(codeObject);
		}

		public void SetWordWrap(bool enabled)
		{
			ilEditor.WordWrap = enabled;

			if (ilEditor.CodeObject != null)
			{
				ilEditor.Text = ilEditor.Text;
			}
		}

		private ProjectExplorer projectExplorer;
		public ProjectExplorer ProjectExplorer
		{
			get
			{
				return projectExplorer;
			}
			set
			{
				projectExplorer = value;
			}
		}

		public void RefreshEditorControl(bool forceFormatRefresh)
		{
			ilEditor.RefreshControl(forceFormatRefresh);
		}

		internal void RefreshEditorControl(bool forceFormatRefresh, uint scrollToOffset)
		{
			ilEditor.RefreshControl(forceFormatRefresh, scrollToOffset);
		}

		private void locateInProjectExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProjectExplorer.LocateTokenNode((TokenBase)CodeObject);
		}

		public void ClearSpecialLines()
		{
			ilEditor.SpecialLines.Clear();
		}

		public void UpdateBreakpoint(FunctionBreakpointInformation breakpoint)
		{
			ilEditor.UpdateBreakpoint(breakpoint);
		}

		public void SetBreakpointAtSelection()
		{
			ilEditor.SetBreakpointAtSelection();
		}

		public void SetRunToCursorAtSelection()
		{
			ilEditor.SetRunToCursorAtSelection();
		}

		public void UpdateFont(Font font)
		{
			Font = font;
			ilEditor.Font = font;
			ilEditor.Text = ilEditor.Text;
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

		public override string ToString()
		{
			return TabText;
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			setIPToolStripMenuItem.Visible = false;

			if (DebugEventHandler.Instance.EventObjects.Frame != null && DebugEventHandler.Instance.EventObjects.Frame.IsActiveFrame && CurrentLine != null)
			{
				MethodDefinition displayedMethod = CodeObject as MethodDefinition;

				if (displayedMethod != null)
				{
					FunctionWrapper currentFunction = DebugEventHandler.Instance.EventObjects.Frame.GetFunction();
					uint currentFunctionToken = currentFunction.GetToken();

					if (displayedMethod.Token == currentFunctionToken)
					{
						ModuleWrapper currentModule = currentFunction.GetModule();
						bool isInMemoryCurrentModule = currentModule.IsInMemory();
						string currentModuleName = string.Empty;

						if (isInMemoryCurrentModule)
						{
							currentModuleName = currentModule.GetNameFromMetaData();
						}
						else
						{
							currentModuleName = currentModule.GetName();

							try
							{
								currentModuleName = Path.GetFileNameWithoutExtension(currentModuleName);
							}
							catch
							{
							}
						}

						currentModuleName = currentModuleName.ToLower();

						if ((isInMemoryCurrentModule && displayedMethod.BaseTypeDefinition.ModuleScope.Name.ToLower() == currentModuleName) || (!isInMemoryCurrentModule && Path.GetFileNameWithoutExtension(displayedMethod.BaseTypeDefinition.ModuleScope.Assembly.FullPath).ToLower() == currentModuleName))
						{
							BaseILCode currentILCode = ilEditor.GetILCodeAtMouseCursor();
							setIPToolStripMenuItem.Visible = true;

							if (currentILCode != null)
							{
								int hResult = DebugEventHandler.Instance.EventObjects.Frame.CanSetIP(Convert.ToUInt32(currentILCode.Offset));

								if (hResult == 0)
								{
									setIPToolStripMenuItem.Enabled = true;
									setIPToolStripMenuItem.Tag = currentILCode;
								}
								else
								{
									COMException comException = Marshal.GetExceptionForHR(hResult) as COMException;

									if (comException != null)
									{
										UIHandler.Instance.DisplayUserWarning(Marshal.GetExceptionForHR(hResult).Message);
									}

									setIPToolStripMenuItem.Enabled = false;
								}
							}
							else
							{
								setIPToolStripMenuItem.Enabled = false;
							}
						}
					}
				}
			}
		}

		private void setIPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			BaseILCode currentILCode = setIPToolStripMenuItem.Tag as BaseILCode;

			if (currentILCode != null)
			{
				try
				{
					DebugEventHandler.Instance.EventObjects.Frame.SetIP(Convert.ToUInt32(currentILCode.Offset));
					DebugEventHandler.Instance.DisplayAllInformation(DebugEventHandler.Instance.State);
				}
				catch (Exception exception)
				{
					UIHandler.Instance.ShowException(exception);
					UIHandler.Instance.DisplayUserWarning(exception.Message);
				}
			}
		}

		private void CodeEditorForm_Enter(object sender, EventArgs e)
		{
			UIHandler.Instance.DocumentActivated(this);
		}
	}
}