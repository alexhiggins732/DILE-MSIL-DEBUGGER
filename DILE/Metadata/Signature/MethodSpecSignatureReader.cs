using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class MethodSpecSignatureReader : BaseSignatureReader
	{
		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			private set
			{
				callingConvention = value;
			}
		}

		private List<BaseSignatureItem> arguments;
		public List<BaseSignatureItem> Arguments
		{
			get
			{
				return arguments;
			}
			private set
			{
				arguments = value;
			}
		}

		public MethodSpecSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength): base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint argumentCount = 0;

			dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out argumentCount));
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

			if (argumentCount > 0)
			{
				Arguments = new List<BaseSignatureItem>();
			}

			int argumentIndex = 0;
			while (argumentIndex < argumentCount && signatureBlob.ToInt64() < SignatureEnd)
			{
				Arguments.Add(ReadSignatureItem(ref signatureBlob));
				argumentIndex++;
			}
		}

		public override void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (Arguments != null && HasGenericMethodParameter)
			{
				foreach (BaseSignatureItem signatureItem in Arguments)
				{
					HelperFunctions.SetSignatureItemToken(AllTokens, signatureItem, typeGenericParameters, methodGenericParameters);
				}
			}
		}
	}
}