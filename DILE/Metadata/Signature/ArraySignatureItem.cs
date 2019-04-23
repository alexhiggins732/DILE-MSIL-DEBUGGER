using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class ArraySignatureItem : BaseSignatureItem
	{
		private BaseSignatureItem type;
		public BaseSignatureItem Type
		{
			get
			{
				return type;
			}
			set
			{
				type = value;
			}
		}

		private uint rank = 0;
		public uint Rank
		{
			get
			{
				return rank;
			}
			set
			{
				rank = value;
			}
		}

		private List<uint> sizes;
		public List<uint> Sizes
		{
			get
			{
				return sizes;
			}
			set
			{
				sizes = value;
			}
		}

		private List<uint> loBounds;
		public List<uint> LoBounds
		{
			get
			{
				return loBounds;
			}
			set
			{
				loBounds = value;
			}
		}

		public ArraySignatureItem()
		{
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(Type.ToString());
			result.Append("[");

			if (Sizes != null)
			{
				throw new NotImplementedException("Sizes are given.");
			}

			for (int index = 0; index < Rank; index++)
			{
				if (LoBounds != null)
				{
					result.Append(LoBounds[index]);
					result.Append("...");
				}

				if (index < Rank - 1)
				{
					result.Append(", ");
				}
			}

			result.Append("]");

			return result.ToString();
		}
	}
}