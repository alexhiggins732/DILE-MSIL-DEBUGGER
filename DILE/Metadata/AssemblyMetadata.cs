using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
	[StructLayout(LayoutKind.Sequential)]
	public struct AssemblyMetadata
	{
		public ushort usMajorVersion;
		public ushort usMinorVersion;
		public ushort usBuildNumber;
		public ushort usRevisionNumber;
		public IntPtr szLocale;
		public uint cbLocale;
		public IntPtr rProcessor;
		public uint ulProcessor;
		public IntPtr rOS;
		public uint ulOS;
	}
}