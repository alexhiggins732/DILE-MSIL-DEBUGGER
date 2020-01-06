using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;

namespace Dile.Debug.Expressions
{
	public class TypeExpression : BaseExpression
	{
		private TypeDefinition typeDefinition;
		public TypeDefinition TypeDefinition
		{
			get
			{
				return typeDefinition;
			}
			private set
			{
				typeDefinition = value;
			}
		}

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

		private bool isArray;
		public bool IsArray
		{
			get
			{
				return isArray;
			}
			set
			{
				isArray = value;
			}
		}

		private bool isParameterlessValueTypeConstructor;
		public bool IsParameterlessValueTypeConstructor
		{
			get
			{
				return isParameterlessValueTypeConstructor;
			}
			set
			{
				isParameterlessValueTypeConstructor = value;
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

		public TypeExpression(string foundExpression, TypeDefinition typeDefinition)
			: this(foundExpression, typeDefinition, null)
		{
		}

		public TypeExpression(string foundExpression, TypeDefinition typeDefinition, MemberExpression nextExpression)
			: base(foundExpression)
		{
			TypeDefinition = typeDefinition;
			NextExpression = nextExpression;
			FoundExpressionLength = FoundExpression.Length;
		}

		public static BaseExpression TryParse(string expressionText)
		{
			TypeExpression result = null;
			int previousDotPosition = 0;
			TypeDefinition typeDefinition = null;
			MemberExpression subexpression = null;
			int characterIndex = 0;
			TypeTreeNodeList allTypeArguments = null;

			while (characterIndex < expressionText.Length && (expressionText[characterIndex] == '.' || Char.IsLetterOrDigit(expressionText[characterIndex])))
			{
				previousDotPosition = characterIndex;
				characterIndex++;
				bool terminatingCharFound = false;
				bool isArray = false;

				while (!terminatingCharFound && characterIndex < expressionText.Length)
				{
					char character = expressionText[characterIndex];

					if (!Char.IsLetterOrDigit(expressionText[characterIndex]))
					{
						terminatingCharFound = true;
					}
					else
					{
						characterIndex++;
					}
				}

				string elementName = string.Empty;

				if (typeDefinition == null)
				{
					elementName = expressionText.Substring(0, characterIndex);
				}
				else
				{
					elementName = expressionText.Substring(previousDotPosition + 1, characterIndex - previousDotPosition - 1);
				}

				TypeArgumentExpression typeArgument = null;
				int typeArgumentsCount = 0;
				int typeArgumentsFoundLength = 0;
				bool isTypeExpressionCreated = false;
				int squareBracketPosition = characterIndex;

				if (characterIndex < expressionText.Length)
				{
					typeArgument = (TypeArgumentExpression)TypeArgumentExpression.TryParse(expressionText.Substring(characterIndex));

					if (typeArgument != null)
					{
						typeArgumentsCount = typeArgument.TypeArguments.TypeTreeNodes.Count;
						typeArgumentsFoundLength = typeArgument.FoundExpressionLength;
						squareBracketPosition += typeArgument.FoundExpressionLength;
					}
				}

				if (expressionText.Length > squareBracketPosition + 1 && expressionText[squareBracketPosition] == '[' && expressionText[squareBracketPosition + 1] == ']')
				{
					isArray = true;
				}

				TypeDefinition tempTypeDefinition = null;

				if (typeDefinition == null)
				{
					typeDefinition = HelperFunctions.FindTypeByName(elementName, typeArgumentsCount);

					if (typeDefinition == null)
					{
						typeDefinition = HelperFunctions.FindTypeByName(elementName, typeArgumentsCount);
					}

					if (typeDefinition != null)
					{
						result = new TypeExpression(elementName, typeDefinition);
						isTypeExpressionCreated = true;
					}
				}
				else
				{
					tempTypeDefinition = typeDefinition.FindNestedTypeByName(elementName, typeArgumentsCount);

					if (tempTypeDefinition == null)
					{
						if (result == null)
						{
							result = new TypeExpression(expressionText.Substring(0, previousDotPosition - 1), typeDefinition);
							isTypeExpressionCreated = true;
						}

						elementName = expressionText.Substring(previousDotPosition);
						MemberExpression memberExpression = MemberExpression.TryParse(elementName);
						result.FoundExpressionLength += memberExpression.FoundExpressionLength;
						characterIndex = result.FoundExpressionLength;

						if (result.NextExpression == null)
						{
							result.NextExpression = memberExpression;
							subexpression = result.NextExpression;
						}
						else
						{
							subexpression.NextExpression = memberExpression;
							subexpression = subexpression.NextExpression;
						}
					}
					else
					{
						typeDefinition = tempTypeDefinition;
						result = new TypeExpression(expressionText.Substring(0, characterIndex), typeDefinition);
						isTypeExpressionCreated = true;
					}
				}

				if (isTypeExpressionCreated)
				{
					if (typeDefinition != null && typeArgument != null)
					{
						if (allTypeArguments == null)
						{
							allTypeArguments = typeArgument.TypeArguments;
						}
						else
						{
							allTypeArguments.TypeTreeNodes.AddRange(typeArgument.TypeArguments.TypeTreeNodes);
						}
					}

					if (result != null)
					{
						int bracketPosition = (typeArgument == null ? characterIndex : characterIndex + typeArgument.FoundExpressionLength);

						if (bracketPosition < expressionText.Length && expressionText[bracketPosition] == '(')
						{
							string parameters = GetMethodParameters(expressionText.Substring(characterIndex + typeArgumentsFoundLength + 1));
							result.TypeArgumentExpression = typeArgument;

							if (typeDefinition.IsValueType && parameters.Length == 0)
							{
								result.IsParameterlessValueTypeConstructor = true;
								result.FoundExpressionLength += 2 + typeArgumentsFoundLength;
								characterIndex += 2 + typeArgumentsFoundLength;
							}
							else
							{
								MemberExpression memberExpression = new MemberExpression(Constants.ConstructorMethodName, parameters);

								if (result.NextExpression == null)
								{
									result.NextExpression = memberExpression;
									subexpression = result.NextExpression;
								}
								else
								{
									subexpression.NextExpression = memberExpression;
									subexpression = subexpression.NextExpression;
								}

								result.FoundExpressionLength += parameters.Length + 2 + typeArgumentsFoundLength;
								characterIndex += parameters.Length + 2 + typeArgumentsFoundLength;
							}
						}
						else
						{
							result.TypeArgumentExpression = typeArgument;
							result.FoundExpressionLength += typeArgumentsFoundLength;
							characterIndex += typeArgumentsFoundLength;
						}
					}
				}

				if (result != null && isArray)
				{
					result.IsArray = true;
					result.FoundExpressionLength += 2;
					characterIndex += 2;
				}
			}

			if (result != null && result.TypeArgumentExpression != null)
			{
				result.TypeArgumentExpression.TypeArguments = allTypeArguments;
			}

			return result;
		}

		public TypeWrapper GetAsTypeWrapper(EvaluationContext context)
		{
			TypeWrapper result = null;
			List<TypeWrapper> typeParameters;

			if (TypeArgumentExpression == null)
			{
				typeParameters = new List<TypeWrapper>(0);
			}
			else
			{
				typeParameters = TypeArgumentExpression.TypeArguments.GetAsTypeWrapperList(context);
			}

			ClassWrapper classWrapper = HelperFunctions.FindClassOfTypeDefintion(context, TypeDefinition);

			if (classWrapper != null && classWrapper.IsVersion2)
			{
				CorElementType elementType = HelperFunctions.GetElementTypeByName(TypeDefinition.FullName);

				if (elementType == CorElementType.ELEMENT_TYPE_END)
				{
					if (TypeDefinition.IsValueType)
					{
						elementType = CorElementType.ELEMENT_TYPE_VALUETYPE;
					}
					else
					{
						elementType = CorElementType.ELEMENT_TYPE_CLASS;
					}
				}

				result = classWrapper.Version2.GetParameterizedType((int)elementType, typeParameters);
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = null;

			if (IsParameterlessValueTypeConstructor)
			{
				ClassWrapper typeClass = HelperFunctions.FindClassOfTypeDefintion(context, TypeDefinition);
				context.EvalWrapper.NewObjectNoConstructor(typeClass);
				BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

				if (evaluationResult.IsSuccessful)
				{
					result = new DebugExpressionResult(context, evaluationResult.Result);
				}
				else
				{
					evaluationResult.ThrowExceptionAccordingToReason();
				}
			}
			else
			{
				TypeTreeNodeList typeWrapperArguments = null;

				if (TypeArgumentExpression != null)
				{
					typeWrapperArguments = TypeArgumentExpression.TypeArguments;
				}

				result = NextExpression.Evaluate(context, null, TypeDefinition, typeWrapperArguments);
			}

			return result;
		}
	}
}