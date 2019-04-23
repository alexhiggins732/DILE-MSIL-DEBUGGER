using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class StringILCode : BaseParameterILCode<uint, UserString>
	{
		public StringILCode()
		{
		}

		public override void DecodeParameter()
		{
			Text = string.Format("{0} \"{1}\"", OpCode.Name, DecodedParameter.Name);
		}
	}
}