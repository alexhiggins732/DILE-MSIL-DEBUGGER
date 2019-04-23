using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class PropertySignatureReader : MethodSignatureReader
	{
		public PropertySignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void  ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint paramCount = 0;

			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<BaseSignatureItem>();
			}

			int paramIndex = 0;
			while (paramIndex < paramCount && signatureBlob.ToInt32() < SignatureEnd)
			{
				Parameters.Add(ReadSignatureItem(ref signatureBlob));
				paramIndex++;
			}
		}
	}
}