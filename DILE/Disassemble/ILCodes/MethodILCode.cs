using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class MethodILCode : BaseParameterILCode<uint, TokenBase>
	{
		public MethodILCode()
		{
		}

		public override void DecodeParameter()
		{
			if (DecodedParameter is MethodDefinition)
			{
				MethodDefinition methodDefinition = (MethodDefinition)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, methodDefinition.Text);
			}
			else if (DecodedParameter is MemberReference)
			{
				MemberReference memberReference = (MemberReference)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, memberReference.Text);
			}
			else if (DecodedParameter is MethodSpec)
			{
				MethodSpec methodSpec = (MethodSpec)DecodedParameter;

				Text = string.Format("{0} {1}", OpCode.Name, methodSpec.Name);
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