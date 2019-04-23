using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Disassemble;

namespace Dile.UI
{
	public partial class QuickSearchPanel : BasePanel
	{
		private const int ItemTypeColumnWidth = 100;

		private QuickSearch quickFinder = null;
		private QuickSearch QuickFinder
		{
			get
			{
				return quickFinder;
			}
			set
			{
				quickFinder = value;
			}
		}

		private Dictionary<Assembly, ListViewGroup> foundAssemblies;
		private Dictionary<Assembly, ListViewGroup> FoundAssemblies
		{
			get
			{
				return foundAssemblies;
			}
			set
			{
				foundAssemblies = value;
			}
		}

		private ToolStripMenuItem displayItemMenuItem;
		private ToolStripMenuItem DisplayItemMenuItem
		{
			get
			{
				return displayItemMenuItem;
			}
			set
			{
				displayItemMenuItem = value;
			}
		}

		private ToolStripMenuItem locateInProjectExplorerMenuItem;
		private ToolStripMenuItem LocateInProjectExplorerMenuItem
		{
			get
			{
				return locateInProjectExplorerMenuItem;
			}
			set
			{
				locateInProjectExplorerMenuItem = value;
			}
		}

		public QuickSearchPanel()
		{
			InitializeComponent();

			foundItemsList.Initialize();

			DisplayItemMenuItem = new ToolStripMenuItem("Display item");
			DisplayItemMenuItem.Click += new EventHandler(DisplayItemMenuItem_Click);
			foundItemsList.ItemContextMenu.Items.Insert(0, DisplayItemMenuItem);

			LocateInProjectExplorerMenuItem = new ToolStripMenuItem("Locate in Project Explorer");
			LocateInProjectExplorerMenuItem.Click += new EventHandler(LocateInProjectExplorerMenuItem_Click);
			foundItemsList.ItemContextMenu.Items.Insert(1, LocateInProjectExplorerMenuItem);

			foundItemsList.ItemContextMenu.Opening += new CancelEventHandler(ItemContextMenu_Opening);
		}

		private void DisplayItemMenuItem_Click(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void LocateInProjectExplorerMenuItem_Click(object sender, EventArgs e)
		{
			if (foundItemsList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = foundItemsList.SelectedItems[0];
				TokenBase tokenObject = selectedItem.Tag as TokenBase;

				if (tokenObject != null)
				{
					UIHandler.Instance.MainForm.ProjectExplorer.LocateTokenNode(tokenObject);
				}
			}
		}

		private void ItemContextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool isOneItemSelected = (foundItemsList.SelectedItems.Count == 1);

			DisplayItemMenuItem.Enabled = isOneItemSelected;
			LocateInProjectExplorerMenuItem.Enabled = isOneItemSelected;
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			foundItemsList.Items.Clear();
		}

		private void FoundItem(object sender, FoundItemEventArgs eventArgs)
		{
			if (sender != QuickFinder)
			{
				eventArgs.Cancel = true;
			}
			else
			{
				ListViewGroup assemblyGroup = (FoundAssemblies.ContainsKey(eventArgs.Assembly) ? FoundAssemblies[eventArgs.Assembly] : null);

				if (assemblyGroup == null)
				{
					assemblyGroup = new ListViewGroup(eventArgs.Assembly.Name);
					foundItemsList.Groups.Add(assemblyGroup);
					FoundAssemblies[eventArgs.Assembly] = assemblyGroup;
				}

				ListViewItem foundItem = new ListViewItem(new string[] { eventArgs.FoundTokenObject.Name, Convert.ToString(eventArgs.FoundTokenObject.ItemType) });
				foundItem.Group = assemblyGroup;
				foundItem.ToolTipText = eventArgs.FoundTokenObject.Name;
				foundItem.Tag = eventArgs.FoundTokenObject;
				foundItemsList.Items.Add(foundItem);
			}
		}

		public void ShowItems()
		{
			searchText.Text = string.Empty;
			ShowItems(string.Empty);
		}

		public void ShowItems(string searchText)
		{
			foundItemsList.Items.Clear();
			foundItemsList.Groups.Clear();
			FoundAssemblies = new Dictionary<Assembly, ListViewGroup>();

			QuickFinder = new QuickSearch(this, new FoundItem(FoundItem));
			QuickFinder.StartSearch(searchText);
		}

		private void settingsButton_Click(object sender, EventArgs e)
		{
			QuickSearchSettingsForm settingsForm = new QuickSearchSettingsForm();
			settingsForm.ShowDialog();
		}

		private void searchText_TextChanged(object sender, EventArgs e)
		{
			ShowItems(searchText.Text);
		}

		private void ShowCodeObject()
		{
			if (foundItemsList.SelectedItems.Count == 1)
			{
				ListViewItem selectedItem = foundItemsList.SelectedItems[0];
				IMultiLine codeObject = selectedItem.Tag as IMultiLine;

				if (codeObject != null)
				{
					UIHandler.Instance.ShowCodeObject(codeObject, new CodeObjectDisplayOptions());
				}
			}
		}

		private void foundItemsList_DoubleClick(object sender, EventArgs e)
		{
			ShowCodeObject();
		}

		private void foundItemsList_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Space)
			{
				ShowCodeObject();
			}
		}
		
		private void foundItemsList_Resize(object sender, EventArgs e)
		{
			itemTypeColumnHeader.Width = ItemTypeColumnWidth;
			itemNameColumnHeader.Width = foundItemsList.Width - ItemTypeColumnWidth;
		}

		private void QuickSearchPanel_VisibleChanged(object sender, EventArgs e)
		{
			if (Visible)
			{
				searchText.Focus();
			}
		}

		private void searchText_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Down)
			{
				foundItemsList.Focus();

				if (foundItemsList.FocusedItem == null && foundItemsList.Items.Count > 0)
				{
					foundItemsList.Items[0].Focused = true;
				}

				if (foundItemsList.FocusedItem != null)
				{
					foundItemsList.FocusedItem.Selected = true;
				}
			}
			else if (e.KeyData == Keys.Escape)
			{
				QuickFinder = null;
			}
		}
	}
}