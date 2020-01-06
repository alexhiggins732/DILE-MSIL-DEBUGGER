using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug.Dump;
using System.Windows.Forms;

namespace Dile.UI.Debug.Dump.Controls
{
	public interface ICategoryDisplayer
	{
		string Name
		{
			get;
		}

		Control CreateControl(DumpDebugger dumpDebugger);
	}
}