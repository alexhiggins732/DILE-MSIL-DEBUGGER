using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Metadata.Signature
{
	public class MethodRefSignatureReader : MethodSignatureReader
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

		public MethodRefSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
			: base(allTokens, signatureBlob, signatureLength)
		{
		}

		public override void ReadSignature()
		{
			uint data;
			uint dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);

			CallingConvention = (CorCallingConvention)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
			uint paramCount = 0;

			if (CallingConvention != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount);
				HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);

				if ((CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_GENERIC)
				{
					dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out paramCount);
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}
			}

			ReturnType = ReadSignatureItem(ref signatureBlob);

			if (paramCount > 0)
			{
				Parameters = new List<BaseSignatureItem>();
			}

			int paramIndex = 0;
			while (paramIndex < paramCount)
			{
				dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);
				CorElementType elementType = (CorElementType)data;

				if (elementType == CorElementType.ELEMENT_TYPE_SENTINEL)
				{
					SentinelFound = true;
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
				}

				if (SentinelFound)
				{
					if (VarargParameters == null)
					{
						VarargParameters = new List<BaseSignatureItem>();
					}

					BaseSignatureItem signatureItem = ReadSignatureItem(ref signatureBlob);
					VarargParameters.Add(signatureItem);
				}
				else
				{
					BaseSignatureItem signatureItem = ReadSignatureItem(ref signatureBlob);
					Parameters.Add(signatureItem);
				}

				paramIndex++;
			}
		}

		public string GetDefinition(string className, string methodName, bool functionPointer)
		{
			string methodDescription = string.Empty;
			string returnTypeText = string.Empty;
			string callingConvention = HelperFunctions.GetCallingConventionName(CallingConvention);

			HelperFunctions.SetSignatureItemToken(AllTokens, ReturnType);
			returnTypeText = ReturnType.ToString();

			StringBuilder parameterList = new StringBuilder();

			if (CallingConvention != CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				if (functionPointer)
				{
					parameterList.Append("*");
				}

				parameterList.Append("(");

				if (Parameters != null)
				{
					for (int parametersIndex = 0; parametersIndex < Parameters.Count; parametersIndex++)
					{
						BaseSignatureItem parameter = Parameters[parametersIndex];
						HelperFunctions.SetSignatureItemToken(AllTokens, parameter);

						if (parametersIndex == Parameters.Count - 1)
						{
							parameterList.Append(parameter);
						}
						else
						{
							parameterList.AppendFormat("{0}, ", parameter);
						}
					}
				}

				if (SentinelFound)
				{
					if (Parameters != null)
					{
						parameterList.Append(", ");
					}

					parameterList.Append("...");

					if (VarargParameters != null)
					{
						parameterList.Append(", ");

						for (int index = 0; index < VarargParameters.Count; index++)
						{
							BaseSignatureItem varargParameter = VarargParameters[index];
							HelperFunctions.SetSignatureItemToken(AllTokens, varargParameter);

							if (index == VarargParameters.Count - 1)
							{
								parameterList.Append(varargParameter);
							}
							else
							{
								parameterList.AppendFormat("{0}, ", varargParameter);
							}
						}
					}
				}
				parameterList.Append(")");
			}

			StringBuilder textBuilder = new StringBuilder();

			if (callingConvention.Length > 0)
			{
				textBuilder.AppendFormat("{0} ", callingConvention);
			}

			textBuilder.AppendFormat("{0} ", returnTypeText);

			textBuilder.AppendFormat("{0}{1}{2}{3}", className, (className == null ? string.Empty : "::"), methodName, parameterList.ToString());

			return textBuilder.ToString();
		}
	}
}
