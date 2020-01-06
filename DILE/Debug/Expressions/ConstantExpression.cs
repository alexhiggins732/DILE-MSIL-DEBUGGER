using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Dile.Debug.Expressions
{
	public class ConstantExpression <T> : BaseExpression
	{
		private const string NullName = "null";

		private static TypeDefinition booleanType;
		private static TypeDefinition BooleanType
		{
			get
			{
				if (booleanType == null)
				{
					booleanType = HelperFunctions.FindTypeByName(Constants.BooleanTypeName) as TypeDefinition;
				}

				return booleanType;
			}
		}

		private static TypeDefinition stringType;
		private static TypeDefinition StringType
		{
			get
			{
				if (stringType == null)
				{
					stringType = HelperFunctions.FindTypeByName(Constants.StringTypeName) as TypeDefinition;
				}

				return stringType;
			}
		}

		private static TypeDefinition charType;
		private static TypeDefinition CharType
		{
			get
			{
				if (charType == null)
				{
					charType = HelperFunctions.FindTypeByName(Constants.CharTypeName) as TypeDefinition;
				}

				return charType;
			}
		}

		private static TypeDefinition int32Type;
		private static TypeDefinition Int32Type
		{
			get
			{
				if (int32Type == null)
				{
					int32Type = HelperFunctions.FindTypeByName(Constants.Int32TypeName) as TypeDefinition;
				}

				return int32Type;
			}
		}

		private static TypeDefinition int64Type;
		private static TypeDefinition Int64Type
		{
			get
			{
				if (int64Type == null)
				{
					int64Type = HelperFunctions.FindTypeByName(Constants.Int64TypeName) as TypeDefinition;
				}

				return int64Type;
			}
		}

		private static TypeDefinition floatType;
		private static TypeDefinition FloatType
		{
			get
			{
				if (floatType == null)
				{
					floatType = HelperFunctions.FindTypeByName(Constants.FloatTypeName) as TypeDefinition;
				}

				return floatType;
			}
		}

		private static TypeDefinition doubleType;
		private static TypeDefinition DoubleType
		{
			get
			{
				if (doubleType == null)
				{
					doubleType = HelperFunctions.FindTypeByName(Constants.DoubleTypeName) as TypeDefinition;
				}

				return doubleType;
			}
		}

		private T constantValue;
		private T ConstantValue
		{
			get
			{
				return constantValue;
			}
			set
			{
				constantValue = value;
			}
		}

		private CorElementType constantValueType;
		private CorElementType ConstantValueType
		{
			get
			{
				return constantValueType;
			}
			set
			{
				constantValueType = value;
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

		static ConstantExpression()
		{
			Project.ProjectChanged += new EventHandler(ProjectChanged);
			Project.ProjectIsSavedChanged += new EventHandler(ProjectChanged);
		}

		private static void ProjectChanged(object sender, EventArgs e)
		{
			stringType = null;
			charType = null;
			int32Type = null;
			int64Type = null;
			floatType = null;
			doubleType = null;
		}

		public ConstantExpression(string foundExpression, T constantValue, CorElementType constantValueType, MemberExpression nextExpression) 
			: base(foundExpression)
		{
			ConstantValue = constantValue;
			ConstantValueType = constantValueType;
			NextExpression = nextExpression;
			FoundExpressionLength = FoundExpression.Length;

			if (NextExpression != null)
			{
				FoundExpressionLength += NextExpression.FoundExpressionLength + 1;
			}
		}

		private static string GetString(int startIndex, string expressionText, char endCharacter)
		{
			int index = startIndex;
			char previousChar = char.MinValue;
			bool endCharacterFound = false;

			while (!endCharacterFound && index < expressionText.Length)
			{
				char character = expressionText[index];

				if (previousChar != '\\' && character == endCharacter)
				{
					endCharacterFound = true;
				}
				else
				{
					previousChar = character;
					index++;
				}
			}

			if (!endCharacterFound)
			{
				index--;
			}

			return expressionText.Substring(startIndex,	index - startIndex);
		}

		public static BaseExpression TryParse(string expressionText)
		{
			BaseExpression result = null;
			char firstChar = expressionText[0];

			if (expressionText.StartsWith(NullName, StringComparison.InvariantCultureIgnoreCase) && (expressionText.Length == NullName.Length || !Char.IsLetterOrDigit(expressionText[NullName.Length])))
			{
				result = new ConstantExpression<object>(NullName, null, CorElementType.ELEMENT_TYPE_CLASS, null);
			}
			else if (firstChar == '\'')
			{
				string characterExpression = GetString(1, expressionText, '\'');
				string foundExpression = expressionText.Substring(0, characterExpression.Length + 2);

				MemberExpression nextExpression = null;
				if (expressionText.Length > foundExpression.Length + 1 && expressionText[foundExpression.Length] == '.')
				{
					nextExpression = MemberExpression.TryParse(expressionText.Substring(foundExpression.Length + 1));
				}

				result = new ConstantExpression<char>(foundExpression, char.Parse(characterExpression), CorElementType.ELEMENT_TYPE_CHAR, nextExpression);
			}
			else if (firstChar == '"')
			{
				string stringExpression = GetString(1, expressionText, '"');
				string foundExpression = expressionText.Substring(0, stringExpression.Length + 2);

				MemberExpression nextExpression = null;
				if (expressionText.Length > foundExpression.Length + 1 && expressionText[foundExpression.Length] == '.')
				{
					nextExpression = MemberExpression.TryParse(expressionText.Substring(foundExpression.Length + 1));
				}

				result = new ConstantExpression<string>(foundExpression, stringExpression, CorElementType.ELEMENT_TYPE_STRING, nextExpression);
			}
			else if (char.IsDigit(firstChar))
			{
				StringBuilder numberBuilder;
				NumberStyles numberStyle;
				string number;
				ParseNumber(expressionText, out numberBuilder, out numberStyle, out number);

				MemberExpression nextExpression = null;

				if (expressionText.Length > number.Length + 2 && expressionText[number.Length] == '.')
				{
					nextExpression = MemberExpression.TryParse(expressionText.Substring(number.Length + 1));
				}

				int intValue;

				if (int.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out intValue))
				{
					result = new ConstantExpression<int>(numberBuilder.ToString(), intValue, CorElementType.ELEMENT_TYPE_I4, nextExpression);
				}
				else
				{
					long longValue;

					if (long.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out longValue))
					{
						result = new ConstantExpression<long>(numberBuilder.ToString(), longValue, CorElementType.ELEMENT_TYPE_I8, nextExpression);
					}
					else if (numberStyle != NumberStyles.HexNumber)
					{
						float floatValue;

						if (float.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out floatValue))
						{
							result = new ConstantExpression<float>(number, floatValue, CorElementType.ELEMENT_TYPE_R4, nextExpression);
						}
						else
						{
							double doubleValue;

							if (double.TryParse(number, numberStyle, CultureInfo.InvariantCulture, out doubleValue))
							{
								result = new ConstantExpression<double>(number, doubleValue, CorElementType.ELEMENT_TYPE_R8, nextExpression);
							}
						}
					}
				}
			}
			else
			{
				bool? expressionValue = null;
				string expressionSubstring = string.Empty;

				if (expressionText.Length > 3)
				{
					expressionSubstring = expressionText.Substring(0, 4);

					if (expressionSubstring == "true")
					{
						expressionValue = true;
					}
					else if (expressionText.Length > 4)
					{
						expressionSubstring = expressionText.Substring(0, 5);

						if (expressionSubstring == "false")
						{
							expressionValue = false;
						}
					}
				}

				if (expressionValue.HasValue)
				{
					MemberExpression nextExpression = null;

					if (expressionText.Length > expressionSubstring.Length && expressionText[expressionSubstring.Length] == '.')
					{
						nextExpression = MemberExpression.TryParse(expressionText.Substring(5));
					}

					result = new ConstantExpression<bool>(expressionSubstring, expressionValue.Value, CorElementType.ELEMENT_TYPE_BOOLEAN, nextExpression);
				}
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = new DebugExpressionResult(context);
			TypeDefinition resultType = null;

			switch (ConstantValueType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					bool constantBool = (bool)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_BOOLEAN, null);
						result.ResultValue.SetGenericValue<bool>(constantBool);
					}
					else
					{
						resultType = BooleanType;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, BooleanType);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<bool>(constantBool);
					}
					break;

				case CorElementType.ELEMENT_TYPE_STRING:
					string constantString = (string)(object)ConstantValue;
					constantString = HelperFunctions.ConvertEscapedCharacters(constantString, false);

					context.EvalWrapper.NewString(constantString);

					BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);
					if (evaluationResult.IsSuccessful)
					{
						result.ResultValue = evaluationResult.Result;
					}
					else
					{
						evaluationResult.ThrowExceptionAccordingToReason();
					}

					resultType = StringType;
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					char constantChar = (char)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_CHAR, null);
						result.ResultValue.SetGenericValue<char>(constantChar);
					}
					else
					{
						resultType = CharType;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, CharType);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<char>(constantChar);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					int constantInt = (int)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_I4, null);
						result.ResultValue.SetGenericValue<int>(constantInt);
					}
					else
					{
						resultType = Int32Type;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, Int32Type);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<int>(constantInt);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					long constantLong = (long)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_I8, null);
						result.ResultValue.SetGenericValue<long>(constantLong);
					}
					else
					{
						resultType = Int64Type;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, Int64Type);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<long>(constantLong);
					}
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					float constantFloat = (float)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_R4, null);
						result.ResultValue.SetGenericValue<float>(constantFloat);
					}
					else
					{
						resultType = FloatType;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, FloatType);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<float>(constantFloat);
					}
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					double constantDouble = (double)(object)ConstantValue;

					if (NextExpression == null)
					{
						result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_R8, null);
						result.ResultValue.SetGenericValue<double>(constantDouble);
					}
					else
					{
						resultType = DoubleType;
						result.ResultValue = HelperFunctions.CreateBoxedValue(context, DoubleType);
						ValueWrapper dereferencedValue = result.ResultValue.DereferenceValue();
						ValueWrapper unboxedValue = dereferencedValue.UnboxValue();
						unboxedValue.SetGenericValue<double>(constantDouble);
					}
					break;

				case CorElementType.ELEMENT_TYPE_CLASS:
					result.ResultValue = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_CLASS, null);
					break;

				default:
					throw new EvaluationException(string.Format("Unable to create constant value of {0} type.", Enum.GetName(typeof(CorElementType), ConstantValueType)));
			}

			if (NextExpression != null)
			{
				result = NextExpression.Evaluate(context, result, resultType, null);
			}

			return result;
		}
	}
}