using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.UI
{
	public class FoundItemEventArgs : EventArgs
	{
		private bool cancel = false;
		public bool Cancel
		{
			get
			{
				return cancel;
			}
			set
			{
				cancel = value;
			}
		}

		private TokenBase foundTokenObject;
		public TokenBase FoundTokenObject
		{
			get
			{
				return foundTokenObject;
			}
			private set
			{
				foundTokenObject = value;
			}
		}

		private Assembly assembly;
		public Assembly Assembly
		{
			get
			{
				return assembly;
			}
			private set
			{
				assembly = value;
			}
		}

		public FoundItemEventArgs(Assembly assembly, TokenBase foundTokenObject)
		{
			Assembly = assembly;
			FoundTokenObject = foundTokenObject;
		}
	}
}