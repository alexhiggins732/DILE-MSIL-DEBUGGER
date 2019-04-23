using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Debug.Dump;
using System.IO;
using WeifenLuo.WinFormsUI.Docking;

using Dile.UI.Debug.Dump.Controls;

namespace Dile.UI
{
	public partial class MemoryDumpInfoPage : DockContent
	{
		public DumpDebugger DumpDebugger
		{
			get;
			private set;
		}

		public MemoryDumpInfoPage(DumpDebugger dumpDebugger)
		{
			InitializeComponent();

			DumpDebugger = dumpDebugger;

			string dumpFilePath = DumpDebugger.GetDumpFilePath();
			txtFilePath.Text = dumpFilePath;
			txtLastWriteTime.Text = Convert.ToString(File.GetLastWriteTime(dumpFilePath));

			PopulateCategories();
		}

		private void PopulateCategories()
		{
			drpCategories.BeginUpdate();
			drpCategories.Items.Clear();

			drpCategories.Items.Add(new CategoryDisplayer<ModuleCategory>("Modules"));
			drpCategories.Items.Add(new CategoryDisplayer<ExceptionCategory>("Exception"));
			drpCategories.Items.Add(new CategoryDisplayer<SystemInfoCategory>("System"));
			drpCategories.Items.Add(new CategoryDisplayer<ThreadCategory>("Thread Info"));
			drpCategories.Items.Add(new CategoryDisplayer<UnloadedModuleCategory>("Unloaded Modules"));
			drpCategories.Items.Add(new CategoryDisplayer<MiscInfoCategory>("Miscellaneous Info"));

			drpCategories.SelectedIndex = -1;
			drpCategories.EndUpdate();
		}

		private void drpCategories_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (drpCategories.SelectedItem != null)
			{
				pnlCategoryControl.Controls.Clear();

				try
				{
					ICategoryDisplayer categoryDisplayer = (ICategoryDisplayer)drpCategories.SelectedItem;
					Control categoryControl = categoryDisplayer.CreateControl(DumpDebugger);
					if (categoryControl != null)
					{
						categoryControl.Dock = DockStyle.Fill;
						pnlCategoryControl.Controls.Add(categoryControl);
					}
				}
				catch (Exception exception)
				{
					lblCategoryNA.Text = "An error occurred while trying to display the category."
						+ Environment.NewLine
						+ "Error message: "
						+ exception.Message;

					pnlCategoryControl.Controls.Add(lblCategoryNA);
				}
			}
		}
	}
}