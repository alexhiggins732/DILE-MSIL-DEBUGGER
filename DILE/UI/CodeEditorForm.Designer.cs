namespace Dile.UI
{
	partial class CodeEditorForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.ilEditor = new Dile.Controls.ILEditorControl();
			this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.locateInProjectExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setIPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// ilEditor
			// 
			this.ilEditor.AcceptsTab = true;
			this.ilEditor.BackColor = System.Drawing.Color.White;
			this.ilEditor.CodeObject = null;
			this.ilEditor.ContextMenuStrip = this.contextMenu;
			this.ilEditor.CurrentLine = null;
			this.ilEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ilEditor.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ilEditor.Location = new System.Drawing.Point(0, 0);
			this.ilEditor.Name = "ilEditor";
			this.ilEditor.ReadOnly = true;
			this.ilEditor.Size = new System.Drawing.Size(604, 336);
			this.ilEditor.TabIndex = 0;
			this.ilEditor.Text = "";
			// 
			// contextMenu
			// 
			this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.locateInProjectExplorerToolStripMenuItem,
            this.setIPToolStripMenuItem});
			this.contextMenu.Name = "contextMenu";
			this.contextMenu.Size = new System.Drawing.Size(198, 48);
			this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
			// 
			// locateInProjectExplorerToolStripMenuItem
			// 
			this.locateInProjectExplorerToolStripMenuItem.Name = "locateInProjectExplorerToolStripMenuItem";
			this.locateInProjectExplorerToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.locateInProjectExplorerToolStripMenuItem.Text = "&Locate in Project Explorer";
			this.locateInProjectExplorerToolStripMenuItem.Click += new System.EventHandler(this.locateInProjectExplorerToolStripMenuItem_Click);
			// 
			// setIPToolStripMenuItem
			// 
			this.setIPToolStripMenuItem.Name = "setIPToolStripMenuItem";
			this.setIPToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
			this.setIPToolStripMenuItem.Text = "&Set IP to this instruction";
			this.setIPToolStripMenuItem.Click += new System.EventHandler(this.setIPToolStripMenuItem_Click);
			// 
			// CodeEditorForm
			// 
			this.ClientSize = new System.Drawing.Size(604, 336);
			this.Controls.Add(this.ilEditor);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.Document)));
			this.DoubleBuffered = true;
			this.Name = "CodeEditorForm";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
			this.TabText = "CodeEditorForm";
			this.Text = "CodeEditorForm";
			this.Enter += new System.EventHandler(this.CodeEditorForm_Enter);
			this.contextMenu.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.ILEditorControl ilEditor;
		private System.Windows.Forms.ContextMenuStrip contextMenu;
		private System.Windows.Forms.ToolStripMenuItem locateInProjectExplorerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setIPToolStripMenuItem;
	}
}