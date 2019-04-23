using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class TypeSpecSignatureReader : BaseSignatureReader
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

		public TypeSpecSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			uint dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);
			CorElementType elementType = (CorElementType)data;

			Type = ReadSignatureItem(ref signatureBlob);
		}

		public override void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (HasGenericMethodParameter)
			{
				HelperFunctions.SetSignatureItemToken(AllTokens, Type, typeGenericParameters, methodGenericParameters);

				if (GenericParameters != null)
				{
					foreach (BaseSignatureItem signatureItem in GenericParameters)
					{
						HelperFunctions.SetSignatureItemToken(AllTokens, Type, typeGenericParameters, methodGenericParameters);
					}
				}
			}
		}
	}
}