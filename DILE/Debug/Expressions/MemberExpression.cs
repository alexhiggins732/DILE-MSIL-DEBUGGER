using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Runtime.InteropServices;

namespace Dile.Debug.Expressions
{
	public class MemberExpression : BaseExpression
	{
		private MemberExpression nextExpression;
		public MemberExpression NextExpression
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

		private TypeArgumentExpression typeArgumentExpression;
		public TypeArgumentExpression TypeArgumentExpression
		{
			get
			{
				return typeArgumentExpression;
			}
			set
			{
				typeArgumentExpression = value;
			}
		}

		private List<List<BaseExpression>> parameterExpressions;
		public List<List<BaseExpression>> ParameterExpressions
		{
			get
			{
				return parameterExpressions;
			}
			private set
			{
				parameterExpressions = value;
			}
		}

		public int ParameterExpressionsLength
		{
			get
			{
				int result = 0;

				if (ParameterExpressions != null)
				{
					foreach (List<BaseExpression> parameterExpression in ParameterExpressions)
					{
						foreach (BaseExpression expression in parameterExpression)
						{
							result += expression.FoundExpressionLength;
						}
					}

					//Each parameter must be separated by a comma, thus add the number of commas also.
					result += ParameterExpressions.Count - 1;
				}

				return result;
			}
		}

		private int foundExpressionLength;
		public override int FoundExpressionLength
		{
			get
			{
				return foundExpressionLength;
			}
			protected set
			{
				foundExpressionLength = value;
			}
		}

		public MemberExpression(string foundExpression)
			: base(foundExpression)
		{
			FoundExpressionLength = FoundExpression.Length;
		}

		public MemberExpression(string foundExpression, string parameters)
			: this(foundExpression)
		{
			Parser parameterParser = new Parser();
			int index = 0;

			while (index < parameters.Length)
			{
				string parameterText = GetMethodParameterText(parameters.Substring(index));
				List<BaseExpression> parameter = parameterParser.Parse(parameterText);

				if (ParameterExpressions == null)
				{
					ParameterExpressions = new List<List<BaseExpression>>();
				}

				ParameterExpressions.Add(parameter);
				index += parameterText.Length + 1;
			}
		}

		public MemberExpression(string foundExpression, List<List<BaseExpression>> parameterExpressions)
			: this(foundExpression)
		{
			ParameterExpressions = parameterExpressions;
		}

		private static bool ValidMemberNameCharacter(char character)
		{
			return (character == '.' || Char.IsLetterOrDigit(character) || character == '_');
		}

		public static MemberExpression TryParse(string expressionText)
		{
			MemberExpression result = null;
			int characterIndex = 0;
			int previousDotPosition = 0;
			MemberExpression subexpression = null;

			while (characterIndex < expressionText.Length && ValidMemberNameCharacter(expressionText[characterIndex]))
			{
				previousDotPosition = characterIndex;
				bool dotFound = false;
				bool firstDotSkipped = (expressionText[characterIndex] == '.');
				characterIndex++;

				while (!dotFound && characterIndex < expressionText.Length && ValidMemberNameCharacter(expressionText[characterIndex]))
				{
					if (expressionText[characterIndex] == '.')
					{
						if (characterIndex - previousDotPosition == 1)
						{
							if (firstDotSkipped)
							{
								throw new ParserException("Invalid member referencing (more than 1 dots are used).");
							}
							else
							{
								previousDotPosition = characterIndex;
								firstDotSkipped = true;
								characterIndex++;
							}
						}
						else
						{
							dotFound = true;
						}
					}
					else
					{
						characterIndex++;
					}
				}

				string elementName = string.Empty;

				if (firstDotSkipped)
				{
					elementName = expressionText.Substring(previousDotPosition + 1, characterIndex - previousDotPosition - 1);
				}
				else
				{
					elementName = expressionText.Substring(previousDotPosition, characterIndex - previousDotPosition);
				}

				MemberExpression memberExpression = null;
				int foundExpressionLength = 0;
				int bracketPosition = 0;
				string parameters = string.Empty;
				int bracketOffset = elementName.Length + previousDotPosition;
				TypeArgumentExpression typeArgumentExpression = null;

				if (characterIndex < expressionText.Length && expressionText[characterIndex] == '<')
				{
					typeArgumentExpression = (TypeArgumentExpression)TypeArgumentExpression.TryParse(expressionText.Substring(characterIndex));
					bracketOffset += typeArgumentExpression.FoundExpressionLength;
				}

				if (firstDotSkipped)
				{
					bracketOffset++;
				}

				if (previousDotPosition < 0)
				{
					bracketOffset++;
				}

				if (bracketOffset < expressionText.Length && expressionText[bracketOffset] == '(')
				{
					bracketPosition = elementName.Length + previousDotPosition + 1;
					parameters = GetMethodParameters(expressionText.Substring(bracketOffset + 1));
				}

				if (bracketPosition > 0)
				{
					memberExpression = new MemberExpression(elementName, parameters);

					if (previousDotPosition < 0)
					{
						previousDotPosition = 0;
					}

					characterIndex += memberExpression.ParameterExpressionsLength + 2;
					foundExpressionLength = elementName.Length + memberExpression.ParameterExpressionsLength + 2;
				}
				else
				{
					memberExpression = new MemberExpression(elementName);
					foundExpressionLength = elementName.Length;
				}

				if (firstDotSkipped)
				{
					foundExpressionLength++;
				}

				if (result == null)
				{
					result = memberExpression;
					result.FoundExpressionLength = foundExpressionLength;
				}
				else if (result.NextExpression == null)
				{
					result.NextExpression = memberExpression;
					subexpression = result.NextExpression;
					result.FoundExpressionLength += foundExpressionLength;
				}
				else
				{
					subexpression.NextExpression = memberExpression;
					subexpression = subexpression.NextExpression;
					result.FoundExpressionLength += foundExpressionLength;
				}

				if (memberExpression != null)
				{
					memberExpression.TypeArgumentExpression = typeArgumentExpression;

					if (typeArgumentExpression != null)
					{
						memberExpression.FoundExpressionLength += typeArgumentExpression.FoundExpressionLength;
						characterIndex += typeArgumentExpression.FoundExpressionLength;
					}
				}
			}

			return result;
		}

