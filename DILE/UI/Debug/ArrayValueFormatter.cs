using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class ArrayValueFormatter : BaseValueFormatter
	{
		private uint elementCount;
		private uint ElementCount
		{
			get
			{
				return elementCount;
			}
			set
			{
				elementCount = value;
			}
		}

		public ArrayValueFormatter(uint elementCount)
		{
			ElementCount = elementCount;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return string.Format("<array count={0}>", HelperFunctions.FormatNumber(ElementCount));
		}
	}
}