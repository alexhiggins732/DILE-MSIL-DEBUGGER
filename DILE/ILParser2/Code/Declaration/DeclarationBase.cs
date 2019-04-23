using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.ILParser2.Grammar;

using Grammar = Dile.ILParser2.Grammar;

namespace Dile.ILParser2.Code.Declaration
{
	public abstract class DeclarationBase
	{
		public string Name
		{
			get;
			private set;
		}

		public LanguageDefinition Definition
		{
			get;
			private set;
		}

		public DeclarationBase(LanguageElement declLanguageElement, string tokenName)
		{
			Definition = declLanguageElement.Definitions.Single(languageDefinition =>
			{
				StringToken firstToken = (StringToken)languageDefinition.Tokens[0];

				return string.Equals(firstToken.Value, tokenName, StringComparison.Ordinal);
			});
		}
	}
}