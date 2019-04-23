namespace Dile.UI
{
	partial class ThreadsPanel
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.threadsGrid = new Dile.Controls.CustomDataGridView();
			this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.threadNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.appDomainColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.threadsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// threadsGrid
			// 
			this.threadsGrid.AllowUserToAddRows = false;
			this.threadsGrid.AllowUserToDeleteRows = false;
			this.threadsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.threadsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.threadsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.threadsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.threadsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.threadsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.threadNameColumn,
            this.appDomainColumn});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.threadsGrid.DefaultCellStyle = dataGridViewCellStyle2;
			this.threadsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.threadsGrid.Location = new System.Drawing.Point(0, 0);
			this.threadsGrid.MultiSelect = true;
			this.threadsGrid.Name = "threadsGrid";
			this.threadsGrid.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.threadsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.threadsGrid.RowHeadersVisible = false;
			this.threadsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.threadsGrid.ShowEditingIcon = false;
			this.threadsGrid.Size = new System.Drawing.Size(381, 273);
			this.threadsGrid.TabIndex = 1;
			this.threadsGrid.Text = "dataGridView1";
			this.threadsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.threadsGrid_CellDoubleClick);
			// 
			// idColumn
			// 
			this.idColumn.HeaderText = "ID";
			this.idColumn.Name = "idColumn";
			this.idColumn.ReadOnly = true;
			this.idColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.idColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.idColumn.Width = 22;
			// 
			// threadNameColumn
			// 
			this.threadNameColumn.FillWeight = 164.8352F;
			this.threadNameColumn.HeaderText = "Thread name";
			this.threadNameColumn.Name = "threadNameColumn";
			this.threadNameColumn.ReadOnly = true;
			this.threadNameColumn.Width = 93;
			// 
			// appDomainColumn
			// 
			this.appDomainColumn.HeaderText = "AppDomain";
			this.appDomainColumn.Name = "appDomainColumn";
			this.appDomainColumn.ReadOnly = true;
			this.appDomainColumn.Width = 85;
			// 
			// ThreadsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(381, 273);
			this.Controls.Add(this.threadsGrid);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "ThreadsPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Threads Panel";
			this.Text = "Threads Panel";
			((System.ComponentModel.ISupportInitialize)(this.threadsGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.CustomDataGridView threadsGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn threadNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn appDomainColumn;

	}
}