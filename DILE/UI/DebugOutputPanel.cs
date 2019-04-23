using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.UI.Debug;
using WeifenLuo.WinFormsUI;

using Dile.Configuration;

namespace Dile.UI
{
	public partial class DebugOutputPanel : BasePanel
	{
		private DebugEventType originalDisplayedDebugEvents;
		private DebugEventType OriginalDisplayedDebugEvents
		{
			get
			{
				return originalDisplayedDebugEvents;
			}
			set
			{
				originalDisplayedDebugEvents = value;
			}
		}

		private List<DebugEventDescriptor> debugEvents = new List<DebugEventDescriptor>();
		private List<DebugEventDescriptor> DebugEvents
		{
			get
			{
				return debugEvents;
			}
			set
			{
				debugEvents = value;
			}
		}

		public DebugOutputPanel()
		{
			InitializeComponent();

			OriginalDisplayedDebugEvents = Settings.Instance.DisplayedDebugEvents;
			Settings.Instance.Changed += new NoArgumentsDelegate(Settings_Changed);
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		private void FilterDebugEvents()
		{
			eventsList.BeginUpdate();
			eventsList.Items.Clear();

			for (int index = 0; index < DebugEvents.Count; index++)
			{
				DebugEventDescriptor debugEvent = DebugEvents[index];

				if (Settings.Instance.DisplayDebugEvent(debugEvent.DebugEventType))
				{
					eventsList.Items.Add(debugEvent);
				}
			}

			eventsList.TopIndex = eventsList.Items.Count - 1;
			eventsList.EndUpdate();
		}

		private void Settings_Changed()
		{
			if (Settings.Instance.DisplayedDebugEvents != OriginalDisplayedDebugEvents)
			{
				FilterDebugEvents();
				OriginalDisplayedDebugEvents = Settings.Instance.DisplayedDebugEvents;
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			eventDetailsTree.Nodes.Clear();
			eventsList.Items.Clear();
			DebugEvents.Clear();
		}

		public void AddEvent(DebugEventDescriptor debugEventDescriptor)
		{
			DebugEvents.Add(debugEventDescriptor);

			if (Settings.Instance.DisplayDebugEvent(debugEventDescriptor.DebugEventType))
			{
				eventsList.Items.Add(debugEventDescriptor);
				eventsList.TopIndex = eventsList.Items.Count - 1;
			}
		}

		private void eventsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (eventsList.SelectedItem != null)
			{
				DebugEventDescriptor eventDescriptor = (DebugEventDescriptor)eventsList.SelectedItem;

				eventDetailsTree.Nodes.Clear();
				eventDescriptor.CreateTree(eventDetailsTree.Nodes);
				eventDetailsTree.ExpandAll();

				if (eventDetailsTree.Nodes.Count > 0)
				{
					eventDetailsTree.SelectedNode = eventDetailsTree.Nodes[0];
				}
			}
		}
	}
}