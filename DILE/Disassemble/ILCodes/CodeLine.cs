using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;

namespace Dile.Disassemble.ILCodes
{
	public class CodeLine
	{
		private int indentation = 0;
		public int Indentation
		{
			get
			{
				return indentation;
			}
			set
			{
				indentation = value;
			}
		}

		private string text;
		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				textLineNumber = -1;
			}
		}

		private int textLineNumber = -1;
		public int TextLineNumber
		{
			get
			{
				if (textLineNumber == -1)
				{
					textLineNumber = 0;

					bool isCarriageReturnFound = false;
					foreach (char character in Text)
					{
						switch (character)
						{
							case '\r':
								isCarriageReturnFound = true;
								textLineNumber++;
								break;

							case '\n':
								if (!isCarriageReturnFound)
								{
									textLineNumber++;
								}
								isCarriageReturnFound = false;
								break;

							case '\v':
								textLineNumber++;
								isCarriageReturnFound = false;
								break;

							default:
								isCarriageReturnFound = false;
								break;
						}
					}
				}

				return textLineNumber;
			}
		}

		public CodeLine()
		{
			Text = string.Empty;
		}

		public CodeLine(int indentation, string text)
		{
			Indentation = indentation;
			Text = text;
		}
	}
}