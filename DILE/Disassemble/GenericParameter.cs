using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class GenericParameter : TextTokenBase, IComparable
	{
		private uint sequence;
		public uint Sequence
		{
			get
			{
				return sequence;
			}
			private set
			{
				sequence = value;
			}
		}

		private CorGenericParamAttr attributes;
		public CorGenericParamAttr Attributes
		{
			get
			{
				return attributes;
			}
			private set
			{
				attributes = value;
			}
		}

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

		private uint kind;
		public uint Kind
		{
			get
			{
				return kind;
			}
			private set
			{
				kind = value;
			}
		}

		private string text;
		public string Text
		{
			get
			{
				return text;
			}
			private set
			{
				text = value;
			}
		}

		private List<uint> constraints;
		public List<uint> Constraints
		{
			get
			{
				return constraints;
			}
			private set
			{
				constraints = value;
			}
		}

		[ThreadStatic()]
		private static StringBuilder textBuilder;
		private static StringBuilder TextBuilder
		{
			get
			{
				if (textBuilder == null)
				{
					textBuilder = new StringBuilder();
				}

				return textBuilder;
			}
		}

		public GenericParameter(IMetaDataImport2 import, uint token, string name, uint sequence, uint attributes, TokenBase owner, uint kind)
		{
			Token = token;
			Name = name;
			Sequence = sequence;
			Attributes = (CorGenericParamAttr)attributes;
			Owner = owner;
			Kind = kind;

			EnumConstraints(import);
		}

		private void EnumConstraints(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] tokens = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumGenericParamConstraints(ref enumHandle, Token, tokens, Convert.ToUInt32(tokens.Length), out count);

			if (count > 0)
			{
				Constraints = new List<uint>();
			}

			while (count > 0)
			{
				for (uint tokensIndex = 0; tokensIndex < count; tokensIndex++)
				{
					uint token = tokens[tokensIndex];
					uint tokenOfParent;
					uint constraintType;

					import.GetGenericParamConstraintProps(token, out tokenOfParent, out constraintType);

					Constraints.Add(constraintType);
				}

				import.EnumGenericParamConstraints(ref enumHandle, Token, tokens, Convert.ToUInt32(tokens.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);

			if (Constraints != null)
			{
				TextBuilder.Length = 0;
				TextBuilder.Append("(");

				for (int index = 0; index < Constraints.Count; index++)
				{
					TokenBase constraint = allTokens[Constraints[index]];
					TextBuilder.Append(constraint.Name);

					if (index < Constraints.Count - 1)
					{
						TextBuilder.Append(", ");
					}
				}

				TextBuilder.Append(") ");
				TextBuilder.Append(Name);
				Text = TextBuilder.ToString();
			}
			else
			{
				Text = Name;
			}
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			if (obj == null || obj.GetType() != typeof(GenericParameter))
			{
				throw new ArgumentException();
			}

			GenericParameter otherParameter = (GenericParameter)obj;

			return Sequence.CompareTo(otherParameter.Sequence);
		}

		#endregion
	}
}