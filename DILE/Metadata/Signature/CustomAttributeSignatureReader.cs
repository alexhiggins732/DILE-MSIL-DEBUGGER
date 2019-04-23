using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public class CustomAttributeSignatureReader
	{
		private List<BaseSignatureItem> readElements;
		public List<BaseSignatureItem> ReadElements
		{
			get
			{
				if (readElements == null)
				{
					readElements = new List<BaseSignatureItem>();
				}

				return readElements;
			}
		}

		protected CustomAttributeSignatureReader()
		{
		}

		public CustomAttributeSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureBlobLength, bool fullSignature)
		{
			if (fullSignature)
			{
				ReadFullSignature(allTokens, signatureBlob, signatureBlobLength);
			}
			else
			{
				ReadNamedArguments(allTokens, ref signatureBlob, signatureBlobLength);
			}
		}

		protected void ReadFullSignature(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureBlobLength)
		{
			ushort prolog = (ushort)Marshal.ReadInt16(signatureBlob);

			if (prolog != 0x0001)
			{
				throw new InvalidOperationException("The custom attribute signature does not start with a valid 'prolog' value.");
			}

			throw new NotImplementedException();
		}

		protected List<CustomAttributeNamedArgument> ReadNamedArguments(Dictionary<uint, TokenBase> allTokens, ref IntPtr signatureBlob, uint signatureBlobLength)
		{
			List<CustomAttributeNamedArgument> result = null;

			//Seems like despite the Microsoft documentation does not mention it, there is a compressed unsigned integer in front of the "named arguments count" that describes the length (in bytes) of the named arguments.
			uint namedArgumentsLength;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out namedArgumentsLength));

			uint namedArgumentsCount;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out namedArgumentsCount));

			result = new List<CustomAttributeNamedArgument>(Convert.ToInt32(namedArgumentsCount));
			for (ushort index = 0; index < namedArgumentsCount; index++)
			{
				CustomAttributeNamedArgument namedArgument = ReadNamedArgument(allTokens, ref signatureBlob, signatureBlobLength);

				result.Add(namedArgument);
			}

			return result;
		}

		protected CustomAttributeNamedArgument ReadNamedArgument(Dictionary<uint, TokenBase> allTokens, ref IntPtr signatureBlob, uint signatureBlobLength)
		{
			CustomAttributeNamedArgument result = new CustomAttributeNamedArgument();

			byte fieldOrProperty = Marshal.ReadByte(signatureBlob);
			HelperFunctions.StepIntPtr(ref signatureBlob, 1);

			switch(fieldOrProperty)
			{
				case 0x53:
					result.ArgumentType = NamedArgumentType.Field;
					break;

				case 0x54:
					result.ArgumentType = NamedArgumentType.Property;
					break;

				default:
					throw new NotSupportedException("Unknown named argument type encountered: " + fieldOrProperty);
			}

			CorElementType arrayElementType = CorElementType.ELEMENT_TYPE_END;
			CorElementType fieldOrPropType = (CorElementType)Marshal.ReadByte(signatureBlob);
			HelperFunctions.StepIntPtr(ref signatureBlob, 1);

			string argumentName = string.Empty;
			string enumTypeName = string.Empty;

			switch(fieldOrPropType)
			{
				case (CorElementType)0x55:
					enumTypeName = ReadSerializedString(ref signatureBlob);
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
					arrayElementType = (CorElementType)Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);
					break;
			}

			argumentName = ReadSerializedString(ref signatureBlob);

			result.ArgumentValue = ReadFixedArgument(ref signatureBlob, fieldOrPropType, arrayElementType, false);

			result.ArgumentValue.EnumTypeName = enumTypeName;
			result.ArgumentValue.Name = argumentName;

			if ((int)arrayElementType == 0x51)
			{
				result.ArgumentValue.ArrayElementType = CorElementType.ELEMENT_TYPE_OBJECT;
			}
			else
			{
				result.ArgumentValue.ArrayElementType = arrayElementType;
			}

			return result;
		}

		protected ICustomAttributeElement ReadFixedArgument(ref IntPtr signatureBlob, CorElementType elementType, CorElementType arrayElementType, bool isRecursiveCall)
		{
			ICustomAttributeElement result = null;

			switch (elementType)
			{
				//0x55 = enum
				case (CorElementType)0x55:
					result = ReadFixedArgument(ref signatureBlob, CorElementType.ELEMENT_TYPE_I4, CorElementType.ELEMENT_TYPE_END, true);
					break;

				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					byte boolValue = Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);

					if (boolValue != 0 && boolValue != 1)
					{
						throw new InvalidOperationException("Bool value in the custom attribute blob contains invalid value: " + boolValue);
					}

					result = new CustomAttributeElement<bool>(elementType, (boolValue == 0 ? false : true));
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					char charValue = (char)Marshal.ReadInt16(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 2);

					result = new CustomAttributeElement<char>(elementType, charValue);
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					float floatValue = HelperFunctions.ConvertToSingle(Convert.ToUInt64(Marshal.ReadInt32(signatureBlob)));
					HelperFunctions.StepIntPtr(ref signatureBlob, 4);

					result = new CustomAttributeElement<float>(elementType, floatValue);
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					double doubleValue = HelperFunctions.ConvertToDouble((ulong)Marshal.ReadInt64(signatureBlob));
					HelperFunctions.StepIntPtr(ref signatureBlob, 8);

					result = new CustomAttributeElement<double>(elementType, doubleValue);
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					sbyte sbyteValue = (sbyte)Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);

					result = new CustomAttributeElement<sbyte>(elementType, sbyteValue);
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					short shortValue = Marshal.ReadInt16(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 2);

					result = new CustomAttributeElement<short>(elementType, shortValue);
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					int intValue = Marshal.ReadInt32(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 4);

					result = new CustomAttributeElement<int>(elementType, intValue);
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					long longValue = Marshal.ReadInt64(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 8);

					result = new CustomAttributeElement<long>(elementType, longValue);
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					byte byteValue = (byte)Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);

					result = new CustomAttributeElement<byte>(elementType, byteValue);
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					ushort ushortValue = (ushort)Marshal.ReadInt16(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 2);

					result = new CustomAttributeElement<ushort>(elementType, ushortValue);
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					uint uintValue = (uint)Marshal.ReadInt32(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 4);

					result = new CustomAttributeElement<uint>(elementType, uintValue);
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					ulong ulongValue = (ulong)Marshal.ReadInt64(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 8);

					result = new CustomAttributeElement<ulong>(elementType, ulongValue);
					break;

				//TODO Find out what is the element type of System.Type...
				case CorElementType.ELEMENT_TYPE_STRING:
					string stringValue = ReadSerializedString(ref signatureBlob);

					result = new CustomAttributeElement<string>(elementType, stringValue);
					break;

				//0x51 = object/boxed value
				case (CorElementType)0x51:
					CorElementType fieldOrPropType = (CorElementType)Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);

					result = ReadFixedArgument(ref signatureBlob, fieldOrPropType, CorElementType.ELEMENT_TYPE_END, true);
					result.IsBoxed = true;
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
					if (isRecursiveCall)
					{
						arrayElementType = (CorElementType)Marshal.ReadByte(signatureBlob);
						HelperFunctions.StepIntPtr(ref signatureBlob, 1);
					}

					uint arrayLength = (uint)Marshal.ReadInt32(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 4);

					List<ICustomAttributeElement> arrayElements = null;

					if (arrayLength != 0xFFFFFFFF)
					{
						arrayElements = new List<ICustomAttributeElement>();

						for (uint index = 0; index < arrayLength; index++)
						{
							arrayElements.Add(ReadFixedArgument(ref signatureBlob, arrayElementType, CorElementType.ELEMENT_TYPE_END, true));
						}
					}

					result = new CustomAttributeElement<List<ICustomAttributeElement>>(arrayElementType, arrayElements);

					if (isRecursiveCall)
					{
						if ((int)arrayElementType == 0x51)
						{
							result.ArrayElementType = CorElementType.ELEMENT_TYPE_OBJECT;
						}
						else
						{
							result.ArrayElementType = arrayElementType;
						}
					}
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown custom attribute element ('{0}').", elementType));
			}

			if (!result.IsBoxed)
			{
				result.ElementType = elementType;
			}

			return result;
		}

		protected string ReadSerializedString(ref IntPtr signatureBlob)
		{
			uint stringLength = Marshal.ReadByte(signatureBlob);
			string stringValue = null;

			if (stringLength == 0xFF)
			{
				//stringValue = null;
				HelperFunctions.StepIntPtr(ref signatureBlob, 1);
			}
			else if (stringLength == 0x00)
			{
				stringValue = string.Empty;
				HelperFunctions.StepIntPtr(ref signatureBlob, 1);
			}
			else
			{
				HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out stringLength));

				byte[] text = HelperFunctions.ReadBlobAsByteArray(signatureBlob, stringLength);
				HelperFunctions.StepIntPtr(ref signatureBlob, stringLength);

				UTF8Encoding encoding = new UTF8Encoding();
				stringValue = encoding.GetString(text, 0, text.Length);
			}

			return stringValue;
		}
	}
}
