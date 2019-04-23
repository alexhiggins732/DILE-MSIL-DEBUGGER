using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug;
using System.Diagnostics;

namespace Dile.UI
{
	public partial class AttachProcessDialog : Form
	{
		public int ProcessID
		{
			get;
			private set;
		}

		private ToolStripMenuItem AttachProcessMenuItem
		{
			get;
			set;
		}

		private ToolStripMenuItem RefreshProcessesMenuItem
		{
			get;
			set;
		}

		public bool IncludeUnmanagedProcesses
		{
			get;
			private set;
		}

		public AttachProcessDialog(bool includeUnmanagedProcesses)
		{
			IncludeUnmanagedProcesses = includeUnmanagedProcesses;

			InitializeComponent();

			managedProcessesGrid.Initialize();

			AttachProcessMenuItem = new ToolStripMenuItem("Attach to process");
			AttachProcessMenuItem.Click += new EventHandler(AttachProcessMenuItem_Click);
			managedProcessesGrid.RowContextMenu.Items.Insert(0, AttachProcessMenuItem);

			RefreshProcessesMenuItem = new ToolStripMenuItem("Refresh processes");
			RefreshProcessesMenuItem.Click += new EventHandler(RefreshProcessesMenuItem_Click);
			managedProcessesGrid.RowContextMenu.Items.Insert(1, RefreshProcessesMenuItem);

			managedProcessesGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);

			managedProcessesGrid.Sort(processNameColumn, ListSortDirection.Ascending);
		}

		private void SetProcessID()
		{
			if (managedProcessesGrid.CurrentRow != null)
			{
				ProcessID = ((ProcessDescription)managedProcessesGrid.CurrentRow.Tag).ID;

				DialogResult = DialogResult.OK;
			}
		}

		private void AttachProcessDialog_Load(object sender, EventArgs e)
		{
			nudAutomaticRefreshInterval.Value = Settings.Instance.AutomaticRefreshInterval;

			ShowProcesses();
		}

		private void ShowProcesses()
		{
			using (MetaHost metaHost = new MetaHost())
			{
				managedProcessesGrid.BeginGridUpdate();
				int selectedRowIndex = (managedProcessesGrid.SelectedRows.Count == 1 ? managedProcessesGrid.SelectedRows[0].Index : 0);
				managedProcessesGrid.Rows.Clear();
				int dileProcessId = Process.GetCurrentProcess().Id;

				foreach (Process process in Process.GetProcesses())
				{
					ProcessDescription processDescription = new ProcessDescription(metaHost, process);

					if ((IncludeUnmanagedProcesses || processDescription.IsManaged) && processDescription.ID != dileProcessId)
					{
						managedProcessesGrid.Rows.Add(processDescription.ID,
							processDescription.Framework,
							processDescription.Name,
							processDescription.MainWindowTitle,
							processDescription.UserName,
							processDescription.FileName);
						managedProcessesGrid.Rows[managedProcessesGrid.Rows.Count - 1].Tag = processDescription;
					}
				}

				managedProcessesGrid.EndGridUpdate();

				if (selectedRowIndex < managedProcessesGrid.Rows.Count)
				{
					while (managedProcessesGrid.SelectedRows.Count > 0)
					{
						managedProcessesGrid.SelectedRows[0].Selected = false;
					}

					managedProcessesGrid.Rows[selectedRowIndex].Selected = true;
					managedProcessesGrid.FirstDisplayedScrollingRowIndex = selectedRowIndex;
				}
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			SetProcessID();
		}

		private void managedProcessesGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			SetProcessID();
		}

		private void AttachProcessMenuItem_Click(object sender, EventArgs e)
		{
			SetProcessID();
		}

		private void RefreshProcessesMenuItem_Click(object sender, EventArgs e)
		{
			ShowProcesses();
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			AttachProcessMenuItem.Enabled = (managedProcessesGrid.SelectedRows.Count == 1);
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			ShowProcesses();
		}

		private void cbAutomaticRefresh_CheckedChanged(object sender, EventArgs e)
		{
			if (cbAutomaticRefresh.Checked)
			{
				tmrAutomaticRefresh.Interval = Convert.ToInt32(nudAutomaticRefreshInterval.Value);
				tmrAutomaticRefresh.Enabled = true;
			}
			else
			{
				tmrAutomaticRefresh.Enabled = false;
			}
		}

		private void tmrAutomaticRefresh_Tick(object sender, EventArgs e)
		{
			tmrAutomaticRefresh.Enabled = false;

			ShowProcesses();

			if (cbAutomaticRefresh.Checked)
			{
				tmrAutomaticRefresh.Interval = Convert.ToInt32(nudAutomaticRefreshInterval.Value);
				tmrAutomaticRefresh.Enabled = true;
			}
		}
	}
}