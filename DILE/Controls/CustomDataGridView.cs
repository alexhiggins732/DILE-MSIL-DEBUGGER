using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.ComponentModel;
using System.Windows.Forms;

namespace Dile.Controls
{
	public class CustomDataGridView : DataGridView
	{
		private ContextMenuStrip rowContextMenu;
		public ContextMenuStrip RowContextMenu
		{
			get
			{
				return rowContextMenu;
			}
			private set
			{
				rowContextMenu = value;
			}
		}

		private void InitializeItemContextMenu()
		{
			RowContextMenu = new ContextMenuStrip();
			ToolStripMenuItem copyMenu = (ToolStripMenuItem)RowContextMenu.Items.Add("Copy to clipboard");
			ToolStripMenuItem displayTextMenu = (ToolStripMenuItem)RowContextMenu.Items.Add("Display text...");

			for (int index = 0; index < Columns.Count; index++)
			{
				DataGridViewColumn columnHeader = Columns[index];

				ToolStripItem copyChildMenu = copyMenu.DropDownItems.Add(columnHeader.HeaderText);
				copyChildMenu.Click += new EventHandler(copyMenu_Click);
				copyChildMenu.Tag = columnHeader;

				ToolStripItem displayTextChildMenu = displayTextMenu.DropDownItems.Add(columnHeader.HeaderText);
				displayTextChildMenu.Click += new EventHandler(displayTextMenu_Click);
				displayTextChildMenu.Tag = columnHeader;
			}

			if (Columns.Count > 1)
			{
				copyMenu.DropDownItems.Add("-");
				displayTextMenu.DropDownItems.Add("-");

				ToolStripItem copyFullRowMenu = copyMenu.DropDownItems.Add("Full row");
				copyFullRowMenu.Click += new EventHandler(copyMenu_Click);

				ToolStripItem displayTextFullRowMenu = displayTextMenu.DropDownItems.Add("Full row");
				displayTextFullRowMenu.Click += new EventHandler(displayTextMenu_Click);
			}
		}

		private void copyMenu_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(GetChosenMenuText(sender));
		}

		private void displayTextMenu_Click(object sender, EventArgs e)
		{
			TextDisplayer.Instance.ShowText(GetChosenMenuText(sender));
		}

		private string GetChosenMenuText(object sender)
		{
			ToolStripItem menuItem = (ToolStripItem)sender;
			StringBuilder result = new StringBuilder();

			if (menuItem.Tag == null)
			{
				for (int rowIndex = 0; rowIndex < SelectedRows.Count; rowIndex++)
				{
					DataGridViewRow selectedRow = SelectedRows[rowIndex];

					for (int cellIndex = 0; cellIndex < selectedRow.Cells.Count; cellIndex++)
					{
						DataGridViewCell cell = selectedRow.Cells[cellIndex];

						if (cell.Value != null)
						{
							result.Append(Convert.ToString(cell.Value));

							if (cellIndex < selectedRow.Cells.Count - 1)
							{
								result.Append("\t");
							}
						}
					}
				}
			}
			else
			{
				DataGridViewColumn sourceColumn = (DataGridViewColumn)menuItem.Tag;

				for (int rowIndex = 0; rowIndex < SelectedRows.Count; rowIndex++)
				{
					DataGridViewRow selectedRow = SelectedRows[rowIndex];
					DataGridViewCell cell = selectedRow.Cells[sourceColumn.Index];

					if (cell.Value == null)
					{
						result.AppendLine();
					}
					else
					{
						if (rowIndex > 0)
						{
							result.AppendLine();
						}
						result.Append(Convert.ToString(cell.Value));
					}
				}
			}

			return result.ToString();
		}

		public void Initialize()
		{
			InitializeItemContextMenu();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if ((e.Button & MouseButtons.Right) == MouseButtons.Right)
			{
				HitTestInfo hitTest = HitTest(e.X, e.Y);

				if (hitTest.Type == DataGridViewHitTestType.Cell)
				{
					Rows[hitTest.RowIndex].Selected = true;
					RowContextMenu.Show(this, e.Location);
				}
			}
		}

		public void BeginGridUpdate()
		{
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
		}

		public void EndGridUpdate()
		{
			AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			if (SortedColumn != null)
			{
				ListSortDirection sortDirection = (SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

				Sort(SortedColumn, sortDirection);
			}
		}

		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && !Rows[e.RowIndex].Selected)
			{
				ClearSelection();
			}

			base.OnCellMouseDown(e);
		}
	}
}