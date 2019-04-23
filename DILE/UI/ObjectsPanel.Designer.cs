namespace Dile.UI
{
	partial class ObjectsPanel
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
			this.objectsGrid = new Dile.Controls.CustomDataGridView();
			this.valueNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).BeginInit();
			this.SuspendLayout();
			// 
			// objectsGrid
			// 
			this.objectsGrid.AllowUserToAddRows = false;
			this.objectsGrid.AllowUserToDeleteRows = false;
			this.objectsGrid.AllowUserToResizeRows = false;
			this.objectsGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.objectsGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.objectsGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.objectsGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.objectsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.objectsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.valueNameColumn,
            this.valueColumn});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.objectsGrid.DefaultCellStyle = dataGridViewCellStyle2;
			this.objectsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectsGrid.GridColor = System.Drawing.SystemColors.ActiveBorder;
			this.objectsGrid.Location = new System.Drawing.Point(0, 0);
			this.objectsGrid.MultiSelect = true;
			this.objectsGrid.Name = "objectsGrid";
			this.objectsGrid.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.objectsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.objectsGrid.RowHeadersVisible = false;
			this.objectsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.objectsGrid.ShowEditingIcon = false;
			this.objectsGrid.Size = new System.Drawing.Size(446, 158);
			this.objectsGrid.TabIndex = 7;
			this.objectsGrid.Text = "dataGridView1";
			this.objectsGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.objectsGrid_CellBeginEdit);
			this.objectsGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectsGrid_CellDoubleClick);
			this.objectsGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.objectsGrid_CellEndEdit);
			// 
			// valueNameColumn
			// 
			this.valueNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueNameColumn.HeaderText = "Local variable";
			this.valueNameColumn.Name = "valueNameColumn";
			this.valueNameColumn.ReadOnly = true;
			// 
			// valueColumn
			// 
			this.valueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.valueColumn.DataPropertyName = "Value";
			this.valueColumn.HeaderText = "Value";
			this.valueColumn.Name = "valueColumn";
			this.valueColumn.ReadOnly = true;
			// 
			// ObjectsPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(446, 158);
			this.Controls.Add(this.objectsGrid);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "ObjectsPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.CustomDataGridView objectsGrid;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueColumn;
	}
}