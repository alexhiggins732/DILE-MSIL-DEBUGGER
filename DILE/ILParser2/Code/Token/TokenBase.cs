using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public abstract class TokenBase
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
	}
}