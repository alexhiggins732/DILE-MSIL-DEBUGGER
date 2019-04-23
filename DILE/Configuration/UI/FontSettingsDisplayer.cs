using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class FontSettingsDisplayer : BaseSettingsDisplayer
	{
		private const string ExampleText = "The quick brown fox jumps over the lazy dog.";

		private ComboBox panelsComboBox;
		private ComboBox PanelsComboBox
		{
			get
			{
				return panelsComboBox;
			}
			set
			{
				panelsComboBox = value;
			}
		}

		private TextBox exampleTextBox;
		private TextBox ExampleTextBox
		{
			get
			{
				return exampleTextBox;
			}
			set
			{
				exampleTextBox = value;
			}
		}

		private void CreatePanelsComboBox(TableLayoutPanel panel)
		{
			PanelsComboBox = new ComboBox();
			PanelsComboBox.BeginUpdate();

			foreach (IChangebleFont changebleFont in Settings.Instance.Panels)
			{
				PanelsComboBox.Items.Add(changebleFont);
			}

			PanelsComboBox.Items.Add(Settings.Instance.CodeEditorFont);

			PanelsComboBox.Sorted = true;
			PanelsComboBox.EndUpdate();
			PanelsComboBox.Dock = DockStyle.Fill;
			PanelsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			PanelsComboBox.SelectedIndexChanged += new EventHandler(panelsComboBox_SelectedIndexChanged);
			panel.Controls.Add(PanelsComboBox);
		}

		private void CreateExampleTextBox(TableLayoutPanel panel)
		{
			ExampleTextBox = new TextBox();
			ExampleTextBox.Dock = DockStyle.Fill;

			panel.Controls.Add(ExampleTextBox);
		}

		private void CreateChangeFontButton(Panel panel)
		{
			Button changeFontButton = new Button();
			changeFontButton.AutoSize = true;
			changeFontButton.Text = "Change font...";
			changeFontButton.Click += new EventHandler(changeFontButton_Click);

			panel.Controls.Add(changeFontButton);
		}

		private void CreateResetFontButton(Panel panel)
		{
			Button resetFontButton = new Button();
			resetFontButton.AutoSize = true;
			resetFontButton.Text = "Reset font";
			resetFontButton.Click += new EventHandler(resetFontButton_Click);

			panel.Controls.Add(resetFontButton);
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.RowCount = 3;
			panel.ColumnCount = 1;

			CreatePanelsComboBox(panel);
			CreateExampleTextBox(panel);

			Panel buttonsPanel = new Panel();
			Panel changeFontPanel = new Panel();
			Panel resetFontPanel = new Panel();

			buttonsPanel.Dock = DockStyle.Fill;
			changeFontPanel.AutoSize = true;
			changeFontPanel.Dock = DockStyle.Left;
			resetFontPanel.AutoSize = true;
			resetFontPanel.Dock = DockStyle.Left;

			CreateChangeFontButton(changeFontPanel);
			CreateResetFontButton(resetFontPanel);

			buttonsPanel.Controls.Add(resetFontPanel);
			buttonsPanel.Controls.Add(changeFontPanel);
			panel.Controls.Add(buttonsPanel);

			PanelsComboBox.SelectedIndex = 0;
		}

		public override void ReadSettings()
		{
			foreach (IChangebleFont changebleFont in panelsComboBox.Items)
			{
				if (changebleFont.TempFont != null)
				{
					if (changebleFont.DockContent != null)
					{
						changebleFont.DockContent.Font = changebleFont.TempFont.GetFont();
					}

					changebleFont.SerializableFont = changebleFont.TempFont;
				}
			}
		}

		private void panelsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			IChangebleFont changebleFont = panelsComboBox.SelectedItem as IChangebleFont;

			if (changebleFont != null)
			{
				if (changebleFont.TempFont == null)
				{
					ExampleTextBox.Font = changebleFont.Font;
				}
				else
				{
					ExampleTextBox.Font = changebleFont.TempFont.GetFont();
				}

				ExampleTextBox.Text = ExampleText;
			}
		}

		private void changeFontButton_Click(object sender, EventArgs e)
		{
			IChangebleFont changebleFont = panelsComboBox.SelectedItem as IChangebleFont;

			if (changebleFont != null)
			{
				FontDialog fontDialog = new FontDialog();
				fontDialog.Font = changebleFont.Font;

				if (fontDialog.ShowDialog() == DialogResult.OK)
				{
					changebleFont.TempFont = new SerializableFont(fontDialog.Font);
					ExampleTextBox.Font = fontDialog.Font;
				}
			}
		}

		private void resetFontButton_Click(object sender, EventArgs e)
		{
			IChangebleFont changebleFont = panelsComboBox.SelectedItem as IChangebleFont;

			if (changebleFont != null)
			{
				changebleFont.TempFont = changebleFont.DefaultFont;
				ExampleTextBox.Font = changebleFont.TempFont.GetFont();
			}
		}

		public override string ToString()
		{
			return "Fonts";
		}
	}
}