using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class ElementSignatureItem : BaseSignatureItem
	{
		private CorElementType elementType;
		public CorElementType ElementType
		{
			get
			{
				return elementType;
			}
			set
			{
				elementType = value;
			}
		}

		public override string ToString()
		{
			string result = string.Empty;

			switch (ElementType)
			{
				case CorElementType.ELEMENT_TYPE_TYPEDBYREF:
					result = "typedref";
					break;
			}

			return result;
		}
	}
}