using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Controls;
using System.Collections.Specialized;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	public partial class StartPage : DocumentContent
	{
		public StartPage()
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			DoubleBuffered = true;
			InitializeComponent();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool result = false;

			if (msg.Msg == Constants.WM_KEYDOWN || msg.Msg == Constants.WM_SYSKEYDOWN)
			{
				if (keyData == (Keys.Control | Keys.Tab))
				{
					UIHandler.Instance.DisplayDocumentSelector();
					result = true;
				}
			}

			if (!result)
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}

			return result;
		}

		private void StartPage_Enter(object sender, EventArgs e)
		{
			UIHandler.Instance.DocumentActivated(this);
		}

		private PathLabel CreateLabel(string text)
		{
			PathLabel result = new PathLabel();
			result.Dock = DockStyle.Fill;
			result.Cursor = Cursors.Hand;
			result.Font = new Font(new FontFamily("Microsoft Sans Serif"), 10);
			result.ForeColor = Color.FromArgb(23, 82, 182);
			result.Text = text;

			result.Click += new EventHandler(PathLabel_Click);

			toolTip.SetToolTip(result, text);

			return result;
		}

		private void PathLabel_Click(object sender, EventArgs e)
		{
			PathLabel pathLabel = (PathLabel)sender;
			RecentItemType itemType = (RecentItemType)pathLabel.Tag;

			switch (itemType)
			{
				case RecentItemType.RecentProject:
					UIHandler.Instance.MainForm.OpenProject(pathLabel.Text, false);
					Settings.Instance.MoveProjectToFirst(pathLabel.Text);
					break;

				case RecentItemType.RecentAssembly:
					UIHandler.Instance.AddAssembly(pathLabel.Text);
					Settings.Instance.MoveAssemblyToFirst(pathLabel.Text);
					break;

				case RecentItemType.RecentDumpFile:
					UIHandler.Instance.OpenDumpFile(pathLabel.Text);
					Settings.Instance.MoveDumpFileToFirst(pathLabel.Text);
					break;

				default:
					throw new NotSupportedException();
			}
		}

		private void PrepareTable(TableLayoutPanel panel, int requiredRowCount)
		{
			if (panel.RowCount < requiredRowCount + 1)
			{
				panel.RowCount = requiredRowCount + 1;

				panel.RowStyles.Clear();
				while (panel.RowStyles.Count < requiredRowCount)
				{
					panel.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
				}
				panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			}
			else if (panel.RowCount > requiredRowCount + 1)
			{
				while (panel.Controls.Count > requiredRowCount)
				{
					panel.Controls.RemoveAt(panel.Controls.Count - 1);
				}

				panel.RowCount = requiredRowCount + 1;
				panel.RowStyles[panel.RowCount] = new RowStyle(SizeType.AutoSize);
			}
		}

		private void DisplayList(IList<string> items, RecentItemType itemType, TableLayoutPanel table)
		{
			PrepareTable(table, items.Count);

			for (int index = 0; index < items.Count; index++)
			{
				string item = items[index];

				if (index < table.Controls.Count)
				{
					PathLabel itemLabel = (PathLabel)table.Controls[index];

					itemLabel.Text = item;
				}
				else
				{
					PathLabel itemLabel = CreateLabel(item);
					itemLabel.Tag = itemType;

					table.Controls.Add(itemLabel);
				}
			}
		}

		private void UpdateRssFeed(RssBrowser rssBrowser, int updatePeriod, DateTime? lastUpdateTime)
		{
			bool updateRss = false;

			if (updatePeriod > 0)
			{
				if (lastUpdateTime == null)
				{
					updateRss = true;
				}
				else if ((DateTime.Now - lastUpdateTime.Value).TotalDays > updatePeriod)
				{
					updateRss = true;
				}
			}

			if (updateRss)
			{
				rssBrowser.UpdateRssAsync();
			}
		}

		private void StartPage_Load(object sender, EventArgs e)
		{
			parentTable.VerticalScroll.Visible = true;
			parentTable.AutoScroll = true;

			appVersionLabel.Text = "DILE v" + Settings.Instance.VersionNumber;

			Settings.Instance.RecentProjectsChanged += new NoArgumentsDelegate(Settings_RecentProjectsChanged);
			Settings.Instance.RecentAssembliesChanged += new NoArgumentsDelegate(Settings_RecentAssembliesChanged);
			Settings.Instance.RecentDumpFilesChanged += new NoArgumentsDelegate(Settings_RecentDumpFilesChanged);

			DisplayList(Settings.Instance.RecentProjects, RecentItemType.RecentProject, recentProjectsTable);
			DisplayList(Settings.Instance.RecentAssemblies, RecentItemType.RecentAssembly, recentAssembliesTable);
			DisplayList(Settings.Instance.RecentDumpFiles, RecentItemType.RecentDumpFile, recentDumpFilesTable);

			projectNewsRssBrowser.RssTransformer = Properties.Resources.RssFeed;
			projectNewsRssBrowser.DisplayDefaultFeed(Settings.Instance.LastProjectNewsFeed);

			releasesRssBrowser.RssTransformer = Properties.Resources.ReleasesRssFeed;
			releasesRssBrowser.DisplayDefaultFeed(Settings.Instance.LastReleasesFeed);

			blogRssBrowser.RssTransformer = Properties.Resources.RssFeed;
			blogRssBrowser.DisplayDefaultFeed(Settings.Instance.LastBlogFeed);

			UpdateRssFeed(projectNewsRssBrowser, Settings.Instance.ProjectNewsFeedUpdatePeriod, Settings.Instance.LastProjectNewsFeedUpdate);
			UpdateRssFeed(releasesRssBrowser, Settings.Instance.ReleasesFeedUpdatePeriod, Settings.Instance.LastReleasesFeedUpdate);
			UpdateRssFeed(blogRssBrowser, Settings.Instance.BlogFeedUpdatePeriod, Settings.Instance.LastBlogFeedUpdate);
		}

		private void Settings_RecentProjectsChanged()
		{
			//TODO Optimize this; don't update the content of the whole table.
			DisplayList(Settings.Instance.RecentProjects, RecentItemType.RecentProject, recentProjectsTable);
		}

		private void Settings_RecentAssembliesChanged()
		{
			//TODO Optimize this; don't update the content of the whole table.
			DisplayList(Settings.Instance.RecentAssemblies, RecentItemType.RecentAssembly, recentAssembliesTable);
		}

		private void Settings_RecentDumpFilesChanged()
		{
			//TODO Optimize this; don't update the content of the whole table.
			DisplayList(Settings.Instance.RecentDumpFiles, RecentItemType.RecentDumpFile, recentDumpFilesTable);
		}

		private void projectNewsRssBrowser_FeedUpdated(object sender, FeedUpdatedEventArgs e)
		{
			try
			{
				Settings.Instance.LastProjectNewsFeed = e.FeedHtml;
				Settings.Instance.LastProjectNewsFeedUpdate = DateTime.Now;
				Settings.SaveConfiguration();
			}
			finally
			{
			}
		}

		private void releasesRssBrowser_FeedUpdated(object sender, FeedUpdatedEventArgs e)
		{
			try
			{
				Settings.Instance.LastReleasesFeed = e.FeedHtml;
				Settings.Instance.LastReleasesFeedUpdate = DateTime.Now;
				Settings.SaveConfiguration();
			}
			finally
			{
			}
		}

		private void blogRssBrowser_FeedUpdated(object sender, FeedUpdatedEventArgs e)
		{
			try
			{
				Settings.Instance.LastBlogFeed = e.FeedHtml;
				Settings.Instance.LastBlogFeedUpdate = DateTime.Now;
				Settings.SaveConfiguration();
			}
			finally
			{
			}
		}

		private void rssBrowser_FileDragDrop(object sender, FileDragDropEventArgs e)
		{
			if (string.Equals(e.DraggedObjectUri.Scheme, "file", StringComparison.OrdinalIgnoreCase))
			{
				UIHandler.Instance.OpenFiles(new StringCollection() { e.DraggedObjectUri.LocalPath });
			}
		}
	}
}