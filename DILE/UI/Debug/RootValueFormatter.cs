using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class RootValueFormatter : BaseValueFormatter
	{
		public RootValueFormatter(string name)
		{
			Name = name;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return Name;
		}
	}
}