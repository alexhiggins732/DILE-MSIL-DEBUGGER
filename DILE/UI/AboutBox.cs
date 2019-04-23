using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Dile.UI
{
	public partial class AboutBox : Form
	{
		public AboutBox()
		{
			InitializeComponent();
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void emailLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("mailto:" + emailLink.Text);
		}

		private void blogLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(blogLink.Text);
		}

		private void projectPageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(projectPageLink.Text);
		}

		private void AboutBox_Load(object sender, EventArgs e)
		{
			AssemblyName executingAssemblyName = Assembly.GetExecutingAssembly().GetName();
			string appName = string.Format("Dotnet IL Editor (DILE) v{0}", Settings.Instance.VersionNumber);
			appNameLabel.Text = appName;
		}
	}
}