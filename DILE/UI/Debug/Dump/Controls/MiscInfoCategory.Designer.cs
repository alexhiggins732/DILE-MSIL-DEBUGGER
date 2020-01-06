namespace Dile.UI.Debug.Dump.Controls
{
	partial class MiscInfoCategory
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblProcessIdNA = new System.Windows.Forms.Label();
			this.txtProcessId = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lblProcessInfoNA = new System.Windows.Forms.Label();
			this.txtProcessKernelTime = new System.Windows.Forms.TextBox();
			this.txtProcessUserTime = new System.Windows.Forms.TextBox();
			this.txtProcessCreateTime = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.lblProcessorInfoNA = new System.Windows.Forms.Label();
			this.txtProcessorCurrentIdleState = new System.Windows.Forms.TextBox();
			this.txtProcessorMhzLimit = new System.Windows.Forms.TextBox();
			this.txtProcessorCurrentMhz = new System.Windows.Forms.TextBox();
			this.txtProcessorMaxMhz = new System.Windows.Forms.TextBox();
			this.txtProcessorMaxIdleState = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.lblProcessIdNA);
			this.groupBox1.Controls.Add(this.txtProcessId);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(4, 4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(409, 46);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Process Id";
			// 
			// lblProcessIdNA
			// 
			this.lblProcessIdNA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblProcessIdNA.Location = new System.Drawing.Point(3, 16);
			this.lblProcessIdNA.Name = "lblProcessIdNA";
			this.lblProcessIdNA.Size = new System.Drawing.Size(403, 27);
			this.lblProcessIdNA.TabIndex = 2;
			this.lblProcessIdNA.Text = "Not Available";
			this.lblProcessIdNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtProcessId
			// 
			this.txtProcessId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessId.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessId.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessId.Location = new System.Drawing.Point(118, 25);
			this.txtProcessId.Name = "txtProcessId";
			this.txtProcessId.ReadOnly = true;
			this.txtProcessId.Size = new System.Drawing.Size(285, 13);
			this.txtProcessId.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(10, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(19, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Id:";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.lblProcessInfoNA);
			this.groupBox2.Controls.Add(this.txtProcessKernelTime);
			this.groupBox2.Controls.Add(this.txtProcessUserTime);
			this.groupBox2.Controls.Add(this.txtProcessCreateTime);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.label3);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(3, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(409, 100);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Process Info";
			// 
			// lblProcessInfoNA
			// 
			this.lblProcessInfoNA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblProcessInfoNA.Location = new System.Drawing.Point(3, 16);
			this.lblProcessInfoNA.Name = "lblProcessInfoNA";
			this.lblProcessInfoNA.Size = new System.Drawing.Size(403, 81);
			this.lblProcessInfoNA.TabIndex = 6;
			this.lblProcessInfoNA.Text = "Not Available";
			this.lblProcessInfoNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtProcessKernelTime
			// 
			this.txtProcessKernelTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessKernelTime.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessKernelTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessKernelTime.Location = new System.Drawing.Point(118, 75);
			this.txtProcessKernelTime.Name = "txtProcessKernelTime";
			this.txtProcessKernelTime.ReadOnly = true;
			this.txtProcessKernelTime.Size = new System.Drawing.Size(285, 13);
			this.txtProcessKernelTime.TabIndex = 5;
			// 
			// txtProcessUserTime
			// 
			this.txtProcessUserTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessUserTime.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessUserTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessUserTime.Location = new System.Drawing.Point(118, 50);
			this.txtProcessUserTime.Name = "txtProcessUserTime";
			this.txtProcessUserTime.ReadOnly = true;
			this.txtProcessUserTime.Size = new System.Drawing.Size(285, 13);
			this.txtProcessUserTime.TabIndex = 3;
			// 
			// txtProcessCreateTime
			// 
			this.txtProcessCreateTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessCreateTime.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessCreateTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessCreateTime.Location = new System.Drawing.Point(118, 25);
			this.txtProcessCreateTime.Name = "txtProcessCreateTime";
			this.txtProcessCreateTime.ReadOnly = true;
			this.txtProcessCreateTime.Size = new System.Drawing.Size(285, 13);
			this.txtProcessCreateTime.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 75);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Kernel Mode Time:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 50);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "User Mode Time:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(10, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Creation Time:";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.lblProcessorInfoNA);
			this.groupBox3.Controls.Add(this.txtProcessorCurrentIdleState);
			this.groupBox3.Controls.Add(this.txtProcessorMhzLimit);
			this.groupBox3.Controls.Add(this.txtProcessorCurrentMhz);
			this.groupBox3.Controls.Add(this.txtProcessorMaxMhz);
			this.groupBox3.Controls.Add(this.txtProcessorMaxIdleState);
			this.groupBox3.Controls.Add(this.label9);
			this.groupBox3.Controls.Add(this.label8);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Location = new System.Drawing.Point(4, 162);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(409, 149);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Processor Info";
			// 
			// lblProcessorInfoNA
			// 
			this.lblProcessorInfoNA.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lblProcessorInfoNA.Location = new System.Drawing.Point(3, 16);
			this.lblProcessorInfoNA.Name = "lblProcessorInfoNA";
			this.lblProcessorInfoNA.Size = new System.Drawing.Size(403, 130);
			this.lblProcessorInfoNA.TabIndex = 10;
			this.lblProcessorInfoNA.Text = "Not Available";
			this.lblProcessorInfoNA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtProcessorCurrentIdleState
			// 
			this.txtProcessorCurrentIdleState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessorCurrentIdleState.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessorCurrentIdleState.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessorCurrentIdleState.Location = new System.Drawing.Point(118, 125);
			this.txtProcessorCurrentIdleState.Name = "txtProcessorCurrentIdleState";
			this.txtProcessorCurrentIdleState.ReadOnly = true;
			this.txtProcessorCurrentIdleState.Size = new System.Drawing.Size(285, 13);
			this.txtProcessorCurrentIdleState.TabIndex = 9;
			// 
			// txtProcessorMhzLimit
			// 
			this.txtProcessorMhzLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessorMhzLimit.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessorMhzLimit.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessorMhzLimit.Location = new System.Drawing.Point(118, 75);
			this.txtProcessorMhzLimit.Name = "txtProcessorMhzLimit";
			this.txtProcessorMhzLimit.ReadOnly = true;
			this.txtProcessorMhzLimit.Size = new System.Drawing.Size(285, 13);
			this.txtProcessorMhzLimit.TabIndex = 5;
			// 
			// txtProcessorCurrentMhz
			// 
			this.txtProcessorCurrentMhz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessorCurrentMhz.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessorCurrentMhz.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessorCurrentMhz.Location = new System.Drawing.Point(118, 50);
			this.txtProcessorCurrentMhz.Name = "txtProcessorCurrentMhz";
			this.txtProcessorCurrentMhz.ReadOnly = true;
			this.txtProcessorCurrentMhz.Size = new System.Drawing.Size(285, 13);
			this.txtProcessorCurrentMhz.TabIndex = 3;
			// 
			// txtProcessorMaxMhz
			// 
			this.txtProcessorMaxMhz.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessorMaxMhz.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessorMaxMhz.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessorMaxMhz.Location = new System.Drawing.Point(118, 25);
			this.txtProcessorMaxMhz.Name = "txtProcessorMaxMhz";
			this.txtProcessorMaxMhz.ReadOnly = true;
			this.txtProcessorMaxMhz.Size = new System.Drawing.Size(285, 13);
			this.txtProcessorMaxMhz.TabIndex = 1;
			// 
			// txtProcessorMaxIdleState
			// 
			this.txtProcessorMaxIdleState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtProcessorMaxIdleState.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.txtProcessorMaxIdleState.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtProcessorMaxIdleState.Location = new System.Drawing.Point(117, 100);
			this.txtProcessorMaxIdleState.Name = "txtProcessorMaxIdleState";
			this.txtProcessorMaxIdleState.ReadOnly = true;
			this.txtProcessorMaxIdleState.Size = new System.Drawing.Size(285, 13);
			this.txtProcessorMaxIdleState.TabIndex = 7;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(10, 125);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(92, 13);
			this.label9.TabIndex = 8;
			this.label9.Text = "Current Idle State:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(10, 100);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(102, 13);
			this.label8.TabIndex = 6;
			this.label8.Text = "Maximum Idle State:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(10, 75);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 13);
			this.label7.TabIndex = 4;
			this.label7.Text = "MHz Limit:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(10, 50);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(69, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Current MHz:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(10, 25);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(79, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Maximum MHz:";
			// 
			// MiscInfoCategory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Name = "MiscInfoCategory";
			this.Size = new System.Drawing.Size(416, 317);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtProcessId;
		private System.Windows.Forms.TextBox txtProcessKernelTime;
		private System.Windows.Forms.TextBox txtProcessUserTime;
		private System.Windows.Forms.TextBox txtProcessCreateTime;
		private System.Windows.Forms.TextBox txtProcessorCurrentIdleState;
		private System.Windows.Forms.TextBox txtProcessorMhzLimit;
		private System.Windows.Forms.TextBox txtProcessorCurrentMhz;
		private System.Windows.Forms.TextBox txtProcessorMaxMhz;
		private System.Windows.Forms.TextBox txtProcessorMaxIdleState;
		private System.Windows.Forms.Label lblProcessIdNA;
		private System.Windows.Forms.Label lblProcessInfoNA;
		private System.Windows.Forms.Label lblProcessorInfoNA;
	}
}
