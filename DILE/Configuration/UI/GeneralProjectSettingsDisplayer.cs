using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class GeneralProjectSettingsDisplayer : BaseSettingsDisplayer
	{
		private TextBox projectNameTextBox = null;
		private TextBox ProjectNameTextBox
		{
			get
			{
				return projectNameTextBox;
			}
			set
			{
				projectNameTextBox = value;
			}
		}

		private void CreateProjectNameControls(TableLayoutPanel panel)
		{
			Label label = new Label();
			label.Dock = DockStyle.Fill;
			label.Text = "Name:";
			label.TextAlign = ContentAlignment.MiddleLeft;
			panel.Controls.Add(label);

			ProjectNameTextBox = new TextBox();
			ProjectNameTextBox.Dock = DockStyle.Fill;
			ProjectNameTextBox.Text = Project.Instance.Name;
			panel.Controls.Add(ProjectNameTextBox);
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 3;

			CreateProjectNameControls(panel);
		}

		public override void ReadSettings()
		{
			Project.Instance.Name = ProjectNameTextBox.Text;
		}

		public override string ToString()
		{
			return "General";
		}
	}
}