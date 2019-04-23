using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
	[ComImport, GuidAttribute("31BCFCE2-DAFB-11D2-9F81-00C04F79A0A3"), CoClass(typeof(CorMetaDataDispenserExClass))]
	public interface MetaDataDispenserEx : IMetaDataDispenserEx
	{
	}
}