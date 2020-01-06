using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public abstract class BaseValueFormatter : IValueFormatter
	{
		private BaseValueRefresher valueRefresher;
		public BaseValueRefresher ValueRefresher
		{
			get
			{
				return valueRefresher;
			}
			set
			{
				valueRefresher = value;
			}
		}

		private bool isComplexType;
		public bool IsComplexType
		{
			get
			{
				return isComplexType;
			}
			set
			{
				isComplexType = value;
			}
		}

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;

				for (int index = 0; index < PointerPrefixCount; index++)
				{
					name = "*" + name;
				}
			}
		}

		private ValueFieldGroup fieldGroup;
		public ValueFieldGroup FieldGroup
		{
			get
			{
				return fieldGroup;
			}
			set
			{
				fieldGroup = value;
			}
		}

		private TreeNode fieldNode;
		public TreeNode FieldNode
		{
			get
			{
				return fieldNode;
			}
			set
			{
				fieldNode = value;
			}
		}

		private int pointerPrefixCount;
		private int PointerPrefixCount
		{
			get
			{
				return pointerPrefixCount;
			}
			set
			{
				pointerPrefixCount = value;
			}
		}

		#region IValueFormatter Members

		public void AddPointerPrefix()
		{
			PointerPrefixCount++;

			if (Name != null)
			{
				Name = "*" + Name;
			}
		}

		public abstract string GetFormattedString(bool useHexaFormatting);

		#endregion
	}
}
