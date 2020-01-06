namespace Dile.UI.Debug.Dump.Controls
{
	partial class ThreadCategory
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
			this.lstThreads = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lvThreadDetails = new Dile.Controls.CustomListView();
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
			this.splitContainer1.Panel1.Controls.Add(this.lstThreads);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.lvThreadDetails);
			this.splitContainer1.Panel2.Controls.Add(this.label2);
			this.splitContainer1.Size = new System.Drawing.Size(450, 306);
			this.splitContainer1.SplitterDistance = 150;
			this.splitContainer1.TabIndex = 0;
			// 
			// lstThreads
			// 
			this.lstThreads.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstThreads.FormattingEnabled = true;
			this.lstThreads.Location = new System.Drawing.Point(0, 13);
			this.lstThreads.Name = "lstThreads";
			this.lstThreads.Size = new System.Drawing.Size(150, 293);
			this.lstThreads.TabIndex = 1;
			this.lstThreads.SelectedIndexChanged += new System.EventHandler(this.lstThreads_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Threads:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Thread details:";
			// 
			// lvThreadDetails
			// 
			this.lvThreadDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmName,
            this.clmValue});
			this.lvThreadDetails.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lvThreadDetails.FullRowSelect = true;
			this.lvThreadDetails.Location = new System.Drawing.Point(0, 13);
			this.lvThreadDetails.Name = "lvThreadDetails";
			this.lvThreadDetails.Size = new System.Drawing.Size(296, 293);
			this.lvThreadDetails.TabIndex = 1;
			this.lvThreadDetails.UseCompatibleStateImageBehavior = false;
			this.lvThreadDetails.View = System.Windows.Forms.View.Details;
			// 
			// clmName
			// 
			this.clmName.Text = "Name";
			this.clmName.Width = 120;
			// 
			// clmValue
			// 
			this.clmValue.Text = "Value";
			this.clmValue.Width = 120;
			// 
			// ThreadCategory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Name = "ThreadCategory";
			this.Size = new System.Drawing.Size(450, 306);
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
		private System.Windows.Forms.ListBox lstThreads;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private Dile.Controls.CustomListView lvThreadDetails;
		private System.Windows.Forms.ColumnHeader clmName;
		private System.Windows.Forms.ColumnHeader clmValue;
	}
}
