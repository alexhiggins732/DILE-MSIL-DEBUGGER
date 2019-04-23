namespace Dile.Controls
{
	partial class SettingsDisplayerControl
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
			this.settingsPanel = new System.Windows.Forms.TableLayoutPanel();
			this.settingCategoriesList = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// settingsPanel
			// 
			this.settingsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 224F));
			this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.settingsPanel.Location = new System.Drawing.Point(153, 0);
			this.settingsPanel.Name = "settingsPanel";
			this.settingsPanel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.settingsPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.settingsPanel.Size = new System.Drawing.Size(224, 206);
			this.settingsPanel.TabIndex = 3;
			// 
			// settingCategoriesList
			// 
			this.settingCategoriesList.Dock = System.Windows.Forms.DockStyle.Left;
			this.settingCategoriesList.FormattingEnabled = true;
			this.settingCategoriesList.IntegralHeight = false;
			this.settingCategoriesList.Location = new System.Drawing.Point(0, 0);
			this.settingCategoriesList.Name = "settingCategoriesList";
			this.settingCategoriesList.Size = new System.Drawing.Size(153, 206);
			this.settingCategoriesList.TabIndex = 2;
			this.settingCategoriesList.SelectedIndexChanged += new System.EventHandler(this.settingCategoriesList_SelectedIndexChanged);
			// 
			// SettingsDisplayerControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.settingsPanel);
			this.Controls.Add(this.settingCategoriesList);
			this.Name = "SettingsDisplayerControl";
			this.Size = new System.Drawing.Size(377, 206);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel settingsPanel;
		private System.Windows.Forms.ListBox settingCategoriesList;
	}
}
