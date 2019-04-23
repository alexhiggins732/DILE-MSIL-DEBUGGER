using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile.UI
{
	public static class ShortcutConverter
	{
		private static Dictionary<string, Shortcut> shortcuts;
		public static Dictionary<string, Shortcut> Shortcuts
		{
			get
			{
				return shortcuts;
			}
			set
			{
				shortcuts = value;
			}
		}

		public static string ConvertShortcutToString(Shortcut shortcut)
		{
			string shortcutString = Convert.ToString(shortcut);
			StringBuilder result = new StringBuilder(shortcutString);

			if (shortcutString.StartsWith("Alt"))
			{
				result.Insert(3, " + ");
			}
			else if (shortcutString.StartsWith("CtrlShift"))
			{
				result.Insert(9, " + ");
				result.Insert(4, " + ");
			}
			else if (shortcutString.StartsWith("Ctrl"))
			{
				result.Insert(4, " + ");
			}
			else if (shortcutString.StartsWith("Shift"))
			{
				result.Insert(5, " + ");
			}

			return result.ToString();
		}

		private static void FillShortcuts()
		{
			if (Shortcuts == null)
			{
				lock (typeof(ShortcutConverter))
				{
					if (Shortcuts == null)
					{
						Array shortcutValues = Enum.GetValues(typeof(Shortcut));
						Shortcuts = new Dictionary<string, Shortcut>(shortcutValues.Length);

						for (int index = 0; index < shortcutValues.Length; index++)
						{
							object shortcutValue = shortcutValues.GetValue(index);
							string shortcutName = Enum.GetName(typeof(Shortcut), shortcutValue);

							Shortcuts[shortcutName] = (Shortcut)shortcutValue;
						}
					}
				}
			}
		}

		public static Shortcut ConvertStringToShortcut(string shortcutString)
		{
			Shortcut result = Shortcut.None;
			FillShortcuts();
			shortcutString = shortcutString.Replace(" + ", string.Empty);

			if (Shortcuts.ContainsKey(shortcutString))
			{
				result = Shortcuts[shortcutString];
			}

			return result;
		}
	}
}