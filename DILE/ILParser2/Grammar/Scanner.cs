using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;
using System.IO;

namespace Dile.ILParser2.Grammar
{
	public class Scanner
	{
		private TokenBase ScanName(TextReader reader)
		{
			TokenBase result = null;
			StringBuilder nameBuilder = new StringBuilder();
			char nextCharacter = (char)reader.Peek();

			while (char.IsLetterOrDigit(nextCharacter) || nextCharacter == '_')
			{
				nameBuilder.Append((char)reader.Read());
				nextCharacter = (char)reader.Peek();
			}

			string name = nameBuilder.ToString();

			if (string.Equals(name, "START", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.Start);
			}
			else if (string.Equals(name, "ID", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.ID);
			}
			else if (string.Equals(name, "QSTRING", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.QString);
			}
			else if (string.Equals(name, "SQSTRING", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.SQString);
			}
			else if (string.Equals(name, "INT32", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.Int32);
			}
			else if (string.Equals(name, "INT64", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.Int64);
			}
			else if (string.Equals(name, "FLOAT64", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.Float64);
			}
			else if (string.Equals(name, "DOTTEDNAME", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.DottedName);
			}
			else if (name.StartsWith("INSTR_", true, CultureInfo.InvariantCulture))
			{
				result = new StringToken(TokenKind.Instruction, name.Substring(6));
			}
			else
			{
				result = new StringToken(TokenKind.Name, nameBuilder.ToString());
			}

			return result;
		}

		private TokenBase ScanComment(TextReader reader)
		{
			TokenBase result = null;
			StringBuilder commentBuilder = new StringBuilder();
			bool starFound = false;
			bool slashFound = false;

			while (!starFound || !slashFound)
			{
				char character = (char)reader.Read();
				commentBuilder.Append(character);

				switch (character)
				{
					case '*':
						starFound = true;
						break;

					case '/':
						if (starFound)
						{
							slashFound = true;
						}
						break;

					default:
						starFound = false;
						slashFound = false;
						break;
				}
			}

			if (string.Equals(commentBuilder.ToString(), "/* EMPTY */", StringComparison.OrdinalIgnoreCase))
			{
				result = new TokenBase(TokenKind.Empty);
			}

			return result;
		}

		private StringToken ScanConstantString(TextReader reader)
		{
			StringToken result = null;
			StringBuilder constantBuilder = new StringBuilder();
			bool escapeCharacterFound = false;
			bool apostropheFound = false;
			bool firstCharacter = true;

			while (!escapeCharacterFound && !apostropheFound)
			{
				char character = (char)reader.Read();

				switch (character)
				{
					case '\\':
						escapeCharacterFound = !escapeCharacterFound;
						apostropheFound = false;
						constantBuilder.Append(character);
						break;

					case '\'':
						if (escapeCharacterFound)
						{
							constantBuilder.Append(character);
							apostropheFound = false;
						}
						else
						{
							if (firstCharacter)
							{
								firstCharacter = false;
							}
							else
							{
								apostropheFound = true;
							}
						}

						escapeCharacterFound = false;
						break;

					default:
						escapeCharacterFound = false;
						apostropheFound = false;
						constantBuilder.Append(character);
						break;
				}
			}

			result = new StringToken(TokenKind.ConstantString, constantBuilder.ToString());

			return result;
		}

		public IEnumerator<TokenBase> Scan(TextReader reader)
		{
			while (reader.Peek() != -1)
			{
				char nextChar = (char)reader.Peek();

				if (char.IsWhiteSpace(nextChar))
				{
					reader.Read();
				}
				else if (char.IsLetterOrDigit(nextChar))
				{
					yield return ScanName(reader);
				}
				else
				{
					switch (nextChar)
					{
						case '/':
							TokenBase comment = ScanComment(reader);

							if (comment != null)
							{
								yield return comment;
							}
							break;

						case '\'':
							yield return ScanConstantString(reader);
							break;

						case ':':
							reader.Read();
							yield return new TokenBase(TokenKind.Colon);
							break;

						case '|':
							reader.Read();
							yield return new TokenBase(TokenKind.Pipe);
							break;

						case ';':
							reader.Read();
							yield return new TokenBase(TokenKind.SemiColon);
							break;

						default:
							throw new ApplicationException("Illegal character.");
					}
				}
			}

			yield return new TokenBase(TokenKind.EOF);
		}
	}
}