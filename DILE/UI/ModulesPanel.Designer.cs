namespace Dile.UI
{
	partial class ModulesPanel
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.modulesGrid = new Dile.Controls.CustomDataGridView();
			this.tokenColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.baseAddressColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.sizeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.isDynamicColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.isInMemoryColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.fileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.assemblyNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.appDomainNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.modulesGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// modulesGrid
			// 
			this.modulesGrid.AllowUserToAddRows = false;
			this.modulesGrid.AllowUserToDeleteRows = false;
			this.modulesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.modulesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.modulesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.modulesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.modulesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.modulesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.modulesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tokenColumn,
            this.baseAddressColumn,
            this.sizeColumn,
            this.isDynamicColumn,
            this.isInMemoryColumn,
            this.fileNameColumn,
            this.nameColumn,
            this.assemblyNameColumn,
            this.appDomainNameColumn});
			this.modulesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modulesGrid.Location = new System.Drawing.Point(0, 0);
			this.modulesGrid.MultiSelect = true;
			this.modulesGrid.Name = "modulesGrid";
			this.modulesGrid.ReadOnly = true;
			this.modulesGrid.RowHeadersVisible = false;
			this.modulesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.modulesGrid.ShowEditingIcon = false;
			this.modulesGrid.Size = new System.Drawing.Size(292, 273);
			this.modulesGrid.TabIndex = 0;
			this.modulesGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.modulesGrid_CellDoubleClick);
			// 
			// tokenColumn
			// 
			this.tokenColumn.HeaderText = "Token";
			this.tokenColumn.MinimumWidth = 61;
			this.tokenColumn.Name = "tokenColumn";
			this.tokenColumn.ReadOnly = true;
			this.tokenColumn.Width = 61;
			// 
			// baseAddressColumn
			// 
			this.baseAddressColumn.HeaderText = "Base Address";
			this.baseAddressColumn.MinimumWidth = 95;
			this.baseAddressColumn.Name = "baseAddressColumn";
			this.baseAddressColumn.ReadOnly = true;
			this.baseAddressColumn.Width = 95;
			// 
			// sizeColumn
			// 
			this.sizeColumn.HeaderText = "Size";
			this.sizeColumn.MinimumWidth = 50;
			this.sizeColumn.Name = "sizeColumn";
			this.sizeColumn.ReadOnly = true;
			this.sizeColumn.Width = 50;
			// 
			// isDynamicColumn
			// 
			this.isDynamicColumn.HeaderText = "Dynamic";
			this.isDynamicColumn.MinimumWidth = 71;
			this.isDynamicColumn.Name = "isDynamicColumn";
			this.isDynamicColumn.ReadOnly = true;
			this.isDynamicColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.isDynamicColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.isDynamicColumn.Width = 71;
			// 
			// isInMemoryColumn
			// 
			this.isInMemoryColumn.HeaderText = "In Memory";
			this.isInMemoryColumn.MinimumWidth = 85;
			this.isInMemoryColumn.Name = "isInMemoryColumn";
			this.isInMemoryColumn.ReadOnly = true;
			this.isInMemoryColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.isInMemoryColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.isInMemoryColumn.Width = 85;
			// 
			// fileNameColumn
			// 
			this.fileNameColumn.HeaderText = "File Name";
			this.fileNameColumn.MinimumWidth = 85;
			this.fileNameColumn.Name = "fileNameColumn";
			this.fileNameColumn.ReadOnly = true;
			this.fileNameColumn.Width = 85;
			// 
			// nameColumn
			// 
			this.nameColumn.HeaderText = "Name";
			this.nameColumn.MinimumWidth = 71;
			this.nameColumn.Name = "nameColumn";
			this.nameColumn.ReadOnly = true;
			this.nameColumn.Width = 71;
			// 
			// assemblyNameColumn
			// 
			this.assemblyNameColumn.HeaderText = "Assembly Name";
			this.assemblyNameColumn.MinimumWidth = 110;
			this.assemblyNameColumn.Name = "assemblyNameColumn";
			this.assemblyNameColumn.ReadOnly = true;
			this.assemblyNameColumn.Width = 110;
			// 
			// appDomainNameColumn
			// 
			this.appDomainNameColumn.HeaderText = "AppDomain Name";
			this.appDomainNameColumn.MinimumWidth = 120;
			this.appDomainNameColumn.Name = "appDomainNameColumn";
			this.appDomainNameColumn.ReadOnly = true;
			this.appDomainNameColumn.Width = 120;
			// 
			// ModulesPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.modulesGrid);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "ModulesPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Modules Panel";
			this.Text = "Modules Panel";
			((System.ComponentModel.ISupportInitialize)(this.modulesGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.CustomDataGridView modulesGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn tokenColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn baseAddressColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn sizeColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn isDynamicColumn;
		private System.Windows.Forms.DataGridViewCheckBoxColumn isInMemoryColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn fileNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn assemblyNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn appDomainNameColumn;

	}
}