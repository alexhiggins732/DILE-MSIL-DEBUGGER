namespace Dile.Controls
{
	partial class ILEditorControl
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
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
			this.components = new System.ComponentModel.Container();
			this.keywordListBox = new System.Windows.Forms.ListBox();
			this.keywordToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// keywordListBox
			// 
			this.keywordListBox.FormattingEnabled = true;
			this.keywordListBox.Location = new System.Drawing.Point(0, 0);
			this.keywordListBox.Name = "keywordListBox";
			this.keywordListBox.ScrollAlwaysVisible = true;
			this.keywordListBox.Size = new System.Drawing.Size(120, 134);
			this.keywordListBox.TabIndex = 0;
			this.keywordListBox.Visible = false;
			// 
			// keywordToolTip
			// 
			this.keywordToolTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
			// 
			// ILEditorControl
			// 
			this.AcceptsTab = true;
			this.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ILEditorControl_MouseMove);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox keywordListBox;
		private System.Windows.Forms.ToolTip keywordToolTip;
	}
}
