using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public class SecuritySignatureReader : CustomAttributeSignatureReader
	{
		private List<SecurityAttributeDescriptor> securityAttributes;
		public List<SecurityAttributeDescriptor> SecurityAttributes
		{
			get
			{
				return securityAttributes;
			}
			private set
			{
				securityAttributes = value;
			}
		}

		public SecuritySignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureBlobLength)
			: base()
		{
			ReadSecuritySignature(allTokens, ref signatureBlob, signatureBlobLength);
		}

		private void ReadSecuritySignature(Dictionary<uint, TokenBase> allTokens, ref IntPtr signatureBlob, uint signatureBlobLength)
		{
			byte dot = Marshal.ReadByte(signatureBlob);
			HelperFunctions.StepIntPtr(ref signatureBlob, 1);

			if (dot != (byte)'.')
			{
				throw new InvalidOperationException("The security signature does not start with a dot ('.') character.");
			}

			uint attributesCount;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out attributesCount));

			if (attributesCount > 0)
			{
				SecurityAttributes = new List<SecurityAttributeDescriptor>(Convert.ToInt32(attributesCount));

				for (uint index = 0; index < attributesCount; index++)
				{
					SecurityAttributeDescriptor securityAttribute = new SecurityAttributeDescriptor();

					securityAttribute.AttributeName = ReadSerializedString(ref signatureBlob);
					securityAttribute.FieldsAndProperties = ReadNamedArguments(allTokens, ref signatureBlob, signatureBlobLength);

					SecurityAttributes.Add(securityAttribute);
				}
			}
		}

		public void AppendString(StringBuilder stringBuilder)
		{
			stringBuilder.Append("\n\t\t\t = {");

			for (int index = 0; index < SecurityAttributes.Count; index++)
			{
				SecurityAttributes[index].AppendString(stringBuilder);

				if (index < SecurityAttributes.Count - 1)
				{
					stringBuilder.Append("\n\t\t\t");
				}
			}

			stringBuilder.Append("}");
		}
	}
}