using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI.Debug;
using System.Drawing;

namespace Dile.Controls
{
	public class BreakpointLine : BaseLineDescriptor
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
				Color result = Color.Red;

				switch(BreakpointState)
				{
					case BreakpointState.Inactive:
						result = Color.Orange;
						break;

					case BreakpointState.Removed:
						result = Color.White;
						break;
				}

				return result;
			}
		}

		private BreakpointState breakpointState;
		public BreakpointState BreakpointState
		{
			get
			{
				return breakpointState;
			}
			set
			{
				breakpointState = value;
			}
		}

		public BreakpointLine(BreakpointState breakpointState, int instructionOffset)
			: base(instructionOffset, false)
		{
			BreakpointState = breakpointState;
		}
	}
}