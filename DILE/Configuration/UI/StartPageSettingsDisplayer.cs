using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class StartPageSettingsDisplayer : BaseSettingsDisplayer
	{
		private NumericUpDown ProjectNewsFeedUpdatePeriodUpDown
		{
			get;
			set;
		}

		private NumericUpDown ReleasesFeedUpdatePeriodUpDown
		{
			get;
			set;
		}

		private NumericUpDown BlogFeedUpdatePeriodUpDown
		{
			get;
			set;
		}

		private CheckBox StartPageEnabledCheckBox
		{
			get;
			set;
		}

		private void CreateStartPageEnabledCheckBox(TableLayoutPanel panel)
		{
			Label descriptionLabel = new Label();
			descriptionLabel.AutoSize = true;
			descriptionLabel.Text = "Automatically display the Start Page when DILE starts.";
			panel.Controls.Add(descriptionLabel);

			StartPageEnabledCheckBox = new CheckBox();
			StartPageEnabledCheckBox.AutoSize = true;
			StartPageEnabledCheckBox.Checked = Settings.Instance.StartPageEnabled;
			panel.Controls.Add(StartPageEnabledCheckBox);
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 4;

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

			ProjectNewsFeedUpdatePeriodUpDown =
				CreateIntegerValueControls(panel, 0, 99, "Project News feed update period (in days, 0 = disabled)", Settings.Instance.ProjectNewsFeedUpdatePeriod);
			ReleasesFeedUpdatePeriodUpDown = CreateIntegerValueControls(panel, 0, 99, "Latest Releases feed update period (in days, 0 = disabled)", Settings.Instance.ReleasesFeedUpdatePeriod);
			BlogFeedUpdatePeriodUpDown = CreateIntegerValueControls(panel, 0, 99, "Blog feed update period (in days, 0 = disabled)", Settings.Instance.BlogFeedUpdatePeriod);
			CreateStartPageEnabledCheckBox(panel);
		}

		public override void ReadSettings()
		{
			Settings.Instance.ProjectNewsFeedUpdatePeriod = Convert.ToInt32(ProjectNewsFeedUpdatePeriodUpDown.Value);
			Settings.Instance.ReleasesFeedUpdatePeriod = Convert.ToInt32(ReleasesFeedUpdatePeriodUpDown.Value);
			Settings.Instance.BlogFeedUpdatePeriod = Convert.ToInt32(BlogFeedUpdatePeriodUpDown.Value);
			Settings.Instance.StartPageEnabled = StartPageEnabledCheckBox.Checked;
		}

		public override string ToString()
		{
			return "Start Page";
		}
	}
}