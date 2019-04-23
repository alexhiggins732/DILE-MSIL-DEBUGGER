using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class FieldMarshalSignatureReader : BaseSignatureReader
	{
		private TypeSignatureItem marshalAsType;
		public TypeSignatureItem MarshalAsType
		{
			get
			{
				return marshalAsType;
			}
			private set
			{
				marshalAsType = value;
			}
		}

		public FieldMarshalSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			MarshalAsType = ReadType(ref signatureBlob);
		}

		public override void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (HasGenericMethodParameter)
			{
				HelperFunctions.SetSignatureItemToken(AllTokens, MarshalAsType, typeGenericParameters, methodGenericParameters);
			}
		}
	}
}