		private DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue, MethodDefinition method, List<DebugExpressionResult> methodParameters, TypeTreeNodeList typeArguments)
		{
			DebugExpressionResult result = null;
			ModuleWrapper module = HelperFunctions.FindModuleOfType(context, method.BaseTypeDefinition);
			FunctionWrapper function = module.GetFunction(method.Token);
			List<ValueWrapper> methodValueParameters = null;

			if (methodParameters != null)
			{
				methodValueParameters = new List<ValueWrapper>(methodParameters.Count);

				for (int index = 0; index < methodParameters.Count; index++)
				{
					methodValueParameters.Add(methodParameters[index].ResultValue);
				}
			}

			BaseEvaluationResult evaluationResult = null;
			List<TypeWrapper> typeWrapperArguments = null;

			if (context.EvalWrapper.IsVersion2)
			{
				if (context.ClassTypeArguments == null)
				{
					if (typeArguments != null)
					{
						typeWrapperArguments = typeArguments.GetAsTypeWrapperList(context);
					}
				}
				else
				{
					typeWrapperArguments = context.ClassTypeArguments.GetAsTypeWrapperList(context);

					if (typeArguments != null)
					{
						typeWrapperArguments.AddRange(typeArguments.GetAsTypeWrapperList(context));
					}
				}
			}

			bool isConstructorMethod = (method.Name == Constants.ConstructorMethodName);

			if (isConstructorMethod)
			{
				if (typeWrapperArguments != null && context.EvalWrapper.IsVersion2)
				{
					context.ClassTypeArguments = typeArguments;
					context.EvalWrapper.Version2.NewParameterizedObject(function, typeWrapperArguments, methodValueParameters);
					evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);
				}
				else
				{
					context.EvalWrapper.NewObject(function, methodValueParameters);
					evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);
				}
			}
			else
			{
				evaluationResult = context.EvaluationHandler.CallFunction(function, methodValueParameters, typeWrapperArguments);
			}

			if (evaluationResult.IsSuccessful)
			{
				result = new DebugExpressionResult(context, evaluationResult.Result);
			}
			else
			{
				evaluationResult.ThrowExceptionAccordingToReason();
			}

			if (isConstructorMethod)
			{
				context.ClassTypeArguments = null;
			}

			return result;
		}

		private List<DebugExpressionResult> EvaluateMethodParameters(EvaluationContext context, DebugExpressionResult thisValue)
		{
			List<DebugExpressionResult> result = new List<DebugExpressionResult>();

			if (ParameterExpressions != null)
			{
				for (int index = 0; index < ParameterExpressions.Count; index++)
				{
					result.Add(ExecuteExpressionList(context, thisValue, ParameterExpressions[index]));
				}
			}

			if (thisValue != null && thisValue.ResultValue != null)
			{
				result.Insert(0, thisValue);
			}

			return result;
		}

		public DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue, TypeDefinition baseType, TypeTreeNodeList typeArguments)
		{
			DebugExpressionResult result = null;

			if (baseType == null)
			{
				baseType = FindThisTypeValueTypeDefinition(context, thisValue);
			}

			Assembly assembly = baseType.ModuleScope.Assembly;

			if (baseType.GetIsEnum())
			{
				result = CreateEnumFieldValue(context, baseType);

				if (NextExpression != null)
				{
					result = NextExpression.Evaluate(context, result);
				}
			}
			else
			{
				List<DebugExpressionResult> methodParameters = EvaluateMethodParameters(context, thisValue);

				TypeTreeNodeList methodTypeArguments = (TypeArgumentExpression == null ? null : TypeArgumentExpression.TypeArguments);

				MethodDefinition method = baseType.FindMethodDefinitionByParameter(context, FoundExpression, typeArguments, methodTypeArguments, methodParameters);

				if (method == null)
				{
					Property property = baseType.FindPropertyByName(FoundExpression);

					if (property != null)
					{
						method = property.BaseTypeDefinition.ModuleScope.Assembly.AllTokens[property.GetterMethodToken] as MethodDefinition;
					}
				}

				if (method == null)
				{
					FieldDefinition fieldDefinition = baseType.FindFieldDefinitionByName(FoundExpression);

					if (fieldDefinition != null)
					{
						if ((fieldDefinition.Flags & CorFieldAttr.fdStatic) == CorFieldAttr.fdStatic)
						{
							ModuleWrapper module = HelperFunctions.FindModuleOfType(context, fieldDefinition.BaseTypeDefinition);
							ClassWrapper thisValueClass = module.GetClass(fieldDefinition.BaseTypeDefinition.Token);

							result = new DebugExpressionResult(context, thisValueClass.GetStaticFieldValue(fieldDefinition.Token, context.FrameWrapper));
						}
						else
						{
							ClassWrapper fieldDefinitionClass = null;

							if (thisValue.ResultClass.GetToken() == fieldDefinition.BaseTypeDefinition.Token)
							{
								fieldDefinitionClass = thisValue.ResultClass;
							}
							else
							{
								ModuleWrapper fieldDefinitionModule = HelperFunctions.FindModuleOfType(context, fieldDefinition.BaseTypeDefinition);

								fieldDefinitionClass = fieldDefinitionModule.GetClass(fieldDefinition.BaseTypeDefinition.Token);
							}

							result = new DebugExpressionResult(context, thisValue.ResultValue.DereferenceValue().GetFieldValue(fieldDefinitionClass, fieldDefinition.Token));
						}
					}
				}
				else
				{
					if (typeArguments != null)
					{
						if (methodTypeArguments == null)
						{
							methodTypeArguments = new TypeTreeNodeList();
						}

						methodTypeArguments.TypeTreeNodes.InsertRange(0, typeArguments.TypeTreeNodes);
					}

					result = Evaluate(context, thisValue, method, methodParameters, methodTypeArguments);
				}

				if (NextExpression != null)
				{
					result = NextExpression.Evaluate(context, result, null, typeArguments);
				}
			}

			return result;
		}

		private DebugExpressionResult CreateEnumFieldValue(EvaluationContext context, TypeDefinition enumType)
		{
			DebugExpressionResult result = null;
			FieldDefinition enumFieldDefinition = enumType.FindFieldDefinitionByName(FoundExpression);

			if (enumFieldDefinition == null)
			{
				throw new EvaluationException(string.Format("Field called '{0}' of type '{1}' cannot be found.", FoundExpression, enumType.FullName));
			}

			result = new DebugExpressionResult(context, context.EvalWrapper.CreateValue((int)enumFieldDefinition.DefaultValueType, null));

			switch (enumFieldDefinition.DefaultValueType)
			{
				case CorElementType.ELEMENT_TYPE_I1:
					result.ResultValue.SetGenericValue<sbyte>(Convert.ToSByte(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					result.ResultValue.SetGenericValue<short>(Convert.ToInt16(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_I:
				case CorElementType.ELEMENT_TYPE_I4:
					result.ResultValue.SetGenericValue<int>(Convert.ToInt32(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					result.ResultValue.SetGenericValue<long>(Convert.ToInt64(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					result.ResultValue.SetGenericValue<byte>(Convert.ToByte(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					result.ResultValue.SetGenericValue<ushort>(Convert.ToUInt16(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_U:
				case CorElementType.ELEMENT_TYPE_U4:
					result.ResultValue.SetGenericValue<uint>(Convert.ToUInt32(enumFieldDefinition.DefaultValueNumber));
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					result.ResultValue.SetGenericValue<ulong>(Convert.ToUInt64(enumFieldDefinition.DefaultValueNumber));
					break;

				default:
					throw new EvaluationException(string.Format("Unable to create enum value of {0} type.", Enum.GetName(typeof(CorElementType), enumFieldDefinition.DefaultValueType)));
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			TypeDefinition thisValueTypeDefinition = FindThisTypeValueTypeDefinition(context, thisValue);

			if (thisValueTypeDefinition == null)
			{
				throw new EvaluationException("The type of a value could not be determined.");
			}

			List<DebugExpressionResult> methodParameters = EvaluateMethodParameters(context, thisValue);

			TypeTreeNodeList classTypeArguments = GetClassTypeArguments(context, thisValue);
			if (classTypeArguments != null && classTypeArguments.TypeTreeNodes.Count > 0)
			{
				context.ClassTypeArguments = classTypeArguments;
			}

			TypeTreeNodeList typeArguments = (TypeArgumentExpression == null ? null : TypeArgumentExpression.TypeArguments);
			MethodDefinition method = thisValueTypeDefinition.FindMethodDefinitionByParameter(context, FoundExpression, null, typeArguments, methodParameters);

			if (method == null)
			{
				Property property = thisValueTypeDefinition.FindPropertyByName(FoundExpression);

				if (property != null)
				{
					method = property.BaseTypeDefinition.ModuleScope.Assembly.AllTokens[property.GetterMethodToken] as MethodDefinition;
				}
			}

			DebugExpressionResult result = null;

			if (method == null)
			{
				FieldDefinition fieldDefinition = thisValueTypeDefinition.FindFieldDefinitionByName(FoundExpression);

				if (fieldDefinition != null)
				{
					if ((fieldDefinition.Flags & CorFieldAttr.fdStatic) == CorFieldAttr.fdStatic)
					{
						result = new DebugExpressionResult(context, thisValue.ResultClass.GetStaticFieldValue(fieldDefinition.Token, context.FrameWrapper));
					}
					else
					{
						ClassWrapper fieldDefinitionClass = null;

						if (thisValue.ResultClass.GetToken() == fieldDefinition.BaseTypeDefinition.Token)
						{
							fieldDefinitionClass = thisValue.ResultClass;
						}
						else
						{
							ModuleWrapper fieldDefinitionModule = HelperFunctions.FindModuleOfType(context, fieldDefinition.BaseTypeDefinition);

							fieldDefinitionClass = fieldDefinitionModule.GetClass(fieldDefinition.BaseTypeDefinition.Token);
						}

						result = new DebugExpressionResult(context, thisValue.ResultValue.DereferenceValue().GetFieldValue(fieldDefinitionClass, fieldDefinition.Token));
					}
				}
			}
			else
			{
				result = Evaluate(context, thisValue, method, methodParameters, typeArguments);
			}

			if (NextExpression != null)
			{
				result = NextExpression.Evaluate(context, result);
			}

			return result;
		}

		private TypeDefinition FindThisTypeValueTypeDefinition(EvaluationContext context, DebugExpressionResult thisValue)
		{
			TypeDefinition result = null;

			if (thisValue.ResultClass == null)
			{
				result = HelperFunctions.GetTypeByElementType((CorElementType)thisValue.ResultValue.ElementType);

				if (result != null)
				{
					ValueWrapper boxedValue = HelperFunctions.CreateBoxedValue(context, result);
					ValueWrapper dereferencedValue = boxedValue.DereferenceValue();
					ValueWrapper unboxedValue = dereferencedValue.UnboxValue();

					HelperFunctions.CastDebugValue(thisValue.ResultValue, unboxedValue, (CorElementType)thisValue.ResultValue.ElementType);
					thisValue.ResultValue = boxedValue;
				}
			}
			else
			{
				result = HelperFunctions.FindTypeOfValue(thisValue.ResultClass);
			}

			return result;
		}
	}
}