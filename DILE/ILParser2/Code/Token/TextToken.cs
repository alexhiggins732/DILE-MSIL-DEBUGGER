using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class TextToken : ValueTokenBase<string>
	{
		public TextToken(string value)
			: base(TokenKind.Text, value)
		{
		}
	}
}