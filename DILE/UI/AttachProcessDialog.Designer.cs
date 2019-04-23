namespace Dile.UI
{
	partial class AttachProcessDialog
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttachProcessDialog));
			this.panel1 = new System.Windows.Forms.Panel();
			this.refreshButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.managedProcessesGrid = new Dile.Controls.CustomDataGridView();
			this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FrameworkVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.processNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.mainWindowTitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.userNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.processFileNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.cbAutomaticRefresh = new System.Windows.Forms.CheckBox();
			this.nudAutomaticRefreshInterval = new System.Windows.Forms.NumericUpDown();
			this.tmrAutomaticRefresh = new System.Windows.Forms.Timer(this.components);
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.managedProcessesGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAutomaticRefreshInterval)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.nudAutomaticRefreshInterval);
			this.panel1.Controls.Add(this.cbAutomaticRefresh);
			this.panel1.Controls.Add(this.refreshButton);
			this.panel1.Controls.Add(this.cancelButton);
			this.panel1.Controls.Add(this.okButton);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 266);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(689, 35);
			this.panel1.TabIndex = 1;
			// 
			// refreshButton
			// 
			this.refreshButton.Location = new System.Drawing.Point(447, 4);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(75, 23);
			this.refreshButton.TabIndex = 2;
			this.refreshButton.Text = "&Refresh";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(609, 4);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "&Cancel";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(528, 4);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// managedProcessesGrid
			// 
			this.managedProcessesGrid.AllowUserToAddRows = false;
			this.managedProcessesGrid.AllowUserToDeleteRows = false;
			this.managedProcessesGrid.AllowUserToResizeRows = false;
			this.managedProcessesGrid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.managedProcessesGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.managedProcessesGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.managedProcessesGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.managedProcessesGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.managedProcessesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.FrameworkVersion,
            this.processNameColumn,
            this.mainWindowTitleColumn,
            this.userNameColumn,
            this.processFileNameColumn});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.managedProcessesGrid.DefaultCellStyle = dataGridViewCellStyle2;
			this.managedProcessesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.managedProcessesGrid.GridColor = System.Drawing.SystemColors.ActiveBorder;
			this.managedProcessesGrid.Location = new System.Drawing.Point(0, 0);
			this.managedProcessesGrid.Name = "managedProcessesGrid";
			this.managedProcessesGrid.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.managedProcessesGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.managedProcessesGrid.RowHeadersVisible = false;
			this.managedProcessesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.managedProcessesGrid.ShowEditingIcon = false;
			this.managedProcessesGrid.Size = new System.Drawing.Size(689, 266);
			this.managedProcessesGrid.TabIndex = 0;
			this.managedProcessesGrid.Text = "dataGridView1";
			this.managedProcessesGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.managedProcessesGrid_CellDoubleClick);
			// 
			// idColumn
			// 
			this.idColumn.HeaderText = "Process ID";
			this.idColumn.Name = "idColumn";
			this.idColumn.ReadOnly = true;
			// 
			// FrameworkVersion
			// 
			this.FrameworkVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.FrameworkVersion.HeaderText = "Framework Version";
			this.FrameworkVersion.Name = "FrameworkVersion";
			this.FrameworkVersion.ReadOnly = true;
			this.FrameworkVersion.Width = 122;
			// 
			// processNameColumn
			// 
			this.processNameColumn.HeaderText = "Process Name";
			this.processNameColumn.Name = "processNameColumn";
			this.processNameColumn.ReadOnly = true;
			// 
			// mainWindowTitleColumn
			// 
			this.mainWindowTitleColumn.HeaderText = "Main Window Title";
			this.mainWindowTitleColumn.Name = "mainWindowTitleColumn";
			this.mainWindowTitleColumn.ReadOnly = true;
			// 
			// userNameColumn
			// 
			this.userNameColumn.HeaderText = "User Name";
			this.userNameColumn.Name = "userNameColumn";
			this.userNameColumn.ReadOnly = true;
			// 
			// processFileNameColumn
			// 
			this.processFileNameColumn.HeaderText = "File Name";
			this.processFileNameColumn.Name = "processFileNameColumn";
			this.processFileNameColumn.ReadOnly = true;
			// 
			// cbAutomaticRefresh
			// 
			this.cbAutomaticRefresh.AutoSize = true;
			this.cbAutomaticRefresh.Location = new System.Drawing.Point(12, 8);
			this.cbAutomaticRefresh.Name = "cbAutomaticRefresh";
			this.cbAutomaticRefresh.Size = new System.Drawing.Size(187, 17);
			this.cbAutomaticRefresh.TabIndex = 0;
			this.cbAutomaticRefresh.Text = "Automatic refreshing (milliseconds)";
			this.cbAutomaticRefresh.UseVisualStyleBackColor = true;
			this.cbAutomaticRefresh.CheckedChanged += new System.EventHandler(this.cbAutomaticRefresh_CheckedChanged);
			// 
			// nudAutomaticRefreshInterval
			// 
			this.nudAutomaticRefreshInterval.Location = new System.Drawing.Point(205, 6);
			this.nudAutomaticRefreshInterval.Maximum = new decimal(new int[] {
            3600000,
            0,
            0,
            0});
			this.nudAutomaticRefreshInterval.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.nudAutomaticRefreshInterval.Name = "nudAutomaticRefreshInterval";
			this.nudAutomaticRefreshInterval.Size = new System.Drawing.Size(80, 20);
			this.nudAutomaticRefreshInterval.TabIndex = 1;
			this.nudAutomaticRefreshInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.nudAutomaticRefreshInterval.ThousandsSeparator = true;
			this.nudAutomaticRefreshInterval.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
			// 
			// tmrAutomaticRefresh
			// 
			this.tmrAutomaticRefresh.Tick += new System.EventHandler(this.tmrAutomaticRefresh_Tick);
			// 
			// AttachProcessDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(689, 301);
			this.Controls.Add(this.managedProcessesGrid);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AttachProcessDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Attach to process";
			this.Load += new System.EventHandler(this.AttachProcessDialog_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.managedProcessesGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudAutomaticRefreshInterval)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.CustomDataGridView managedProcessesGrid;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn FrameworkVersion;
		private System.Windows.Forms.DataGridViewTextBoxColumn processNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn mainWindowTitleColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn userNameColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn processFileNameColumn;
		private System.Windows.Forms.NumericUpDown nudAutomaticRefreshInterval;
		private System.Windows.Forms.CheckBox cbAutomaticRefresh;
		private System.Windows.Forms.Timer tmrAutomaticRefresh;
	}
}