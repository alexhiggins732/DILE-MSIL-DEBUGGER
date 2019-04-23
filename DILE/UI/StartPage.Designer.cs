namespace Dile.UI
{
	partial class StartPage
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
			this.components = new System.ComponentModel.Container();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.appVersionLabel = new System.Windows.Forms.Label();
			this.parentTable = new System.Windows.Forms.TableLayoutPanel();
			this.releasesRssBrowser = new Dile.Controls.RssBrowser();
			this.label2 = new System.Windows.Forms.Label();
			this.recentDumpFilesTable = new System.Windows.Forms.TableLayoutPanel();
			this.recentAssembliesTable = new System.Windows.Forms.TableLayoutPanel();
			this.recentProjectsTable = new System.Windows.Forms.TableLayoutPanel();
			this.projectNewsRssBrowser = new Dile.Controls.RssBrowser();
			this.blogRssBrowser = new Dile.Controls.RssBrowser();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.parentTable.SuspendLayout();
			this.SuspendLayout();
			// 
			// appVersionLabel
			// 
			this.appVersionLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.appVersionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.appVersionLabel.Location = new System.Drawing.Point(0, 0);
			this.appVersionLabel.Name = "appVersionLabel";
			this.appVersionLabel.Size = new System.Drawing.Size(618, 21);
			this.appVersionLabel.TabIndex = 9;
			this.appVersionLabel.Text = "DILE v0.2.12";
			this.appVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// parentTable
			// 
			this.parentTable.ColumnCount = 6;
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.parentTable.Controls.Add(this.releasesRssBrowser, 2, 3);
			this.parentTable.Controls.Add(this.label2, 2, 2);
			this.parentTable.Controls.Add(this.recentDumpFilesTable, 4, 1);
			this.parentTable.Controls.Add(this.recentAssembliesTable, 2, 1);
			this.parentTable.Controls.Add(this.recentProjectsTable, 0, 1);
			this.parentTable.Controls.Add(this.projectNewsRssBrowser, 0, 3);
			this.parentTable.Controls.Add(this.blogRssBrowser, 4, 3);
			this.parentTable.Controls.Add(this.label6, 4, 2);
			this.parentTable.Controls.Add(this.label5, 0, 2);
			this.parentTable.Controls.Add(this.label1, 4, 0);
			this.parentTable.Controls.Add(this.label4, 2, 0);
			this.parentTable.Controls.Add(this.label3, 0, 0);
			this.parentTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.parentTable.Location = new System.Drawing.Point(0, 21);
			this.parentTable.Name = "parentTable";
			this.parentTable.RowCount = 4;
			this.parentTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.parentTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.parentTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.parentTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.parentTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.parentTable.Size = new System.Drawing.Size(618, 504);
			this.parentTable.TabIndex = 10;
			// 
			// releasesRssBrowser
			// 
			this.parentTable.SetColumnSpan(this.releasesRssBrowser, 2);
			this.releasesRssBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.releasesRssBrowser.Location = new System.Drawing.Point(207, 45);
			this.releasesRssBrowser.Name = "releasesRssBrowser";
			this.releasesRssBrowser.RssTransformer = null;
			this.releasesRssBrowser.RssUrl = "http://sourceforge.net/api/file/index/project-id/112895/mtime/desc/limit/20/rss";
			this.releasesRssBrowser.Size = new System.Drawing.Size(198, 456);
			this.releasesRssBrowser.TabIndex = 16;
			this.releasesRssBrowser.TabStop = false;
			this.releasesRssBrowser.FeedUpdated += new Dile.Controls.FeedUpdatedEventHandler(this.releasesRssBrowser_FeedUpdated);
			this.releasesRssBrowser.FileDragDrop += new Dile.Controls.FileDragDropEventHandler(this.rssBrowser_FileDragDrop);
			// 
			// label2
			// 
			this.label2.AutoEllipsis = true;
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label2, 2);
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic);
			this.label2.Location = new System.Drawing.Point(207, 24);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(198, 18);
			this.label2.TabIndex = 15;
			this.label2.Text = "Latest Releases";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// recentDumpFilesTable
			// 
			this.recentDumpFilesTable.AutoSize = true;
			this.recentDumpFilesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.recentDumpFilesTable.ColumnCount = 1;
			this.parentTable.SetColumnSpan(this.recentDumpFilesTable, 2);
			this.recentDumpFilesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.recentDumpFilesTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.recentDumpFilesTable.Location = new System.Drawing.Point(411, 21);
			this.recentDumpFilesTable.Name = "recentDumpFilesTable";
			this.recentDumpFilesTable.RowCount = 1;
			this.recentDumpFilesTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.recentDumpFilesTable.Size = new System.Drawing.Size(204, 1);
			this.recentDumpFilesTable.TabIndex = 14;
			// 
			// recentAssembliesTable
			// 
			this.recentAssembliesTable.AutoSize = true;
			this.recentAssembliesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.recentAssembliesTable.ColumnCount = 1;
			this.parentTable.SetColumnSpan(this.recentAssembliesTable, 2);
			this.recentAssembliesTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.recentAssembliesTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.recentAssembliesTable.Location = new System.Drawing.Point(207, 21);
			this.recentAssembliesTable.Name = "recentAssembliesTable";
			this.recentAssembliesTable.RowCount = 1;
			this.recentAssembliesTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.recentAssembliesTable.Size = new System.Drawing.Size(198, 1);
			this.recentAssembliesTable.TabIndex = 13;
			// 
			// recentProjectsTable
			// 
			this.recentProjectsTable.AutoSize = true;
			this.recentProjectsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.recentProjectsTable.ColumnCount = 1;
			this.parentTable.SetColumnSpan(this.recentProjectsTable, 2);
			this.recentProjectsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.recentProjectsTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.recentProjectsTable.Location = new System.Drawing.Point(3, 21);
			this.recentProjectsTable.Name = "recentProjectsTable";
			this.recentProjectsTable.RowCount = 1;
			this.recentProjectsTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.recentProjectsTable.Size = new System.Drawing.Size(198, 1);
			this.recentProjectsTable.TabIndex = 12;
			// 
			// projectNewsRssBrowser
			// 
			this.parentTable.SetColumnSpan(this.projectNewsRssBrowser, 2);
			this.projectNewsRssBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.projectNewsRssBrowser.Location = new System.Drawing.Point(3, 45);
			this.projectNewsRssBrowser.Name = "projectNewsRssBrowser";
			this.projectNewsRssBrowser.RssTransformer = null;
			this.projectNewsRssBrowser.RssUrl = "http://sourceforge.net/export/rss2_projnews.php?group_id=112895";
			this.projectNewsRssBrowser.Size = new System.Drawing.Size(198, 456);
			this.projectNewsRssBrowser.TabIndex = 11;
			this.projectNewsRssBrowser.TabStop = false;
			this.projectNewsRssBrowser.FeedUpdated += new Dile.Controls.FeedUpdatedEventHandler(this.projectNewsRssBrowser_FeedUpdated);
			this.projectNewsRssBrowser.FileDragDrop += new Dile.Controls.FileDragDropEventHandler(this.rssBrowser_FileDragDrop);
			// 
			// blogRssBrowser
			// 
			this.parentTable.SetColumnSpan(this.blogRssBrowser, 2);
			this.blogRssBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.blogRssBrowser.Location = new System.Drawing.Point(411, 45);
			this.blogRssBrowser.Name = "blogRssBrowser";
			this.blogRssBrowser.RssTransformer = null;
			this.blogRssBrowser.RssUrl = "http://feeds.feedburner.com/pzsolt?format=xml";
			this.blogRssBrowser.Size = new System.Drawing.Size(204, 456);
			this.blogRssBrowser.TabIndex = 10;
			this.blogRssBrowser.TabStop = false;
			this.blogRssBrowser.FeedUpdated += new Dile.Controls.FeedUpdatedEventHandler(this.blogRssBrowser_FeedUpdated);
			this.blogRssBrowser.FileDragDrop += new Dile.Controls.FileDragDropEventHandler(this.rssBrowser_FileDragDrop);
			// 
			// label6
			// 
			this.label6.AutoEllipsis = true;
			this.label6.AutoSize = true;
			this.label6.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label6, 2);
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic);
			this.label6.Location = new System.Drawing.Point(411, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(204, 18);
			this.label6.TabIndex = 9;
			this.label6.Text = "Blog";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.AutoEllipsis = true;
			this.label5.AutoSize = true;
			this.label5.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label5, 2);
			this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic);
			this.label5.Location = new System.Drawing.Point(3, 24);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(198, 18);
			this.label5.TabIndex = 8;
			this.label5.Text = "Project News";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label1, 2);
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic);
			this.label1.Location = new System.Drawing.Point(411, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(204, 18);
			this.label1.TabIndex = 7;
			this.label1.Text = "Recent Memory Dump Files";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label4, 2);
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic);
			this.label4.Location = new System.Drawing.Point(207, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(198, 18);
			this.label4.TabIndex = 6;
			this.label4.Text = "Recent Assemblies";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.AutoEllipsis = true;
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.SystemColors.Control;
			this.parentTable.SetColumnSpan(this.label3, 2);
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(3, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(198, 18);
			this.label3.TabIndex = 2;
			this.label3.Text = "Recent Projects";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// StartPage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(618, 525);
			this.Controls.Add(this.parentTable);
			this.Controls.Add(this.appVersionLabel);
			this.DoubleBuffered = true;
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "StartPage";
			this.TabText = "Start Page";
			this.Text = "StartPage";
			this.Load += new System.EventHandler(this.StartPage_Load);
			this.Enter += new System.EventHandler(this.StartPage_Enter);
			this.parentTable.ResumeLayout(false);
			this.parentTable.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.Label appVersionLabel;
		private System.Windows.Forms.TableLayoutPanel parentTable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private Controls.RssBrowser projectNewsRssBrowser;
		private Controls.RssBrowser blogRssBrowser;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TableLayoutPanel recentAssembliesTable;
		private System.Windows.Forms.TableLayoutPanel recentProjectsTable;
		private System.Windows.Forms.TableLayoutPanel recentDumpFilesTable;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label label2;
		private Controls.RssBrowser releasesRssBrowser;
	}
}