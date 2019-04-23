using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Grammar
{
	public class StringToken : TokenBase
	{
		public string Value
		{
			get;
			private set;
		}

		public StringToken(TokenKind kind, string value)
			: base(kind)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Kind + ", " + Value;
		}
	}
}
