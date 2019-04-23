using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;
using Dile.UI.Debug;

namespace Dile.UI
{
	public partial class BreakpointsPanel : BasePanel
	{
		private ToolStripMenuItem displayCodeMenuItem;
		private ToolStripMenuItem DisplayCodeMenuItem
		{
			get
			{
				return displayCodeMenuItem;
			}
			set
			{
				displayCodeMenuItem = value;
			}
		}

		private ToolStripMenuItem activateBreakpointMenuItem;
		private ToolStripMenuItem ActivateBreakpointMenuItem
		{
			get
			{
				return activateBreakpointMenuItem;
			}
			set
			{
				activateBreakpointMenuItem = value;
			}
		}

		private ToolStripMenuItem deactivateBreakpointMenuItem;
		private ToolStripMenuItem DeactivateBreakpointMenuItem
		{
			get
			{
				return deactivateBreakpointMenuItem;
			}
			set
			{
				deactivateBreakpointMenuItem = value;
			}
		}

		private ToolStripMenuItem removeBreakpointMenuItem;
		private ToolStripMenuItem RemoveBreakpointMenuItem
		{
			get
			{
				return removeBreakpointMenuItem;
			}
			set
			{
				removeBreakpointMenuItem = value;
			}
		}

		public BreakpointsPanel()
		{
			InitializeComponent();

			breakpointsGrid.Initialize();

			DisplayCodeMenuItem = new ToolStripMenuItem("Display breakpoint location");
			DisplayCodeMenuItem.Click += new EventHandler(DisplayCodeMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(0, DisplayCodeMenuItem);

			ActivateBreakpointMenuItem = new ToolStripMenuItem("Activate breakpoint");
			ActivateBreakpointMenuItem.Click += new EventHandler(ActivateBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(1, ActivateBreakpointMenuItem);

			DeactivateBreakpointMenuItem = new ToolStripMenuItem("Deactivate breakpoint");
			DeactivateBreakpointMenuItem.Click += new EventHandler(DeactivateBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(2, DeactivateBreakpointMenuItem);

			RemoveBreakpointMenuItem = new ToolStripMenuItem("Remove breakpoint");
			RemoveBreakpointMenuItem.Click += new EventHandler(RemoveBreakpointMenuItem_Click);
			breakpointsGrid.RowContextMenu.Items.Insert(3, RemoveBreakpointMenuItem);

			breakpointsGrid.RowContextMenu.Opening += new CancelEventHandler(RowContextMenu_Opening);
		}

		private void RowContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool isOneRowSelected = (breakpointsGrid.SelectedRows.Count == 1);

			DisplayCodeMenuItem.Enabled = isOneRowSelected;
			ActivateBreakpointMenuItem.Enabled = isOneRowSelected;
			DeactivateBreakpointMenuItem.Enabled = isOneRowSelected;
			RemoveBreakpointMenuItem.Enabled = isOneRowSelected;

			if (isOneRowSelected)
			{
				BreakpointInformation breakpoint = breakpointsGrid.SelectedRows[0].Tag as BreakpointInformation;
				bool breakpointActive = (breakpoint.State == BreakpointState.Active);

				ActivateBreakpointMenuItem.Visible = !breakpointActive;
				DeactivateBreakpointMenuItem.Visible = breakpointActive;
			}
		}

		private void DisplayCodeMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedRows.Count == 1)
			{
				BreakpointInformation breakpoint = breakpointsGrid.SelectedRows[0].Tag as BreakpointInformation;

				if (breakpoint != null)
				{
					breakpoint.NavigateTo();
				}
			}
		}

