using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public interface IValueFormatter
	{
		BaseValueRefresher ValueRefresher
		{
			get;
			set;
		}

		bool IsComplexType
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		ValueFieldGroup FieldGroup
		{
			get;
			set;
		}

		TreeNode FieldNode
		{
			get;
			set;
		}

		void AddPointerPrefix();
		string GetFormattedString(bool useHexaFormatting);
	}
}