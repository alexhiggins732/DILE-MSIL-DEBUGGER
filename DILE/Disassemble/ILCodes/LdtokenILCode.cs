using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class LdtokenILCode : BaseParameterILCode<uint, TokenBase>
	{
		public LdtokenILCode()
		{
		}

		public override void DecodeParameter()
		{
			if (DecodedParameter is FieldDefinition)
			{
				Text = string.Format("ldtoken field {0}", ((FieldDefinition)DecodedParameter).Text);
			}
			else
			{
				Text = string.Format("ldtoken {0}", DecodedParameter.Name);
			}
		}

		public void SetGenericsMethodParameters(Dictionary<uint, TokenBase> allTokens, List<GenericParameter> typeGenericParameters, List<GenericParameter> methodGenericParameters)
		{
			if (DecodedParameter is TextTokenBase && DecodedParameter is IHasSignature)
			{
				IHasSignature hasSignature = (IHasSignature)DecodedParameter;

				if (hasSignature.SignatureReader.HasGenericMethodParameter)
				{
					hasSignature.SignatureReader.SetGenericParametersOfMethod(typeGenericParameters, methodGenericParameters);
					((TextTokenBase)DecodedParameter).LazyInitialize(allTokens);
				}
			}
		}
	}
}