using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Controls
{
	public class Word
	{
		private int startPosition = 0;
		public int StartPosition
		{
			get
			{
				return startPosition;
			}

			set
			{
				startPosition = value;
			}
		}

		private int endPosition = 0;
		public int EndPosition
		{
			get
			{
				return endPosition;
			}

			set
			{
				endPosition = value;
			}
		}

		public int Length
		{
			get
			{
				return EndPosition - StartPosition;
			}
		}

		private StringBuilder wordBuilder = new StringBuilder();
		public StringBuilder WordBuilder
		{
			get
			{
				return wordBuilder;
			}

			set
			{
				wordBuilder = value;
			}
		}

		private bool insideComment = false;
		public bool InsideComment
		{
			get
			{
				return insideComment;
			}

			set
			{
				insideComment = value;
			}
		}

		private bool isFirstWordInLine = false;
		public bool IsFirstWordInLine
		{
			get
			{
				return isFirstWordInLine;
			}

			set
			{
				isFirstWordInLine = value;
			}
		}

		private bool insideSquareBracket = false;
		public bool InsideSquareBracket
		{
			get
			{
				return insideSquareBracket;
			}

			set
			{
				insideSquareBracket = value;
			}
		}

		private bool insideAngleBracket = false;
		public bool InsideAngleBracket
		{
			get
			{
				return insideAngleBracket;
			}

			set
			{
				insideAngleBracket = value;
			}
		}

		private bool insideRoundBracket = false;
		public bool InsideRoundBracket
		{
			get
			{
				return insideRoundBracket;
			}

			set
			{
				insideRoundBracket = value;
			}
		}

		private bool isFunctionName = false;
		public bool IsFunctionName
		{
			get
			{
				return isFunctionName;
			}

			set
			{
				isFunctionName = value;
			}
		}

		private int lineNumber = 0;
		public int LineNumber
		{
			get
			{
				return lineNumber;
			}

			set
			{
				lineNumber = value;
			}
		}

		public Word()
		{
		}
	}
}