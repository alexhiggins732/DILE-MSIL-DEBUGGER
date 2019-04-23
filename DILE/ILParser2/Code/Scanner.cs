using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.ILParser2.Code.Token;
using System.Globalization;
using System.IO;

namespace Dile.ILParser2.Code
{
	public class Scanner
	{
		public TextReader ILReader
		{
			get;
			private set;
		}

		private IList<TokenBase> PeekedTokens
		{
			get;
			set;
		}

		public TokenBase CurrentToken
		{
			get;
			private set;
		}

		private StringBuilder TextBuilder
		{
			get;
			set;
		}

		public Scanner(TextReader ilReader)
		{
			ILReader = ilReader;

			PeekedTokens = new List<TokenBase>();
			TextBuilder = new StringBuilder();
		}

		private TokenBase ScanText()
		{
			TokenBase result = null;
			TextBuilder.Length = 0;
			char nextCharacter;

			do
			{
				char character = (char)ILReader.Read();
				TextBuilder.Append(character);

				nextCharacter = (char)ILReader.Read();
			} while (char.IsLetterOrDigit(nextCharacter));

			return result;
		}

		private TokenBase ScanNumber()
		{
			TokenBase result = null;
			TextBuilder.Length = 0;
			char nextCharacter;

			do
			{
				char character = (char)ILReader.Read();
				TextBuilder.Append(character);

				nextCharacter = (char)ILReader.Peek();
			}
			while (char.IsDigit(nextCharacter) || nextCharacter == '.' || nextCharacter == '+' || nextCharacter == '-');

			NumberStyles defaultNumberStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowHexSpecifier | NumberStyles.AllowLeadingSign;

			string stringNumber = TextBuilder.ToString();
			int intNumber;
			long longNumber;
			double doubleNumber;
			if (int.TryParse(stringNumber, defaultNumberStyles, CultureInfo.InvariantCulture, out intNumber))
			{
				result = new Int32Token(intNumber);
			}
			else if (long.TryParse(stringNumber, defaultNumberStyles, CultureInfo.InvariantCulture, out longNumber))
			{
				result = new Int64Token(longNumber);
			}
			else if (double.TryParse(stringNumber, defaultNumberStyles, CultureInfo.InvariantCulture, out doubleNumber))
			{
				result = new Float64Token(doubleNumber);
			}
			else
			{
				throw new InvalidOperationException();
			}

			return result;
		}

		private TokenBase ScanSingleQuotedString()
		{
			SQStringToken result = null;

			ILReader.Read();
			bool isEscaped = false;
			char character = (char)ILReader.Read();

			if (character == '\\')
			{
				isEscaped = true;
				character = (char)ILReader.Read();
			}

			result = new SQStringToken(isEscaped, character);

			character = (char)ILReader.Read();

			if (character != '\'')
			{
				throw new InvalidOperationException();
			}

			return result;
		}

		private TokenBase ScanQuotedString()
		{
			QStringToken result = null;
			TextBuilder.Length = 0;
			char character;
			char nextCharacter;

			do
			{
				character = (char)ILReader.Read();

				if (character == '\\')
				{
					TextBuilder.Append(character);
					character = (char)ILReader.Read();
				}

				TextBuilder.Append(character);
				nextCharacter = (char)ILReader.Peek();
			} while (nextCharacter != '"');

			ILReader.Read();

			result = new QStringToken(TextBuilder.ToString());

			return result;
		}

		private TokenBase ScanCharacter()
		{
			CharacterToken result = null;
			char character = (char)ILReader.Read();

			result = new CharacterToken(character);

			return result;
		}

		public bool MoveNext()
		{
			bool result = false;

			if (ILReader.Peek() != -1)
			{
				result = true;
				char nextChar = (char)ILReader.Peek();

				if (char.IsWhiteSpace(nextChar))
				{
					ILReader.Read();
					MoveNext();
				}
				else if (nextChar == '.' || char.IsLetter(nextChar))
				{
					CurrentToken = ScanText();
				}
				else if (char.IsDigit(nextChar))
				{
					CurrentToken = ScanNumber();
				}
				else if (nextChar == '\'')
				{
					CurrentToken = ScanSingleQuotedString();
				}
				else if (nextChar == '"')
				{
					CurrentToken = ScanQuotedString();
				}
				else
				{
					CurrentToken = ScanCharacter();
				}
			}

			return result;
		}
	}
}