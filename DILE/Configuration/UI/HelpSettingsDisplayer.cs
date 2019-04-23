using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class HelpSettingsDisplayer : BaseSettingsDisplayer
	{
		private CheckBox SyncOpCodeHelperPanelCheckBox
		{
			get;
			set;
		}

		private void CreateSyncOpCodeHelperPanelCheckBox(TableLayoutPanel panel)
		{
			Label descriptionLabel = new Label();
			descriptionLabel.AutoSize = true;
			descriptionLabel.Text = "Automatically update the IL Instructions panel when the mouse hovers over IL code";
			panel.Controls.Add(descriptionLabel);

			SyncOpCodeHelperPanelCheckBox = new CheckBox();
			SyncOpCodeHelperPanelCheckBox.AutoSize = true;
			SyncOpCodeHelperPanelCheckBox.Checked = Settings.Instance.SyncOpCodeHelper;
			panel.Controls.Add(SyncOpCodeHelperPanelCheckBox);
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 1;

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

			CreateSyncOpCodeHelperPanelCheckBox(panel);
		}

		public override void ReadSettings()
		{
			Settings.Instance.SyncOpCodeHelper = SyncOpCodeHelperPanelCheckBox.Checked;
		}

		public override string ToString()
		{
			return "Help";
		}
	}
}
