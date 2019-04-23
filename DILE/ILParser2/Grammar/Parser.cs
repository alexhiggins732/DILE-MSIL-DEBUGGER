using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.ExtensionMethods.IList;

namespace Dile.ILParser2.Grammar
{
	public class Parser
	{
		public IEnumerator<TokenBase> TokenStream
		{
			get;
			private set;
		}

		public Parser(IEnumerator<TokenBase> tokenStream)
		{
			TokenStream = tokenStream;
		}

		private IList<LanguageDefinition> ParseDefinitions()
		{
			List<LanguageDefinition> result = new List<LanguageDefinition>();
			LanguageDefinition definition = new LanguageDefinition();

			while (TokenStream.Current.Kind != TokenKind.SemiColon)
			{
				switch(TokenStream.Current.Kind)
				{
					case TokenKind.Pipe:
						result.Add(definition);
						definition = new LanguageDefinition();
						break;

					default:
						definition.Tokens.Add(TokenStream.Current);
						break;
				}

				TokenStream.MoveNext();
			}

			result.Add(definition);

			return result;
		}

		private string GetLanguageElementName()
		{
			string result = string.Empty;
			StringToken stringToken = TokenStream.Current as StringToken;

			if (stringToken == null)
			{
				result = Convert.ToString(TokenStream.Current.Kind);
			}
			else
			{
				result = stringToken.Value;
			}

			return result;
		}

		private LanguageElement ParseLanguageElement()
		{
			LanguageElement result = new LanguageElement(TokenStream.Current);

			TokenStream.MoveNext();
			if (TokenStream.Current.Kind != TokenKind.Colon)
			{
				throw new InvalidOperationException();
			}

			TokenStream.MoveNext();
			result.Definitions.AddRange(ParseDefinitions());
			TokenStream.MoveNext();

			return result;
		}

		public IList<LanguageElement> Parse()
		{
			List<LanguageElement> result = new List<LanguageElement>();

			TokenStream.MoveNext();
			while (TokenStream.Current.Kind != TokenKind.EOF)
			{
				LanguageElement languageElement = ParseLanguageElement();
				result.Add(languageElement);
			}

			return result;
		}
	}
}