using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class FieldILCode : BaseParameterILCode<uint, TokenBase>
	{
		public override void DecodeParameter()
		{
			if (DecodedParameter is FieldDefinition)
			{
				Text = string.Format("{0} {1}", OpCode.Name, ((FieldDefinition)DecodedParameter).Text);
			}
			else
			{
				Text = string.Format("{0} {1}", OpCode.Name, DecodedParameter);
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