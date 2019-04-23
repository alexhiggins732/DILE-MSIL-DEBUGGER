using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
  [ComImport, GuidAttribute("D0E80DD3-12D4-11d3-B39D-00C04FF81795"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IHostFilter
  {
		int MarkToken(uint tk);
  }
}