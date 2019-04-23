using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public abstract class BaseLineDescriptor
	{
		public abstract Color ForeColor
		{
			get;
		}

		public abstract Color BackColor
		{
			get;
		}

		private int instructionOffset;
		public int InstructionOffset
		{
			get
			{
				return instructionOffset;
			}
			protected set
			{
				instructionOffset = value;
			}
		}

		private bool scrollToOffset;
		public bool ScrollToOffset
		{
			get
			{
				return scrollToOffset;
			}
			protected set
			{
				scrollToOffset = value;
			}
		}

		public BaseLineDescriptor(int instructionOffset, bool scrollToOffset)
		{
			InstructionOffset = instructionOffset;
			ScrollToOffset = scrollToOffset;
		}
	}
}