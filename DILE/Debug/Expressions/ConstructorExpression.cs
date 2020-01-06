using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Runtime.InteropServices;

namespace Dile.Debug.Expressions
{
	public class ConstructorExpression : BaseExpression
	{
		public const char NewKeywordSeparatorChar = '|';

		private TypeExpression constructedType;
		public TypeExpression ConstructedType
		{
			get
			{
				return constructedType;
			}
			private set
			{
				constructedType = value;
			}
		}

		private List<List<BaseExpression>> arrayParameters;
		private List<List<BaseExpression>> ArrayParameters
		{
			get
			{
				return arrayParameters;
			}
			set
			{
				arrayParameters = value;
			}
		}

		private List<BaseExpression> nextExpressions;
		private List<BaseExpression> NextExpressions
		{
			get
			{
				return nextExpressions;
			}
			set
			{
				nextExpressions = value;
			}
		}

		public ConstructorExpression(string foundExpression, TypeExpression constructedType, string arrayParametersText, List<BaseExpression> nextExpressions)
			: base(foundExpression)
		{
			ConstructedType = constructedType;
			NextExpressions = nextExpressions;

			if (!string.IsNullOrEmpty(arrayParametersText))
			{
				Parser parameterParser = new Parser();
				int index = 0;
				ArrayParameters = new List<List<BaseExpression>>();

				while (index < arrayParametersText.Length)
				{
					string parameterText = GetArrayParameterText(arrayParametersText.Substring(index));
					List<BaseExpression> parameter = parameterParser.Parse(parameterText);

					ArrayParameters.Add(parameter);
					index += parameterText.Length + 1;
				}
			}
		}

		public static BaseExpression TryParse(string expressionText)
		{
			ConstructorExpression result = null;
			bool startBracketFound = (expressionText[0] == '(');
			int newKeywordIndex = 0;

			if (startBracketFound)
			{
				newKeywordIndex++;
			}

			if (expressionText.Length > newKeywordIndex + 4 && expressionText.Substring(newKeywordIndex, 4) == "new" + NewKeywordSeparatorChar)
			{
				TypeExpression constructedType = TypeExpression.TryParse(expressionText.Substring(newKeywordIndex + 4)) as TypeExpression;

				if (constructedType == null)
				{
					throw new EvaluationException(string.Format("The definition of the type ('{0}') could not be found.", expressionText.Substring(newKeywordIndex + 4)));
				}

				bool createResult = true;
				List<BaseExpression> nextExpressions = null;
				string foundExpression = string.Empty;
				string arrayParametersText = string.Empty;

				if (expressionText.Length < newKeywordIndex + 4 + constructedType.FoundExpressionLength)
				{
					createResult = false;
				}
				else
				{
					foundExpression = expressionText.Substring(newKeywordIndex, newKeywordIndex + 4 + constructedType.FoundExpressionLength);

					if (constructedType.IsArray)
					{
						arrayParametersText = GetArrayParameters(expressionText.Substring(foundExpression.Length + 1));
						foundExpression = expressionText.Substring(0, foundExpression.Length + arrayParametersText.Length + 2);
					}

					if (startBracketFound)
					{
						if (expressionText.Length > foundExpression.Length && expressionText[foundExpression.Length] == ')')
						{
							foundExpression = expressionText.Substring(0, foundExpression.Length + 1);
							Parser parser = new Parser();
							nextExpressions = parser.Parse(expressionText.Substring(foundExpression.Length));

							int foundExpressionLength = foundExpression.Length;
							foreach (BaseExpression expression in nextExpressions)
							{
								foundExpressionLength += expression.FoundExpressionLength;
							}

							if (foundExpressionLength > expressionText.Length)
							{
								foundExpressionLength = expressionText.Length;
							}

							foundExpression = expressionText.Substring(0, foundExpressionLength);
						}
						else
						{
							createResult = false;
						}
					}
				}

				if (createResult)
				{
					result = new ConstructorExpression(foundExpression, constructedType, arrayParametersText, nextExpressions);
				}
			}

			return result;
		}

		private List<ValueWrapper> EvaluateArrayParameters(EvaluationContext context)
		{
			List<ValueWrapper> result = new List<ValueWrapper>(ArrayParameters.Count);

			for (int index = 0; index < ArrayParameters.Count; index++)
			{
				List<BaseExpression> arrayParameter = ArrayParameters[index];

				result.Add(ExecuteExpressionList(context, null, arrayParameter).ResultValue);
			}

			return result;
		}

