namespace Dile.UI
{
	partial class DebugOutputPanel
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
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.eventsList = new System.Windows.Forms.ListBox();
			this.eventDetailsTree = new System.Windows.Forms.TreeView();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.eventsList);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.eventDetailsTree);
			this.splitContainer.Size = new System.Drawing.Size(292, 217);
			this.splitContainer.SplitterDistance = 151;
			this.splitContainer.SplitterWidth = 1;
			this.splitContainer.TabIndex = 0;
			this.splitContainer.Text = "splitContainer1";
			// 
			// eventsList
			// 
			this.eventsList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventsList.FormattingEnabled = true;
			this.eventsList.IntegralHeight = false;
			this.eventsList.Location = new System.Drawing.Point(0, 0);
			this.eventsList.Name = "eventsList";
			this.eventsList.Size = new System.Drawing.Size(151, 217);
			this.eventsList.TabIndex = 0;
			this.eventsList.SelectedIndexChanged += new System.EventHandler(this.eventsList_SelectedIndexChanged);
			// 
			// eventDetailsTree
			// 
			this.eventDetailsTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eventDetailsTree.Location = new System.Drawing.Point(0, 0);
			this.eventDetailsTree.Name = "eventDetailsTree";
			this.eventDetailsTree.Size = new System.Drawing.Size(140, 217);
			this.eventDetailsTree.TabIndex = 0;
			// 
			// DebugOutputPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 217);
			this.Controls.Add(this.splitContainer);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "DebugOutputPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Debug Output Panel";
			this.Text = "Debug Output Panel";
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ListBox eventsList;
		private System.Windows.Forms.TreeView eventDetailsTree;
	}
}