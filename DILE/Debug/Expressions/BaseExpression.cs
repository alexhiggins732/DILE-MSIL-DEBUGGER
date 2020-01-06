using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Globalization;

namespace Dile.Debug.Expressions
{
	public abstract class BaseExpression
	{
		private string foundExpression;
		public string FoundExpression
		{
			get
			{
				return foundExpression;
			}
			private set
			{
				foundExpression = value;
			}
		}

		public virtual int FoundExpressionLength
		{
			get
			{
				return FoundExpression.Length;
			}
			protected set
			{
				throw new NotSupportedException();
			}
		}

		protected BaseExpression(string foundExpression)
		{
			FoundExpression = foundExpression;
		}

		private static IgnoredCharPair FindIgnoredCharPair(IgnoredCharPair[] ignoredCharPairs, char character)
		{
			IgnoredCharPair result = null;
			int index = 0;

			while (result == null && index < ignoredCharPairs.Length)
			{
				IgnoredCharPair ignoredCharPair = ignoredCharPairs[index];

				if (ignoredCharPair.IsStartCharacter(character))
				{
					result = ignoredCharPair;
				}
				else
				{
					index++;
				}
			}

			return result;
		}

		public static string GetMethodParameters(string expressionText)
		{
			return GetParameters(expressionText, '(', ')', false);
		}

		public static string GetArrayParameters(string expressionText)
		{
			return GetParameters(expressionText, '{', '}', false);
		}

		public static string GetOperatorParameters(string expressionText)
		{
			return GetParameters(expressionText, '(', ')', true);
		}

		private static string GetParameters(string expressionText, char startBracket, char endBracket, bool includeEndBracket)
		{
			int index = 0;
			int depth = 1;
			char? stringStartChar = null;
			bool foundChar = false;

			while (depth > 0 && index < expressionText.Length)
			{
				char character = expressionText[index];

				if (!stringStartChar.HasValue && (character == '"' || character == '\''))
				{
					stringStartChar = character;
					index++;
				}
				else if (stringStartChar.HasValue)
				{
					if (character == stringStartChar)
					{
						if (!foundChar && character == '\'')
						{
							throw new InvalidOperationException("A character expression must contain 1 character: " + expressionText);
						}

						stringStartChar = null;
						index++;
					}
					else if (character == '\\')
					{
						if (foundChar && stringStartChar == '\'')
						{
							throw new InvalidOperationException("A character expression cannot contain more than 1 character: " + expressionText);
						}

						int parsedCharLength;

						if (string.IsNullOrEmpty(HelperFunctions.ParseEscapedCharacter(expressionText, index, out parsedCharLength)))
						{
							throw new InvalidOperationException("Unrecognized escape sequence encountered in the string: " + expressionText);
						}
						else
						{
							index += parsedCharLength;
						}

						foundChar = true;
					}
					else
					{
						index++;
					}
				}
				else
				{
					if (character == startBracket)
					{
						depth++;
					}
					else if (character == endBracket)
					{
						depth--;

						if (includeEndBracket && depth == 0)
						{
							index++;
						}
					}

					if (depth > 0)
					{
						index++;
					}
				}
			}

			return expressionText.Substring(0, index);
		}

		public static string GetArrayParameterText(string arrayParametersText)
		{
			IgnoredCharPair[] ignoredCharPairs = new IgnoredCharPair[2];
			ignoredCharPairs[0] = new IgnoredCharPair('(', ')');
			ignoredCharPairs[1] = new IgnoredCharPair('<', '>');

			return GetParameterText(arrayParametersText, '{', '}', ',', ignoredCharPairs);
		}

		public static string GetMethodParameterText(string methodParametersText)
		{
			IgnoredCharPair[] ignoredCharPairs = new IgnoredCharPair[1];
			ignoredCharPairs[0] = new IgnoredCharPair('<', '>');

			return GetParameterText(methodParametersText, '(', ')', ',', ignoredCharPairs);
		}

		private static string GetParameterText(string parameters, char startBracket, char endBracket, char separator, IgnoredCharPair[] ignoredCharPairs)
		{
			int index = 0;
			int depth = 0;
			bool parameterEndFound = false;
			char? stringStartChar = null;
			bool foundChar = false;
			IgnoredCharPair foundIgnoredCharPair = null;
			int ignoredPartDepth = 0;
			bool foundIgnoredEndChar = false;

			while (!parameterEndFound && index < parameters.Length)
			{
				char character = parameters[index];

				if (foundIgnoredCharPair == null)
				{
					foundIgnoredCharPair = FindIgnoredCharPair(ignoredCharPairs, character);

					if (foundIgnoredCharPair != null)
					{
						ignoredPartDepth++;
						index++;
					}
				}
				else if (foundIgnoredCharPair != null)
				{
					if (foundIgnoredCharPair.IsEndCharacter(character))
					{
						ignoredPartDepth--;
						foundIgnoredEndChar = true;
						foundIgnoredCharPair = null;
					}
					else if (foundIgnoredCharPair.IsStartCharacter(character))
					{
						ignoredPartDepth++;
					}

					index++;
				}
				
				if (index < parameters.Length && ignoredPartDepth == 0 && !foundIgnoredEndChar)
				{
					if (!stringStartChar.HasValue && (character == '"' || character == '\''))
					{
						stringStartChar = character;
						index++;
					}
					else if (stringStartChar.HasValue)
					{
						if (character == stringStartChar)
						{
							if (!foundChar && character == '\'')
							{
								throw new InvalidOperationException("A character expression must contain 1 character: " + parameters);
							}

							stringStartChar = null;
							index++;
						}
						else if (character == '\\')
						{
							if (foundChar && stringStartChar == '\'')
							{
								throw new InvalidOperationException("A character expression cannot contain more than 1 character: " + parameters);
							}

							int parsedCharLength;

							if (string.IsNullOrEmpty(HelperFunctions.ParseEscapedCharacter(parameters, index, out parsedCharLength)))
							{
								throw new InvalidOperationException("Unrecognized escape sequence encountered in the string: " + parameters);
							}
							else
							{
								index += parsedCharLength;
							}

							foundChar = true;
						}
						else
						{
							index++;
						}
					}
					else
					{
						if (character == startBracket)
						{
							depth++;
							index++;
						}
						else if (character == endBracket)
						{
							depth--;
							index++;
						}
						else if (character == separator && depth == 0)
						{
							parameterEndFound = true;
						}
						else
						{
							index++;
						}
					}
				}

				foundIgnoredEndChar = false;
			}

			return parameters.Substring(0, index);
		}

