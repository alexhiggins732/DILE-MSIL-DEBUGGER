using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	[Serializable()]
	public class SerializableFont
	{
		private string familyName;
		[XmlAttribute()]
		public string FamilyName
		{
			get
			{
				return familyName;
			}
			set
			{
				familyName = value;
			}
		}

		private float size;
		[XmlAttribute()]
		public float Size
		{
			get
			{
				return size;
			}
			set
			{
				size = value;
			}
		}

		private FontStyle fontStyle;
		[XmlAttribute()]
		public FontStyle FontStyle
		{
			get
			{
				return fontStyle;
			}
			set
			{
				fontStyle = value;
			}
		}

		private GraphicsUnit graphicsUnit;
		[XmlAttribute()]
		public GraphicsUnit GraphicsUnit
		{
			get
			{
				return graphicsUnit;
			}
			set
			{
				graphicsUnit = value;
			}
		}

		private byte gdiCharset;
		[XmlAttribute()]
		public byte GdiCharset
		{
			get
			{
				return gdiCharset;
			}
			set
			{
				gdiCharset = value;
			}
		}

		private bool gdiVerticalFont;
		[XmlAttribute()]
		public bool GdiVerticalFont
		{
			get
			{
				return gdiVerticalFont;
			}
			set
			{
				gdiVerticalFont = value;
			}
		}

		public SerializableFont()
		{
		}

		public SerializableFont(Font font) : this()
		{
			UpdateFromFont(font);
		}

		public Font GetFont()
		{
			return new Font(FamilyName, Size, FontStyle, GraphicsUnit, GdiCharset, GdiVerticalFont);
		}

		public void UpdateFromFont(Font font)
		{
			FamilyName = font.FontFamily.Name;
			Size = font.Size;
			FontStyle = font.Style;
			GraphicsUnit = font.Unit;
			GdiCharset = font.GdiCharSet;
			GdiVerticalFont = font.GdiVerticalFont;
		}
	}
}