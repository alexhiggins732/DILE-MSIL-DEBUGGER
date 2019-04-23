namespace Dile.UI
{
	partial class TextDisplayer
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
			this.closeButton = new System.Windows.Forms.Button();
			this.textTab = new System.Windows.Forms.TabControl();
			this.textEditorTabPage = new System.Windows.Forms.TabPage();
			this.reformatXmlButton = new System.Windows.Forms.Button();
			this.escapeCharactersCheckBox = new System.Windows.Forms.CheckBox();
			this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
			this.textBox = new System.Windows.Forms.TextBox();
			this.webBrowserTabPage = new System.Windows.Forms.TabPage();
			this.webBrowser = new System.Windows.Forms.WebBrowser();
			this.textTab.SuspendLayout();
			this.textEditorTabPage.SuspendLayout();
			this.webBrowserTabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(515, 344);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 2;
			this.closeButton.Text = "&Close";
			this.closeButton.UseVisualStyleBackColor = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// textTab
			// 
			this.textTab.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.textTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textTab.Controls.Add(this.textEditorTabPage);
			this.textTab.Controls.Add(this.webBrowserTabPage);
			this.textTab.Location = new System.Drawing.Point(12, 12);
			this.textTab.Name = "textTab";
			this.textTab.SelectedIndex = 0;
			this.textTab.Size = new System.Drawing.Size(588, 326);
			this.textTab.TabIndex = 5;
			this.textTab.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.textTab_Selecting);
			// 
			// textEditorTabPage
			// 
			this.textEditorTabPage.Controls.Add(this.reformatXmlButton);
			this.textEditorTabPage.Controls.Add(this.escapeCharactersCheckBox);
			this.textEditorTabPage.Controls.Add(this.wordWrapCheckBox);
			this.textEditorTabPage.Controls.Add(this.textBox);
			this.textEditorTabPage.Location = new System.Drawing.Point(4, 4);
			this.textEditorTabPage.Name = "textEditorTabPage";
			this.textEditorTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.textEditorTabPage.Size = new System.Drawing.Size(580, 300);
			this.textEditorTabPage.TabIndex = 0;
			this.textEditorTabPage.Text = "Text Editor";
			this.textEditorTabPage.UseVisualStyleBackColor = true;
			// 
			// reformatXmlButton
			// 
			this.reformatXmlButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.reformatXmlButton.Location = new System.Drawing.Point(453, 271);
			this.reformatXmlButton.Name = "reformatXmlButton";
			this.reformatXmlButton.Size = new System.Drawing.Size(121, 23);
			this.reformatXmlButton.TabIndex = 8;
			this.reformatXmlButton.Text = "Reformat XML string";
			this.reformatXmlButton.UseVisualStyleBackColor = true;
			this.reformatXmlButton.Click += new System.EventHandler(this.reformatXmlButton_Click);
			// 
			// escapeCharactersCheckBox
			// 
			this.escapeCharactersCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.escapeCharactersCheckBox.AutoSize = true;
			this.escapeCharactersCheckBox.Checked = true;
			this.escapeCharactersCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.escapeCharactersCheckBox.Location = new System.Drawing.Point(131, 277);
			this.escapeCharactersCheckBox.Name = "escapeCharactersCheckBox";
			this.escapeCharactersCheckBox.Size = new System.Drawing.Size(151, 17);
			this.escapeCharactersCheckBox.TabIndex = 7;
			this.escapeCharactersCheckBox.Text = "Display escape characters";
			this.escapeCharactersCheckBox.UseVisualStyleBackColor = true;
			this.escapeCharactersCheckBox.CheckedChanged += new System.EventHandler(this.escapeCharactersCheckBox_CheckedChanged);
			// 
			// wordWrapCheckBox
			// 
			this.wordWrapCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.wordWrapCheckBox.AutoSize = true;
			this.wordWrapCheckBox.Checked = true;
			this.wordWrapCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.wordWrapCheckBox.Location = new System.Drawing.Point(6, 275);
			this.wordWrapCheckBox.Name = "wordWrapCheckBox";
			this.wordWrapCheckBox.Size = new System.Drawing.Size(119, 17);
			this.wordWrapCheckBox.TabIndex = 6;
			this.wordWrapCheckBox.Text = "Word wrap enabled";
			this.wordWrapCheckBox.UseVisualStyleBackColor = true;
			this.wordWrapCheckBox.CheckedChanged += new System.EventHandler(this.wordWrapCheckBox_CheckedChanged);
			// 
			// textBox
			// 
			this.textBox.AcceptsReturn = true;
			this.textBox.AcceptsTab = true;
			this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.MaxLength = 0;
			this.textBox.Multiline = true;
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox.Size = new System.Drawing.Size(574, 265);
			this.textBox.TabIndex = 5;
			// 
			// webBrowserTabPage
			// 
			this.webBrowserTabPage.Controls.Add(this.webBrowser);
			this.webBrowserTabPage.Location = new System.Drawing.Point(4, 4);
			this.webBrowserTabPage.Name = "webBrowserTabPage";
			this.webBrowserTabPage.Padding = new System.Windows.Forms.Padding(3);
			this.webBrowserTabPage.Size = new System.Drawing.Size(580, 300);
			this.webBrowserTabPage.TabIndex = 1;
			this.webBrowserTabPage.Text = "Web browser";
			this.webBrowserTabPage.UseVisualStyleBackColor = true;
			// 
			// webBrowser
			// 
			this.webBrowser.AllowWebBrowserDrop = false;
			this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser.Location = new System.Drawing.Point(3, 3);
			this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser.Name = "webBrowser";
			this.webBrowser.Size = new System.Drawing.Size(574, 294);
			this.webBrowser.TabIndex = 0;
			// 
			// TextDisplayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.closeButton;
			this.ClientSize = new System.Drawing.Size(601, 373);
			this.Controls.Add(this.textTab);
			this.Controls.Add(this.closeButton);
			this.Name = "TextDisplayer";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Text";
			this.textTab.ResumeLayout(false);
			this.textEditorTabPage.ResumeLayout(false);
			this.textEditorTabPage.PerformLayout();
			this.webBrowserTabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button closeButton;
		private System.Windows.Forms.TabControl textTab;
		private System.Windows.Forms.TabPage textEditorTabPage;
		private System.Windows.Forms.Button reformatXmlButton;
		private System.Windows.Forms.CheckBox escapeCharactersCheckBox;
		private System.Windows.Forms.CheckBox wordWrapCheckBox;
		private System.Windows.Forms.TextBox textBox;
		private System.Windows.Forms.TabPage webBrowserTabPage;
		private System.Windows.Forms.WebBrowser webBrowser;
	}
}