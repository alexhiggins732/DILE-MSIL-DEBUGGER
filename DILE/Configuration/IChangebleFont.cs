using System;

using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.Configuration
{
	interface IChangebleFont
	{
		SerializableFont DefaultFont
		{
			get;
			set;
		}

		DockContent DockContent
		{
			get;
			set;
		}

		Font Font
		{
			get;
			set;
		}

		SerializableFont SerializableFont
		{
			get;
			set;
		}

		SerializableFont TempFont
		{
			get;
			set;
		}
	}
}
