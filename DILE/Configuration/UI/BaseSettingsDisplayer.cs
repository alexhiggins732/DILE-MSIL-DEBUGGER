using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public abstract class BaseSettingsDisplayer
	{
		private List<Control> createdControls;
		protected List<Control> CreatedControls
		{
			get
			{
				return createdControls;
			}
			set
			{
				createdControls = value;
			}
		}

		private List<ColumnStyle> columnStyles;
		protected List<ColumnStyle> ColumnStyles
		{
			get
			{
				return columnStyles;
			}
			set
			{
				columnStyles = value;
			}
		}

		private List<RowStyle> rowStyles;
		protected List<RowStyle> RowStyles
		{
			get
			{
				return rowStyles;
			}
			set
			{
				rowStyles = value;
			}
		}

		private bool isInitialized = false;
		public bool IsInitialized
		{
			get
			{
				return isInitialized;
			}
			private set
			{
				isInitialized = value;
			}
		}

		private int rowCount;
		private int RowCount
		{
			get
			{
				return rowCount;
			}
			set
			{
				rowCount = value;
			}
		}

		private int columnCount;
		private int ColumnCount
		{
			get
			{
				return columnCount;
			}
			set
			{
				columnCount = value;
			}
		}

		private Label CreateTitleLabel(int indentation, string text)
		{
			Label result = new Label();
			result.Dock = DockStyle.Fill;
			result.Padding = new Padding(indentation, 0, 0, 0);
			result.Text = text;
			result.TextAlign = ContentAlignment.MiddleLeft;

			return result;
		}

		protected CheckBox CreateCheckBox(TableLayoutPanel panel, int indentation, string text, bool value)
		{
			CheckBox result;

			Label label = CreateTitleLabel(indentation, text);

			result = new CheckBox();
			result.CheckAlign = ContentAlignment.MiddleLeft;
			result.Checked = value;
			result.Dock = DockStyle.Left;

			panel.Controls.Add(label);
			panel.Controls.Add(result);

			return result;
		}

		protected CheckBox CreateCheckBox(TableLayoutPanel panel, string text, bool value)
		{
			return CreateCheckBox(panel, 0, text, value);
		}

		protected NumericUpDown CreateIntegerValueControls(TableLayoutPanel panel, decimal minimum, decimal maximum, string text, decimal value)
		{
			NumericUpDown result = null;

			Label label = CreateTitleLabel(0, text);

			result = new NumericUpDown();
			result.Minimum = minimum;
			result.Maximum = maximum;
			result.Value = value;
			result.Width = 60;

			panel.Controls.Add(label);
			panel.Controls.Add(result);

			return result;
		}

		protected abstract void CreateControls(TableLayoutPanel panel);

		protected virtual void DisplayCreatedControls(TableLayoutPanel panel)
		{
			panel.RowCount = RowCount;
			panel.ColumnCount = ColumnCount;
			HelperFunctions.CopyListElements(CreatedControls, panel.Controls);
			HelperFunctions.CopyListElements(ColumnStyles, panel.ColumnStyles);
			HelperFunctions.CopyListElements(RowStyles, panel.RowStyles);
		}

		public void DisplayControls(TableLayoutPanel panel)
		{
			panel.Visible = false;

			if (IsInitialized)
			{
				DisplayCreatedControls(panel);
			}
			else
			{
				CreateControls(panel);
				ColumnCount = panel.ColumnCount;
				RowCount = panel.RowCount;

				if (panel.Controls.Count > 0)
				{
					CreatedControls = new List<Control>(panel.Controls.Count);
					HelperFunctions.CopyListElements(panel.Controls, CreatedControls);
				}

				if (panel.ColumnStyles.Count > 0)
				{
					ColumnStyles = new List<ColumnStyle>(panel.ColumnStyles.Count);
					HelperFunctions.CopyListElements(panel.ColumnStyles, ColumnStyles);
				}

				if (panel.RowStyles.Count > 0)
				{
					RowStyles = new List<RowStyle>(panel.RowStyles.Count);
					HelperFunctions.CopyListElements(panel.RowStyles, RowStyles);
				}

				IsInitialized = true;
			}

			panel.Visible = true;
		}

		public abstract void ReadSettings();
	}
}