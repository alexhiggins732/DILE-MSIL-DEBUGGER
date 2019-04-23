using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Grammar
{
	public class LanguageDefinition
	{
		public IList<TokenBase> Tokens
		{
			get;
			private set;
		}

		public LanguageDefinition()
		{
			Tokens = new List<TokenBase>();
		}
	}
}
