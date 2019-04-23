using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public partial class SettingsDialog : Form
	{
		public SettingsDialog()
		{
			InitializeComponent();
		}

		public DialogResult DisplaySettings(MainMenu mainMenu)
		{
			List<BaseSettingsDisplayer> settingsDisplayers = new List<BaseSettingsDisplayer>()
			{
				new GeneralSettingsDisplayer(),
				new TextEditorSettingsDisplayer(),
				new StartPageSettingsDisplayer(),
				new FontSettingsDisplayer(),
				new ShortcutSettingsDisplayer(mainMenu),
				new DebuggingSettingsDisplayer(),
				new DisplayedDebuggingEventsDisplayer(),
				new HelpSettingsDisplayer()
			};
			settingsDisplayerControl.DisplaySettings(settingsDisplayers);

			return ShowDialog();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			settingsDisplayerControl.UpdateSettings();
			Settings.Instance.SettingsUpdated();

			DialogResult = DialogResult.OK;
		}
	}
}