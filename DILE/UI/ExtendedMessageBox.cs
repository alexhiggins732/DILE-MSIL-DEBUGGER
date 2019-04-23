using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Dile.UI
{
	public partial class ExtendedMessageBox : Form
	{
		private ExtendedDialogResult extendedDialogResult = ExtendedDialogResult.None;
		public ExtendedDialogResult ExtendedDialogResult
		{
			get
			{
				return extendedDialogResult;
			}
			private set
			{
				extendedDialogResult = value;
			}
		}

		public ExtendedMessageBox()
		{
			InitializeComponent();
		}

		public void ShowMessage(string title, string message)
		{
			Text = title;
			messageTextBox.Text = message;
			messageTextBox.SelectionStart = 0;
			messageTextBox.SelectionLength = 0;

			ShowDialog();
		}

		private void yesButton_Click(object sender, EventArgs e)
		{
			ExtendedDialogResult = ExtendedDialogResult.Yes;
			Close();
		}

		private void noButton_Click(object sender, EventArgs e)
		{
			ExtendedDialogResult = ExtendedDialogResult.No;
			Close();
		}

		private void yesToAllButton_Click(object sender, EventArgs e)
		{
			ExtendedDialogResult = ExtendedDialogResult.YesToAll;
			Close();
		}

		private void noToAllButton_Click(object sender, EventArgs e)
		{
			ExtendedDialogResult = ExtendedDialogResult.NoToAll;
			Close();
		}
	}
}