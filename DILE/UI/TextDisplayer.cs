using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;

namespace Dile.UI
{
	public partial class TextDisplayer : Form
	{
		private static TextDisplayer instance;
		public static TextDisplayer Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new TextDisplayer();
				}

				return instance;
			}
		}

		public TextDisplayer()
		{
			InitializeComponent();
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			webBrowser.DocumentText = string.Empty;
			Close();
		}

		private void wordWrapCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateWordWrap();
		}

		private void UpdateWordWrap()
		{
			if (wordWrapCheckBox.Checked)
			{
				textBox.ScrollBars = ScrollBars.Vertical;
				textBox.WordWrap = true;
			}
			else
			{
				textBox.ScrollBars = ScrollBars.Both;
				textBox.WordWrap = false;
			}
		}

		public void ShowText(string text)
		{
			try
			{
				if (!text.StartsWith("\"") && !text.StartsWith("'"))
				{
					textBox.Text = string.Empty;
					escapeCharactersCheckBox.Checked = false;
					textBox.Text = text;
				}
				else if (escapeCharactersCheckBox.Checked)
				{
					textBox.Text = text;
				}
				else
				{
					textBox.Text = HelperFunctions.ConvertEscapedCharacters(text, true);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("An exception occurred while trying to display the text:\n\n" + exception.ToString());
			}

			textBox.SelectionStart = 0;
			textBox.SelectionLength = 0;

			textTab.SelectedTab = textEditorTabPage;
			ShowDialog();
		}

		private void escapeCharactersCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (escapeCharactersCheckBox.Checked)
				{
					textBox.Text = HelperFunctions.ShowEscapeCharacters(textBox.Text, true);
				}
				else
				{
					textBox.Text = HelperFunctions.ConvertEscapedCharacters(textBox.Text, true);
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show("An exception occurred while trying to display the text:\n\n" + exception.ToString());
			}
		}

		private void reformatXmlButton_Click(object sender, EventArgs e)
		{
			XmlWriter xmlWriter = null;
			StringWriter stringWriter = null;

			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(textBox.Text);

				stringWriter = new StringWriter();
				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "\t";
				settings.OmitXmlDeclaration = true;
				xmlWriter = XmlTextWriter.Create(stringWriter, settings);

				xmlDocument.Save(xmlWriter);
				textBox.Text = stringWriter.ToString();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "DILE - XML Reformatting Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
			finally
			{
				if (xmlWriter != null)
				{
					xmlWriter.Close();
				}

				if (stringWriter != null)
				{
					stringWriter.Close();
				}
			}
		}

		private void textTab_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if (e.TabPage == webBrowserTabPage)
			{
				webBrowser.DocumentText = textBox.Text;
			}
		}
	}
}