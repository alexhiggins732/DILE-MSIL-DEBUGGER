using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration.UI;
using Dile.Disassemble;

namespace Dile.UI
{
	partial class ProjectProperties : Form
	{
		public ProjectProperties()
		{
			InitializeComponent();
		}

		public DialogResult DisplaySettings()
		{
			List<BaseSettingsDisplayer> settingsDisplayers = new List<BaseSettingsDisplayer>(1);
			settingsDisplayers.Add(new GeneralProjectSettingsDisplayer());
			settingsDisplayers.Add(new ProjectStartupSettingsDisplayer());
			settingsDisplayers.Add(new ProjectExceptionSettingsDisplayer());
			settingsDisplayers.Add(new ProjectDebuggingSettingsDisplayer());
			settingsDisplayerControl.DisplaySettings(settingsDisplayers);

			return ShowDialog();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			settingsDisplayerControl.UpdateSettings();
			Project.Instance.IsSaved = false;
			DialogResult = DialogResult.OK;
		}
	}
}