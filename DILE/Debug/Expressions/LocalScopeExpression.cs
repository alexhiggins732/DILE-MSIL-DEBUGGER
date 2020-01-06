using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.UI.Debug;

namespace Dile.Debug.Expressions
{
	public class LocalScopeExpression : BaseExpression
	{
		private bool isArgument;
		private bool IsArgument
		{
			get
			{
				return isArgument;
			}
			set
			{
				isArgument = value;
			}
		}

		private uint variableIndex;
		private uint VariableIndex
		{
			get
			{
				return variableIndex;
			}
			set
			{
				variableIndex = value;
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

		public LocalScopeExpression(string foundExpression, bool isArgument, uint variableIndex)
			: base(foundExpression)
		{
			IsArgument = isArgument;
			VariableIndex = variableIndex;
			FoundExpressionLength = FoundExpression.Length;
		}

		private static string GetIndex(string expressionText, int startIndex)
		{
			string result = string.Empty;
			int index = startIndex;

			while (index < expressionText.Length && char.IsDigit(expressionText[index]))
			{
				index++;
			}

			if (index > startIndex)
			{
				result = expressionText.Substring(startIndex, index - startIndex);
			}

			return result;
		}

		public static BaseExpression TryParse(string expressionText)
		{
			LocalScopeExpression result = null;

			if (expressionText.Length > 2)
			{
				char character = expressionText[0];
				bool? argument = null;

				if (character == 'A')
				{
					argument = true;
				}
				else if (character == 'V')
				{
					argument = false;
				}

				if (argument.HasValue)
				{
					character = expressionText[1];

					if (character == '_')
					{
						string variableIndexText = GetIndex(expressionText, 2);

						if (variableIndexText.Length > 0)
						{
							uint variableIndex;

							if (uint.TryParse(variableIndexText, out variableIndex))
							{
								int characterIndex = variableIndexText.Length + 2;
								result = new LocalScopeExpression(expressionText.Substring(0, characterIndex), argument.Value, variableIndex);
							}
						}
					}
				}
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = null;

			if (IsArgument)
			{
				result = new DebugExpressionResult(context, context.FrameWrapper.GetArgument(VariableIndex));
			}
			else
			{
				result = new DebugExpressionResult(context, context.FrameWrapper.GetLocalVariable(VariableIndex));
			}

			return result;
		}
	}
}