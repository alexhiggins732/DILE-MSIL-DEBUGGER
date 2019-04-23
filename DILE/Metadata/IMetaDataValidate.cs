using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata
{
  [ComImport, GuidAttribute("4709C9C6-81FF-11D3-9FC7-00C04F79A0A3"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
  public interface IMetaDataValidate
  {
		int ValidatorInit(uint dwModuleType, [MarshalAs(UnmanagedType.Interface)]object pUnk);

		int ValidateMetaData();
  }
}