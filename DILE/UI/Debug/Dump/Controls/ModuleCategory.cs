using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug.Dump;

namespace Dile.UI.Debug.Dump.Controls
{
	public partial class ModuleCategory : UserControl, ICategoryControl
	{
		public ModuleCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			lvModuleDetails.BeginUpdate();

			foreach (ListViewItem item in lvModuleDetails.Items)
			{
				if (item.Tag != null)
				{
					item.SubItems[1].Text = HelperFunctions.FormatNumber(item.Tag);
				}
			}

			lvModuleDetails.EndUpdate();
		}

		#region ICategoryControl Members
		public void Initialize(DumpDebugger dumpDebugger)
		{
			lstModules.BeginUpdate();
			lstModules.Items.Clear();

			foreach (DumpModuleInfo module in dumpDebugger.GetModules()
				.OrderBy(module => module.Name))
			{
				lstModules.Items.Add(new ToStringWrapper<DumpModuleInfo>(module, module.Name));
			}

			lstModules.EndUpdate();
		}
		#endregion

		private string CreateVersionString(Dile.Debug.Dump.Version version)
		{
			return version.Major + "." + version.Minor + "." + version.Revision + "." + version.Build;
		}

		private void lstModules_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToStringWrapper<DumpModuleInfo> moduleWrapper = lstModules.SelectedItem as ToStringWrapper<DumpModuleInfo>;
			if (moduleWrapper != null)
			{
				lvModuleDetails.BeginUpdate();
				lvModuleDetails.Items.Clear();

				lvModuleDetails.Items.Add(new ListViewItem(new string[] 
				{
					"Base Address",
					HelperFunctions.FormatNumber(moduleWrapper.WrappedObject.BaseAddress)
				})).Tag = moduleWrapper.WrappedObject.BaseAddress;

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Checksum",
					HelperFunctions.FormatNumber(moduleWrapper.WrappedObject.Checksum)
				})).Tag = moduleWrapper.WrappedObject.Checksum;

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"File Type",
					Convert.ToString(moduleWrapper.WrappedObject.FileType)
				}));

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"File Version",
					CreateVersionString(moduleWrapper.WrappedObject.FileVersion)
				}));

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Local Timestamp",
					Convert.ToString(moduleWrapper.WrappedObject.Timestamp.ToLocalTime())
				}));

				lvModuleDetails.Items.Add(new ListViewItem(new string[] { "Name", moduleWrapper.WrappedObject.Name }));

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Product Version",
					CreateVersionString(moduleWrapper.WrappedObject.ProductVersion)
				}));

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Size (in memory)",
					HelperFunctions.FormatNumber(moduleWrapper.WrappedObject.Size)
				})).Tag = moduleWrapper.WrappedObject.Size;

				lvModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"UTC Timestamp",
					Convert.ToString(moduleWrapper.WrappedObject.Timestamp.ToUniversalTime())
				}));

				lvModuleDetails.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvModuleDetails.EndUpdate();
			}
		}
	}
}