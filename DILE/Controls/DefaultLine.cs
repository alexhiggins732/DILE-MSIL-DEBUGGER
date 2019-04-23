using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;

namespace Dile.Controls
{
	public class DefaultLine : BaseLineDescriptor
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
				return Color.White;
			}
		}

		public DefaultLine()
			: base(0, false)
		{
		}
	}
}