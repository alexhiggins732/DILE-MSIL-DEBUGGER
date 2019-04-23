using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
	public struct COR_SECATTR
	{
		public uint tkCtor;
		public IntPtr pCustomAttribute;
		public uint cbCustomAttribute;
	}
}