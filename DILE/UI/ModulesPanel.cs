using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using System.Diagnostics;
using System.IO;
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public partial class ModulesPanel : BasePanel
	{
		private ToolStripItem addModuleMenuItem;
		private ToolStripItem AddModuleMenuItem
		{
			get
			{
				return addModuleMenuItem;
			}
			set
			{
				addModuleMenuItem = value;
			}
		}

		private ToolStripMenuItem AddModuleFromDumpMenuItem
		{
			get;
			set;
		}

		public ModulesPanel()
		{
			InitializeComponent();
			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);

			modulesGrid.Initialize();
			modulesGrid.Sort(fileNameColumn, ListSortDirection.Ascending);
			modulesGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);

			AddModuleFromDumpMenuItem = new ToolStripMenuItem("Add module to project from memory dump");
			AddModuleFromDumpMenuItem.Click += new EventHandler(AddModuleFromDumpMenuItem_Click);
			modulesGrid.RowContextMenu.Items.Insert(0, AddModuleFromDumpMenuItem);

			AddModuleMenuItem = new ToolStripMenuItem("Add module to the project");
			AddModuleMenuItem.Click += new EventHandler(AddModuleMenuItem_Click);
			modulesGrid.RowContextMenu.Items.Insert(0, AddModuleMenuItem);
		}

		protected override bool IsDebugPanel()
		{
			return true;
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			modulesGrid.Tag = GetSelectedRowIndex();
			modulesGrid.Rows.Clear();
		}

		protected override void OnInitializePanel()
		{
			base.OnInitializePanel();

			ShowModules();
		}

		private void FormatNumberInCell(DataGridViewCell cell)
		{
			cell.Value = HelperFunctions.FormatNumber(cell.Tag);
		}

		private void AssignNumberValueToCell<T>(DataGridViewCell cell, T tag)
		{
			cell.Tag = tag;
			cell.Value = HelperFunctions.FormatNumber(tag);
		}

		private int GetSelectedRowIndex()
		{
			return (modulesGrid.SelectedRows.Count == 1 ? modulesGrid.SelectedRows[0].Index : 0);
		}

		private void SetSelectedRow(int rowIndex)
		{
			if (rowIndex < modulesGrid.Rows.Count)
			{
				while (modulesGrid.SelectedRows.Count > 0)
				{
					modulesGrid.SelectedRows[0].Selected = false;
				}

				modulesGrid.Rows[rowIndex].Selected = true;
				modulesGrid.FirstDisplayedScrollingRowIndex = rowIndex;
				modulesGrid.Tag = rowIndex;
			}
		}

		private void ShowModules()
		{
			int selectedRowIndex = (modulesGrid.Tag == null ? GetSelectedRowIndex() : (int)modulesGrid.Tag);

			modulesGrid.BeginGridUpdate();
			modulesGrid.Rows.Clear();

			List<ModuleWrapper> modules = DebugEventHandler.Instance.EventObjects.Process.GetModules();
			modulesGrid.Rows.Add(modules.Count);
			for (int index = 0; index < modules.Count; index++)
			{
				ModuleWrapper module = modules[index];
				DataGridViewRow row = modulesGrid.Rows[index];

				AddModuleToGrid(module, row);
			}

			modulesGrid.EndGridUpdate();
			SetSelectedRow(selectedRowIndex);
		}

		private void AddModuleToGrid(ModuleWrapper module, DataGridViewRow row)
		{
			AssignNumberValueToCell<uint>(row.Cells[0], module.GetToken());
			AssignNumberValueToCell<ulong>(row.Cells[1], module.GetBaseAddress());
			AssignNumberValueToCell<uint>(row.Cells[2], module.GetSize());

			row.Cells[3].Value = module.IsDynamic();
			row.Cells[4].Value = module.IsInMemory();

			string moduleName = module.GetName();

			try
			{
				row.Cells[5].Value = Path.GetFileName(moduleName);
			}
			catch
			{
			}

			row.Cells[6].Value = moduleName;

			AssemblyWrapper assembly = module.GetAssembly();
			row.Cells[7].Value = assembly.GetName();

			AppDomainWrapper appDomain = assembly.GetAppDomain();
			row.Cells[8].Value = appDomain.GetName();

			row.Tag = module;
		}

		public void AddModules(ModuleWrapper[] modules)
		{
			int selectedRowIndex = (modulesGrid.Tag == null ? GetSelectedRowIndex() : (int)modulesGrid.Tag);
			int rowsCount = modulesGrid.Rows.Count;

			modulesGrid.BeginGridUpdate();
			modulesGrid.Rows.Add(modules.Length);

			for (int index = 0; index < modules.Length; index++)
			{
				ModuleWrapper module = modules[index];
				DataGridViewRow row = modulesGrid.Rows[rowsCount + index];

				AddModuleToGrid(module, row);
			}

			modulesGrid.EndGridUpdate();
			SetSelectedRow(selectedRowIndex);
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			if (modulesGrid.Rows != null)
			{
				foreach (DataGridViewRow row in modulesGrid.Rows)
				{
					FormatNumberInCell(row.Cells[0]);
					FormatNumberInCell(row.Cells[1]);
					FormatNumberInCell(row.Cells[2]);
				}
			}
		}

		private void modulesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (modulesGrid.SelectedRows.Count > 0)
			{
				DataGridViewRow selectedRow = modulesGrid.SelectedRows[0];
				string moduleName = (string)selectedRow.Cells[nameColumn.Name].Value;

				if (!Project.Instance.IsAssemblyLoaded(moduleName))
				{
					string message = string.Format("Would you like to add the {0} assembly to the project?", moduleName);

					if (MessageBox.Show(message, "DILE - Load assembly?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
					{
						AddSelectedModuleToProject(selectedRow, false);
					}
				}
			}
		}

		private void AddModuleMenuItem_Click(object sender, EventArgs e)
		{
			DataGridViewRow selectedRow = modulesGrid.SelectedRows[0];

			AddSelectedModuleToProject(selectedRow, false);
		}

		private void AddModuleFromDumpMenuItem_Click(object sender, EventArgs e)
		{
			DataGridViewRow selectedRow = modulesGrid.SelectedRows[0];

			AddSelectedModuleToProject(selectedRow, true);
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			if (modulesGrid.SelectedRows.Count == 1)
			{
				DataGridViewRow selectedRow = modulesGrid.SelectedRows[0];
				string moduleName = (string)selectedRow.Cells[nameColumn.Name].Value;

				AddModuleMenuItem.Enabled = true;
				AddModuleMenuItem.Visible = false;

				if (!Project.Instance.IsAssemblyLoaded(moduleName))
				{
					AddModuleMenuItem.Visible = true;
					AddModuleFromDumpMenuItem.Visible = (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging);
				}
			}
			else
			{
				AddModuleMenuItem.Enabled = false;
			}
		}

		private void AddSelectedModuleToProject(DataGridViewRow selectedRow, bool forceLoadingFromMemory)
		{
			ModuleWrapper module = (ModuleWrapper)selectedRow.Tag;

			if (forceLoadingFromMemory || module.IsInMemory())
			{
				UIHandler.Instance.AddAssembly(module);
			}
			else
			{
				string moduleName = (string)selectedRow.Cells[nameColumn.Name].Value;
				UIHandler.Instance.AddAssembly(moduleName);
			}
		}
	}
}