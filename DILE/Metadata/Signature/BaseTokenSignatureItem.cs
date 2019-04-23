using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public abstract class BaseTokenSignatureItem : BaseSignatureItem
	{
		private BaseSignatureItem nextItem = null;
		public BaseSignatureItem NextItem
		{
			get
			{
				return nextItem;
			}
			set
			{
				nextItem = value;
			}
		}

		private TokenBase tokenObject;
		public TokenBase TokenObject
		{
			get
			{
				return tokenObject;
			}
			set
			{
				tokenObject = value;
			}
		}

		private uint token = 0;
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

		public BaseTokenSignatureItem()
		{
		}

		public string GetTokenObjectName(bool fullName)
		{
			return HelperFunctions.GetTokenObjectName(TokenObject, fullName);
		}
	}
}