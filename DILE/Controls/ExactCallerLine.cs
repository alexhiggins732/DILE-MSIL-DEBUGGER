using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public class ExactCallerLine : BaseLineDescriptor
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
				return Color.LightGreen;
			}
		}

		public ExactCallerLine(int instructionOffset)
			: base(instructionOffset, true)
		{
		}
	}
}