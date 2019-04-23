using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.UI;
using System.Drawing;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.Configuration
{
	[Serializable()]
	public class PanelDisplayer : BaseMenuInformation, IChangebleFont
	{
		[XmlIgnore()]
		public SerializableFont DefaultFont
		{
			get
			{
				return Settings.DefaultFont;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		private bool panelVisible;
		[XmlAttribute()]
		public bool PanelVisible
		{
			get
			{
				return panelVisible;
			}
			set
			{
				panelVisible = value;
			}
		}

		[XmlIgnore()]
		public Font Font
		{
			get
			{
				return (SerializableFont == null ? null : SerializableFont.GetFont());
			}
			set
			{
				SerializableFont = new SerializableFont(value);
			}
		}

		private SerializableFont serializableFont;
		public SerializableFont SerializableFont
		{
			get
			{
				return serializableFont;
			}
			set
			{
				serializableFont = value;
			}
		}

		private SerializableFont tempFont;
		[XmlIgnore()]
		public SerializableFont TempFont
		{
			get
			{
				return tempFont;
			}
			set
			{
				tempFont = value;
			}
		}

		[XmlIgnore()]
		public DockContent DockContent
		{
			get
			{
				return Panel;
			}
			set
			{
				Panel = value as BasePanel;
			}
		}

		private BasePanel panel;
		[XmlIgnore()]
		public BasePanel Panel
		{
			get
			{
				return panel;
			}
			set
			{
				panel = value;
			}
		}

		public override string ToString()
		{
			return panel.TabText;
		}
	}
}