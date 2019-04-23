using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class NumberILCode<NumberType> : BaseParameterILCode<NumberType, NumberType>
	{
		public NumberILCode()
		{
		}

		public override void DecodeParameter()
		{
			if (DecodedParameter is float || DecodedParameter is double)
			{
				Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter);
			}
			else
			{
				Text = string.Format("{0} 0x{1:x}", OpCode.Name, DecodedParameter);
			}
		}
	}
}