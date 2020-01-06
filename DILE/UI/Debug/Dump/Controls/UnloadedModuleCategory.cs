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
	public partial class UnloadedModuleCategory : UserControl, ICategoryControl
	{
		public UnloadedModuleCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			lvUnloadedModuleDetails.BeginUpdate();

			foreach (ListViewItem item in lvUnloadedModuleDetails.Items)
			{
				if (item.Tag != null)
				{
					item.SubItems[1].Text = HelperFunctions.FormatNumber(item.Tag);
				}
			}

			lvUnloadedModuleDetails.EndUpdate();
		}

		public void Initialize(DumpDebugger dumpDebugger)
		{
			lstUnloadedModules.BeginUpdate();
			lstUnloadedModules.Items.Clear();

			foreach (DumpUnloadedModuleInfo unloadedModule in dumpDebugger.GetUnloadedModules()
				.OrderBy(module => module.Name))
			{
				lstUnloadedModules.Items.Add(new ToStringWrapper<DumpUnloadedModuleInfo>(unloadedModule, unloadedModule.Name));
			}

			lstUnloadedModules.EndUpdate();
		}

		private void lstUnloadedModules_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToStringWrapper<DumpUnloadedModuleInfo> unloadedModuleWrapper =
				lstUnloadedModules.SelectedItem as ToStringWrapper<DumpUnloadedModuleInfo>;

			if (unloadedModuleWrapper != null)
			{
				lvUnloadedModuleDetails.BeginUpdate();
				lvUnloadedModuleDetails.Items.Clear();

				lvUnloadedModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Base Address",
					HelperFunctions.FormatNumber(unloadedModuleWrapper.WrappedObject.BaseAddress)
				})).Tag = unloadedModuleWrapper.WrappedObject.BaseAddress;

				lvUnloadedModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Checksum",
					HelperFunctions.FormatNumber(unloadedModuleWrapper.WrappedObject.Checksum)
				})).Tag = unloadedModuleWrapper.WrappedObject.Checksum;

				lvUnloadedModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Name",
					unloadedModuleWrapper.WrappedObject.Name
				}));

				lvUnloadedModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Size",
					HelperFunctions.FormatNumber(unloadedModuleWrapper.WrappedObject.Size)
				})).Tag = unloadedModuleWrapper.WrappedObject.Size;

				lvUnloadedModuleDetails.Items.Add(new ListViewItem(new string[]
				{
					"Timestamp",
					Convert.ToString(unloadedModuleWrapper.WrappedObject.Timestamp.ToLocalTime())
				}));

				lvUnloadedModuleDetails.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvUnloadedModuleDetails.EndUpdate();
			}
		}
	}
}