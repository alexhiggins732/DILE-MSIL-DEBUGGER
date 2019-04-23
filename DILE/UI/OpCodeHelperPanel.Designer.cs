namespace Dile.UI
{
	partial class OpCodeHelperPanel
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.linkMsdn = new System.Windows.Forms.LinkLabel();
			this.lblDescription = new System.Windows.Forms.Label();
			this.cmbOpCodes = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.linkMsdn, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.cmbOpCodes, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(284, 426);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// linkMsdn
			// 
			this.linkMsdn.AutoSize = true;
			this.linkMsdn.Dock = System.Windows.Forms.DockStyle.Top;
			this.linkMsdn.Location = new System.Drawing.Point(3, 40);
			this.linkMsdn.Name = "linkMsdn";
			this.linkMsdn.Size = new System.Drawing.Size(278, 13);
			this.linkMsdn.TabIndex = 6;
			this.linkMsdn.TabStop = true;
			this.linkMsdn.Text = "Click here to read more about this instruction on MSDN...";
			this.linkMsdn.Visible = false;
			this.linkMsdn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkMsdn_LinkClicked);
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Dock = System.Windows.Forms.DockStyle.Top;
			this.lblDescription.Location = new System.Drawing.Point(3, 27);
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size(278, 13);
			this.lblDescription.TabIndex = 5;
			this.lblDescription.Visible = false;
			// 
			// cmbOpCodes
			// 
			this.cmbOpCodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.cmbOpCodes.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cmbOpCodes.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cmbOpCodes.Location = new System.Drawing.Point(3, 3);
			this.cmbOpCodes.Name = "cmbOpCodes";
			this.cmbOpCodes.Size = new System.Drawing.Size(278, 21);
			this.cmbOpCodes.TabIndex = 2;
			this.cmbOpCodes.SelectedIndexChanged += new System.EventHandler(this.cmbOpCodes_SelectedIndexChanged);
			// 
			// OpCodeHelperPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 426);
			this.Controls.Add(this.tableLayoutPanel1);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "OpCodeHelperPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
			this.TabText = "IL Instructions";
			this.Text = "IL Instructions";
			this.Load += new System.EventHandler(this.OpCodeHelperPanel_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.LinkLabel linkMsdn;
		private System.Windows.Forms.Label lblDescription;
		private System.Windows.Forms.ComboBox cmbOpCodes;


	}
}