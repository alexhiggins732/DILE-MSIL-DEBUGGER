using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI.Debug;
using System.Runtime.Serialization;

namespace Dile.Debug
{
	public class MissingModuleException : ApplicationException
	{
		private MissingModule missingModule;
		public MissingModule MissingModule
		{
			get
			{
				return missingModule;
			}
			private set
			{
				missingModule = value;
			}
		}

		public MissingModuleException()
			: base()
		{
		}

		public MissingModuleException(string message, ModuleWrapper module)
			: base(message)
		{
			if (module != null)
			{
				MissingModule = new MissingModule(module);
			}
		}

		protected MissingModuleException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public MissingModuleException(string message, ModuleWrapper module, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}