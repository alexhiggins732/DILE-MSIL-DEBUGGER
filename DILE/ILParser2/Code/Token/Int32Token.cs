using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class Int32Token : ValueTokenBase<int>
	{
		public Int32Token(int value)
			: base(TokenKind.Int32, value)
		{
		}
	}
}