using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class ProjectDebuggingSettingsDisplayer : BaseSettingsDisplayer
	{
		private CheckedListBox debugEventsList = null;
		private CheckedListBox DebugEventsList
		{
			get
			{
				return debugEventsList;
			}
			set
			{
				debugEventsList = value;
			}
		}

		private void FillDebugEventsList()
		{
			DebugEventsList.CheckOnClick = true;
			DebugEventsList.BeginUpdate();
			DebugEventsList.Items.Clear();

			foreach(string suspendableDebugEventName in Enum.GetNames(typeof(SuspendableDebugEvent)))
			{
				SuspendableDebugEvent suspendableDebugEvent = (SuspendableDebugEvent)Enum.Parse(typeof(SuspendableDebugEvent), suspendableDebugEventName);

				if (suspendableDebugEvent != SuspendableDebugEvent.None)
				{
					DebugEventsList.Items.Add(suspendableDebugEvent, Project.Instance.SuspendOnDebugEvent(suspendableDebugEvent));
				}
			}

			DebugEventsList.EndUpdate();
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 1;

			Label label = new Label();
			label.Dock = DockStyle.Fill;
			label.Text = "Pause the debuggee on the following debugging events:";
			panel.Controls.Add(label);

			DebugEventsList = new CheckedListBox();
			DebugEventsList.Dock = DockStyle.Fill;
			FillDebugEventsList();
			panel.Controls.Add(DebugEventsList);
		}

		public override void ReadSettings()
		{
			Project.Instance.SuspendingDebugEvents.Clear();

			foreach (SuspendableDebugEvent suspendableDebugEvent in DebugEventsList.CheckedItems)
			{
				Project.Instance.SuspendingDebugEvents.Add(suspendableDebugEvent);
			}
		}

		public override string ToString()
		{
			return "Debugging";
		}
	}
}