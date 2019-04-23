using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class ArgumentILCode<NumberType> : BaseParameterILCode<NumberType, string>
	{
		public override void DecodeParameter()
		{
			Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter);
		}
	}
}