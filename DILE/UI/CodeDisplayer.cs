using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Controls;
using Dile.Debug;
using Dile.Disassemble;
using Dile.UI.Debug;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public class CodeDisplayer
	{
		private IMultiLine codeObject;
		public IMultiLine CodeObject
		{
			get
			{
				return codeObject;
			}
			set
			{
				codeObject = value;
			}
		}

		private CodeEditorForm window;
		internal CodeEditorForm Window
		{
			get
			{
				return window;
			}
			private set
			{
				window = value;
			}
		}

		private DockPanel dockPanel;
		public DockPanel DockPanel
		{
			get
			{
				return dockPanel;
			}
			set
			{
				dockPanel = value;
			}
		}

		internal CodeDisplayer(DockPanel dockPanel, IMultiLine codeObject, CodeEditorForm window)
		{
			DockPanel = dockPanel;
			CodeObject = codeObject;
			Window = window;
		}

		public void ShowCodeObject(CodeObjectDisplayOptions options)
		{
			bool initialized = true;

			try
			{
				initialized = (CodeObject.CodeLines != null);
				Window.MdiParent = UIHandler.Instance.MainForm;

				if (!initialized)
				{
					UIHandler.Instance.ResetProgressBar();
					UIHandler.Instance.SetProgressBarMaximum(2);
					UIHandler.Instance.SetProgressBarVisible(true);
					UIHandler.Instance.SetProgressText("Creating text representation of the object...\n", false);
					CodeObject.Initialize();
					UIHandler.Instance.StepProgressBar(1);

					UIHandler.Instance.SetProgressText("Displaying the text...\n", false);
					Window.ShowCodeObject(codeObject);
					Window.Show(DockPanel);

					UIHandler.Instance.SetProgressText("Ready ", false);
					UIHandler.Instance.SetProgressText("\n\n", true);
				}

				if (Window.IsDisposed)
				{
					CodeEditorForm newWindow = new CodeEditorForm();
					newWindow.UpdateFont(Settings.Instance.CodeEditorFont.Font);
					Window.CopySettings(newWindow);
					Window = newWindow;

					Window.ShowCodeObject(codeObject);
					Window.CurrentLine = options.CurrentLine;

					Window.Show(DockPanel);
				}
				else
				{
					if (Window.Visible)
					{
						Window.CurrentLine = options.CurrentLine;
					}
					else
					{
						Window.ShowCodeObject(codeObject);
						Window.CurrentLine = options.CurrentLine;
						Window.DockPanel = DockPanel;
						Window.Show(DockPanel);
					}

					Window.Activate();
				}

				Window.AddSpecialLines(options.SpecialLinesToAdd);
				Window.ProjectExplorer = UIHandler.Instance.MainForm.ProjectExplorer;

				if (options.IsNavigateSet)
				{
					Window.RefreshEditorControl(true, options.NavigateToOffset);
				}
				else
				{
					Window.RefreshEditorControl(true);
				}
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}
			finally
			{
				if (!initialized)
				{
					UIHandler.Instance.SetProgressBarVisible(false);
				}
			}
		}

		public void ClearSpecialLines()
		{
			if (!Window.IsDisposed)
			{
				Window.ClearSpecialLines();
			}
		}

		public void ClearCurrentLine()
		{
			if (!Window.IsDisposed)
			{
				Window.CurrentLine = null;
			}
		}

		public void Refresh()
		{
			if (!Window.IsDisposed)
			{
				Window.RefreshEditorControl(true);
			}
		}

		public void UpdateBreakpoint(BreakpointInformation breakpointInformation)
		{
			FunctionBreakpointInformation functionBreakpointInformation = breakpointInformation as FunctionBreakpointInformation;

			if (!Window.IsDisposed && Window.Visible && functionBreakpointInformation != null)
			{
				Window.UpdateBreakpoint(functionBreakpointInformation);
			}
		}
	}
}