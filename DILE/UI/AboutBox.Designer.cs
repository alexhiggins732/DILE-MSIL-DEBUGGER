namespace Dile.UI
{
	partial class AboutBox
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
			this.appNameLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.projectPageLink = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.blogLink = new System.Windows.Forms.LinkLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.emailLink = new System.Windows.Forms.LinkLabel();
			this.closeButton = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// appNameLabel
			// 
			this.appNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.appNameLabel.BackColor = System.Drawing.Color.Transparent;
			this.appNameLabel.Location = new System.Drawing.Point(0, 20);
			this.appNameLabel.Name = "appNameLabel";
			this.appNameLabel.Size = new System.Drawing.Size(311, 13);
			this.appNameLabel.TabIndex = 1;
			this.appNameLabel.Text = "Dotnet IL Editor (DILE) v0.2.12";
			this.appNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(12, 65);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Created by:";
			// 
			// projectPageLink
			// 
			this.projectPageLink.AutoSize = true;
			this.projectPageLink.BackColor = System.Drawing.Color.Transparent;
			this.projectPageLink.Location = new System.Drawing.Point(93, 154);
			this.projectPageLink.Name = "projectPageLink";
			this.projectPageLink.Size = new System.Drawing.Size(131, 13);
			this.projectPageLink.TabIndex = 9;
			this.projectPageLink.TabStop = true;
			this.projectPageLink.Text = "http://dile.sourceforge.net";
			this.projectPageLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.projectPageLink_LinkClicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Location = new System.Drawing.Point(93, 65);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(69, 13);
			this.label3.TabIndex = 3;
			this.label3.Text = "Petrény Zsolt";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Location = new System.Drawing.Point(12, 154);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Project page:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Location = new System.Drawing.Point(12, 123);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(74, 13);
			this.label5.TabIndex = 6;
			this.label5.Text = "Personal blog:";
			// 
			// blogLink
			// 
			this.blogLink.AutoSize = true;
			this.blogLink.BackColor = System.Drawing.Color.Transparent;
			this.blogLink.Location = new System.Drawing.Point(93, 123);
			this.blogLink.Name = "blogLink";
			this.blogLink.Size = new System.Drawing.Size(131, 13);
			this.blogLink.TabIndex = 7;
			this.blogLink.TabStop = true;
			this.blogLink.Text = "http://pzsolt.blogspot.com";
			this.blogLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.blogLink_LinkClicked);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Location = new System.Drawing.Point(12, 96);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(78, 13);
			this.label6.TabIndex = 4;
			this.label6.Text = "E-mail address:";
			// 
			// emailLink
			// 
			this.emailLink.AutoSize = true;
			this.emailLink.BackColor = System.Drawing.Color.Transparent;
			this.emailLink.Location = new System.Drawing.Point(93, 96);
			this.emailLink.Name = "emailLink";
			this.emailLink.Size = new System.Drawing.Size(116, 13);
			this.emailLink.TabIndex = 5;
			this.emailLink.TabStop = true;
			this.emailLink.Text = "dile.project@gmail.com";
			this.emailLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.emailLink_LinkClicked);
			// 
			// closeButton
			// 
			this.closeButton.BackColor = System.Drawing.Color.Transparent;
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(221, 251);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 0;
			this.closeButton.Text = "&Close";
			this.closeButton.UseVisualStyleBackColor = false;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Location = new System.Drawing.Point(12, 191);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(265, 39);
			this.label7.TabIndex = 10;
			this.label7.Text = "See the readme.txt and license.txt for more information.\r\n\r\nCopyright (C) 2013 Pe" +
					"trény Zsolt";
			// 
			// AboutBox
			// 
			this.AcceptButton = this.closeButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::Dile.Properties.Resources.DILE;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(308, 286);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.emailLink);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.blogLink);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.projectPageLink);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.appNameLabel);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.Load += new System.EventHandler(this.AboutBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label appNameLabel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel projectPageLink;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel blogLink;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.LinkLabel emailLink;
		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.Label label7;
	}
}