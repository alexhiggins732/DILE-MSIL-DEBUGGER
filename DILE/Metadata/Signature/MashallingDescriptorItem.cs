using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Metadata.Signature
{
	public class MashallingDescriptorItem : BaseTokenSignatureItem
	{
		private CorNativeType nativeType;
		public CorNativeType NativeType
		{
			get
			{
				return nativeType;
			}
			set
			{
				nativeType = value;
			}
		}

		private VariantType variantType;
		public VariantType VariantType
		{
			get
			{
				return variantType;
			}
			set
			{
				variantType = value;
			}
		}

		private bool isNativeType;
		public bool IsNativeType
		{
			get
			{
				return isNativeType;
			}
			set
			{
				isNativeType = value;
			}
		}

		private string guid;
		public string Guid
		{
			get
			{
				return guid;
			}
			set
			{
				guid = value;
			}
		}

		private string unmanagedType;
		public string UnmanagedType
		{
			get
			{
				return unmanagedType;
			}
			set
			{
				unmanagedType = value;
			}
		}

		private string managedType;
		public string ManagedType
		{
			get
			{
				return managedType;
			}
			set
			{
				managedType = value;
			}
		}

		private string cookie;
		public string Cookie
		{
			get
			{
				return cookie;
			}
			set
			{
				cookie = value;
			}
		}

		private int paramNumber;
		public int ParamNumber
		{
			get
			{
				return paramNumber;
			}
			set
			{
				paramNumber = value;
			}
		}

		private int elemMultiply;
		public int ElemMultiply
		{
			get
			{
				return elemMultiply;
			}
			set
			{
				elemMultiply = value;
			}
		}

		private int numberElem;
		public int NumberElem
		{
			get
			{
				return numberElem;
			}
			set
			{
				numberElem = value;
			}
		}

		public override string ToString()
		{
			string result;

			if (IsNativeType)
			{
				result = NativeTypeToString();
			}
			else
			{
				result = VariantTypeToString();
			}

			return result;
		}

		private string VariantTypeToString()
		{
			string result = string.Empty;

			switch (VariantType)
			{
				case VariantType.VT_INT:
					result = "int";
					break;

				case VariantType.VT_UINT:
					result = "unsigned int";
					break;

				case VariantType.VT_CY:
					result = "currency";
					break;

				case VariantType.VT_DATE:
					result = "date";
					break;

				case VariantType.VT_BSTR:
					result = "bstr";
					break;

				case VariantType.VT_DISPATCH:
					result = "idispatch";
					break;

				case VariantType.VT_ERROR:
					result = "error";
					break;

				case VariantType.VT_BOOL:
					result = "bool";
					break;

				case VariantType.VT_VARIANT:
					result = "variant";
					break;

				case VariantType.VT_UNKNOWN:
					result = "iunknown";
					break;

				case VariantType.VT_DECIMAL:
					result = "decimal";
					break;

				case VariantType.VT_UI1:
					result = "unsigned int8";
					break;

				case VariantType.VT_UI2:
					result = "unsigned int16";
					break;

				case VariantType.VT_UI4:
					result = "unsigned int32";
					break;

				case VariantType.VT_UI8:
					result = "unsigned int64";
					break;

				case VariantType.VT_I1:
					result = "int8";
					break;

				case VariantType.VT_I2:
					result = "int16";
					break;

				case VariantType.VT_I4:
					result = "int32";
					break;

				case VariantType.VT_I8:
					result = "int64";
					break;

				case VariantType.VT_R4:
					result = "float32";
					break;

				case VariantType.VT_R8:
					result = "float64";
					break;
			}

			return result;
		}

		private bool CreateParameterText(StringBuilder stringBuilder, string parameter, bool parameterFound)
		{
			bool result = (parameter != null && parameter.Length > 0);

			if (parameterFound || result)
			{
				if (parameterFound)
				{
					stringBuilder.Append(", \"");
				}
				else
				{
					stringBuilder.Append("\"");
				}

				stringBuilder.Append(parameter);
				stringBuilder.Append("\"");
			}

			return result;
		}

		private string NativeTypeToString()
		{
			string result = string.Empty;
			bool addNextItem = true;

			switch (NativeType)
			{
				case CorNativeType.NATIVE_TYPE_INT:
					result = "int";
					break;

				case CorNativeType.NATIVE_TYPE_UINT:
					result = "unsigned int";
					break;

				case CorNativeType.NATIVE_TYPE_U1:
					result = "unsigned int8";
					break;

				case CorNativeType.NATIVE_TYPE_U2:
					result = "unsigned int16";
					break;

				case CorNativeType.NATIVE_TYPE_U4:
					result = "unsigned int32";
					break;

				case CorNativeType.NATIVE_TYPE_U8:
					result = "unsigned int64";
					break;

				case CorNativeType.NATIVE_TYPE_I1:
					result = "int8";
					break;

				case CorNativeType.NATIVE_TYPE_I2:
					result = "int16";
					break;

				case CorNativeType.NATIVE_TYPE_I4:
					result = "int32";
					break;

				case CorNativeType.NATIVE_TYPE_I8:
					result = "int64";
					break;

				case CorNativeType.NATIVE_TYPE_R4:
					result = "float32";
					break;

				case CorNativeType.NATIVE_TYPE_R8:
					result = "float64";
					break;

				case CorNativeType.NATIVE_TYPE_BSTR:
					result = "bstr";
					break;

				case CorNativeType.NATIVE_TYPE_ANSIBSTR:
					result = "ansi bstr";
					break;

				case CorNativeType.NATIVE_TYPE_IDISPATCH:
					result = "idispatch";
					break;

				case CorNativeType.NATIVE_TYPE_IUNKNOWN:
					result = "iunknown";
					break;

				case CorNativeType.NATIVE_TYPE_INTF:
					result = "interface";
					break;

				case CorNativeType.NATIVE_TYPE_SAFEARRAY:
					result = "safearray";
					break;

				case CorNativeType.NATIVE_TYPE_STRUCT:
					result = "struct";
					break;

				case CorNativeType.NATIVE_TYPE_CUSTOMMARSHALER:
					StringBuilder resultBuilder = new StringBuilder();
					bool parameterFound = false;

					parameterFound = CreateParameterText(resultBuilder, Guid, parameterFound);
					parameterFound = CreateParameterText(resultBuilder, UnmanagedType, parameterFound);
					parameterFound = CreateParameterText(resultBuilder, ManagedType, parameterFound);
					parameterFound = CreateParameterText(resultBuilder, Cookie, parameterFound);

					resultBuilder.Insert(0, "custom(");
					resultBuilder.Append(")");
					result = resultBuilder.ToString();
					break;

				case CorNativeType.NATIVE_TYPE_LPSTR:
					result = "lpstr";
					break;

				case CorNativeType.NATIVE_TYPE_LPTSTR:
					result = "lptstr";
					break;

				case CorNativeType.NATIVE_TYPE_LPWSTR:
					result = "lpwstr";
					break;

				case CorNativeType.NATIVE_TYPE_LPSTRUCT:
					result = "lpstruct";
					break;

				case CorNativeType.NATIVE_TYPE_ASANY:
					result = "as any";
					break;

				case CorNativeType.NATIVE_TYPE_ARRAY:
					addNextItem = false;
					resultBuilder = new StringBuilder();

					resultBuilder.Append(NextItem);
					resultBuilder.Append("[");

					if (ParamNumber >= 0)
					{
						resultBuilder.Append(" + ");
						resultBuilder.Append(ParamNumber);
					}
					resultBuilder.Append("]");

					result = resultBuilder.ToString();
					break;

				case CorNativeType.NATIVE_TYPE_MAX:
					break;

				case CorNativeType.NATIVE_TYPE_BOOLEAN:
					result = "bool";
					break;

				case CorNativeType.NATIVE_TYPE_VARIANTBOOL:
					result = "variant bool";
					break;

				case CorNativeType.NATIVE_TYPE_FUNC:
					result = "method";
					break;

				case CorNativeType.NATIVE_TYPE_BYVALSTR:
					result = "byvalstr";
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDSYSSTRING:
					result = string.Format("fixed sysstring [{0}]", NumberElem);
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDARRAY:
					result = string.Format("fixed array [{0}]", NumberElem);
					addNextItem = false;
					break;

				case CorNativeType.NATIVE_TYPE_ERROR:
					result = "error";
					break;

				case CorNativeType.NATIVE_TYPE_CURRENCY:
					result = "currency";
					break;

				case CorNativeType.NATIVE_TYPE_DATE:
					result = "date";
					break;

				case CorNativeType.NATIVE_TYPE_DECIMAL:
					result = "decimal";
					break;

				case CorNativeType.NATIVE_TYPE_OBJECTREF:
					result = "objectref";
					break;

				case CorNativeType.NATIVE_TYPE_SYSCHAR:
					result = "syschar";
					break;

				case CorNativeType.NATIVE_TYPE_TBSTR:
					result = "tbstr";
					break;

				case CorNativeType.NATIVE_TYPE_VARIANT:
					result = "variant";
					break;

				case CorNativeType.NATIVE_TYPE_VOID:
					result = "void";
					break;

				case (CorNativeType)0x2e:
					result = "{ 2E }";
					break;

				case CorNativeType.NATIVE_TYPE_HSTRING:
					result = "{ 2F }";
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown native type ('{0}').", NativeType));
			}

			if (addNextItem && NextItem != null)
			{
				result += " " + NextItem.ToString();
			}

			return result;
		}
	}
}