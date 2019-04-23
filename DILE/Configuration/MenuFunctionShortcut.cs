using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	[Serializable()]
	public class MenuFunctionShortcut : IComparable
	{
		private MenuFunction menuFunction;
		[XmlAttribute()]
		public MenuFunction MenuFunction
		{
			get
			{
				return menuFunction;
			}
			set
			{
				menuFunction = value;
			}
		}

		private Shortcut shortcut;
		[XmlAttribute()]
		public Shortcut Shortcut
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

		#region IComparable Members

		public int CompareTo(object obj)
		{
			int result = 0;

			if (obj == null)
			{
				result = 1;
			}
			else
			{
				MenuFunctionShortcut otherShortcut = obj as MenuFunctionShortcut;

				if (otherShortcut != null)
				{
					result = MenuFunction.CompareTo(otherShortcut.MenuFunction);

					if (result == 0)
					{
						result = Shortcut.CompareTo(otherShortcut.Shortcut);
					}
				}
				else
				{
					throw new ArgumentException("Incorrect argument type: " + obj.GetType().FullName, "obj");
				}
			}

			return result;
		}

		#endregion

		public override bool Equals(object obj)
		{
			return (CompareTo(obj) == 0);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}