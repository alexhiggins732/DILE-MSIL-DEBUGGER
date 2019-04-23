using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class FieldSignatureReader : BaseSignatureReader
	{
		private BaseSignatureItem type;
		public BaseSignatureItem Type
		{
			get
			{
				return type;
			}
			private set
			{
				type = value;
			}
		}

		private List<BaseSignatureItem> genericParameters;
		public List<BaseSignatureItem> GenericParameters
		{
			get
			{
				return genericParameters;
			}
			private set
			{
				genericParameters = value;
			}
		}

		public FieldSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CorCallingConvention callingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			Type = ReadSignatureItem(ref signatureBlob);

			if (signatureBlob.ToInt64() < SignatureEnd)
			{
				uint genericParamCount;
				HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out genericParamCount));

				if (genericParamCount > 0)
				{
					GenericParameters = new List<BaseSignatureItem>();

					for (int genericParamIndex = 0; genericParamIndex < genericParamCount; genericParamIndex++)
					{
						GenericParameters.Add(ReadSignatureItem(ref signatureBlob));
					}
				}
			}
		}

		public override void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (HasGenericMethodParameter)
			{
				HelperFunctions.SetSignatureItemToken(AllTokens, Type, typeGenericParameters, methodGenericParameters);

				if (GenericParameters != null)
				{
					foreach (BaseSignatureItem genericParameter in GenericParameters)
					{
						HelperFunctions.SetSignatureItemToken(AllTokens, genericParameter, typeGenericParameters, methodGenericParameters);
					}
				}
			}
		}
	}
}