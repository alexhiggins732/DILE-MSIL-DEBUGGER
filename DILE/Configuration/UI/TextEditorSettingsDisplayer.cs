using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class TextEditorSettingsDisplayer : BaseSettingsDisplayer
	{
		private CheckBox EscapeNamesCheckBox
		{
			get;
			set;
		}

		private RadioButton EscapeUsingCharacterCodeRadioButton
		{
			get;
			set;
		}

		private RadioButton EscapeUsingTokenRadioButton
		{
			get;
			set;
		}

		private CheckBox CopyILAddressesCheckBox
		{
			get;
			set;
		}

		private RadioButton CreateIndentedRadioButton(TableLayoutPanel panel, string description, bool enabled, bool isChecked)
		{
			RadioButton result;

			Label descriptionLabel = new Label();
			descriptionLabel.AutoSize = true;
			descriptionLabel.Margin = new Padding(15, 0, 0, 0);
			descriptionLabel.Text = description;
			panel.Controls.Add(descriptionLabel);

			result = new RadioButton();
			result.AutoSize = true;
			result.Checked = isChecked;
			result.Enabled = enabled;
			panel.Controls.Add(result);

			return result;
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 5;

			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));

			bool isEscapingEnabled;
			bool isEscapingUsingCharacterCodesEnabled;
			switch (Settings.Instance.NameEscapingType)
			{
				case EscapingType.None:
					isEscapingEnabled = false;
					isEscapingUsingCharacterCodesEnabled = false;
					break;

				case EscapingType.Token:
					isEscapingEnabled = true;
					isEscapingUsingCharacterCodesEnabled = false;
					break;

				case EscapingType.CharacterCode:
				default:
					isEscapingEnabled = true;
					isEscapingUsingCharacterCodesEnabled = true;
					break;
			}

			EscapeNamesCheckBox = CreateCheckBox(panel, "Escape undisplayable characters in names.", isEscapingEnabled);
			EscapeUsingCharacterCodeRadioButton = CreateIndentedRadioButton(panel, "Using character codes", isEscapingEnabled, isEscapingUsingCharacterCodesEnabled);
			EscapeUsingTokenRadioButton = CreateIndentedRadioButton(panel, "Using tokens", isEscapingEnabled, !isEscapingUsingCharacterCodesEnabled);
			CopyILAddressesCheckBox = CreateCheckBox(panel, "Copy text to clipboard with IL addresses.", Settings.Instance.CopyILAddresses);

			EscapeNamesCheckBox.CheckedChanged += EscapeNamesCheckBox_CheckedChanged;
		}

		private void EscapeNamesCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			EscapeUsingCharacterCodeRadioButton.Enabled = EscapeNamesCheckBox.Checked;
			EscapeUsingTokenRadioButton.Enabled = EscapeNamesCheckBox.Checked;

			if (EscapeNamesCheckBox.Checked
				&& Settings.Instance.NameEscapingType == EscapingType.None)
			{
				EscapeUsingCharacterCodeRadioButton.Checked = true;
			}
		}

		public override void ReadSettings()
		{
			if (EscapeNamesCheckBox.Checked)
			{
				Settings.Instance.NameEscapingType = (EscapeUsingTokenRadioButton.Checked ? EscapingType.Token : EscapingType.CharacterCode);
			}
			else
			{
				Settings.Instance.NameEscapingType = EscapingType.None;
			}

			Settings.Instance.CopyILAddresses = CopyILAddressesCheckBox.Checked;
		}

		public override string ToString()
		{
			return "Text Editor";
		}
	}
}