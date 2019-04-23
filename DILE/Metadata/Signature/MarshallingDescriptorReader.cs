using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public class MarshallingDescriptorReader: BaseSignatureReader
	{
		private MashallingDescriptorItem marshallingDescriptor;
		public MashallingDescriptorItem MarshallingDescriptor
		{
			get
			{
				return marshallingDescriptor;
			}
			private set
			{
				marshallingDescriptor = value;
			}
		}

		private int parameterCount;
		public int ParameterCount
		{
			get
			{
				return parameterCount;
			}
			set
			{
				parameterCount = value;
			}
		}

		public MarshallingDescriptorReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength, int parameterCount) : base(allTokens, signatureBlob, signatureLength)
		{
			ParameterCount = parameterCount;
		}

		public override void ReadSignature()
		{
			MarshallingDescriptor = ReadNativeType();
		}

		private string ReadString()
		{
			string result = null;

			uint stringLength;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out stringLength));

			if (stringLength > 0)
			{
				byte[] text = new byte[stringLength];

				for (int index = 0; index < stringLength; index++)
				{
					text[index] = Marshal.ReadByte(signatureBlob, index);
				}

				HelperFunctions.StepIntPtr(ref signatureBlob, stringLength);
				UTF8Encoding encoding = new UTF8Encoding();
				result = encoding.GetString(text, 0, text.Length);
			}

			return result;
		}

		private MashallingDescriptorItem ReadVariantType()
		{
			MashallingDescriptorItem result = new MashallingDescriptorItem();

			uint data;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));

			result.VariantType = (VariantType)data;
			result.IsNativeType = false;

			return result;
		}

		private MashallingDescriptorItem ReadNativeType()
		{
			MashallingDescriptorItem result = new MashallingDescriptorItem();

			uint data;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));

			result.NativeType = (CorNativeType)data;
			result.IsNativeType = true;

			switch(result.NativeType)
			{
				case CorNativeType.NATIVE_TYPE_SAFEARRAY:
					result.NextItem = ReadVariantType();
					break;

				case CorNativeType.NATIVE_TYPE_CUSTOMMARSHALER:
					result.Guid = ReadString();
					result.UnmanagedType = ReadString();
					result.ManagedType = ReadString();
					result.Cookie = ReadString();
					break;

				case CorNativeType.NATIVE_TYPE_ARRAY:
					result.NextItem = ReadNativeType();

					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.ParamNumber = (int)data;

					if (result.ParamNumber > ParameterCount)
					{
						result.ParamNumber = -1;
					}

					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.ElemMultiply = (int)data;

					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDSYSSTRING:
					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;
					break;

				case CorNativeType.NATIVE_TYPE_FIXEDARRAY:
					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out data));
					result.NumberElem = (int)data;

					result.NextItem = ReadNativeType();
					break;
			}

			return result;
		}
	}
}