		private void ActivateBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedRows.Count == 1)
			{
				FunctionBreakpointInformation functionBreakpoint = breakpointsGrid.SelectedRows[0].Tag as FunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					ActivateBreakpoint(functionBreakpoint);
					UIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
				}
			}
		}

		private void DeactivateBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedRows.Count == 1)
			{
				FunctionBreakpointInformation functionBreakpoint = breakpointsGrid.SelectedRows[0].Tag as FunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					DeactivateBreakpoint(functionBreakpoint);
					UIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
				}
			}
		}

		private void RemoveBreakpointMenuItem_Click(object sender, EventArgs e)
		{
			if (breakpointsGrid.SelectedRows.Count == 1)
			{
				FunctionBreakpointInformation functionBreakpoint = breakpointsGrid.SelectedRows[0].Tag as FunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					RemoveBreakpoint(functionBreakpoint);
					breakpointsGrid.Rows.Remove(breakpointsGrid.SelectedRows[0]);
				}
			}
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			breakpointsGrid.Rows.Clear();
		}

		public void DisplayBreakpoints()
		{
			breakpointsGrid.BeginGridUpdate();
			breakpointsGrid.Rows.Clear();

			foreach (FunctionBreakpointInformation functionBreakpoint in Project.Instance.FunctionBreakpoints)
			{
				AddBreakpoint(functionBreakpoint);
			}

			breakpointsGrid.EndGridUpdate();
		}

		public void AddBreakpoint(BreakpointInformation breakpoint)
		{
			DataGridViewRow row = breakpointsGrid.Rows[breakpointsGrid.Rows.Add()];

			row.Cells[0].Value = (breakpoint.State == BreakpointState.Active);
			row.Cells[1].Value = breakpoint.DisplayName;
			row.Cells[2].Value = breakpoint.OffsetValue;
			row.Tag = breakpoint;
		}

		private DataGridViewRow FindRow(BreakpointInformation breakpoint)
		{
			DataGridViewRow result = null;
			int index = 0;
			bool found = false;

			while (!found && index < breakpointsGrid.Rows.Count)
			{
				DataGridViewRow row = breakpointsGrid.Rows[index++];

				if (breakpoint.CompareTo(row.Tag) == 0)
				{
					result = row;
					found = true;
				}
			}

			return result;
		}

		public void DeactivateBreakpoint(BreakpointInformation breakpoint)
		{
			ToggleBreakpointActive(breakpoint, false);
		}

		public void ActivateBreakpoint(BreakpointInformation breakpoint)
		{
			ToggleBreakpointActive(breakpoint, true);
		}

		private void ToggleBreakpointActive(BreakpointInformation breakpoint, bool isActive)
		{
			DataGridViewRow row = FindRow(breakpoint);
			breakpoint.State = (isActive ? BreakpointState.Active : BreakpointState.Inactive);

			if (row != null)
			{
				row.Cells[0].Value = isActive;
			}

			Project.Instance.IsSaved = false;
		}

		public void RemoveBreakpoint(BreakpointInformation breakpoint)
		{
			DataGridViewRow row = FindRow(breakpoint);
			breakpointsGrid.Rows.Remove(row);
			Project.Instance.IsSaved = false;
		}

		private void breakpointsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (breakpointsGrid.CurrentRow != null)
			{
				BreakpointInformation breakpoint = (BreakpointInformation)breakpointsGrid.CurrentRow.Tag;

				breakpoint.NavigateTo();
			}
		}

		private void breakpointsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				DataGridViewRow row = breakpointsGrid.Rows[e.RowIndex];
				DataGridViewCell cell = row.Cells[e.ColumnIndex];
				BreakpointInformation breakpoint = (BreakpointInformation)row.Tag;

				bool active = !((bool)cell.Value);
				breakpoint.State = (active ? BreakpointState.Active : BreakpointState.Inactive);
				cell.Value = active;

				FunctionBreakpointInformation functionBreakpoint = breakpoint as FunctionBreakpointInformation;

				if (functionBreakpoint != null)
				{
					UIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
				}

				Project.Instance.IsSaved = false;
			}
		}

		private void breakpointsGrid_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
		{
			FunctionBreakpointInformation functionBreakpoint = e.Row.Tag as FunctionBreakpointInformation;
			
			if (functionBreakpoint != null)
			{
				RemoveBreakpoint(functionBreakpoint);
			}
		}

		private static void RemoveBreakpoint(FunctionBreakpointInformation functionBreakpoint)
		{	
			functionBreakpoint.State = BreakpointState.Removed;
			UIHandler.Instance.UpdateBreakpoint(functionBreakpoint.MethodDefinition, functionBreakpoint);
			Project.Instance.FunctionBreakpoints.Remove(functionBreakpoint);
			Project.Instance.IsSaved = false;
		}
	}
}