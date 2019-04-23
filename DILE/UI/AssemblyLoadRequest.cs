using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug;

namespace Dile.UI
{
	public class AssemblyLoadRequest
	{
		public string FilePath
		{
			get;
			private set;
		}

		public ModuleWrapper Module
		{
			get;
			private set;
		}

		public AssemblyLoadRequest(string filePath)
		{
			FilePath = filePath;
		}

		public AssemblyLoadRequest(ModuleWrapper module)
		{
			Module = module;
		}
	}
}