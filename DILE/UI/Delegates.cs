using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.UI
{
	public delegate void FoundItem(object sender, FoundItemEventArgs args);
	public delegate void NoArgumentsDelegate();
	public delegate void StringArrayDelegate(string[] array);
	public delegate void AssembliesLoadedDelegate(List<Assembly> assemblies, bool isProjectChanged);
}