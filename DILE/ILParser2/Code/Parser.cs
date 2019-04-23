using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.ILParser2.Code.Declaration;
using Dile.ILParser2.Grammar;

using Grammar = Dile.ILParser2.Grammar;

namespace Dile.ILParser2.Code
{
	public class Parser
	{
		private IList<DeclarationBase> Declarations
		{
			get;
			set;
		}

		public Parser(IList<LanguageElement> languageElements)
		{
			Declarations = new List<DeclarationBase>();

			LanguageElement declLanguageElement = languageElements.Single(languageElement => languageElement.Declaration.Kind == Grammar.TokenKind.Name
				&& string.Equals(((StringToken)languageElement.Declaration).Value, "decl", StringComparison.Ordinal));

			Declarations.Add(new AssemblyReferenceDeclaration(declLanguageElement));
		}
	}
}