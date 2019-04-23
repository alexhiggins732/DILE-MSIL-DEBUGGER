using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class CustomAttribute : TokenBase
	{
		private TokenBase owner;
		public TokenBase Owner
		{
			get
			{
				return owner;
			}
			private set
			{
				owner = value;
			}
		}

		private uint type;
		public uint Type
		{
			get
			{
				return type;
			}
			private set
			{
				type = value;
			}
		}

		private IntPtr blob;
		public IntPtr Blob
		{
			get
			{
				return blob;
			}
			private set
			{
				blob = value;
			}
		}

		private uint blobLength;
		public uint BlobLength
		{
			get
			{
				return blobLength;
			}
			private set
			{
				blobLength = value;
			}
		}

		[ThreadStatic()]
		private static StringBuilder nameBuilder;
		private static StringBuilder NameBuilder
		{
			get
			{
				if (nameBuilder == null)
				{
					nameBuilder = new StringBuilder();
				}

				return nameBuilder;
			}
		}

		public CustomAttribute(IMetaDataImport2 import, uint token, TokenBase owner, uint type, IntPtr blob, uint blobLength)
		{
			Token = token;
			Owner = owner;
			Type = type;
			Blob = blob;
			BlobLength = blobLength;
		}

		public void SetText(Dictionary<uint, TokenBase> allTokens)
		{
			NameBuilder.Length = 0;
			NameBuilder.Append(".custom ");
			NameBuilder.Append(allTokens[type]);

			NameBuilder.Append(" = (");
			uint lastByte = BlobLength - 1;
			for (int blobIndex = 0; blobIndex < BlobLength; blobIndex++)
			{
				NameBuilder.Append(HelperFunctions.FormatAsHexNumber(Marshal.ReadByte(Blob), 2));

				if (blobIndex < lastByte)
				{
					NameBuilder.Append(" ");
					HelperFunctions.StepIntPtr(ref blob);
				}
			}
			NameBuilder.Append(")");

			Name = NameBuilder.ToString();
		}
	}
}