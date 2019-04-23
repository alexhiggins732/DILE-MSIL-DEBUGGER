using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public abstract class BaseSignatureReader
	{
		private uint signatureLength;
		protected uint SignatureLength
		{
			get
			{
				return signatureLength;
			}
			private set
			{
				signatureLength = value;
			}
		}

		private Dictionary<uint, TokenBase> allTokens;
		public Dictionary<uint, TokenBase> AllTokens
		{
			get
			{
				return allTokens;
			}
			private set
			{
				allTokens = value;
			}
		}

		private uint signatureEnd;
		protected uint SignatureEnd
		{
			get
			{
				return signatureEnd;
			}
			private set
			{
				signatureEnd = value;
			}
		}

		protected IntPtr signatureBlob;
		protected IntPtr SignatureBlob
		{
			get
			{
				return signatureBlob;
			}
			private set
			{
				signatureBlob = value;
			}
		}

		private bool hasGenericMethodParameter;
		public bool HasGenericMethodParameter
		{
			get
			{
				return hasGenericMethodParameter;
			}
			protected set
			{
				hasGenericMethodParameter = value;
			}
		}

		public BaseSignatureReader(Dictionary<uint, TokenBase> allTokens, IntPtr signatureBlob, uint signatureLength)
		{
			AllTokens = allTokens;
			SignatureBlob = signatureBlob;
			SignatureLength = signatureLength;
            var ptr64 = signatureBlob.ToInt64();

            SignatureEnd = unchecked((uint)ptr64) + signatureLength;
		}

		public abstract void ReadSignature();

		public virtual void SetGenericParametersOfMethod(List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
		}

		protected BaseSignatureItem ReadSignatureItem(ref IntPtr signatureBlob)
		{
			BaseSignatureItem result = null;
			uint data;
			uint dataLength = SignatureCompression.CorSigUncompressData(signatureBlob, out data);
			CorElementType elementType = (CorElementType)data;

			switch (elementType)
			{
				case CorElementType.ELEMENT_TYPE_CMOD_OPT:
				case CorElementType.ELEMENT_TYPE_CMOD_REQD:
					CustomModifier customModifier = ReadCustomModifier(ref signatureBlob);
					result = customModifier;
					customModifier.NextItem = ReadSignatureItem(ref signatureBlob);
					break;

				case CorElementType.ELEMENT_TYPE_END:
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					result = new EndSignatureItem();
					break;

				case CorElementType.ELEMENT_TYPE_BYREF:
					HelperFunctions.StepIntPtr(ref signatureBlob);
					result = ReadSignatureItem(ref signatureBlob);
					
					if (result is ArraySignatureItem)
					{
						ArraySignatureItem arrayItem = (ArraySignatureItem)result;
						ILocalVarSignatureItem localVarItem = (ILocalVarSignatureItem)arrayItem.Type;
						localVarItem.ByRef = true;
					}
					else if (result is VarSignatureItem)
					{
						VarSignatureItem varItem = ((VarSignatureItem)result);
						varItem.ByRef = true;
					}
					else
					{
						while (result is CustomModifier)
						{
							result = ((CustomModifier)result).NextItem;
						}

						ILocalVarSignatureItem localVarItem = (ILocalVarSignatureItem)result;
						localVarItem.ByRef = true;
					}
					break;

				case CorElementType.ELEMENT_TYPE_TYPEDBYREF:
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					ElementSignatureItem elementItem = new ElementSignatureItem();
					elementItem.ElementType = elementType;
					result = elementItem;
					break;

				case CorElementType.ELEMENT_TYPE_PINNED:
					HelperFunctions.StepIntPtr(ref signatureBlob);
					result = ReadSignatureItem(ref signatureBlob);
					((TypeSignatureItem)result).Pinned = true;
					break;

				case CorElementType.ELEMENT_TYPE_ARRAY:
					{
						HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
						uint value;
						ArraySignatureItem arrayItem = new ArraySignatureItem();
						arrayItem.Type = ReadSignatureItem(ref signatureBlob);
						HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out value));
						arrayItem.Rank = value;

						HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out value));
						if (value > 0)
						{
							arrayItem.Sizes = new List<uint>(Convert.ToInt32(value));

							for (int index = 0; index < value; index++)
							{
								uint size;

								HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out size));
								arrayItem.Sizes.Add(size);
							}
						}

						HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out value));
						if (value > 0)
						{
							arrayItem.LoBounds = new List<uint>(Convert.ToInt32(value));

							for (int index = 0; index < value; index++)
							{
								uint loBound;

								HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out loBound));
								arrayItem.LoBounds.Add(loBound);
							}
						}

						result = arrayItem;
					}
					break;

				case CorElementType.ELEMENT_TYPE_GENERICINST:
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					TypeSignatureItem genericType = (TypeSignatureItem)ReadType(ref signatureBlob);
					result = genericType;
					uint genericParametersCount = 0;
					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(SignatureBlob, out genericParametersCount));

					if (genericParametersCount > 0)
					{
						genericType.GenericParameters = new List<BaseSignatureItem>();

						for (uint genericParametersIndex = 0; genericParametersIndex < genericParametersCount; genericParametersIndex++)
						{
							genericType.GenericParameters.Add(ReadSignatureItem(ref signatureBlob));
						}
					}
					break;

				case CorElementType.ELEMENT_TYPE_FNPTR:
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					MethodRefSignatureReader signatureReader = new MethodRefSignatureReader(AllTokens, signatureBlob, SignatureLength);
					signatureReader.ReadSignature();
					signatureBlob = signatureReader.SignatureBlob;
					result = new FunctionPointerSignatureItem(signatureReader);
					break;

				case CorElementType.ELEMENT_TYPE_MVAR:
				case CorElementType.ELEMENT_TYPE_VAR:
					HelperFunctions.StepIntPtr(ref signatureBlob, dataLength);
					byte number = Marshal.ReadByte(signatureBlob);
					HelperFunctions.StepIntPtr(ref signatureBlob, 1);
					VarSignatureItem varSignatureItem = new VarSignatureItem(number, elementType);
					result = varSignatureItem;

					if (!HasGenericMethodParameter && varSignatureItem.MethodParameter)
					{
						HasGenericMethodParameter = true;
					}
					break;

				case CorElementType.ELEMENT_TYPE_MAX:
				case CorElementType.ELEMENT_TYPE_R4_HFA:
				case CorElementType.ELEMENT_TYPE_R8_HFA:
					throw new NotImplementedException(string.Format("Unknown signature element ('{0}').", elementType));

				default:
					result = ReadType(ref signatureBlob);
					break;
			}

			return result;
		}

		protected CustomModifier ReadCustomModifier(ref IntPtr signatureBlob)
		{
			CustomModifier result = new CustomModifier();

			uint data;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			result.Modifier = (CorElementType)data;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out data));
			result.TypeToken = SignatureCompression.CorSigUncompressToken(data);

			return result;
		}

		protected TypeSignatureItem ReadType(ref IntPtr signatureBlob)
		{
			TypeSignatureItem result = new TypeSignatureItem();

			uint data;
			HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out data));

			result.ElementType = (CorElementType)data;
			switch (result.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_VALUETYPE:
				case CorElementType.ELEMENT_TYPE_CLASS:
					HelperFunctions.StepIntPtr(ref signatureBlob, SignatureCompression.CorSigUncompressData(signatureBlob, out data));
					result.Token = SignatureCompression.CorSigUncompressToken(data);
					break;

				case CorElementType.ELEMENT_TYPE_SZARRAY:
				case CorElementType.ELEMENT_TYPE_PTR:
					CorElementType elementType = CorElementType.ELEMENT_TYPE_BYREF;
					bool addElementType = false;
					List<CustomModifier> customModifiers = new List<CustomModifier>();

					do
					{
						if (addElementType)
						{
							CustomModifier customModifier = ReadCustomModifier(ref signatureBlob);
							customModifiers.Add(customModifier);
						}

						addElementType = true;
						SignatureCompression.CorSigUncompressData(signatureBlob, out data);
						elementType = (CorElementType)data;
					} while (elementType == CorElementType.ELEMENT_TYPE_CMOD_OPT || elementType == CorElementType.ELEMENT_TYPE_CMOD_REQD);

					BaseSignatureItem nextItem = ReadSignatureItem(ref signatureBlob);
					result.NextItem = nextItem;

					if (nextItem.GetType() == typeof(TypeSignatureItem) && customModifiers.Count > 0)
					{
						((TypeSignatureItem)nextItem).CustomModifiers = customModifiers;
					}
					break;
			}

			return result;
		}
	}
}