using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Controls
{
	[StructLayout(LayoutKind.Sequential)]
	public class NativePoint
	{
		public int X;
		public int Y;
	}
}