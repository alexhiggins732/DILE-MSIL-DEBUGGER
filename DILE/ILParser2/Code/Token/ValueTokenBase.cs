using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code.Token
{
	public abstract class ValueTokenBase<TValue> : TokenBase
	{
		public TValue Value
		{
			get;
			private set;
		}

		public ValueTokenBase(TokenKind kind, TValue value)
			: base(kind)
		{
			Value = value;
		}
	}
}