using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class TypeILCode : BaseParameterILCode<uint, TokenBase>
	{
		public override void DecodeParameter()
		{
			string parameterText = string.Empty;

			if (DecodedParameter != null)
			{
				Type decodedParameterType = DecodedParameter.GetType();

				if (decodedParameterType == typeof(MethodDefinition))
				{	
					parameterText = ((MethodDefinition)DecodedParameter).Text;
				}
				else if (decodedParameterType == typeof(MemberReference))
				{
					parameterText = ((MemberReference)DecodedParameter).Text;
				}
				else if (decodedParameterType == typeof(TypeReference))
				{
					parameterText = ((TypeReference)DecodedParameter).FullName;
				}
				else if (decodedParameterType == typeof(TypeDefinition))
				{	
					parameterText = ((TypeDefinition)DecodedParameter).FullName;
				}
				else if (decodedParameterType == typeof(TypeSpecification))
				{
					parameterText = ((TypeSpecification)DecodedParameter).ToString();
				}
			}

			Text = string.Format("{0} {1}", OpCode.Name, parameterText);
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