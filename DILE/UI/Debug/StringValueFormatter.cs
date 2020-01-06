using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class StringValueFormatter : BaseValueFormatter
	{
		private string stringValue;
		private string StringValue
		{
			get
			{
				return stringValue;
			}
			set
			{
				stringValue = value;
			}
		}

		public StringValueFormatter(string stringValue)
		{
			StringValue = stringValue;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return StringValue;
		}
	}
}