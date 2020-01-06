using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug.Dump;

namespace Dile.UI.Debug.Dump.Controls
{
	public partial class ThreadCategory : UserControl, ICategoryControl
	{
		private DumpThreadInfo[] Threads
		{
			get;
			set;
		}

		public ThreadCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			int selectedIndex = lstThreads.SelectedIndex;
			DisplayThreads();
			lstThreads.SelectedIndex = selectedIndex;
		}

		private void DisplayThreads()
		{
			lstThreads.BeginUpdate();
			lstThreads.Items.Clear();

			foreach (DumpThreadInfo thread in Threads)
			{
				lstThreads.Items.Add(new ToStringWrapper<DumpThreadInfo>(thread, HelperFunctions.FormatNumber(thread.Id)));
			}

			lstThreads.EndUpdate();
		}

		public void Initialize(DumpDebugger dumpDebugger)
		{
			Threads = dumpDebugger.GetThreads();
			
			DisplayThreads();
		}

		private void lstThreads_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToStringWrapper<DumpThreadInfo> threadWrapper = lstThreads.SelectedItem as ToStringWrapper<DumpThreadInfo>;

			if (threadWrapper != null)
			{
				lvThreadDetails.BeginUpdate();
				lvThreadDetails.Items.Clear();

				lvThreadDetails.Items.Add(new ListViewItem(new string[]
				{
					"Suspend Count",
					HelperFunctions.FormatNumber(threadWrapper.WrappedObject.SuspendCount)
				})).Tag = threadWrapper.WrappedObject.SuspendCount;

				lvThreadDetails.Items.Add(new ListViewItem(new string[]
				{
					"Priority Class",
					Convert.ToString(threadWrapper.WrappedObject.PriorityClass)
				}));

				lvThreadDetails.Items.Add(new ListViewItem(new string[]
				{
					"Priority",
					Convert.ToString(threadWrapper.WrappedObject.Priority)
				}));

				lvThreadDetails.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
				lvThreadDetails.EndUpdate();
			}
		}
	}
}
