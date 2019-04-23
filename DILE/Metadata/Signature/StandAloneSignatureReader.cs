using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;

namespace Dile.Metadata.Signature
{
	public class StandAloneSignatureReader : MethodSignatureReader
	{
		private List<BaseSignatureItem> varargParameters;
		public List<BaseSignatureItem> VarargParameters
		{
			get
			{
				return varargParameters;
			}
			private set
			{
				varargParameters = value;
			}
		}

		private bool sentinelFound = false;
		public bool SentinelFound
		{
			get
			{
				return sentinelFound;
			}
			private set
			{
				sentinelFound = value;
			}
		}

		private BaseSignatureItem fieldSignatureItem;
		public BaseSignatureItem FieldSignatureItem
		{
			get
			{
				return fieldSignatureItem;
			}
			private set
			{
				fieldSignatureItem = value;
			}
		}

		public StandAloneSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			int dataLength = Convert.ToInt32(SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			CallingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

			if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				ReadFieldSignature(ref signatureBlob);
			}
			else if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_LOCAL_SIG)
			{
				ReadLocalVarSignature(ref signatureBlob);
			}
			else
			{
				ReadStandAloneMethodSignature(ref signatureBlob);
			}
		}

		private void ReadLocalVarSignature(ref IntPtr signatureBlob)
		{
			uint count;

			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out count));

			if (count > 0)
			{
				Parameters = new List<BaseSignatureItem>();
			}

			int index = 0;
			while (index < count)
			{
				index++;
				uint data;
				uint dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);
				bool pinned = ((CorElementType)data == CorElementType.ELEMENT_TYPE_PINNED);
				
				if (pinned)
				{
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);
				}

				bool byRef = ((CorElementType)data == CorElementType.ELEMENT_TYPE_BYREF);

				if (byRef)
				{
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}

				BaseSignatureItem signatureItem = ReadSignatureItem(ref signatureBlob);
				ILocalVarSignatureItem localVarSignatureItem = null;

				if (signatureItem is ArraySignatureItem)
				{
					ArraySignatureItem arraySignatureItem = (ArraySignatureItem)signatureItem;
					localVarSignatureItem = (ILocalVarSignatureItem)arraySignatureItem.Type;
				}
				else if (signatureItem is TypeSignatureItem)
				{
					localVarSignatureItem = (TypeSignatureItem)signatureItem;
				}

				if (localVarSignatureItem != null)
				{
					localVarSignatureItem.ByRef = byRef;
					localVarSignatureItem.Pinned = pinned;
				}

				Parameters.Add(signatureItem);
			}
		}

		private void ReadStandAloneMethodSignature(ref IntPtr signatureBlob)
		{
			uint paramCount = 0;

			if (CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS)
			{
				uint data;
				HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out data));
				CorCallingConvention explicitThis = (CorCallingConvention)data;

				if (explicitThis == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS)
				{
					CallingConvention |= CorCallingConvention.IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS;
				}
				else
				{
					paramCount = data;
				}
			}
			else
			{
				HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount));
			}

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<BaseSignatureItem>();

				int paramIndex = 0;
				while (paramIndex < paramCount)
				{
					uint data;
					SignatureCompression.CorSigUncompressData(signatureBlob, out data);
					CorElementType elementType = (CorElementType)data;

					if (elementType == CorElementType.ELEMENT_TYPE_SENTINEL)
					{
						throw new NotImplementedException("Sentinel found.");
					}

					if (SentinelFound)
					{
						if (VarargParameters == null)
						{
							VarargParameters = new List<BaseSignatureItem>();
						}

						VarargParameters.Add(ReadSignatureItem(ref signatureBlob));
					}
					else
					{
						Parameters.Add(ReadSignatureItem(ref signatureBlob));
					}
					paramIndex++;
				}
			}
		}

		private void ReadFieldSignature(ref IntPtr signatureBlob)
		{
			FieldSignatureItem = ReadSignatureItem(ref signatureBlob);
		}
	}
}