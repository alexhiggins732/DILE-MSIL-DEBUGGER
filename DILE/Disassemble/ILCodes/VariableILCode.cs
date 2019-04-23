using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class VariableILCode<NumberType> : BaseParameterILCode<NumberType, NumberType>
	{
		public VariableILCode()
		{
		}

		public override void DecodeParameter()
		{
			Text = string.Format("{0} V_{1}", OpCode.Name, DecodedParameter);
		}
	}
}