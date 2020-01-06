using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

//C++ operator precedence: ms-help://MS.VSCC.v80/MS.MSDN.v80/MS.VisualStudio.v80.en/dv_vclang/html/95c1f0ba-dad8-4034-b039-f79a904f112f.htm

namespace Dile.Debug.Expressions
{
	public class OperatorExpression : BaseExpression
	{
		private const string BracketOperator = "(";
		private const string EndBracketOperator = ")";

		private const string AddOperator = "+";
		private const string SubtractOperator = "-";
		private const string MultiplyOperator = "*";
		private const string DivideOperator = "/";
		private const string ModuloOperator = "%";

		private const string BinaryAndOperator = "&";
		private const string BinaryOrOperator = "|";
		private const string BinaryXorOperator = "^";
		private const string NotOperator = "!";
		private const string ComplementOperator = "~";

		private const string AndOperator = "&&";
		private const string OrOperator = "||";

		private const string ShiftLeftOperator = "<<";
		private const string ShiftRightOperator = ">>";

		private const string EqualOperator = "==";
		private const string NotEqualOperator = "!=";
		private const string LessThanOperator = "<";
		private const string GreaterThanOperator = ">";
		private const string LessThanOrEqualOperator = "<=";
		private const string GreaterThanOrEqualOperator = ">=";

		private static readonly string[] ShortOperators = new string[] { BracketOperator, EndBracketOperator, AddOperator, SubtractOperator, MultiplyOperator, DivideOperator, ModuloOperator, BinaryAndOperator, BinaryOrOperator, BinaryXorOperator, NotOperator, ComplementOperator, LessThanOperator, GreaterThanOperator };

		private static readonly string[] LongOperators = new string[] { AndOperator, OrOperator, ShiftLeftOperator, ShiftRightOperator, EqualOperator, NotEqualOperator, LessThanOrEqualOperator, GreaterThanOrEqualOperator };

		private static readonly object syncObject = new object();

