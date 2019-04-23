using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class Int64Token : ValueTokenBase<long>
	{
		public Int64Token(long value)
			: base(TokenKind.Int64, value)
		{
		}
	}
}