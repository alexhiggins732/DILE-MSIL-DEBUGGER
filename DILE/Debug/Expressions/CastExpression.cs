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
	public class CastExpression : BaseExpression
	{
		private TypeExpression typeToCast;
		private TypeExpression TypeToCast
		{
			get
			{
				return typeToCast;
			}
			set
			{
				typeToCast = value;
			}
		}

		private List<BaseExpression> expressionsToCast;
		private List<BaseExpression> ExpressionsToCast
		{
			get
			{
				return expressionsToCast;
			}
			set
			{
				expressionsToCast = value;
			}
		}

		private MemberExpression nextExpression;
		private MemberExpression NextExpression
		{
			get
			{
				return nextExpression;
			}
			set
			{
				nextExpression = value;
			}
		}

		public CastExpression(TypeExpression typeToCast, List<BaseExpression> expressionsToCast, MemberExpression nextExpression, string foundExpression)
			: base(foundExpression)
		{
			TypeToCast = typeToCast;
			ExpressionsToCast = expressionsToCast;
			NextExpression = nextExpression;
		}

		public static CastExpression TryParse(string expressionText)
		{
			CastExpression result = null;

			if (expressionText[0] == '(')
			{
				int startBracketCount = 1;

				if (expressionText[1] == '(')
				{
					startBracketCount++;
				}

				int endBracketPosition = expressionText.IndexOf(')');

				if (endBracketPosition > -1)
				{
					string typeToCastExpression = expressionText.Substring(startBracketCount, endBracketPosition - startBracketCount);
					Parser parser = new Parser();
					List<BaseExpression> parsedExpressions = parser.Parse(typeToCastExpression);

					if (parsedExpressions != null && parsedExpressions.Count == 1)
					{
						TypeExpression typeToCast = parsedExpressions[0] as TypeExpression;

						if (typeToCast != null)
						{
							string castedExpression = GetMethodParameters(expressionText.Substring(endBracketPosition + 1));
							parsedExpressions = parser.Parse(castedExpression);

							if (parsedExpressions != null && parsedExpressions.Count > 0)
							{
								int foundExpressionLength = typeToCast.FoundExpressionLength + startBracketCount + 1;

								foreach (BaseExpression parsedExpression in parsedExpressions)
								{
									foundExpressionLength += parsedExpression.FoundExpressionLength;
								}

								if (expressionText[expressionText.Length - 2] == ')' && expressionText.Length - 2 > endBracketPosition)
								{
									foundExpressionLength++;
								}

								MemberExpression nextExpression = null;

								if (expressionText.Length > foundExpressionLength + 1)
								{
									nextExpression = MemberExpression.TryParse(expressionText.Substring(foundExpressionLength + 1));
								}

								if (startBracketCount > 1)
								{
									int bracketPosition = foundExpressionLength + startBracketCount - 2;
									if (expressionText.Length > bracketPosition && expressionText[bracketPosition] == ')')
									{
										foundExpressionLength++;
									}
								}

								if (nextExpression != null)
								{
									foundExpressionLength += nextExpression.FoundExpressionLength;
								}

								if (foundExpressionLength > expressionText.Length)
								{
									foundExpressionLength = expressionText.Length;
								}

								result = new CastExpression(typeToCast, parsedExpressions, nextExpression, expressionText.Substring(0, foundExpressionLength));
							}
						}
					}
				}
			}

			return result;
		}

		private DebugExpressionResult ConvertUsingOverloadedOperator(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = null;

			List<DebugExpressionResult> operatorParameters = new List<DebugExpressionResult>(1);
			operatorParameters.Add(thisValue);

			MethodDefinition explicitOperatorMethod = null;
			MethodDefinition implicitOperatorMethod = null;
			MethodDefinition operatorMethod = null;

			if (HelperFunctions.GetElementTypeByName(TypeToCast.TypeDefinition.FullName) == CorElementType.ELEMENT_TYPE_END)
			{
				explicitOperatorMethod = TypeToCast.TypeDefinition.FindMethodDefinitionByParameter(context, Constants.ExplicitOperatorMethodName, null, null, operatorParameters, TypeToCast.TypeDefinition, TypeToCast.IsArray);
				implicitOperatorMethod = TypeToCast.TypeDefinition.FindMethodDefinitionByParameter(context, Constants.ImplicitOperatorMethodName, null, null, operatorParameters, TypeToCast.TypeDefinition, TypeToCast.IsArray);

				if (explicitOperatorMethod != null && implicitOperatorMethod != null)
				{
					throw new EvaluationException(string.Format("Both an explicit and an implicit cast operator is defined on the type ({0}) that could be used for the conversion.", TypeToCast.TypeDefinition.FullName));
				}

				if (explicitOperatorMethod != null)
				{
					operatorMethod = explicitOperatorMethod;
				}
				else
				{
					operatorMethod = implicitOperatorMethod;
				}
			}

			TypeDefinition thisType = null;

			if (HelperFunctions.HasValueClass(thisValue.ResultValue) && !HelperFunctions.IsArrayElementType((CorElementType)thisValue.ResultValue.ElementType))
			{
				thisType = HelperFunctions.FindTypeOfValue(thisValue.ResultClass);

				explicitOperatorMethod = thisType.FindMethodDefinitionByParameter(context, Constants.ExplicitOperatorMethodName, null, null, operatorParameters, TypeToCast.TypeDefinition, TypeToCast.IsArray);
				implicitOperatorMethod = thisType.FindMethodDefinitionByParameter(context, Constants.ImplicitOperatorMethodName, null, null, operatorParameters, TypeToCast.TypeDefinition, TypeToCast.IsArray);
			}

			if (explicitOperatorMethod != null && implicitOperatorMethod != null)
			{
				throw new EvaluationException(string.Format("Both an explicit and an implicit cast operator is defined on the type ({0}) that could be used for the conversion.", thisType.FullName));
			}

			if (explicitOperatorMethod != null && operatorMethod != null)
			{
				throw new EvaluationException(string.Format("More than 1 suitable cast operator has been found on the following types: {0}, {1}."));
			}

			if (implicitOperatorMethod != null && operatorMethod != null)
			{
				throw new EvaluationException(string.Format("More than 1 suitable cast operator has been found on the following types: {0}, {1}.", TypeToCast.TypeDefinition.FullName, thisType.FullName));
			}

			if (operatorMethod == null)
			{
				if (implicitOperatorMethod != null)
				{
					operatorMethod = implicitOperatorMethod;
				}
				else
				{
					operatorMethod = explicitOperatorMethod;
				}
			}

			if (operatorMethod != null)
			{
				List<ValueWrapper> operatorArguments = new List<ValueWrapper>(1);
				operatorArguments.Add(thisValue.ResultValue);

				BaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, operatorMethod, operatorArguments);

				if (evaluationResult.IsSuccessful)
				{
					result = new DebugExpressionResult(context, evaluationResult.Result);
				}
				else
				{
					evaluationResult.ThrowExceptionAccordingToReason();
				}
			}

			return result;
		}

		#region Working but not used
		//private void CastArray(EvaluationContext context, DebugExpressionResult arrayExpression, TypeDefinition elementTypeDefinition)
		//{
		//  if (!HelperFunctions.IsArrayElementType((CorElementType)arrayExpression.ResultValue.ElementType))
		//  {
		//    throw new InvalidOperationException();
		//  }

		//  ArrayValueWrapper arrayWrapper = arrayExpression.ResultValue.ConvertToArrayValue();
		//  ClassWrapper elementClass = HelperFunctions.FindClassOfTypeDefintion(context.ProcessWrapper, elementTypeDefinition);
		//  context.EvalWrapper.NewArray((int)CorElementType.ELEMENT_TYPE_SZARRAY, elementClass, arrayWrapper.GetCount());

		//  ValueWrapper resultArray = null;
		//  ArrayValueWrapper resultArrayWrapper = null;
		//  BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

		//  if (evaluationResult.IsSuccessful)
		//  {
		//    resultArray = evaluationResult.Result;
		//    resultArrayWrapper = resultArray.ConvertToArrayValue();
		//  }
		//  else
		//  {
		//    Marshal.ThrowExceptionForHR(evaluationResult.HResult);
		//  }

		//  arrayWrapper = arrayExpression.ResultValue.ConvertToArrayValue();

		//  for (uint index = 0; index < arrayWrapper.GetCount(); index++)
		//  {
		//    DebugExpressionResult element = new DebugExpressionResult(context, arrayWrapper.GetElementAtPosition(index));

		//    CastValue(context, element, false, elementTypeDefinition, HelperFunctions.IsArrayElementType((CorElementType)element.ResultValue.ElementType));

		//    ValueWrapper resultArrayElement = resultArrayWrapper.GetElementAtPosition(index);

		//    if (HelperFunctions.HasValueClass(element.ResultValue))
		//    {
		//      resultArrayElement.SetValue(element.ResultValue);
		//    }
		//    else
		//    {
		//      HelperFunctions.CastDebugValue(element.ResultValue, resultArrayElement);
		//    }
		//  }

		//  arrayExpression.ResultValue = resultArray;
		//}
		#endregion

		private void CastValue(EvaluationContext context, DebugExpressionResult valueExpression, bool nextExpressionExists, TypeDefinition typeDefToCast, bool castToArrayType)
		{
			if ((nextExpressionExists || HelperFunctions.HasValueClass(valueExpression.ResultValue)) && typeDefToCast.FullName != Constants.DecimalTypeName)
			{
				DebugExpressionResult convertedValue = ConvertUsingOverloadedOperator(context, valueExpression);

				if (convertedValue == null)
				{
					TypeDefinition valueExpressionTypeDef = HelperFunctions.FindTypeOfValue(valueExpression.ResultClass);

					if (!valueExpressionTypeDef.IsSubclassOrImplements(typeDefToCast))
					{
						throw new EvaluationException(string.Format("A(n) {0} type object cannot be casted to {1} type.", valueExpressionTypeDef.FullName, typeDefToCast.FullName));
					}

					ModuleWrapper module = HelperFunctions.FindModuleOfType(context, typeDefToCast);
					ClassWrapper classWrapper = module.GetClass(typeDefToCast.Token);
					valueExpression.ResultClass = classWrapper;
				}
				else
				{
					valueExpression.ResultValue = convertedValue.ResultValue;
				}
			}
			else
			{
				CorElementType castedValueElementType = HelperFunctions.GetElementTypeByName(typeDefToCast.FullName);

				if (castedValueElementType != CorElementType.ELEMENT_TYPE_END)
				{
					ValueWrapper castedValue = context.EvalWrapper.CreateValue((int)castedValueElementType, null);
					HelperFunctions.CastDebugValue(valueExpression.ResultValue, castedValue);
					valueExpression.ResultValue = castedValue;
				}
				else if (HelperFunctions.ExpandTypeName(typeDefToCast.FullName) == Constants.DecimalTypeName)
				{
					valueExpression.ResultValue = HelperFunctions.CastToDecimal(context, valueExpression.ResultValue);
				}
				else
				{
					throw new EvaluationException(string.Format("Unable to cast the value ({0}).", Enum.GetName(typeof(CorElementType), valueExpression.ResultValue.ElementType)));
				}
			}
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = base.ExecuteExpressionList(context, thisValue, ExpressionsToCast);

			CastValue(context, result, (NextExpression != null), TypeToCast.TypeDefinition, TypeToCast.IsArray);

			if (NextExpression != null && result != null)
			{
				result = NextExpression.Evaluate(context, result, TypeToCast.TypeDefinition, null);
			}

			return result;
		}
	}
}