		private void FillCreatedArray(EvaluationContext context, ValueWrapper array, List<ValueWrapper> arrayParameterValues)
		{
			ArrayValueWrapper arrayValue = array.ConvertToArrayValue();

			for (int index = 0; index < arrayParameterValues.Count; index++)
			{
				ValueWrapper arrayElement = arrayValue.GetElementAtPosition(Convert.ToUInt32(index));
				ValueWrapper arrayParameterValue = arrayParameterValues[index];

				if (HelperFunctions.HasValueClass(arrayElement))
				{
					if (!HelperFunctions.HasValueClass(arrayParameterValue))
					{
						TypeDefinition arrayParameterTypeDef = HelperFunctions.GetTypeByElementType((CorElementType)arrayParameterValue.ElementType);

						ValueWrapper boxedValue = HelperFunctions.CreateBoxedValue(context, arrayParameterTypeDef);

						ValueWrapper dereferencedValue = boxedValue.DereferenceValue();
						HelperFunctions.CastDebugValue(arrayParameterValue, dereferencedValue.UnboxValue(), (CorElementType)arrayParameterValue.ElementType);

						arrayParameterValue = boxedValue;
						arrayValue = array.ConvertToArrayValue();
					}

					arrayElement.SetValue(arrayParameterValue);
				}
				else
				{
					HelperFunctions.CastDebugValue(arrayParameterValue, arrayElement);
				}
			}
		}

		private void ValidateArrayParameters(EvaluationContext context, List<ValueWrapper> arrayParameterValues)
		{
			for (int index = 0; index < arrayParameterValues.Count; index++)
			{
				ValueWrapper arrayParameterValue = arrayParameterValues[index];
				TypeDefinition arrayParameterTypeDef = null;
				bool isArrayParameterNull = false;

				if (HelperFunctions.HasValueClass(arrayParameterValue))
				{
					isArrayParameterNull = arrayParameterValue.IsNull();

					if (!isArrayParameterNull)
					{
						arrayParameterTypeDef = HelperFunctions.FindTypeOfValue(context, new DebugExpressionResult(context, arrayParameterValue));
					}
				}
				else
				{
					arrayParameterTypeDef = HelperFunctions.GetTypeByElementType((CorElementType)arrayParameterValue.ElementType);
				}

				if (isArrayParameterNull)
				{
					if (ConstructedType.TypeDefinition.IsValueType)
					{
						throw new EvaluationException("Value type array cannot be created with null parameter. (parameter index = " + Convert.ToString(index));
					}
				}
				else if (!HelperFunctions.TypeDefinitionsMatch(arrayParameterTypeDef, ConstructedType.TypeDefinition))
				{
					throw new EvaluationException(string.Format("The type of the {0} parameter does not match the type of the array ({1}). (parameter index == {2})", arrayParameterTypeDef.FullName, ConstructedType.TypeDefinition, index));
				}
			}
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = null;

			if (ConstructedType.IsArray)
			{
				List<ValueWrapper> arrayParameterValues = EvaluateArrayParameters(context);
				ValidateArrayParameters(context, arrayParameterValues);

				CorElementType elementType = HelperFunctions.GetElementTypeByName(ConstructedType.TypeDefinition.FullName);

				if (elementType == CorElementType.ELEMENT_TYPE_END)
				{
					ClassWrapper elementClass = HelperFunctions.FindClassOfTypeDefintion(context, ConstructedType.TypeDefinition);

					if (ConstructedType.TypeArgumentExpression == null)
					{
						elementType = (ConstructedType.TypeDefinition.IsValueType ? CorElementType.ELEMENT_TYPE_SZARRAY : CorElementType.ELEMENT_TYPE_CLASS);

						context.EvalWrapper.NewArray((int)elementType, elementClass, Convert.ToUInt32(arrayParameterValues.Count));
					}
					else
					{
						int arrayRank = 1;
						List<uint> dimensions = new List<uint>(arrayRank);
						dimensions.Add(Convert.ToUInt32(arrayParameterValues.Count));

						List<uint> lowerBounds = new List<uint>(arrayRank);
						lowerBounds.Add(0);

						elementType = (ConstructedType.TypeDefinition.IsValueType ? CorElementType.ELEMENT_TYPE_VALUETYPE : CorElementType.ELEMENT_TYPE_CLASS);
						List<TypeWrapper> elementTypeArguments = ConstructedType.TypeArgumentExpression.TypeArguments.GetAsTypeWrapperList(context);
						TypeWrapper arrayElementType = elementClass.Version2.GetParameterizedType((int)elementType, elementTypeArguments);

						AppDomainWrapper appDomain = context.ThreadWrapper.GetAppDomain();
						TypeWrapper arrayType = appDomain.Version2.GetArrayOrPointerType((int)CorElementType.ELEMENT_TYPE_SZARRAY, Convert.ToUInt32(arrayRank), arrayElementType);

						context.EvalWrapper.Version2.NewParameterizedArray(arrayType, Convert.ToUInt32(arrayRank), dimensions, lowerBounds);
					}
				}
				else
				{
					context.EvalWrapper.NewArray((int)elementType, null, Convert.ToUInt32(arrayParameterValues.Count));
				}

				BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

				if (evaluationResult.IsSuccessful)
				{
					result = new DebugExpressionResult(context, evaluationResult.Result);
					FillCreatedArray(context, result.ResultValue, arrayParameterValues);
				}
				else
				{
					evaluationResult.ThrowExceptionAccordingToReason();
				}
			}
			else
			{
				result = ConstructedType.Evaluate(context, null);
			}

			if (NextExpressions != null)
			{
				result = ExecuteExpressionList(context, result, NextExpressions);
			}

			return result;
		}
	}
}