		protected static void ParseNumber(string expressionText, out StringBuilder numberBuilder, out NumberStyles numberStyle, out string number)
		{
			numberBuilder = new StringBuilder();
			int charIndex = 0;
			bool digitFound = true;
			numberStyle = NumberStyles.Any;
			bool firstDotFound = false;

			while (digitFound && charIndex < expressionText.Length)
			{
				char character = expressionText[charIndex++];

				if (char.IsDigit(character))
				{
					numberBuilder.Append(character);
				}
				else if (character == '.' && !firstDotFound)
				{
					numberBuilder.Append(character);
					firstDotFound = true;
				}
				else if (character == 'x' && numberBuilder[0] == '0' && numberBuilder.Length == 1)
				{
					numberStyle = NumberStyles.HexNumber;
					numberBuilder.Append(character);
				}
				else
				{
					if (numberStyle == NumberStyles.HexNumber)
					{
						char lowerCharacter = char.ToLower(character);

						if (lowerCharacter >= 'a' && lowerCharacter <= 'f')
						{
							numberBuilder.Append(character);
						}
						else
						{
							digitFound = false;
						}
					}
					else
					{
						digitFound = false;
					}
				}
			}

			number = string.Empty;

			if (numberStyle == NumberStyles.HexNumber)
			{
				number = numberBuilder.ToString(2, numberBuilder.Length - 2);
			}
			else
			{
				number = numberBuilder.ToString();
			}
		}

		protected DebugExpressionResult ExecuteExpressionList(EvaluationContext context, DebugExpressionResult thisValue, List<BaseExpression> expressions)
		{
			return ExecuteExpressionList(context, thisValue, expressions, null);
		}

		protected DebugExpressionResult ExecuteExpressionList(EvaluationContext context, DebugExpressionResult thisValue, List<BaseExpression> expressions, TypeDefinition baseType)
		{
			DebugExpressionResult result = thisValue;

			if (expressions != null)
			{
				for (int index = 0; index < expressions.Count; index++)
				{
					BaseExpression expression = expressions[index];

					if (baseType == null)
					{
						result = expression.Evaluate(context, result);
					}
					else
					{
						MemberExpression memberExpression = expression as MemberExpression;

						if (memberExpression != null)
						{
							result = memberExpression.Evaluate(context, result, baseType, null);

							baseType = null;
						}
					}

					OperatorExpression operatorExpression = expression as OperatorExpression;

					if (operatorExpression != null)
					{
						while (!operatorExpression.IsEvaluationComplete)
						{
							result = operatorExpression.Evaluate(context, result);
						}
					}
				}
			}

			return result;
		}

		protected TypeTreeNodeList GetClassTypeArguments(EvaluationContext context, DebugExpressionResult thisValue)
		{
			TypeTreeNodeList result = null;

			if (thisValue.ResultValue.IsVersion2)
			{
				List<TypeWrapper> typeParameterWrappers = thisValue.ResultValue.Version2.GetExactType().EnumerateAllTypeParameters();

				if (typeParameterWrappers.Count > 0)
				{
					result = new TypeTreeNodeList();

					for (int index = 0; index < typeParameterWrappers.Count; index++)
					{
						TypeWrapper typeParameterWrapper = typeParameterWrappers[index];
						TypeTreeNode typeArgument = HelperFunctions.GetValueTypeTree(context, typeParameterWrapper);

						if (typeArgument == null)
						{
							CorElementType typeParameterElementType = (CorElementType)typeParameterWrapper.ElementType;
							TypeDefinition typeArgumentTypeDef = HelperFunctions.GetTypeByElementType(typeParameterElementType);
							bool isTypeParameterArray = HelperFunctions.IsArrayElementType(typeParameterElementType);

							typeArgument = new TypeTreeNode(typeArgumentTypeDef, isTypeParameterArray);
						}

						result.TypeTreeNodes.Add(typeArgument);
					}
				}
			}

			return result;
		}

		public virtual DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			return null;
		}
	}
}