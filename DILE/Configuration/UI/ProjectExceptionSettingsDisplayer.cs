using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;
using Dile.Disassemble;
using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class ProjectExceptionSettingsDisplayer : BaseSettingsDisplayer
	{
		private CustomDataGridView exceptionsGrid = null;
		private CustomDataGridView ExceptionsGrid
		{
			get
			{
				return exceptionsGrid;
			}
			set
			{
				exceptionsGrid = value;
			}
		}

		private void CreateExceptionGrid(TableLayoutPanel panel)
		{
			ExceptionsGrid = new CustomDataGridView();
			ExceptionsGrid.AllowUserToAddRows = false;
			ExceptionsGrid.AllowUserToDeleteRows = true;
			ExceptionsGrid.AutoGenerateColumns = false;
			ExceptionsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			ExceptionsGrid.BorderStyle = BorderStyle.None;
			ExceptionsGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
			ExceptionsGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
			ExceptionsGrid.ColumnHeadersHeight *= 2;
			ExceptionsGrid.Dock = DockStyle.Fill;
			ExceptionsGrid.MultiSelect = true;
			ExceptionsGrid.RowHeadersVisible = false;
			ExceptionsGrid.ScrollBars = ScrollBars.Both;
			ExceptionsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			DataGridViewCheckBoxColumn skipColumn = new DataGridViewCheckBoxColumn();
			skipColumn.DataPropertyName = "Skip";
			skipColumn.HeaderText = "Skip";
			ExceptionsGrid.Columns.Add(skipColumn);

			DataGridViewTextBoxColumn assemblyPathColumn = new DataGridViewTextBoxColumn();
			assemblyPathColumn.DataPropertyName = "AssemblyPath";
			assemblyPathColumn.HeaderText = "Assembly";
			assemblyPathColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(assemblyPathColumn);

			DataGridViewTextBoxColumn tokenColumn = new DataGridViewTextBoxColumn();
			tokenColumn.DataPropertyName = "ExceptionClassTokenString";
			tokenColumn.HeaderText = "Exception Token";
			tokenColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(tokenColumn);

			DataGridViewTextBoxColumn classNameColumn = new DataGridViewTextBoxColumn();
			classNameColumn.DataPropertyName = "ExceptionClassName";
			classNameColumn.HeaderText = "Exception Class";
			classNameColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(classNameColumn);

			DataGridViewTextBoxColumn methodTokenColumn = new DataGridViewTextBoxColumn();
			methodTokenColumn.DataPropertyName = "ThrowingMethodTokenString";
			methodTokenColumn.HeaderText = "Method Token";
			methodTokenColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(methodTokenColumn);

			DataGridViewTextBoxColumn methodNameColumn = new DataGridViewTextBoxColumn();
			methodNameColumn.DataPropertyName = "ThrowingMethodName";
			methodNameColumn.HeaderText = "Method Name";
			methodNameColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(methodNameColumn);

			DataGridViewTextBoxColumn ipColumn = new DataGridViewTextBoxColumn();
			ipColumn.DataPropertyName = "IPAsString";
			ipColumn.HeaderText = "IP";
			ipColumn.ReadOnly = true;
			ExceptionsGrid.Columns.Add(ipColumn);

			BindingSource bindingSource = new BindingSource(Project.Instance.Exceptions, string.Empty);
			ExceptionsGrid.DataSource = bindingSource;

			panel.Controls.Add(ExceptionsGrid);

			ExceptionsGrid.Initialize();
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 1;

			CreateExceptionGrid(panel);
		}

		public override void ReadSettings()
		{
		}

		public override string ToString()
		{
			return "Exception handling";
		}
	}
}