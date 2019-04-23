namespace Dile.UI
{
	partial class MemoryDumpInfoPage
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.txtLastWriteTime = new System.Windows.Forms.TextBox();
			this.txtFilePath = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.pnlCategoryControl = new System.Windows.Forms.Panel();
			this.drpCategories = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lblCategoryNA = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.pnlCategoryControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.txtLastWriteTime);
			this.groupBox1.Controls.Add(this.txtFilePath);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(725, 80);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "General information";
			// 
			// txtLastWriteTime
			// 
			this.txtLastWriteTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtLastWriteTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtLastWriteTime.Location = new System.Drawing.Point(95, 48);
			this.txtLastWriteTime.Name = "txtLastWriteTime";
			this.txtLastWriteTime.ReadOnly = true;
			this.txtLastWriteTime.Size = new System.Drawing.Size(618, 13);
			this.txtLastWriteTime.TabIndex = 3;
			// 
			// txtFilePath
			// 
			this.txtFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtFilePath.Location = new System.Drawing.Point(95, 22);
			this.txtFilePath.Name = "txtFilePath";
			this.txtFilePath.ReadOnly = true;
			this.txtFilePath.Size = new System.Drawing.Size(618, 13);
			this.txtFilePath.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(77, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Last write time:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Dump file path:";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.pnlCategoryControl);
			this.groupBox2.Controls.Add(this.drpCategories);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(0, 80);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(725, 452);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Additional information";
			// 
			// pnlCategoryControl
			// 
			this.pnlCategoryControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.pnlCategoryControl.Controls.Add(this.lblCategoryNA);
			this.pnlCategoryControl.Location = new System.Drawing.Point(15, 43);
			this.pnlCategoryControl.Name = "pnlCategoryControl";
			this.pnlCategoryControl.Size = new System.Drawing.Size(698, 397);
			this.pnlCategoryControl.TabIndex = 2;
			// 
			// drpCategories
			// 
			this.drpCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.drpCategories.FormattingEnabled = true;
			this.drpCategories.Location = new System.Drawing.Point(70, 16);
			this.drpCategories.Name = "drpCategories";
			this.drpCategories.Size = new System.Drawing.Size(207, 21);
			this.drpCategories.TabIndex = 1;
			this.drpCategories.SelectedIndexChanged += new System.EventHandler(this.drpCategories_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 19);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(52, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Category:";
			// 
			// lblCategoryNA
			// 
			this.lblCategoryNA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblCategoryNA.Location = new System.Drawing.Point(0, 0);
			this.lblCategoryNA.Name = "lblCategoryNA";
			this.lblCategoryNA.Size = new System.Drawing.Size(698, 397);
			this.lblCategoryNA.TabIndex = 0;
			this.lblCategoryNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// MemoryDumpInfoPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(725, 532);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "MemoryDumpInfoPage";
			this.Text = "MemoryDumpInfoPage";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.pnlCategoryControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox txtLastWriteTime;
		private System.Windows.Forms.TextBox txtFilePath;
		private System.Windows.Forms.ComboBox drpCategories;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel pnlCategoryControl;
		private System.Windows.Forms.Label lblCategoryNA;
	}
}