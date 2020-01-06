using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public abstract class BaseValueRefresher
	{
		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			protected set
			{
				name = value;
			}
		}

		private bool isObjectValue = false;
		public bool IsObjectValue
		{
			get
			{
				return isObjectValue;
			}
			set
			{
				isObjectValue = value;
			}
		}

		private MissingModule missingModule;
		public MissingModule MissingModule
		{
			get
			{
				return missingModule;
			}
			set
			{
				missingModule = value;
			}
		}

		public BaseValueRefresher(string name)
		{
			Name = name;
		}

		public abstract ValueWrapper GetRefreshedValue();

		public void AddPointerToName()
		{
			Name = "*" + Name;
		}
	}
}