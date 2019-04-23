using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Disassemble.ILCodes;
using Dile.UI;
using System.Xml.Serialization;

namespace Dile.Disassemble
{
	public abstract class TokenBase
	{
		private uint token;
		[XmlIgnore()]
		public uint Token
		{
			get
			{
				return token;
			}
			set
			{
				token = value;
			}
		}

		private bool isMetadataRead = false;
		[XmlIgnore()]
		public bool IsMetadataRead
		{
			get
			{
				return isMetadataRead;
			}
			set
			{
				isMetadataRead = value;
			}
		}

		private string originalName;
		private string escapedName;
		[XmlIgnore()]
		public virtual string Name
		{
			get
			{
				if (originalName != null
					&& escapedName == null)
				{
					escapedName = HelperFunctions.EscapeName(GetType().Name, Token, originalName);
				}

				return escapedName;
			}
			set
			{
				originalName = HelperFunctions.QuoteName(value);
				escapedName = null;
			}
		}

		public virtual SearchOptions ItemType
		{
			get
			{
				return SearchOptions.None;
			}
		}

		protected TokenBase()
		{
		}

		public void ReadMetadata()
		{
			if (!IsMetadataRead)
			{
				//HACK Warning: the boolean is set to true before the reading would really occur.
				IsMetadataRead = true;
				ReadMetadataInformation();
			}
		}

		protected virtual void ReadMetadataInformation()
		{
		}
	}
}