using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class CustomModifier : BaseTokenSignatureItem
	{
		private CorElementType modifier;
		public CorElementType Modifier
		{
			get
			{
				return modifier;
			}
			set
			{
				modifier = value;
			}
		}

		private uint typeToken;
		public uint TypeToken
		{
			get
			{
				return typeToken;
			}
			set
			{
				typeToken = value;
			}
		}

		public CustomModifier()
		{
		}

		public CustomModifier(CorElementType modifier, uint typeToken)
		{
			Modifier = modifier;
			TypeToken = typeToken;
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			ToString(result);

			return result.ToString();
		}

		public void ToString(StringBuilder stringBuilder)
		{
			if (Modifier == CorElementType.ELEMENT_TYPE_CMOD_OPT)
			{
				stringBuilder.Insert(0, string.Format("modopt({0})", TokenObject));
			}
			else
			{
				stringBuilder.Insert(0, string.Format("modreq({0})", TokenObject));
			}

			if (NextItem != null)
			{
				Type nextItemType = NextItem.GetType();

				if (nextItemType == typeof(TypeSignatureItem))
				{
					stringBuilder.Insert(0, " ");
					stringBuilder.Insert(0, NextItem);
				}
				else if (nextItemType == typeof(CustomModifier))
				{
					stringBuilder.Insert(0, " ");
					((CustomModifier)NextItem).ToString(stringBuilder);
				}
			}
		}
	}
}