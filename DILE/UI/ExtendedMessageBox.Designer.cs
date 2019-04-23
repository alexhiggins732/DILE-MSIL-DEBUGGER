namespace Dile.UI
{
	partial class ExtendedMessageBox
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
			this.messageTextBox = new System.Windows.Forms.TextBox();
			this.yesButton = new System.Windows.Forms.Button();
			this.noButton = new System.Windows.Forms.Button();
			this.yesToAllButton = new System.Windows.Forms.Button();
			this.noToAllButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// messageTextBox
			// 
			this.messageTextBox.AcceptsReturn = true;
			this.messageTextBox.AcceptsTab = true;
			this.messageTextBox.BackColor = System.Drawing.SystemColors.Control;
			this.messageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.messageTextBox.Location = new System.Drawing.Point(12, 12);
			this.messageTextBox.Multiline = true;
			this.messageTextBox.Name = "messageTextBox";
			this.messageTextBox.ReadOnly = true;
			this.messageTextBox.Size = new System.Drawing.Size(332, 185);
			this.messageTextBox.TabIndex = 0;
			// 
			// yesButton
			// 
			this.yesButton.Location = new System.Drawing.Point(12, 211);
			this.yesButton.Name = "yesButton";
			this.yesButton.Size = new System.Drawing.Size(75, 23);
			this.yesButton.TabIndex = 1;
			this.yesButton.Text = "&Yes";
			this.yesButton.UseVisualStyleBackColor = true;
			this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
			// 
			// noButton
			// 
			this.noButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.noButton.Location = new System.Drawing.Point(94, 211);
			this.noButton.Name = "noButton";
			this.noButton.Size = new System.Drawing.Size(75, 23);
			this.noButton.TabIndex = 2;
			this.noButton.Text = "&No";
			this.noButton.UseVisualStyleBackColor = true;
			this.noButton.Click += new System.EventHandler(this.noButton_Click);
			// 
			// yesToAllButton
			// 
			this.yesToAllButton.Location = new System.Drawing.Point(176, 211);
			this.yesToAllButton.Name = "yesToAllButton";
			this.yesToAllButton.Size = new System.Drawing.Size(75, 23);
			this.yesToAllButton.TabIndex = 3;
			this.yesToAllButton.Text = "Y&es to all";
			this.yesToAllButton.UseVisualStyleBackColor = true;
			this.yesToAllButton.Click += new System.EventHandler(this.yesToAllButton_Click);
			// 
			// noToAllButton
			// 
			this.noToAllButton.Location = new System.Drawing.Point(258, 211);
			this.noToAllButton.Name = "noToAllButton";
			this.noToAllButton.Size = new System.Drawing.Size(75, 23);
			this.noToAllButton.TabIndex = 4;
			this.noToAllButton.Text = "N&o to all";
			this.noToAllButton.UseVisualStyleBackColor = true;
			this.noToAllButton.Click += new System.EventHandler(this.noToAllButton_Click);
			// 
			// ExtendedMessageBox
			// 
			this.AcceptButton = this.noButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.noButton;
			this.ClientSize = new System.Drawing.Size(363, 246);
			this.ControlBox = false;
			this.Controls.Add(this.noToAllButton);
			this.Controls.Add(this.yesToAllButton);
			this.Controls.Add(this.noButton);
			this.Controls.Add(this.yesButton);
			this.Controls.Add(this.messageTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExtendedMessageBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ExtendedMessageBox";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox messageTextBox;
		private System.Windows.Forms.Button yesButton;
		private System.Windows.Forms.Button noButton;
		private System.Windows.Forms.Button yesToAllButton;
		private System.Windows.Forms.Button noToAllButton;
	}
}