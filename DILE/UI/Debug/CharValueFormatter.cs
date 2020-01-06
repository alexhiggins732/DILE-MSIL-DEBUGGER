using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class CharValueFormatter : BaseValueFormatter, ISimpleTypeValueFormatter
	{
		private string charValue;
		private string CharValue
		{
			get
			{
				return charValue;
			}
			set
			{
				charValue = value;
			}
		}

		public CharValueFormatter(char charValue)
		{
			CharValue = string.Format("'{0}'", charValue);
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return CharValue;
		}

		#region ISimpleTypeValueFormatter Members

		public string GetNumberTypeName()
		{
			return Constants.CharTypeName;
		}

		#endregion
	}
}