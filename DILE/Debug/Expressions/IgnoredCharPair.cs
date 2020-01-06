using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Debug.Expressions
{
	public class IgnoredCharPair
	{
		private char startCharacter;
		public char StartCharacter
		{
			get
			{
				return startCharacter;
			}
			private set
			{
				startCharacter = value;
			}
		}

		private char endCharacter;
		public char EndCharacter
		{
			get
			{
				return endCharacter;
			}
			private set
			{
				endCharacter = value;
			}
		}

		public IgnoredCharPair(char startCharacter, char endCharacter)
		{
			StartCharacter = startCharacter;
			EndCharacter = endCharacter;
		}

		public bool IsStartCharacter(char character)
		{
			return (character == StartCharacter);
		}

		public bool IsEndCharacter(char character)
		{
			return (character == EndCharacter);
		}
	}
}