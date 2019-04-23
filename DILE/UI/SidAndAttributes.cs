using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.UI
{
	[StructLayout(LayoutKind.Sequential)]
	public struct SidAndAttributes
	{
		public IntPtr Sid;
		public int Attributes;
	} 
}