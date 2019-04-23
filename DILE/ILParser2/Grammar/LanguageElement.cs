using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Grammar
{
	public class LanguageElement
	{
		public TokenBase Declaration
		{
			get;
			private set;
		}

		public IList<LanguageDefinition> Definitions
		{
			get;
			private set;
		}

		public LanguageElement(TokenBase declaration)
		{
			Declaration = declaration;

			Definitions = new List<LanguageDefinition>();
		}
	}
}