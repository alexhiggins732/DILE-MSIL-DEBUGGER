using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class CustomAttributeElement<T> : BaseSignatureItem, ICustomAttributeElement
	{
		public bool IsArray
		{
			get
			{
				return (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY);
			}
		}

		public bool IsEnum
		{
			get
			{
				return ((int)ElementType == 0x55);
			}
		}

		private bool isBoxed;
		public bool IsBoxed
		{
			get
			{
				return isBoxed;
			}
			set
			{
				isBoxed = value;
			}
		}

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		private string enumTypeName;
		public string EnumTypeName
		{
			get
			{
				return enumTypeName;
			}
			set
			{
				enumTypeName = value;
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

		private CorElementType arrayElementType;
		public CorElementType ArrayElementType
		{
			get
			{
				return arrayElementType;
			}
			set
			{
				arrayElementType = value;
			}
		}

		private T elementValue;
		public T ElementValue
		{
			get
			{
				return elementValue;
			}
			set
			{
				elementValue = value;
			}
		}

		public CustomAttributeElement(CorElementType elementType, T elementValue)
		{
			ElementType = elementType;
			ElementValue = elementValue;
		}

		public void AppendName(StringBuilder stringBuilder)
		{
			stringBuilder.Append(Name);
		}

		public void AppendEnumTypeName(StringBuilder stringBuilder)
		{
			stringBuilder.Append(EnumTypeName);
		}

		public void AppendElementType(StringBuilder stringBuilder, bool includeArrayLength)
		{
			if (IsEnum)
			{
				stringBuilder.Append("enum");
			}
			else
			{
				if (ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)
				{
					stringBuilder.Append(HelperFunctions.GetElementTypeShortName(ArrayElementType));
					stringBuilder.Append("[");

					if (includeArrayLength)
					{
						List<ICustomAttributeElement> arrayElements = (List<ICustomAttributeElement>)(object)ElementValue;
						stringBuilder.Append(arrayElements.Count);
					}

					stringBuilder.Append("]");
				}
				else
				{
					stringBuilder.Append(HelperFunctions.GetElementTypeShortName(ElementType));
				}
			}
		}

		public void AppendElementValue(StringBuilder stringBuilder)
		{
			switch(ElementType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					stringBuilder.Append(ElementValue.ToString().ToLowerInvariant());
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					stringBuilder.Append(HelperFunctions.FormatAsHexNumber(ElementValue, 4));
					break;

				case CorElementType.ELEMENT_TYPE_STRING:
					if (ElementValue == null)
					{
						stringBuilder.Append("nullref");
					}
					else
					{
						stringBuilder.Append("'");
						stringBuilder.Append(ElementValue);
						stringBuilder.Append("'");
					}
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
					stringBuilder.Append("(");
					List<ICustomAttributeElement> arrayElements = (List<ICustomAttributeElement>)(object)ElementValue;

					for (int index = 0; index < arrayElements.Count; index++)
					{
						ICustomAttributeElement arrayElement = arrayElements[index];

						if (arrayElement.IsArray)
						{
							arrayElement.AppendElementType(stringBuilder, true);
						}
						else if (arrayElement.IsBoxed)
						{
							arrayElement.AppendElementType(stringBuilder, false);
							stringBuilder.Append("(");
						}

						arrayElement.AppendElementValue(stringBuilder);

						if (arrayElement.IsBoxed && !arrayElement.IsArray)
						{
							stringBuilder.Append(")");
						}

						if (index < arrayElements.Count - 1)
						{
							stringBuilder.Append(", ");
						}
					}

					stringBuilder.Append(")");
					break;

				default:
					stringBuilder.Append(ElementValue);
					break;
			}
		}
	}
}