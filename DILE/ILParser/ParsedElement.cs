using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public class ParsedElement<T> : IParsedElement
	{
		private LanguageElement languageElement;
		public LanguageElement LanguageElement
		{
			get
			{
				return languageElement;
			}
			private set
			{
				languageElement = value;
			}
		}

		private T parsedValue;
		public T ParsedValue
		{
			get
			{
				return parsedValue;
			}
			private set
			{
				parsedValue = value;
			}
		}

		public ParsedElement(LanguageElement languageElement, T parsedValue)
		{
			LanguageElement = languageElement;
			ParsedValue = parsedValue;
		}
	}
}