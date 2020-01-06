namespace Dile.UI.Debug.Dump.Controls
{
	partial class ExceptionCategory
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtCode = new System.Windows.Forms.TextBox();
			this.txtContinuable = new System.Windows.Forms.TextBox();
			this.txtRecord = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.lblClrException = new System.Windows.Forms.Label();
			this.txtClrException = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(84, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Exception code:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 32);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(66, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Continuable:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 58);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(90, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "Exception record:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 84);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Exception address:";
			// 
			// txtCode
			// 
			this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtCode.Location = new System.Drawing.Point(106, 6);
			this.txtCode.Name = "txtCode";
			this.txtCode.ReadOnly = true;
			this.txtCode.Size = new System.Drawing.Size(226, 13);
			this.txtCode.TabIndex = 1;
			// 
			// txtContinuable
			// 
			this.txtContinuable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtContinuable.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtContinuable.Location = new System.Drawing.Point(106, 32);
			this.txtContinuable.Name = "txtContinuable";
			this.txtContinuable.ReadOnly = true;
			this.txtContinuable.Size = new System.Drawing.Size(226, 13);
			this.txtContinuable.TabIndex = 3;
			// 
			// txtRecord
			// 
			this.txtRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtRecord.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtRecord.Location = new System.Drawing.Point(106, 58);
			this.txtRecord.Name = "txtRecord";
			this.txtRecord.ReadOnly = true;
			this.txtRecord.Size = new System.Drawing.Size(226, 13);
			this.txtRecord.TabIndex = 5;
			// 
			// txtAddress
			// 
			this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtAddress.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAddress.Location = new System.Drawing.Point(106, 84);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.ReadOnly = true;
			this.txtAddress.Size = new System.Drawing.Size(226, 13);
			this.txtAddress.TabIndex = 7;
			// 
			// lblClrException
			// 
			this.lblClrException.AutoSize = true;
			this.lblClrException.Location = new System.Drawing.Point(3, 107);
			this.lblClrException.Name = "lblClrException";
			this.lblClrException.Size = new System.Drawing.Size(80, 13);
			this.lblClrException.TabIndex = 8;
			this.lblClrException.Text = "CLR exception:";
			// 
			// txtClrException
			// 
			this.txtClrException.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtClrException.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtClrException.Location = new System.Drawing.Point(106, 107);
			this.txtClrException.Name = "txtClrException";
			this.txtClrException.ReadOnly = true;
			this.txtClrException.Size = new System.Drawing.Size(226, 13);
			this.txtClrException.TabIndex = 9;
			// 
			// ExceptionCategory
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtClrException);
			this.Controls.Add(this.lblClrException);
			this.Controls.Add(this.txtAddress);
			this.Controls.Add(this.txtRecord);
			this.Controls.Add(this.txtContinuable);
			this.Controls.Add(this.txtCode);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "ExceptionCategory";
			this.Size = new System.Drawing.Size(335, 128);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtCode;
		private System.Windows.Forms.TextBox txtContinuable;
		private System.Windows.Forms.TextBox txtRecord;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Label lblClrException;
		private System.Windows.Forms.TextBox txtClrException;
	}
}
