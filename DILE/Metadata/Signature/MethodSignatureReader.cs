using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class MethodSignatureReader : BaseSignatureReader
	{
		private List<BaseSignatureItem> parameters;
		public List<BaseSignatureItem> Parameters
		{
			get
			{
				return parameters;
			}
			protected set
			{
				parameters = value;
			}
		}

		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			protected set
			{
				callingConvention = value;
			}
		}

		private BaseSignatureItem returnType;
		public BaseSignatureItem ReturnType
		{
			get
			{
				return returnType;
			}
			protected set
			{
				returnType = value;
			}
		}

		public MethodSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{	
			uint data;
			int dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint paramCount = 0;

			if (CallingConvention != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
				HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

				if ((CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC)
				{
					dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}
			}

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<BaseSignatureItem>();
			}

			int paramIndex = 0;
			while (paramIndex < paramCount && signatureBlob.ToInt64() < SignatureEnd)
			{
				Parameters.Add(ReadSignatureItem(ref signatureBlob));
				paramIndex++;
			}
		}

		public override void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (HasGenericMethodParameter)
			{
				HelperFunctions.SetSignatureItemToken(AllTokens, ReturnType, typeGenericParameters, methodGenericParameters);

				if (Parameters != null)
				{
					foreach (BaseSignatureItem parameter in Parameters)
					{
						HelperFunctions.SetSignatureItemToken(AllTokens, parameter, typeGenericParameters, methodGenericParameters);
					}
				}
			}
		}
	}
}