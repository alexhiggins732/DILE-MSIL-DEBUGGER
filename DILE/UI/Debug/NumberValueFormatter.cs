using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class NumberValueFormatter<T> : BaseValueFormatter, ISimpleTypeValueFormatter
	{
		private T number;
		private T Number
		{
			get
			{
				return number;
			}
			set
			{
				number = value;
			}
		}

		public NumberValueFormatter(T number)
		{
			Number = number;
		}

		public string GetNumberTypeName()
		{
			return (Number == null ? "null value" : Number.GetType().FullName);
		}

		private bool IsIntegerType()
		{
			bool result = false;
			Type numberType = null;

			if (Number != null)
			{
				numberType = Number.GetType();

				if (numberType == typeof(byte) ||
					numberType == typeof(ushort) ||
					numberType == typeof(uint) ||
					numberType == typeof(ulong) ||
					numberType == typeof(sbyte) ||
					numberType == typeof(short) ||
					numberType == typeof(int) ||
					numberType == typeof(long))
				{
					result = true;
				}
			}

			return result;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return (IsIntegerType() ? HelperFunctions.FormatNumber(Number) : Convert.ToString(Number));
		}
	}
}