using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser2.Grammar
{
	public class TokenBase
	{
		public TokenKind Kind
		{
			get;
			private set;
		}

		public TokenBase(TokenKind kind)
		{
			Kind = kind;
		}

		public override string ToString()
		{
			return Convert.ToString(Kind);
		}
	}
}