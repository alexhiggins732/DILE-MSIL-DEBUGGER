using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class Float64Token : ValueTokenBase<double>
	{
		public Float64Token(double value)
			: base(TokenKind.Float64, value)
		{
		}
	}
}