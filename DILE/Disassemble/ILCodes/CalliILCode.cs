using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class CalliILCode : BaseParameterILCode<uint, StandAloneSignature>
	{
		public override void DecodeParameter()
		{
			Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter.Text);
		}
	}
}