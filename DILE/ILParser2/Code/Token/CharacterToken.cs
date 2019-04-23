using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class CharacterToken : ValueTokenBase<char>
	{
		public CharacterToken(char value)
			: base(TokenKind.Character, value)
		{
		}
	}
}