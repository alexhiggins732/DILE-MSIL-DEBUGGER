using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public class QStringToken : ValueTokenBase<string>
	{
		public QStringToken(string value)
			: base(TokenKind.QString, value)
		{
		}
	}
}