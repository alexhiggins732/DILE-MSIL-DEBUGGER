namespace Dile.UI.Debug.Dump.Controls
{
	partial class UnloadedModuleCategory
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.lstUnloadedModules = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lvUnloadedModuleDetails = new Dile.Controls.CustomListView();
			this.clmName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.clmValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.lstUnloadedModules);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lvUnloadedModuleDetails);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Size = new System.Drawing.Size(460, 283);
			this.splitContainer1.SplitterDistance = 250;
			this.splitContainer1.TabIndex = 0;
			// 
			// lstUnloadedModules
			// 
			this.lstUnloadedModules.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstUnloadedModules.FormattingEnabled = true;
			this.lstUnloadedModules.Location = new System.Drawing.Point(0, 13);
			this.lstUnloadedModules.Name = "lstUnloadedModules";
			this.lstUnloadedModules.Size = new System.Drawing.Size(250, 270);
			this.lstUnloadedModules.TabIndex = 1;
			this.lstUnloadedModules.SelectedIndexChanged += new System.EventHandler(this.lstUnloadedModules_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(98, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Unloaded modules:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(126, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Unloaded module details:";
			// 
			// lvUnloadedModuleDetails
			// 
			this.lvUnloadedModuleDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmValue});
			this.lvUnloadedModuleDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvUnloadedModuleDetails.FullRowSelect = true;
			this.lvUnloadedModuleDetails.Location = new System.Drawing.Point(0, 13);
			this.lvUnloadedModuleDetails.Name = "lvUnloadedModuleDetails";
			this.lvUnloadedModuleDetails.Size = new System.Drawing.Size(206, 270);
			this.lvUnloadedModuleDetails.TabIndex = 0;
			this.lvUnloadedModuleDetails.UseCompatibleStateImageBehavior = false;
			this.lvUnloadedModuleDetails.View = System.Windows.Forms.View.Details;
			// 
			// clmName
			// 
			this.clmName.Text = "Name";
			this.clmName.Width = 120;
			// 
			// clmValue
			// 
			this.clmValue.Text = "Value";
			// 
			// UnloadedModuleCategory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "UnloadedModuleCategory";
			this.Size = new System.Drawing.Size(460, 283);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox lstUnloadedModules;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Dile.Controls.CustomListView lvUnloadedModuleDetails;
		private System.Windows.Forms.ColumnHeader clmName;
		private System.Windows.Forms.ColumnHeader clmValue;
	}
}