using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;

namespace Dile.UI.Debug
{
	public class MissingModule
	{
		private ModuleWrapper Module
		{
			get;
			set;
		}

		public string MissingModuleName
		{
			get
			{
				return Module.GetName();
			}
		}

		public bool IsInMemoryModule
		{
			get
			{
				return Module.IsInMemory();
			}
		}

		public MissingModule(ModuleWrapper module)
		{
			Module = module;
		}

		public void AddModuleToProject(bool treatAsInMemory)
		{
			if (!Project.Instance.IsAssemblyLoaded(MissingModuleName))
			{
				if (IsInMemoryModule || treatAsInMemory)
				{
					UIHandler.Instance.AddAssembly(Module);
				}
				else
				{
					UIHandler.Instance.AddAssembly(MissingModuleName);
				}
			}
		}
	}
}