using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;

namespace Dile.Controls
{
	public class Parser
	{
		private UnicodeCategory[] tokens;
		public UnicodeCategory[] Tokens
		{
			get
			{
				return tokens;
			}

			set
			{
				tokens = value;
			}
		}

		private List<Word> words;
		public List<Word> Words
		{
			get
			{
				return words;
			}

			set
			{
				words = value;
			}
		}

		private List<Comment> comments;
		public List<Comment> Comments
		{
			get
			{
				return comments;
			}

			set
			{
				comments = value;
			}
		}

		public Parser(string text)
		{
			char[] textChars = text.ToCharArray();
			Tokens = new UnicodeCategory[textChars.Length];
			Words = new List<Word>();
			Comments = new List<Comment>();

			Word word = new Word();
			Comment comment = new Comment();
			UnicodeCategory previousCharCategory = UnicodeCategory.Surrogate;
			char previousChar = char.MinValue;
			int commentDepth = 0;
			bool isFirstWordInLine = true;
			bool insideSquareBracket = false;
			bool insideAngleBracket = false;
			bool insideRoundBracket = false;
			bool insideString = false;
			bool colon = false;
			bool oneLineComment = false;
			bool newLine = false;
			int lineNumber = 0;

			for (int textCharsIndex = 0; textCharsIndex < textChars.Length; textCharsIndex++)
			{
				char textChar = textChars[textCharsIndex];
				UnicodeCategory charCategory = char.GetUnicodeCategory(textChar);
				Tokens[textCharsIndex] = charCategory;

				if (textCharsIndex == textChars.Length - 1 && commentDepth > 0)
				{
					comment.EndPosition = textCharsIndex + 1;
				}

				switch (charCategory)
				{
					case UnicodeCategory.OtherPunctuation: // / * : , . ! "
						if (textChars.Length > textCharsIndex + 1 && !oneLineComment)
						{
							char nextChar = textChars[textCharsIndex + 1];
							bool commentFound = false;

							if (commentDepth == 0)
							{
								if (!insideString && textChar == '"')
								{
									insideString = true;
								}
								else if (insideString && textChar == '"' && previousChar != '\\')
								{
									insideString = false;
								}
							}

							if (!insideString)
							{
								if ((textChar == '/' && nextChar == '*'))
								{
									commentFound = true;
									commentDepth++;
								}
								else if (textChar == '/' && nextChar == '/')
								{
									commentFound = true;
									oneLineComment = true;
									commentDepth++;
								}
								else if (textChar == '*' && nextChar == '/' && commentDepth > 0)
								{
									while (commentDepth > 0)
									{
										comments[comments.Count - commentDepth].EndPosition = textCharsIndex + 2;
										commentDepth--;
									}
									commentDepth = 0;
									colon = false;
								}
							}

							if (commentFound)
							{
								colon = false;
								comment = new Comment();
								comment.StartPosition = textCharsIndex;
								comment.OneLineComment = oneLineComment;
								Comments.Add(comment);
							}
						}

						if (textChar == '.' && (previousCharCategory == UnicodeCategory.UppercaseLetter || previousCharCategory == UnicodeCategory.LowercaseLetter || previousCharCategory == UnicodeCategory.ConnectorPunctuation || previousCharCategory == UnicodeCategory.DecimalDigitNumber))
						{
							word.WordBuilder.Append(textChar);
							colon = false;
						}

						if (textChar == ':' && previousChar == ':')
						{
							colon = true;
						}
						break;

					case UnicodeCategory.Control:
						if (textChar == '\n')
						{
							newLine = true;
							isFirstWordInLine = true;
							if (oneLineComment)
							{
								comment.EndPosition = textCharsIndex;
								commentDepth--;
							}
							oneLineComment = false;
							lineNumber++;
						}
						colon = false;
						break;

					case UnicodeCategory.SpaceSeparator:
						colon = false;
						break;

					case UnicodeCategory.OpenPunctuation: // [ ( {
						if (textChar == '[')
						{
							insideSquareBracket = true;
						}
						else if (textChar == '(')
						{
							insideRoundBracket = true;
						}
						colon = false;
						break;

					case UnicodeCategory.ClosePunctuation: // ] ) }
						if (textChar == ']')
						{
							insideSquareBracket = false;
						}
						else if (textChar == ')')
						{
							insideRoundBracket = false;
						}
						colon = false;
						break;

					case UnicodeCategory.MathSymbol: // < >
						if (textChar == '<')
						{
							insideAngleBracket = true;
						}
						else if (textChar == '>')
						{
							insideAngleBracket = false;
						}

						colon = false;
						break;

					case UnicodeCategory.ConnectorPunctuation: //_
					case UnicodeCategory.DecimalDigitNumber:
					case UnicodeCategory.UppercaseLetter:
					case UnicodeCategory.LowercaseLetter:
						if ((previousCharCategory != UnicodeCategory.UppercaseLetter && previousCharCategory != UnicodeCategory.LowercaseLetter && previousCharCategory != UnicodeCategory.ConnectorPunctuation && previousCharCategory != UnicodeCategory.DecimalDigitNumber && previousChar != '.') || newLine)
						{
							newLine = false;
							word = new Word();
							Words.Add(word);
							word.StartPosition = textCharsIndex;
							word.InsideComment = (commentDepth > 0);
							word.IsFirstWordInLine = (!word.InsideComment && isFirstWordInLine);
							word.InsideSquareBracket = insideSquareBracket;
							word.InsideRoundBracket = insideRoundBracket;
							word.InsideAngleBracket = insideAngleBracket;
							word.IsFunctionName = colon;
							word.LineNumber = lineNumber;

							if (word.IsFirstWordInLine)
							{
								isFirstWordInLine = false;
							}
						}

						word.WordBuilder.Append(textChar);
						word.EndPosition = textCharsIndex + 1;
						colon = false;
						break;
				}

				previousChar = textChar;
				previousCharCategory = charCategory;
			}
		}

		public Word FindWordByExactPosition(int position)
		{
			Word result = null;
			int wordsIndex = 0;

			while (result == null && wordsIndex < Words.Count)
			{
				Word word = Words[wordsIndex++];

				if (position <= word.EndPosition)
				{
					result = word;
				}
			}

			return result;
		}

		public Word FindWordByPosition(int position)
		{
			Word result = null;
			int wordsIndex = 0;
			bool found = false;

			while (!found && wordsIndex < Words.Count)
			{
				result = Words[wordsIndex++];

				found = (position <= result.EndPosition);
			}

			return result;
		}
	}
}