using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class SQStringToken : ValueTokenBase<char>
	{
		public bool IsEscaped
		{
			get;
			private set;
		}

		public SQStringToken(bool isEscaped, char value)
			: base(TokenKind.SQString, value)
		{
			IsEscaped = isEscaped;
		}
	}
}