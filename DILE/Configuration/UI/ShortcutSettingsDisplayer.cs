using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class ShortcutSettingsDisplayer : BaseSettingsDisplayer
	{
		private MainMenu menu;
		private MainMenu Menu
		{
			get
			{
				return menu;
			}
			set
			{
				menu = value;
			}
		}

		private TreeView menuTree;
		private TreeView MenuTree
		{
			get
			{
				return menuTree;
			}
			set
			{
				menuTree = value;
			}
		}

		private ComboBox shortcutComboBox;
		private ComboBox ShortcutComboBox
		{
			get
			{
				return shortcutComboBox;
			}
			set
			{
				shortcutComboBox = value;
			}
		}

		private bool handleSelectedItemChanged = true;
		private bool HandleSelectedItemChanged
		{
			get
			{
				return handleSelectedItemChanged;
			}
			set
			{
				handleSelectedItemChanged = value;
			}
		}

		public ShortcutSettingsDisplayer(MainMenu menu)
		{
			Menu = menu;
		}

		private void AddMenuTreeNodes(TreeNodeCollection nodes, Menu.MenuItemCollection menuItems)
		{
			for (int index = 0; index < menuItems.Count; index++)
			{
				MenuItem menuItem = menuItems[index];

				if (menuItem.Tag != null || (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0))
				{
					MenuItemShortcut menuItemShortcut = new MenuItemShortcut(menuItem);
					TreeNode node = nodes.Add(menuItemShortcut.GetText());
					node.Tag = menuItemShortcut;

					if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
					{
						AddMenuTreeNodes(node.Nodes, menuItem.MenuItems);
					}
				}
			}
		}

		private void CreateMenuTree(TableLayoutPanel panel)
		{
			MenuTree = new TreeView();
			MenuTree.Dock = DockStyle.Fill;

			MenuTree.BeginUpdate();
			AddMenuTreeNodes(MenuTree.Nodes, Menu.MenuItems);
			MenuTree.EndUpdate();
			panel.Controls.Add(MenuTree);

			MenuTree.AfterSelect += new TreeViewEventHandler(MenuTree_AfterSelect);

			if (MenuTree.Nodes != null && MenuTree.Nodes.Count > 0)
			{
				MenuTree.SelectedNode = MenuTree.Nodes[0];
			}
		}

		private void MenuTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			MenuItemShortcut menuItemShortcut = (MenuItemShortcut)e.Node.Tag;
			HandleSelectedItemChanged = false;

			if (menuItemShortcut.HasSubNodes)
			{
				ShortcutComboBox.Enabled = false;
				ShortcutComboBox.SelectedItem = null;
			}
			else
			{
				ShortcutComboBox.Enabled = true;
				ShortcutComboBox.SelectedItem = menuItemShortcut.GetShortcutText();
			}

			HandleSelectedItemChanged = true;
		}

		private void CreateShortcutComboBox(TableLayoutPanel panel)
		{
			ShortcutComboBox = new ComboBox();
			ShortcutComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

			Array shortcutValues = Enum.GetValues(typeof(Shortcut));
			for (int index = 0; index < shortcutValues.Length; index++)
			{
				ShortcutComboBox.Items.Add(ShortcutConverter.ConvertShortcutToString((Shortcut)shortcutValues.GetValue(index)));
			}

			ShortcutComboBox.SelectedIndexChanged += new EventHandler(ShortcutComboBox_SelectedIndexChanged);

			panel.Controls.Add(ShortcutComboBox);
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, ShortcutComboBox.Width));
		}

		private void ShortcutComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (HandleSelectedItemChanged && MenuTree.SelectedNode != null && ShortcutComboBox.SelectedItem != null)
			{
				MenuItemShortcut menuItemShortcut = (MenuItemShortcut)MenuTree.SelectedNode.Tag;
				menuItemShortcut.UpdateShortcut(ShortcutConverter.ConvertStringToShortcut((string)ShortcutComboBox.SelectedItem));
				MenuTree.SelectedNode.Text = menuItemShortcut.GetText();
			}
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 2;
			panel.RowCount = 1;
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

			CreateMenuTree(panel);
			CreateShortcutComboBox(panel);
		}

		private void UpdateMenuItems(TreeNodeCollection nodes)
		{
			foreach (TreeNode node in nodes)
			{
				MenuItemShortcut menuItemShortcut = (MenuItemShortcut)node.Tag;
				menuItemShortcut.UpdateMenuItem();

				if (node.Nodes != null && node.Nodes.Count > 0)
				{
					UpdateMenuItems(node.Nodes);
				}
			}
		}

		public override void ReadSettings()
		{
			UpdateMenuItems(MenuTree.Nodes);
		}

		public override string ToString()
		{
			return "Shortcuts";
		}
	}
}