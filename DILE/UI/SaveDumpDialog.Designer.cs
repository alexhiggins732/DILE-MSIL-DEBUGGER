namespace Dile.UI
{
	partial class SaveDumpDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveDumpDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.processIdSelector = new System.Windows.Forms.NumericUpDown();
			this.browseProcessButton = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.dumpFilePathTextBox = new System.Windows.Forms.TextBox();
			this.browseDumpFilePathButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.saveDumpFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.dumpTypeListBox = new System.Windows.Forms.CheckedListBox();
			this.msdnLink = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.processIdSelector)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(60, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Process Id:";
			// 
			// processIdSelector
			// 
			this.processIdSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.processIdSelector.Location = new System.Drawing.Point(15, 25);
			this.processIdSelector.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.processIdSelector.Name = "processIdSelector";
			this.processIdSelector.Size = new System.Drawing.Size(176, 20);
			this.processIdSelector.TabIndex = 1;
			this.processIdSelector.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.processIdSelector.ThousandsSeparator = true;
			// 
			// browseProcessButton
			// 
			this.browseProcessButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseProcessButton.Location = new System.Drawing.Point(197, 25);
			this.browseProcessButton.Name = "browseProcessButton";
			this.browseProcessButton.Size = new System.Drawing.Size(75, 23);
			this.browseProcessButton.TabIndex = 2;
			this.browseProcessButton.Text = "Browse...";
			this.browseProcessButton.UseVisualStyleBackColor = true;
			this.browseProcessButton.Click += new System.EventHandler(this.browseProcessButton_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 63);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(61, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Dump type:";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 224);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Dump file path:";
			// 
			// dumpFilePathTextBox
			// 
			this.dumpFilePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dumpFilePathTextBox.Location = new System.Drawing.Point(15, 240);
			this.dumpFilePathTextBox.Name = "dumpFilePathTextBox";
			this.dumpFilePathTextBox.Size = new System.Drawing.Size(176, 20);
			this.dumpFilePathTextBox.TabIndex = 6;
			// 
			// browseDumpFilePathButton
			// 
			this.browseDumpFilePathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.browseDumpFilePathButton.Location = new System.Drawing.Point(197, 240);
			this.browseDumpFilePathButton.Name = "browseDumpFilePathButton";
			this.browseDumpFilePathButton.Size = new System.Drawing.Size(75, 23);
			this.browseDumpFilePathButton.TabIndex = 7;
			this.browseDumpFilePathButton.Text = "Browse...";
			this.browseDumpFilePathButton.UseVisualStyleBackColor = true;
			this.browseDumpFilePathButton.Click += new System.EventHandler(this.browseDumpFilePathButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.saveButton.Location = new System.Drawing.Point(116, 280);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(75, 23);
			this.saveButton.TabIndex = 8;
			this.saveButton.Text = "&Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(197, 280);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 9;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// saveDumpFileDialog
			// 
			this.saveDumpFileDialog.DefaultExt = "dmp";
			this.saveDumpFileDialog.Filter = "Memory dump files (*.dmp)|*.dmp|All files (*.*)|*.*";
			this.saveDumpFileDialog.SupportMultiDottedExtensions = true;
			// 
			// dumpTypeListBox
			// 
			this.dumpTypeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dumpTypeListBox.FormattingEnabled = true;
			this.dumpTypeListBox.Location = new System.Drawing.Point(15, 79);
			this.dumpTypeListBox.Name = "dumpTypeListBox";
			this.dumpTypeListBox.Size = new System.Drawing.Size(257, 139);
			this.dumpTypeListBox.TabIndex = 4;
			// 
			// msdnLink
			// 
			this.msdnLink.AutoSize = true;
			this.msdnLink.Location = new System.Drawing.Point(79, 63);
			this.msdnLink.Name = "msdnLink";
			this.msdnLink.Size = new System.Drawing.Size(179, 13);
			this.msdnLink.TabIndex = 10;
			this.msdnLink.TabStop = true;
			this.msdnLink.Text = "(See MSDN for detailed description.)";
			this.msdnLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.msdnLink_LinkClicked);
			// 
			// SaveDumpDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(284, 315);
			this.Controls.Add(this.msdnLink);
			this.Controls.Add(this.dumpTypeListBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.browseDumpFilePathButton);
			this.Controls.Add(this.dumpFilePathTextBox);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.browseProcessButton);
			this.Controls.Add(this.processIdSelector);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SaveDumpDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save dump file...";
			this.Load += new System.EventHandler(this.SaveDumpDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.processIdSelector)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown processIdSelector;
		private System.Windows.Forms.Button browseProcessButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox dumpFilePathTextBox;
		private System.Windows.Forms.Button browseDumpFilePathButton;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.SaveFileDialog saveDumpFileDialog;
		private System.Windows.Forms.CheckedListBox dumpTypeListBox;
		private System.Windows.Forms.LinkLabel msdnLink;
	}
}