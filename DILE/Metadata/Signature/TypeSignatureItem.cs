using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class TypeSignatureItem : BaseTokenSignatureItem, IMethodParameter, ILocalVarSignatureItem
	{
		private bool byRef = false;
		public bool ByRef
		{
			get
			{
				return byRef;
			}
			set
			{
				byRef = value;
			}
		}

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

		public IMethodParameter ArrayElementType
		{
			get
			{
				return (IMethodParameter)NextItem;
			}
			set
			{
				NextItem = (BaseSignatureItem)value;
			}
		}

		private List<CustomModifier> customModifiers = null;
		public List<CustomModifier> CustomModifiers
		{
			get
			{
				return customModifiers;
			}
			set
			{
				customModifiers = value;
			}
		}

		private bool pinned = false;
		public bool Pinned
		{
			get
			{
				return pinned;
			}
			set
			{
				pinned = value;
			}
		}

		private List<BaseSignatureItem> genericParameters;
		public List<BaseSignatureItem> GenericParameters
		{
			get
			{
				return genericParameters;
			}
			set
			{
				genericParameters = value;
			}
		}

		public TypeSignatureItem()
		{
		}

		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			if (CustomModifiers != null)
			{
				result.Append(" ");

				for(int index = 0; index < CustomModifiers.Count; index++)
				{
					if (index < CustomModifiers.Count - 1)
					{
						result.Append(CustomModifiers[index]);
						result.Append(" ");
					}
					else
					{
						result.Append(CustomModifiers[index]);
					}
				}
			}

			switch (ElementType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					result.Insert(0, "bool");
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					result.Insert(0, "char");
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					result.Insert(0, "int8");
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					result.Insert(0, "int16");
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					result.Insert(0, "int32");
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					result.Insert(0, "int64");
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					result.Insert(0, "uint8");
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					result.Insert(0, "uint16");
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					result.Insert(0, "uint32");
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					result.Insert(0, "uint64");
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					result.Insert(0, "float32");
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					result.Insert(0, "float64");
					break;

				case CorElementType.ELEMENT_TYPE_I:
					result.Insert(0, "native int");
					break;

				case CorElementType.ELEMENT_TYPE_U:
					result.Insert(0, "unsigned native int");
					break;

				case CorElementType.ELEMENT_TYPE_OBJECT:
					result.Insert(0, "object");
					break;

				case CorElementType.ELEMENT_TYPE_STRING:
					result.Insert(0, "string");
					break;

				case CorElementType.ELEMENT_TYPE_VALUETYPE:
					string tokenObjectName = GetTokenObjectName(true);

					result.Insert(0, tokenObjectName);
					result.Insert(0, "valuetype ");
					break;

				case CorElementType.ELEMENT_TYPE_CLASS:
					tokenObjectName = GetTokenObjectName(true);

					result.Insert(0, tokenObjectName);
					result.Insert(0, "class ");
					break;

				case CorElementType.ELEMENT_TYPE_PTR:
					result.Insert(0, NextItem);
					result.Append("*");
					break;

				case CorElementType.ELEMENT_TYPE_VOID:
					result.Insert(0, "void");
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
					result.Append(NextItem);
					result.Append("[]");
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown element type ('{0}').", ElementType));
			}

			if (GenericParameters != null)
			{
				result.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					result.Append(GenericParameters[index].ToString());

					if (index < GenericParameters.Count - 1)
					{
						result.Append(", ");
					}
				}

				result.Append(">");
			}

			if (ByRef)
			{
				result.Append("&");
			}

			if (Pinned)
			{
				result.Append(" pinned");
			}

			return result.ToString();
		}
	}
}