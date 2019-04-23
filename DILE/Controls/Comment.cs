using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Controls
{
	public class Comment
	{
		private int startPosition;
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

		private int endPosition;
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

		private bool oneLineComment = false;
		public bool OneLineComment
		{
			get
			{
				return oneLineComment;
			}

			set
			{
				oneLineComment = value;
			}
		}


		public int Length
		{
			get
			{
				return EndPosition - StartPosition;
			}
		}

		public Comment()
		{
		}

		public Comment(int startPosition, int endPosition, bool oneLineComment)
		{
			StartPosition = startPosition;
			EndPosition = endPosition;
			OneLineComment = oneLineComment;
		}
	}
}