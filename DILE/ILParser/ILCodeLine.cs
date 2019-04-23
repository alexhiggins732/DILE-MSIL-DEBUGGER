using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public class ILCodeLine
	{
		private string parsedLine;
		public string ParsedLine
		{
			get
			{
				return parsedLine;
			}
			private set
			{
				parsedLine = value;
			}
		}

		private IParsedElement[] parsedElements;
		public IParsedElement[] ParsedElements
		{
			get
			{
				return parsedElements;
			}
			private set
			{
				parsedElements = value;
			}
		}

		public ILCodeLine(string parsedLine, IParsedElement[] parsedElements)
		{
			ParsedLine = parsedLine;
			ParsedElements = parsedElements;
		}
	}
}