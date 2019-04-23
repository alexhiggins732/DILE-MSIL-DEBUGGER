using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace Dile.Controls
{
	public partial class HighlightLabel : Label
	{
		public HighlightLabel()
		{
			InitializeComponent();
		}

		public HighlightLabel(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
		}

		private void HighlightLabel_Enter(object sender, EventArgs e)
		{
			BorderStyle = BorderStyle.FixedSingle;
			BackColor = SystemColors.Highlight;
			ForeColor = SystemColors.HighlightText;
		}

		private void HighlightLabel_Leave(object sender, EventArgs e)
		{
			BorderStyle = BorderStyle.None;
			BackColor = SystemColors.Control;
			ForeColor = SystemColors.ControlText;
		}
	}
}
