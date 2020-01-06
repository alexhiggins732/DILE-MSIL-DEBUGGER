using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class BoolValueFormatter : BaseValueFormatter, ISimpleTypeValueFormatter
	{
		private string boolValue;
		private string BoolValue
		{
			get
			{
				return boolValue;
			}
			set
			{
				boolValue = value;
			}
		}

		public BoolValueFormatter(bool boolValue)
		{
			BoolValue = Convert.ToString(boolValue);
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return BoolValue;
		}

		#region ISimpleTypeValueFormatter Members

		public string GetNumberTypeName()
		{
			return Constants.BooleanTypeName;
		}

		#endregion
	}
}