		private string operatorText;
		private string OperatorText
		{
			get
			{
				return operatorText;
			}
			set
			{
				operatorText = value;
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

		private int precedence;
		public int Precedence
		{
			get
			{
				return precedence;
			}
			private set
			{
				precedence = value;
			}
		}

		private int evaluatedExpressionIndex = -1;
		private int EvaluatedExpressionIndex
		{
			get
			{
				return evaluatedExpressionIndex;
			}
			set
			{
				evaluatedExpressionIndex = value;
			}
		}

		public bool IsEvaluationComplete
		{
			get
			{
				return (EvaluatedExpressionIndex >= NextExpressions.Count);
			}
		}

		private bool? isOneOperand = null;
		private bool IsOneOperand
		{
			get
			{
				if (isOneOperand == null)
				{
					isOneOperand = IsOneOperandOperator(OperatorText);
				}

				return isOneOperand.Value;
			}
		}

		public bool IsEndBracket
		{
			get
			{
				return (OperatorText == EndBracketOperator);
			}
		}

		private static readonly Dictionary<string, DynamicMethod> operatorMethods = new Dictionary<string, DynamicMethod>();
		private static Dictionary<string, DynamicMethod> OperatorMethods
		{
			get
			{
				return operatorMethods;
			}
		}

		public OperatorExpression(string operatorText, List<BaseExpression> nextExpressions, string foundExpression)
			: base(foundExpression)
		{
			OperatorText = operatorText;
			NextExpressions = nextExpressions;

			Precedence = GetPrecedence(operatorText);

			if (OperatorText != BracketOperator && OperatorText != EndBracketOperator && NextExpressions != null && NextExpressions.Count > 0)
			{
				OperatorExpression childOperatorExpression = NextExpressions[0] as OperatorExpression;

				if (childOperatorExpression != null && (childOperatorExpression.OperatorText == AddOperator || childOperatorExpression.OperatorText == SubtractOperator))
				{
					childOperatorExpression.isOneOperand = true;
					childOperatorExpression.precedence = 11;
				}	
			}
		}

		private string GetOverloadedOperatorName(string operatorText)
		{
			string result = string.Empty;

			switch(operatorText)
			{
				case AddOperator:
					if (IsOneOperand)
					{
						result = "op_UnaryPlus";
					}
					else
					{
						result = "op_Addition";
					}
					break;

				case SubtractOperator:
					if (IsOneOperand)
					{
						result = "op_UnaryNegation";
					}
					else
					{
						result = "op_Subtraction";
					}
					break;

				case NotOperator:
					result = "op_LogicalNot";
					break;

				case ComplementOperator:
					result = "op_OnesComplement";
					break;

				case MultiplyOperator:
					result = "op_Multiply";
					break;

				case DivideOperator:
					result = "op_Division";
					break;

				case ModuloOperator:
					result = "op_Modulus";
					break;

				case BinaryAndOperator:
					result = "op_BitwiseAnd";
					break;

				case BinaryOrOperator:
					result = "op_BitwiseOr";
					break;

				case BinaryXorOperator:
					result = "op_ExclusiveOr";
					break;

				case ShiftLeftOperator:
					result = "op_LeftShift";
					break;

				case ShiftRightOperator:
					result = "op_RightShift";
					break;
			}

			return result;
		}

		private static int GetPrecedence(string operatorText)
		{
			int result = 0;

			switch (operatorText)
			{
				case BracketOperator:
				case EndBracketOperator:
					result = 12;
					break;

				case ComplementOperator:
				case NotOperator:
					result = 11;
					break;

				case MultiplyOperator:
				case DivideOperator:
				case ModuloOperator:
					result = 10;
					break;

				case AddOperator:
				case SubtractOperator:
					result = 9;
					break;

				case ShiftLeftOperator:
				case ShiftRightOperator:
					result = 8;
					break;

				case LessThanOperator:
				case GreaterThanOperator:
				case LessThanOrEqualOperator:
				case GreaterThanOrEqualOperator:
					result = 7;
					break;

				case EqualOperator:
				case NotEqualOperator:
					result = 6;
					break;

				case BinaryAndOperator:
					result = 5;
					break;

				case BinaryXorOperator:
					result = 4;
					break;

				case BinaryOrOperator:
					result = 3;
					break;

				case AndOperator:
					result = 2;
					break;

				case OrOperator:
					result = 1;
					break;

				default:
					throw new EvaluationException(string.Format("The precedence value of the {0} operator cannot be determined.", operatorText));
			}

			return result;
		}

		public static OperatorExpression TryParse(string expressionText)
		{
			OperatorExpression result = null;
			int operatorIndex = 0;
			string operatorText = string.Empty;

			if (expressionText.Length > 1)
			{
				operatorIndex = Array.IndexOf(LongOperators, expressionText.Substring(0, 2));

				if (operatorIndex > -1)
				{
					operatorText = LongOperators[operatorIndex];
				}
			}

			if (operatorText.Length == 0)
			{
				operatorIndex = Array.IndexOf(ShortOperators, expressionText.Substring(0, 1));

				if (operatorIndex > -1)
				{
					operatorText = ShortOperators[operatorIndex];
				}
			}

			if (operatorText.Length > 0)
			{
				string foundExpression = expressionText.Substring(operatorText.Length);
				bool isBracketOperator = (operatorText == BracketOperator);
				
				if (isBracketOperator)
				{
					foundExpression = BaseExpression.GetOperatorParameters(foundExpression);
				}

				int foundExpressionLength = foundExpression.Length + operatorText.Length;
				Parser parser = new Parser();
				List<BaseExpression> nextExpressions = parser.Parse(foundExpression);

				if (foundExpressionLength > expressionText.Length)
				{
					foundExpressionLength = expressionText.Length;
				}

				result = new OperatorExpression(operatorText, nextExpressions, expressionText.Substring(0, foundExpressionLength));
			}

			return result;
		}

		private bool OperandsUnordered(CorElementType leftValueType, CorElementType rightValueType)
		{
			return OperandsUnordered(leftValueType, rightValueType, false);
		}

		private bool OperandsUnordered(CorElementType leftValueType, CorElementType rightValueType, bool isRecursiveCall)
		{
			bool result = false;

			if ((leftValueType == CorElementType.ELEMENT_TYPE_R4 || leftValueType == CorElementType.ELEMENT_TYPE_R8) && (rightValueType != CorElementType.ELEMENT_TYPE_R4 || rightValueType != CorElementType.ELEMENT_TYPE_R8))
			{
				result = true;
			}
			else if (!isRecursiveCall)
			{
				result = OperandsUnordered(rightValueType, leftValueType, true);
			}

			return result;
		}

		private static bool IsOneOperandOperator(string operatorText)
		{
			bool result = false;

			switch (operatorText)
			{
				case NotOperator:
				case ComplementOperator:
					result = true;
					break;
			}

			return result;
		}

		private bool IsConditionalOperator()
		{
			bool result = false;

			switch (OperatorText)
			{
				case LessThanOperator:
				case GreaterThanOperator:
				case LessThanOrEqualOperator:
				case GreaterThanOrEqualOperator:
				case EqualOperator:
				case NotEqualOperator:
					result = true;
					break;
			}

			return result;
		}

		private bool IsLogicalOperator()
		{
			bool result = false;

			switch(OperatorText)
			{
				case AndOperator:
				case BinaryAndOperator:
				case OrOperator:
				case BinaryOrOperator:
				case BinaryXorOperator:
					result = true;
					break;
			}

			return result;
		}

		private List<OpCode> GetOperatorOpCodes(bool unorderedOperands)
		{
			List<OpCode> result = new List<OpCode>();

			switch (OperatorText)
			{
				case ComplementOperator:
					result.Add(OpCodes.Not);
					break;

				case MultiplyOperator:
					result.Add(OpCodes.Mul);
					break;

				case DivideOperator:
					result.Add(OpCodes.Div);
					break;

				case ModuloOperator:
					result.Add(OpCodes.Rem);
					break;

				case AddOperator:
					result.Add(OpCodes.Add);
					break;

				case SubtractOperator:
					result.Add(OpCodes.Sub);
					break;

				case ShiftLeftOperator:
					result.Add(OpCodes.Shl);
					break;

				case ShiftRightOperator:
					result.Add(OpCodes.Shr);
					break;

				case LessThanOperator:
					if (unorderedOperands)
					{
						result.Add(OpCodes.Clt_Un);
					}
					else
					{
						result.Add(OpCodes.Clt);
					}
					break;

				case GreaterThanOperator:
					if (unorderedOperands)
					{
						result.Add(OpCodes.Cgt_Un);
					}
					else
					{
						result.Add(OpCodes.Cgt);
					}
					break;

				case LessThanOrEqualOperator:
					if (unorderedOperands)
					{
						result.Add(OpCodes.Cgt_Un);
					}
					else
					{
						result.Add(OpCodes.Cgt);
					}

					result.Add(OpCodes.Ldc_I4_0);
					result.Add(OpCodes.Ceq);
					break;

				case GreaterThanOrEqualOperator:
					if (unorderedOperands)
					{
						result.Add(OpCodes.Clt_Un);
					}
					else
					{
						result.Add(OpCodes.Clt);
					}

					result.Add(OpCodes.Ldc_I4_0);
					result.Add(OpCodes.Ceq);
					break;

				case EqualOperator:
					result.Add(OpCodes.Ceq);
					break;

				case NotEqualOperator:
					result.Add(OpCodes.Ceq);
					result.Add(OpCodes.Ldc_I4_0);
					result.Add(OpCodes.Ceq);
					break;

				case BinaryAndOperator:
					result.Add(OpCodes.And);
					break;

				case BinaryXorOperator:
					result.Add(OpCodes.Xor);
					break;

				case BinaryOrOperator:
					result.Add(OpCodes.Or);
					break;

				case NotOperator:
				default:
					throw new EvaluationException(string.Format("The operator ({0}) has no corresponding OpCode.", OperatorText));
			}

			return result;
		}

		private ValueWrapper ExecuteOperator<T>(EvaluationContext context, int resultValueType, ValueWrapper operandValue)
		{
			ValueWrapper result = null;
			DynamicMethod operatorMethod = null;
			string operatorMethodName = string.Format("{0}_{1}", typeof(T), OperatorText);

			if (OperatorMethods.ContainsKey(operatorMethodName))
			{
				operatorMethod = OperatorMethods[operatorMethodName];
			}
			else
			{
				lock (syncObject)
				{
					if (OperatorMethods.ContainsKey(operatorMethodName))
					{
						operatorMethod = OperatorMethods[operatorMethodName];
					}
					else
					{
						operatorMethod = new DynamicMethod(string.Empty, typeof(T), new Type[] { typeof(T) }, typeof(OperatorExpression));
						ILGenerator ilGenerator = operatorMethod.GetILGenerator();
						ilGenerator.Emit(OpCodes.Ldarg_0);

						List<OpCode> operatorCodes = GetOperatorOpCodes(false);
						for (int index = 0; index < operatorCodes.Count; index++)
						{
							ilGenerator.Emit(operatorCodes[index]);
						}

						ilGenerator.Emit(OpCodes.Ret);

						OperatorMethods[operatorMethodName] = operatorMethod;
					}
				}
			}

			T operationResult = (T)operatorMethod.Invoke(this, new object[] { operandValue.GetGenericValue<T>() });

			result = context.EvalWrapper.CreateValue((int)resultValueType, null);
			result.SetGenericValue<T>(operationResult);

			return result;
		}

		private ValueWrapper ExecuteOperator<X, Y, Z>(EvaluationContext context, int resultValueType, ValueWrapper leftValue, ValueWrapper rightValue, Nullable<OpCode> extraOpCode1, Nullable<OpCode> extraOpCode2)
		{
			ValueWrapper result = null;
			DynamicMethod operatorMethod = null;
			string operatorMethodName = string.Empty;
			bool unorderedOperands = false;

			if (IsConditionalOperator())
			{
				unorderedOperands = OperandsUnordered((CorElementType)leftValue.ElementType, (CorElementType)rightValue.ElementType);
				operatorMethodName = string.Format("{0}_{1}_{2}_{3}_{4}", typeof(X), typeof(Y), typeof(Z), OperatorText, unorderedOperands);
			}
			else
			{
				operatorMethodName = string.Format("{0}_{1}_{2}_{3}", typeof(X), typeof(Y), typeof(Z), OperatorText);
			}

			if (OperatorMethods.ContainsKey(operatorMethodName))
			{
				operatorMethod = OperatorMethods[operatorMethodName];
			}
			else
			{
				lock (syncObject)
				{
					if (OperatorMethods.ContainsKey(operatorMethodName))
					{
						operatorMethod = OperatorMethods[operatorMethodName];
					}
					else
					{
						operatorMethod = new DynamicMethod(string.Empty, typeof(X), new Type[] { typeof(Y), typeof(Z) }, typeof(OperatorExpression));
						ILGenerator ilGenerator = operatorMethod.GetILGenerator();
						ilGenerator.Emit(OpCodes.Ldarg_0);

						if (extraOpCode1 != null)
						{
							ilGenerator.Emit(extraOpCode1.Value);
						}

						ilGenerator.Emit(OpCodes.Ldarg_1);

						if (extraOpCode2 != null)
						{
							ilGenerator.Emit(extraOpCode2.Value);
						}

						List<OpCode> operatorCodes = GetOperatorOpCodes(unorderedOperands);
						for (int index = 0; index < operatorCodes.Count; index++)
						{
							ilGenerator.Emit(operatorCodes[index]);
						}

						ilGenerator.Emit(OpCodes.Ret);

						OperatorMethods[operatorMethodName] = operatorMethod;
					}
				}
			}

			X operationResult = (X)operatorMethod.Invoke(this, new object[] { leftValue.GetGenericValue<Y>(), rightValue.GetGenericValue<Z>() });

			result = context.EvalWrapper.CreateValue((int)resultValueType, null);
			result.SetGenericValue<X>(operationResult);

			return result;
		}

		private ValueWrapper ExecuteOperator<T>(EvaluationContext context, ValueWrapper leftValue, ValueWrapper rightValue, Nullable<OpCode> extraOpCode2)
		{
			ValueWrapper result = null;
			Nullable<OpCode> extraOpCode1 = null;

			switch ((CorElementType)rightValue.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, bool>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, bool>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, char>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, char>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, sbyte>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, sbyte>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, byte>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, byte>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, short>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, short>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, ushort>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, ushort>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, int>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, int>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, uint>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, uint>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, long>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, long>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, ulong>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, ulong>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					if (extraOpCode2 == null)
					{
						extraOpCode1 = OpCodes.Conv_R4;

						if (IsConditionalOperator())
						{
							result = ExecuteOperator<bool, T, float>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
						else
						{
							result = ExecuteOperator<float, T, float>(context, (int)CorElementType.ELEMENT_TYPE_R4, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
					}
					else
					{
						extraOpCode2 = null;
						extraOpCode1 = null;

						if (IsConditionalOperator())
						{
							result = ExecuteOperator<bool, T, float>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
						else
						{
							result = ExecuteOperator<T, T, float>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
					}
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					if (extraOpCode2 == null)
					{
						extraOpCode1 = OpCodes.Conv_R8;

						if (IsConditionalOperator())
						{
							result = ExecuteOperator<bool, T, double>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
						else
						{
							result = ExecuteOperator<double, T, double>(context, (int)CorElementType.ELEMENT_TYPE_R8, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
					}
					else
					{
						extraOpCode2 = null;
						extraOpCode1 = null;

						if (IsConditionalOperator())
						{
							result = ExecuteOperator<bool, T, double>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
						else
						{
							result = ExecuteOperator<T, T, double>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
						}
					}
					break;

				case CorElementType.ELEMENT_TYPE_I:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, int>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, int>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				case CorElementType.ELEMENT_TYPE_U:
					if (IsConditionalOperator())
					{
						result = ExecuteOperator<bool, T, uint>(context, (int)CorElementType.ELEMENT_TYPE_BOOLEAN, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					else
					{
						result = ExecuteOperator<T, T, uint>(context, leftValue.ElementType, leftValue, rightValue, extraOpCode1, extraOpCode2);
					}
					break;

				default:
					throw new EvaluationException(string.Format("The {0} operator cannot be evaluated on a(n) {1} value.", OperatorText, Enum.GetName(typeof(CorElementType), rightValue.ElementType)));
			}

			return result;
		}

		private ValueWrapper ExecuteOverloadedOperator(EvaluationContext context, ValueWrapper leftValue, ValueWrapper rightValue)
		{
			ValueWrapper result = null;

			TypeDefinition leftValueTypeDef = HelperFunctions.FindTypeOfValue(context, new DebugExpressionResult(context, leftValue));

			if (leftValueTypeDef == null)
			{
				throw new EvaluationException(string.Format("The definition of the type ({0}) could not be found.", Enum.GetName(typeof(CorElementType), leftValue.ElementType)));
			}

			string operatorName = GetOverloadedOperatorName(OperatorText);
			MethodDefinition operatorMethodDef = null;
			List<ValueWrapper> functionArguments = new List<ValueWrapper>();

			if (!string.IsNullOrEmpty(operatorName))
			{
				List<DebugExpressionResult> parameters = new List<DebugExpressionResult>();
				parameters.Add(new DebugExpressionResult(context, leftValue));
				functionArguments.Add(leftValue);

				if (!IsOneOperand)
				{
					parameters.Add(new DebugExpressionResult(context, rightValue));
					functionArguments.Add(rightValue);
				}

				operatorMethodDef = leftValueTypeDef.FindMethodDefinitionByParameter(context, operatorName, null, null, parameters);
			}

			if (operatorMethodDef == null)
			{
				if (IsLogicalOperator())
				{
					if (leftValue == null)
					{
						throw new EvaluationException(string.Format("The left value of the logical operator ({0}) cannot be null.", OperatorText));
					}

					if (rightValue == null)
					{
						throw new EvaluationException(string.Format("The right value of the logical operator ({0}) cannot be null.", OperatorText));
					}

					bool leftValueBool = (leftValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN);
					bool rightValueBool = (rightValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN);
					TypeDefinition boolTypeDef = HelperFunctions.FindTypeByName(Constants.BooleanTypeName, Constants.MscorlibName);

					if (boolTypeDef == null)
					{
						throw new EvaluationException("The type definion of System.Boolean type could not be found. Please, add the mscorlib.dll assembly to the DILE project.");
					}

					bool convertLeftValueToBool = false;
					bool convertRightValueToBool = false;

					if (!leftValueBool && !rightValueBool)
					{
						if (OperatorText == AndOperator)
						{
							ValueWrapper falseValue = ExecuteFalseOperator(context, leftValue, boolTypeDef);

							if (falseValue == null)
							{
								OperatorText = BinaryAndOperator;
								result = ExecuteOverloadedOperator(context, leftValue, rightValue);
							}
							else
							{
								if (falseValue.GetGenericValue<bool>())
								{
									OperatorText = BinaryAndOperator;
									result = ExecuteOverloadedOperator(context, leftValue, rightValue);
								}
								else
								{
									result = falseValue;
								}
							}
						}
						else if (OperatorText == OrOperator)
						{
							ValueWrapper trueValue = ExecuteTrueOperator(context, leftValue, boolTypeDef);

							if (trueValue == null)
							{
								OperatorText = BinaryOrOperator;
								result = ExecuteOverloadedOperator(context, leftValue, rightValue);
							}
							else
							{
								if (trueValue.GetGenericValue<bool>())
								{
									result = trueValue;
								}
								else
								{
									OperatorText = BinaryOrOperator;
									result = ExecuteOverloadedOperator(context, leftValue, rightValue);
								}
							}
						}
						else
						{
							convertLeftValueToBool = true;
							convertRightValueToBool = true;
						}
					}
					else
					{
						if (leftValueBool)
						{
							if (!HelperFunctions.HasValueClass(rightValue))
							{
								throw new EvaluationException("The right value cannot be converted to boolean type.");
							}

							convertRightValueToBool = true;
						}
						else
						{
							if (!HelperFunctions.HasValueClass(leftValue))
							{
								throw new EvaluationException("The left value cannot be converted to boolean type.");
							}

							convertLeftValueToBool = true;
						}
					}

					if (convertLeftValueToBool)
					{
						leftValue = ExecuteBoolConversionOperator(context, leftValue, boolTypeDef);
					}

					if (convertRightValueToBool)
					{
						rightValue = ExecuteBoolConversionOperator(context, rightValue, boolTypeDef);
					}

					if (convertLeftValueToBool || convertRightValueToBool)
					{
						result = ExecuteOperator(context, leftValue, rightValue);
					}
				}
				else
				{
					throw new EvaluationException(string.Format("No overloaded operator method could be found on the {0} type for the {1} operator.", leftValueTypeDef.FullName, OperatorText));
				}
			}
			else
			{
				BaseEvaluationResult evalResult = context.EvaluationHandler.CallMethod(context, operatorMethodDef, functionArguments);

				if (evalResult.IsSuccessful)
				{
					result = evalResult.Result;
				}
				else
				{
					evalResult.ThrowExceptionAccordingToReason();
				}
			}

			return result;
		}

		private static ValueWrapper ExecuteTrueOperator(EvaluationContext context, ValueWrapper valueWrapper, TypeDefinition boolTypeDef)
		{
			return ExecuteTrueFalseOperator(context, valueWrapper, boolTypeDef, true);
		}

		private static ValueWrapper ExecuteFalseOperator(EvaluationContext context, ValueWrapper valueWrapper, TypeDefinition boolTypeDef)
		{
			return ExecuteTrueFalseOperator(context, valueWrapper, boolTypeDef, false);
		}

		private static ValueWrapper ExecuteTrueFalseOperator(EvaluationContext context, ValueWrapper valueWrapper, TypeDefinition boolTypeDef, bool executeTrueOperator)
		{
			DebugExpressionResult valueParameter = new DebugExpressionResult(context, valueWrapper);
			TypeDefinition valueTypeDefinition = HelperFunctions.FindTypeOfValue(context, valueParameter);

			if (valueTypeDefinition == null)
			{
				throw new EvaluationException("The type definition of a value could not be found while searching for overloaded true/false operator.");
			}

			List<DebugExpressionResult> parameters = new List<DebugExpressionResult>(1);
			parameters.Add(valueParameter);

			string operatorMethodName = (executeTrueOperator ? "op_True" : "op_False");
			MethodDefinition operatorMethodDef = valueTypeDefinition.FindMethodDefinitionByParameter(context, operatorMethodName, null, null, parameters, boolTypeDef, false);

			if (operatorMethodDef != null)
			{
				List<ValueWrapper> arguments = new List<ValueWrapper>(1);
				arguments.Add(valueWrapper);

				BaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, operatorMethodDef, arguments);

				if (evaluationResult.IsSuccessful)
				{
					valueWrapper = evaluationResult.Result;
				}
				else
				{
					evaluationResult.ThrowExceptionAccordingToReason();
				}
			}

			return valueWrapper;
		}

		private static ValueWrapper ExecuteBoolConversionOperator(EvaluationContext context, ValueWrapper valueWrapper, TypeDefinition boolTypeDef)
		{
			DebugExpressionResult valueParameter = new DebugExpressionResult(context, valueWrapper);
			TypeDefinition valueTypeDefinition = HelperFunctions.FindTypeOfValue(context, valueParameter);

			if (valueTypeDefinition == null)
			{
				throw new EvaluationException("The type definition of a value could not be found while searching for overloaded boolean cast operator.");
			}

			List<DebugExpressionResult> parameters = new List<DebugExpressionResult>(1);
			parameters.Add(valueParameter);

			MethodDefinition implicitBoolOperator = valueTypeDefinition.FindMethodDefinitionByParameter(context, Constants.ImplicitOperatorMethodName, null, null, parameters, boolTypeDef, false);
			MethodDefinition explicitBoolOperator = valueTypeDefinition.FindMethodDefinitionByParameter(context, Constants.ExplicitOperatorMethodName, null, null, parameters, boolTypeDef, false);

			if (implicitBoolOperator == null && explicitBoolOperator == null)
			{
				throw new EvaluationException(string.Format("Neither explicit nor implicit cast operator has been found on the {0} type for casting a value to System.Boolean.", valueTypeDefinition.FullName));
			}

			if (implicitBoolOperator != null && explicitBoolOperator != null)
			{
				throw new EvaluationException(string.Format("Both an explicit and an implicit cast operator is defined on the type ({0}) that could be used for casting a value to System.Boolean.", valueTypeDefinition.FullName));
			}

			List<ValueWrapper> arguments = new List<ValueWrapper>(1);
			arguments.Add(valueWrapper);

			BaseEvaluationResult evaluationResult = null;

			if (implicitBoolOperator != null)
			{
				evaluationResult = context.EvaluationHandler.CallMethod(context, implicitBoolOperator, arguments);
			}
			else
			{
				evaluationResult = context.EvaluationHandler.CallMethod(context, explicitBoolOperator, arguments);
			}

			if (evaluationResult.IsSuccessful)
			{
				valueWrapper = evaluationResult.Result;
			}
			else
			{
				evaluationResult.ThrowExceptionAccordingToReason();
			}

			return valueWrapper;
		}

		private bool IsString(EvaluationContext context, ValueWrapper valueWrapper)
		{
			bool result = false;

			if (valueWrapper.ElementType == (int)CorElementType.ELEMENT_TYPE_STRING)
			{
				result = true;
			}
			else
			{
				TypeDefinition valueTypeDefinition = HelperFunctions.FindTypeOfValue(context, new DebugExpressionResult(context, valueWrapper));

				if (valueTypeDefinition != null && valueTypeDefinition.FullName == Constants.StringTypeName)
				{
					result = true;
				}
			}

			return result;
		}

		private ValueWrapper ExecuteOperator(EvaluationContext context, ValueWrapper leftValue, ValueWrapper rightValue)
		{
			ValueWrapper result = null;
			bool leftValueHasClass = HelperFunctions.HasValueClass(leftValue);
			bool rightValueHasClass = HelperFunctions.HasValueClass(rightValue);
			bool isLeftValueString = (leftValueHasClass ? IsString(context, leftValue) : false);
			bool isRightValueString = (rightValueHasClass ? IsString(context, rightValue) : false);

			if (leftValue == null && (OperatorText == AddOperator || OperatorText == SubtractOperator))
			{
				isOneOperand = true;
			}

			if (IsOneOperand && rightValueHasClass)
			{
				result = ExecuteOverloadedOperator(context, rightValue, null);
			}
			else if (IsOneOperand && (OperatorText == AddOperator || OperatorText == SubtractOperator))
			{
				result = ExecuteUnaryOperator(context, rightValue);
			}
			else if ((isLeftValueString || isRightValueString) && OperatorText == AddOperator)
			{
				result = ConcatenateValues(context, isLeftValueString, isRightValueString, leftValue, rightValue);
			}
			else if (leftValueHasClass)
			{
				result = ExecuteOverloadedOperator(context, leftValue, rightValue);
			}
			else
			{
				switch (OperatorText)
				{
					case EndBracketOperator:
						result = leftValue;
						break;

					case NotOperator:
						if (rightValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN)
						{
							bool boolValue = rightValue.GetGenericValue<bool>();
							result = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_BOOLEAN, null);
							result.SetGenericValue<bool>(!boolValue);
						}
						else
						{
							throw new EvaluationException("Not operator can be evaluated only on boolean values.");
						}
						break;

					case ComplementOperator:
						result = ExecuteComplementOperator(context, rightValue, result);
						break;

					case AndOperator:
						result = ExecuteAndOperator(context, leftValue, rightValue, result);
						break;

					case OrOperator:
						result = ExecuteOrOperator(context, leftValue, rightValue, result);
						break;

					case MultiplyOperator:
					case DivideOperator:
					case ModuloOperator:
					case AddOperator:
					case SubtractOperator:
					case ShiftLeftOperator:
					case ShiftRightOperator:
					case BinaryAndOperator:
					case BinaryXorOperator:
					case BinaryOrOperator:
					case LessThanOperator:
					case GreaterThanOperator:
					case LessThanOrEqualOperator:
					case GreaterThanOrEqualOperator:
					case EqualOperator:
					case NotEqualOperator:
						switch ((CorElementType)leftValue.ElementType)
						{
							case CorElementType.ELEMENT_TYPE_BOOLEAN:
								result = ExecuteOperator<bool>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_CHAR:
								result = ExecuteOperator<char>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_I1:
								result = ExecuteOperator<sbyte>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_U1:
								result = ExecuteOperator<byte>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_I2:
								result = ExecuteOperator<short>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_U2:
								result = ExecuteOperator<ushort>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_I4:
								result = ExecuteOperator<int>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_U4:
								result = ExecuteOperator<uint>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_I8:
								result = ExecuteOperator<long>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_U8:
								result = ExecuteOperator<ulong>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_R4:
								result = ExecuteOperator<float>(context, leftValue, rightValue, OpCodes.Conv_R4);
								break;

							case CorElementType.ELEMENT_TYPE_R8:
								result = ExecuteOperator<double>(context, leftValue, rightValue, OpCodes.Conv_R8);
								break;

							case CorElementType.ELEMENT_TYPE_I:
								result = ExecuteOperator<int>(context, leftValue, rightValue, null);
								break;

							case CorElementType.ELEMENT_TYPE_U:
								result = ExecuteOperator<uint>(context, leftValue, rightValue, null);
								break;

							default:
								throw new EvaluationException(string.Format("Unable to evaluate {0} operator on a {1} value.", OperatorText, Enum.GetName(typeof(CorElementType), leftValue.ElementType)));
						}
						break;

					default:
						throw new EvaluationException(string.Format("Unable to evaluate {0} operator.", OperatorText));
				}
			}

			return result;
		}

		private ValueWrapper ExecuteUnaryOperator(EvaluationContext context, ValueWrapper valueWrapper)
		{
			ValueWrapper result = valueWrapper;

			if (OperatorText == SubtractOperator)
			{
				result = context.EvalWrapper.CreateValue(valueWrapper.ElementType, null);

				switch ((CorElementType)valueWrapper.ElementType)
				{
					case CorElementType.ELEMENT_TYPE_CHAR:
						result.SetGenericValue<char>((char)(-1 * valueWrapper.GetGenericValue<char>()));
						break;

					case CorElementType.ELEMENT_TYPE_I1:
						result.SetGenericValue<sbyte>((sbyte)(-1 * valueWrapper.GetGenericValue<sbyte>()));
						break;

					case CorElementType.ELEMENT_TYPE_I2:
						result.SetGenericValue<short>((short)(-1 * valueWrapper.GetGenericValue<short>()));
						break;

					case CorElementType.ELEMENT_TYPE_I4:
						result.SetGenericValue<int>(-1 * valueWrapper.GetGenericValue<int>());
						break;

					case CorElementType.ELEMENT_TYPE_I8:
						result.SetGenericValue<long>(-1 * valueWrapper.GetGenericValue<long>());
						break;

					case CorElementType.ELEMENT_TYPE_R4:
						result.SetGenericValue<float>(-1 * valueWrapper.GetGenericValue<float>());
						break;

					case CorElementType.ELEMENT_TYPE_R8:
						result.SetGenericValue<double>(-1 * valueWrapper.GetGenericValue<double>());
						break;

					case CorElementType.ELEMENT_TYPE_I:
						result.SetGenericValue<int>(-1 * valueWrapper.GetGenericValue<int>());
						break;

					default:
						throw new EvaluationException(string.Format("Unable to evaluate {0} operator on a {1} value.", OperatorText, Enum.GetName(typeof(CorElementType), valueWrapper.ElementType)));
				}
			}

			return result;
		}

		private string ConvertValueTypeToString(ValueWrapper valueWrapper)
		{
			string result = string.Empty;

			if (valueWrapper.IsBoxedValue())
			{
				valueWrapper = valueWrapper.UnboxValue();
			}

			switch((CorElementType)valueWrapper.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_BOOLEAN:
					result = valueWrapper.GetGenericValue<bool>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_CHAR:
					result = valueWrapper.GetGenericValue<char>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					result = valueWrapper.GetGenericValue<sbyte>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					result = valueWrapper.GetGenericValue<byte>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					result = valueWrapper.GetGenericValue<short>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					result = valueWrapper.GetGenericValue<ushort>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					result = valueWrapper.GetGenericValue<int>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					result = valueWrapper.GetGenericValue<uint>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					result = valueWrapper.GetGenericValue<long>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					result = valueWrapper.GetGenericValue<ulong>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_I:
					result = valueWrapper.GetGenericValue<int>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_U:
					result = valueWrapper.GetGenericValue<uint>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					result = valueWrapper.GetGenericValue<float>().ToString();
					break;

				case CorElementType.ELEMENT_TYPE_R8:
					result = valueWrapper.GetGenericValue<double>().ToString();
					break;

				default:
					throw new EvaluationException(string.Format("Unable to cast a {0} value to string.", Enum.GetName(typeof(CorElementType), valueWrapper.ElementType)));
			}

			return result;
		}

		private string ConvertToString(EvaluationContext context, ValueWrapper valueWrapper)
		{
			string result = string.Empty;
			DebugExpressionResult valueExpressionResult = new DebugExpressionResult(context, valueWrapper);
			TypeDefinition valueTypeDefinition = null;
			bool valueHasClass = HelperFunctions.HasValueClass(valueWrapper);

			if (valueHasClass)
			{
				valueTypeDefinition = HelperFunctions.FindTypeOfValue(context, valueExpressionResult);
			}
			else
			{
				valueTypeDefinition = HelperFunctions.GetTypeByElementType((CorElementType)valueWrapper.ElementType);
			}

			if (valueTypeDefinition == null)
			{
				throw new EvaluationException("The type definition of a value could not be found while trying to cast it to string.");
			}

			if (valueHasClass)
			{
				TypeDefinition stringTypeDefinition = HelperFunctions.FindTypeByName(Constants.StringTypeName, Constants.MscorlibName);

				if (stringTypeDefinition == null)
				{
					throw new EvaluationException("The type definion of System.String type could not be found. Please, add the mscorlib.dll assembly to the DILE project.");
				}

				ValueWrapper stringValueWrapper = null;
				MethodDefinition conversionMethodDefinition = stringTypeDefinition.FindImplicitCastOperator(context, valueExpressionResult, stringTypeDefinition, false);

				if (conversionMethodDefinition == null)
				{
					List<DebugExpressionResult> parameters = new List<DebugExpressionResult>(1);
					parameters.Add(valueExpressionResult);

					conversionMethodDefinition = valueTypeDefinition.FindMethodDefinitionByParameter(context, Constants.ToStringMethodName, null, null, parameters, stringTypeDefinition, false);

					if (conversionMethodDefinition == null)
					{
						throw new EvaluationException(string.Format("Neither implicit string cast operator nor the ToString() method has been found on the following type: {0}", valueTypeDefinition.FullName));
					}
				}

				List<ValueWrapper> arguments = new List<ValueWrapper>(1);
				arguments.Add(valueWrapper);

				BaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, conversionMethodDefinition, arguments);

				if (evaluationResult.IsSuccessful)
				{
					stringValueWrapper = evaluationResult.Result;
				}
				else
				{
					evaluationResult.ThrowExceptionAccordingToReason();
				}

				result = GetStringValue(stringValueWrapper);
			}
			else
			{
				result = ConvertValueTypeToString(valueWrapper);
			}

			return result;
		}

		private string GetStringValue(ValueWrapper stringValueWrapper)
		{
			string result = string.Empty;

			if (stringValueWrapper != null && !stringValueWrapper.IsNull())
			{
				ValueWrapper dereferencedString = stringValueWrapper.DereferenceValue();

				if (dereferencedString != null)
				{
					result = dereferencedString.GetStringValue();
				}
			}

			return result;
		}

		private ValueWrapper ConcatenateValues(EvaluationContext context, bool isLeftValueString, bool isRightValueString, ValueWrapper leftValue, ValueWrapper rightValue)
		{
			ValueWrapper result = null;
			string resultString = string.Empty;

			if (isLeftValueString)
			{
				resultString = GetStringValue(leftValue);

				if (isRightValueString)
				{
					resultString += GetStringValue(rightValue);
				}
				else
				{
					resultString += ConvertToString(context, rightValue);
				}
			}
			else if (isRightValueString)
			{
				resultString = ConvertToString(context, leftValue);
				resultString += GetStringValue(rightValue);
			}

			context.EvalWrapper.NewString(resultString);
			BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

			if (evaluationResult.IsSuccessful)
			{
				result = evaluationResult.Result;
			}
			else
			{
				evaluationResult.ThrowExceptionAccordingToReason();
			}

			return result;
		}

		private static ValueWrapper ExecuteOrOperator(EvaluationContext context, ValueWrapper leftValue, ValueWrapper rightValue, ValueWrapper result)
		{
			if (leftValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN && rightValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN)
			{
				bool leftBoolValue = leftValue.GetGenericValue<bool>();
				bool rightBoolValue = rightValue.GetGenericValue<bool>();
				result = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_BOOLEAN, null);

				result.SetGenericValue<bool>(leftBoolValue || rightBoolValue);
			}
			else
			{
				throw new EvaluationException("The || operator can be executed only on two boolean values.");
			}

			return result;
		}

		private static ValueWrapper ExecuteAndOperator(EvaluationContext context, ValueWrapper leftValue, ValueWrapper rightValue, ValueWrapper result)
		{
			if (leftValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN && rightValue.ElementType == (int)CorElementType.ELEMENT_TYPE_BOOLEAN)
			{
				bool leftBoolValue = leftValue.GetGenericValue<bool>();
				bool rightBoolValue = rightValue.GetGenericValue<bool>();
				result = context.EvalWrapper.CreateValue((int)CorElementType.ELEMENT_TYPE_BOOLEAN, null);

				result.SetGenericValue<bool>(leftBoolValue && rightBoolValue);
			}
			else
			{
				throw new EvaluationException("The && operator can be executed only on two boolean values.");
			}

			return result;
		}

		private ValueWrapper ExecuteComplementOperator(EvaluationContext context, ValueWrapper rightValue, ValueWrapper result)
		{
			switch ((CorElementType)rightValue.ElementType)
			{
				case CorElementType.ELEMENT_TYPE_I:
					result = ExecuteOperator<int>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					result = ExecuteOperator<sbyte>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					result = ExecuteOperator<short>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					result = ExecuteOperator<int>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_I8:
					result = ExecuteOperator<long>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_U:
					result = ExecuteOperator<uint>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					result = ExecuteOperator<byte>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					result = ExecuteOperator<ushort>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					result = ExecuteOperator<uint>(context, rightValue.ElementType, rightValue);
					break;

				case CorElementType.ELEMENT_TYPE_U8:
					result = ExecuteOperator<ulong>(context, rightValue.ElementType, rightValue);
					break;

				default:
					throw new EvaluationException(string.Format("The ~ operator cannot be evaluated on a(n) {0} value.", Enum.GetName(typeof(CorElementType), rightValue.ElementType)));
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = thisValue;

			if (NextExpressions != null && NextExpressions.Count > 0)
			{
				if (OperatorText == EndBracketOperator)
				{
					if (EvaluatedExpressionIndex <= -1)
					{
						EvaluatedExpressionIndex = 0;
					}
					else
					{
						int index = 0;

						while (index < NextExpressions.Count)
						{
							BaseExpression expression = NextExpressions[index++];
							OperatorExpression operatorExpression = expression as OperatorExpression;

							if (operatorExpression == null)
							{
								result = expression.Evaluate(context, result);
							}
							else
							{
								while (!operatorExpression.IsEvaluationComplete)
								{
									result = operatorExpression.Evaluate(context, result);
								}
							}
						}

						EvaluatedExpressionIndex = NextExpressions.Count;
					}
				}
				else if (OperatorText == BracketOperator)
				{
					bool endBracketFound = false;
					int index = 0;

					if (EvaluatedExpressionIndex > -1)
					{
						index = EvaluatedExpressionIndex;
						EvaluatedExpressionIndex = NextExpressions.Count;
					}

					while (!endBracketFound && index < NextExpressions.Count)
					{
						BaseExpression expression = NextExpressions[index++];
						OperatorExpression operatorExpression = expression as OperatorExpression;

						if (operatorExpression != null)
						{
							if (operatorExpression.OperatorText == EndBracketOperator)
							{
								if (EvaluatedExpressionIndex > -1)
								{
									while (!operatorExpression.IsEvaluationComplete)
									{
										result = operatorExpression.Evaluate(context, result);
									}
								}
								else
								{
									endBracketFound = true;
									index--;
								}
							}
							else
							{
								while (!operatorExpression.IsEvaluationComplete)
								{
									result = operatorExpression.Evaluate(context, result);
								}
							}
						}
						else
						{
							result = expression.Evaluate(context, result);
						}
					}

					EvaluatedExpressionIndex = index;
				}
				else
				{
					bool recentlyEvaluated = false;
					int index = 0;

					if (EvaluatedExpressionIndex > -1)
					{
						index = EvaluatedExpressionIndex;
						EvaluatedExpressionIndex = NextExpressions.Count;
					}

					while (!recentlyEvaluated && index < NextExpressions.Count)
					{
						BaseExpression nextExpression = NextExpressions[index];

						if (EvaluatedExpressionIndex > -1)
						{
							result = nextExpression.Evaluate(context, result);
						}
						else
						{
							OperatorExpression nextOperatorExpression = nextExpression as OperatorExpression;

							if (nextOperatorExpression != null)
							{
								if (nextOperatorExpression.Precedence > Precedence)
								{
									result = nextOperatorExpression.Evaluate(context, result);

									result.ResultValue = ExecuteOperator(context, thisValue.ResultValue, result.ResultValue);

									result = nextOperatorExpression.Evaluate(context, result);
								}
								else if (nextOperatorExpression.precedence == Precedence && OperatorText != EndBracketOperator)
								{
									result.ResultValue = ExecuteOperator(context, thisValue.ResultValue, result.ResultValue);

									result = nextOperatorExpression.Evaluate(context, result);
								}
								else
								{
									result.ResultValue = ExecuteOperator(context, thisValue.ResultValue, result.ResultValue);
								}

								EvaluatedExpressionIndex = index;
								recentlyEvaluated = true;
							}
							else
							{
								result = nextExpression.Evaluate(context, result);
							}
						}

						index++;
					}

					if (!recentlyEvaluated && EvaluatedExpressionIndex < 0)
					{
						ValueWrapper leftValue = (thisValue == null ? null : thisValue.ResultValue);
						result.ResultValue = ExecuteOperator(context, leftValue, result.ResultValue);
						EvaluatedExpressionIndex = NextExpressions.Count;
					}
				}
			}
			else
			{
				result.ResultValue = ExecuteOperator(context, thisValue.ResultValue, null);
				EvaluatedExpressionIndex = NextExpressions.Count;
			}

			return result;
		}
	}
}