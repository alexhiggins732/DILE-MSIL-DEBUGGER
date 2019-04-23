using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public class ILParser
	{
		private LanguageElement startElement;
		public LanguageElement StartElement
		{
			get
			{
				return startElement;
			}
			private set
			{
				startElement = value;
			}
		}

		public ILParser(LanguageElement startElement)
		{
			StartElement = startElement;
		}
	}
}