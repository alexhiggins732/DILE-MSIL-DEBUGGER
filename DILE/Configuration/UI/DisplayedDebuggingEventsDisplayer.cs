using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI.Debug;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class DisplayedDebuggingEventsDisplayer : BaseSettingsDisplayer
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

			foreach (string debugEventName in Enum.GetNames(typeof(DebugEventType)))
			{
				DebugEventType debugEventType = (DebugEventType)Enum.Parse(typeof(DebugEventType), debugEventName);

				if (debugEventType != DebugEventType.None && debugEventType != DebugEventType.AllSet)
				{
					DebugEventsList.Items.Add(debugEventType, Settings.Instance.DisplayDebugEvent(debugEventType));
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
			label.Text = "Display the following debugging events in the Debug Output Panel:";
			panel.Controls.Add(label);

			DebugEventsList = new CheckedListBox();
			DebugEventsList.Dock = DockStyle.Fill;
			FillDebugEventsList();
			panel.Controls.Add(DebugEventsList);
		}

		public override void ReadSettings()
		{
			DebugEventType displayedDebugEvents = DebugEventType.None;

			foreach (DebugEventType debugEventType in DebugEventsList.CheckedItems)
			{
				displayedDebugEvents |= debugEventType;
			}

			Settings.Instance.DisplayedDebugEvents = displayedDebugEvents;
		}

		public override string ToString()
		{
			return "Displayed debug events";
		}
	}
}