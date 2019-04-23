using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class DebuggingSettingsDisplayer : BaseSettingsDisplayer
	{
		private CheckBox enableLoadClassCheckBox = null;
		private CheckBox EnableLoadClassCheckBox
		{
			get
			{
				return enableLoadClassCheckBox;
			}
			set
			{
				enableLoadClassCheckBox = value;
			}
		}

		private CheckBox warnUnloadedAssemblyCheckBox = null;
		private CheckBox WarnUnloadedAssemblyCheckBox
		{
			get
			{
				return warnUnloadedAssemblyCheckBox;
			}
			set
			{
				warnUnloadedAssemblyCheckBox = value;
			}
		}

		private CheckBox stopOnExceptionCheckBox = null;
		private CheckBox StopOnExceptionCheckBox
		{
			get
			{
				return stopOnExceptionCheckBox;
			}
			set
			{
				stopOnExceptionCheckBox = value;
			}
		}

		private CheckBox stopOnlyOnUnhandledExceptionCheckBox = null;
		private CheckBox StopOnlyOnUnhandledExceptionCheckBox
		{
			get
			{
				return stopOnlyOnUnhandledExceptionCheckBox;
			}
			set
			{
				stopOnlyOnUnhandledExceptionCheckBox = value;
			}
		}

		private CheckBox stopOnMdaNotificationCheckBox = null;
		private CheckBox StopOnMdaNotificationCheckBox
		{
			get
			{
				return stopOnMdaNotificationCheckBox;
			}
			set
			{
				stopOnMdaNotificationCheckBox = value;
			}
		}

		private CheckBox displayHexaNumbers = null;
		private CheckBox DisplayHexaNumbers
		{
			get
			{
				return displayHexaNumbers;
			}
			set
			{
				displayHexaNumbers = value;
			}
		}

		private CheckBox EvaluateMethodsInObjectViewer
		{
			get;
			set;
		}

		private CheckBox detachOnQuit = null;
		private CheckBox DetachOnQuit
		{
			get
			{
				return detachOnQuit;
			}
			set
			{
				detachOnQuit = value;
			}
		}

		private Label funcEvalTimeoutLabel = null;
		private Label FuncEvalTimeoutLabel
		{
			get
			{
				return funcEvalTimeoutLabel;
			}
			set
			{
				funcEvalTimeoutLabel = value;
			}
		}

		private NumericUpDown funcEvalTimeoutUpDown = null;
		private NumericUpDown FuncEvalTimeoutUpDown
		{
			get
			{
				return funcEvalTimeoutUpDown;
			}
			set
			{
				funcEvalTimeoutUpDown = value;
			}
		}

		private Label funcEvalAbortTimeoutLabel = null;
		private Label FuncEvalAbortTimeoutLabel
		{
			get
			{
				return funcEvalAbortTimeoutLabel;
			}
			set
			{
				funcEvalAbortTimeoutLabel = value;
			}
		}

		private NumericUpDown funcEvalAbortTimeoutUpDown = null;
		private NumericUpDown FuncEvalAbortTimeoutUpDown
		{
			get
			{
				return funcEvalAbortTimeoutUpDown;
			}
			set
			{
				funcEvalAbortTimeoutUpDown = value;
			}
		}

		private NumericUpDown AutomaticRefreshIntervalUpDown
		{
			get;
			set;
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 12;

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

			EnableLoadClassCheckBox = CreateCheckBox(panel,
				"Enable (un)load class callbacks during debugging?",
				Settings.Instance.IsLoadClassEnabled);

			StopOnExceptionCheckBox = CreateCheckBox(panel,
				"Stop the debuggee when exception thrown?",
				Settings.Instance.StopOnException);
			StopOnExceptionCheckBox.CheckedChanged += new EventHandler(StopOnExceptionCheckBox_CheckedChanged);

			StopOnlyOnUnhandledExceptionCheckBox = CreateCheckBox(panel,
				20,
				"Stop the debuggee only when an unhandled exception thrown?",
				Settings.Instance.StopOnlyOnUnhandledException);
			StopOnlyOnUnhandledExceptionCheckBox.Enabled = Settings.Instance.StopOnException;

			StopOnMdaNotificationCheckBox = CreateCheckBox(panel,
				"Stop the debuggee on MDA (Managed Debug Assistant) notification?",
				Settings.Instance.StopOnMdaNotification);

			WarnUnloadedAssemblyCheckBox = CreateCheckBox(panel,
				"Warn if the debuggee loads an assembly which is not added to the DILE project?",
				Settings.Instance.WarnUnloadedAssembly);

			DisplayHexaNumbers = CreateCheckBox(panel,
				"Display numbers in hexadecimal format?",
				Settings.Instance.DisplayHexaNumbers);

			EvaluateMethodsInObjectViewer = CreateCheckBox(panel,
				"Evaluate property and ToString() calls in the Object Viewer?",
				Settings.Instance.EvaluateMethodsInObjectViewer);

			DetachOnQuit = CreateCheckBox(panel,
				"Detach from debuggee when DILE is quit (rather than stopping the debuggee)?",
				Settings.Instance.DetachOnQuit);

			FuncEvalTimeoutUpDown = CreateIntegerValueControls(panel,
				5,
				999,
				"Time to wait for a function evaluation before aborting it (in seconds):",
				Settings.Instance.FuncEvalTimeout);
			FuncEvalAbortTimeoutUpDown = CreateIntegerValueControls(panel,
				5,
				999,
				"Time to wait for successfully aborting a function evaluation before ignoring it (in seconds):",
				Settings.Instance.FuncEvalAbortTimeout);
			AutomaticRefreshIntervalUpDown = CreateIntegerValueControls(panel,
				100,
				3600000,
				"Default automatic process list refreshing interval in the Attach to Process dialog.",
				Settings.Instance.AutomaticRefreshInterval);
		}

		public override void ReadSettings()
		{
			Settings.Instance.IsLoadClassEnabled = EnableLoadClassCheckBox.Checked;
			Settings.Instance.StopOnException = StopOnExceptionCheckBox.Checked;
			Settings.Instance.StopOnlyOnUnhandledException = StopOnlyOnUnhandledExceptionCheckBox.Checked;
			Settings.Instance.StopOnMdaNotification = StopOnMdaNotificationCheckBox.Checked;
			Settings.Instance.WarnUnloadedAssembly = WarnUnloadedAssemblyCheckBox.Checked;
			Settings.Instance.DisplayHexaNumbers = DisplayHexaNumbers.Checked;
			Settings.Instance.EvaluateMethodsInObjectViewer = EvaluateMethodsInObjectViewer.Checked;
			Settings.Instance.DetachOnQuit = DetachOnQuit.Checked;
			Settings.Instance.FuncEvalTimeout = Convert.ToInt32(FuncEvalTimeoutUpDown.Value);
			Settings.Instance.FuncEvalAbortTimeout = Convert.ToInt32(FuncEvalAbortTimeoutUpDown.Value);
			Settings.Instance.AutomaticRefreshInterval = Convert.ToInt32(AutomaticRefreshIntervalUpDown.Value);
		}

		private void StopOnExceptionCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			StopOnlyOnUnhandledExceptionCheckBox.Enabled = StopOnExceptionCheckBox.Checked;
		}

		public override string ToString()
		{
			return "Debugging";
		}
	}
}