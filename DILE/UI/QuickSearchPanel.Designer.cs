namespace Dile.UI
{
	partial class QuickSearchPanel
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
			this.panel = new System.Windows.Forms.Panel();
			this.searchText = new System.Windows.Forms.TextBox();
			this.settingsButton = new System.Windows.Forms.Button();
			this.foundItemsList = new Dile.Controls.CustomListView();
			this.itemNameColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.itemTypeColumnHeader = new System.Windows.Forms.ColumnHeader();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add(this.searchText);
			this.panel.Controls.Add(this.settingsButton);
			this.panel.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel.Location = new System.Drawing.Point(0, 0);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(292, 20);
			this.panel.TabIndex = 0;
			// 
			// searchText
			// 
			this.searchText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.searchText.Location = new System.Drawing.Point(0, 0);
			this.searchText.Name = "searchText";
			this.searchText.Size = new System.Drawing.Size(270, 20);
			this.searchText.TabIndex = 0;
			this.searchText.TextChanged += new System.EventHandler(this.searchText_TextChanged);
			this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
			// 
			// settingsButton
			// 
			this.settingsButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.settingsButton.FlatAppearance.BorderSize = 0;
			this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.settingsButton.Location = new System.Drawing.Point(270, 0);
			this.settingsButton.Name = "settingsButton";
			this.settingsButton.Size = new System.Drawing.Size(22, 20);
			this.settingsButton.TabIndex = 1;
			this.settingsButton.TabStop = false;
			this.settingsButton.Text = "...";
			this.settingsButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
			// 
			// foundItemsList
			// 
			this.foundItemsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.itemNameColumnHeader,
            this.itemTypeColumnHeader});
			this.foundItemsList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.foundItemsList.FullRowSelect = true;
			this.foundItemsList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.foundItemsList.Location = new System.Drawing.Point(0, 20);
			this.foundItemsList.MultiSelect = true;
			this.foundItemsList.Name = "foundItemsList";
			this.foundItemsList.ShowItemToolTips = true;
			this.foundItemsList.Size = new System.Drawing.Size(292, 346);
			this.foundItemsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.foundItemsList.TabIndex = 1;
			this.foundItemsList.UseCompatibleStateImageBehavior = false;
			this.foundItemsList.View = System.Windows.Forms.View.Details;
			this.foundItemsList.DoubleClick += new System.EventHandler(this.foundItemsList_DoubleClick);
			this.foundItemsList.Resize += new System.EventHandler(this.foundItemsList_Resize);
			this.foundItemsList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.foundItemsList_KeyDown);
			// 
			// itemNameColumnHeader
			// 
			this.itemNameColumnHeader.Text = "Item Name";
			// 
			// itemTypeColumnHeader
			// 
			this.itemTypeColumnHeader.Text = "Item Type";
			// 
			// QuickSearchPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 366);
			this.Controls.Add(this.foundItemsList);
			this.Controls.Add(this.panel);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
									| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.Name = "QuickSearchPanel";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRight;
			this.TabText = "Quick Search";
			this.Text = "Quick Search";
			this.VisibleChanged += new System.EventHandler(this.QuickSearchPanel_VisibleChanged);
			this.panel.ResumeLayout(false);
			this.panel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.TextBox searchText;
		private System.Windows.Forms.Button settingsButton;
		private Dile.Controls.CustomListView foundItemsList;
		private System.Windows.Forms.ColumnHeader itemNameColumnHeader;
		private System.Windows.Forms.ColumnHeader itemTypeColumnHeader;
	}
}