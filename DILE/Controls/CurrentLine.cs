using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public class CurrentLine : BaseLineDescriptor
	{
		public override Color ForeColor
		{
			get
			{
				return Color.Black;
			}
		}

		public override Color BackColor
		{
			get
			{
				return Color.Yellow;
			}
		}

		public CurrentLine(int instructionOffset)
			: base(instructionOffset, true)
		{
		}
	}
}