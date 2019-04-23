namespace Dile.UI
{
	partial class DocumentSelectorForm
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
			this.documentsPanel = new System.Windows.Forms.TableLayoutPanel();
			this.SuspendLayout();
			// 
			// documentsPanel
			// 
			this.documentsPanel.AutoSize = true;
			this.documentsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.documentsPanel.ColumnCount = 2;
			this.documentsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.documentsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.documentsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.documentsPanel.Location = new System.Drawing.Point(0, 0);
			this.documentsPanel.Name = "documentsPanel";
			this.documentsPanel.Size = new System.Drawing.Size(0, 0);
			this.documentsPanel.TabIndex = 0;
			// 
			// DocumentSelectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(0, 0);
			this.ControlBox = false;
			this.Controls.Add(this.documentsPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DocumentSelectorForm";
			this.Opacity = 0.9;
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "DocumentSelectorForm";
			this.Deactivate += new System.EventHandler(this.DocumentSelectorForm_Deactivate);
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DocumentSelectorForm_KeyUp);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DocumentSelectorForm_KeyDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel documentsPanel;


	}
}