using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Windows.Forms;
using System.Xml;

namespace Dile.Configuration
{
	public class MenuItemShortcut
	{
		private MenuItem menuItem;
		private MenuItem MenuItem
		{
			get
			{
				return menuItem;
			}
			set
			{
				menuItem = value;
			}
		}

		private Shortcut shortcut;
		private Shortcut Shortcut
		{
			get
			{
				return shortcut;
			}
			set
			{
				shortcut = value;
			}
		}

		public bool HasSubNodes
		{
			get
			{
				return (MenuItem.MenuItems != null && MenuItem.MenuItems.Count > 0);
			}
		}

		public MenuItemShortcut(MenuItem menuItem)
		{
			MenuItem = menuItem;
			Shortcut = MenuItem.Shortcut;
		}

		public string GetText()
		{
			string result = menuItem.Text.Replace("&", string.Empty);

			if (!HasSubNodes)
			{
				result = string.Format("{0} [{1}]", result, GetShortcutText());
			}

			return result;
		}

		public string GetShortcutText()
		{
			return ShortcutConverter.ConvertShortcutToString(Shortcut);
		}

		public void UpdateShortcut(Shortcut shortcut)
		{
			Shortcut = shortcut;
		}

		public void UpdateMenuItem()
		{
			MenuItem.Shortcut = Shortcut;
		}
	}
}