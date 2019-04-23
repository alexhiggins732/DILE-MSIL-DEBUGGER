using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	[Serializable()]
	public class BaseMenuInformation : IMenuInformation
	{
		private MenuFunction menuFunction;
		[XmlAttribute()]
		public MenuFunction MenuFunction
		{
			get
			{
				return menuFunction;
			}
			set
			{
				menuFunction = value;
			}
		}

		public BaseMenuInformation()
		{
		}

		public BaseMenuInformation(MenuFunction menuFunction)
			: this()
		{
			MenuFunction = menuFunction;
		}
	}
}