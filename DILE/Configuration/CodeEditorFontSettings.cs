using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.Configuration
{
	public class CodeEditorFontSettings : IChangebleFont
	{
		private SerializableFont defaultFont;
		[XmlIgnore()]
		public SerializableFont DefaultFont
		{
			get
			{
				return defaultFont;
			}
			set
			{
				defaultFont = value;
			}
		}

		private string title;
		[XmlIgnore()]
		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
			}
		}

		#region IChangebleFont Members

		[XmlIgnore()]
		public DockContent DockContent
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotSupportedException();
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

		#endregion

		public override string ToString()
		{
			return Title;
		}
	}
}