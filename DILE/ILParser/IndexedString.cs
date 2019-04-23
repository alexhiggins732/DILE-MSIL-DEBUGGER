using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public class IndexedString
	{
		private int position;
		public int Position
		{
			get
			{
				return position;
			}
			set
			{
				position = value;
			}
		}

		private string text;
		public string Text
		{
			get
			{
				return text;
			}
			private set
			{
				text = value;
			}
		}

		public IndexedString(string text)
		{
			Position = 0;
			Text = text;
		}

		public bool ReadExpression(string expectedExpression)
		{
			bool result = false;

			if (Text.StartsWith(expectedExpression, StringComparison.InvariantCulture))
			{
				Position += expectedExpression.Length;
				result = true;
			}

			return result;
		}

		public string ReadID()
		{
			string result = string.Empty;
			Position++;
			int startPosition = Position;

			//TODO add more rules about IDs
			if (!Char.IsLetter(Text[Position]))
			{
				throw new InvalidOperationException("The first character of an ID must be a letter.");
			}

			while (Position < Text.Length && Text[Position] != ' ')
			{
				Position++;
			}

			result = Text.Substring(startPosition, Position - startPosition);

			return result;
		}
	}
}