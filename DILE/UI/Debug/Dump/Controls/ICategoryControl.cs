using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug.Dump;

namespace Dile.UI.Debug.Dump.Controls
{
	public interface ICategoryControl
	{
		void Initialize(DumpDebugger dumpDebugger);
	}
}