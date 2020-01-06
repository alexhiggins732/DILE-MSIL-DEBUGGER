using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Debug.Expressions
{
	public class Parser
	{
		private string StripUnnecessaryWhiteSpaces(string expressionText)
		{
			StringBuilder expression = new StringBuilder(expressionText);
			bool insideString = false;
			char previousCharacter = ' ';
			int index = 0;

			while (index < expression.Length)
			{
				char character = expression[index];

				if (character == '\'' || character == '"')
				{
					if (!insideString)
					{
						insideString = true;
					}
					else if (previousCharacter != '\\')
					{
						insideString = false;
					}

					index++;
				}
				else if (!insideString && Char.IsWhiteSpace(expression[index]))
				{
					if (expression.Length >= index)
					{
						if (index >= 3 && expression[index - 3] == 'n' && expression[index - 2] == 'e' && expression[index - 1] == 'w')
						{
							expression[index] = ConstructorExpression.NewKeywordSeparatorChar;
						}
						else
						{
							expression.Remove(index, 1);
						}
					}
					else
					{
						expression.Remove(index, 1);
					}
				}
				else
				{
					index++;
				}

				previousCharacter = character;
			}

			return expression.ToString();
		}

		public List<BaseExpression> Parse(string expressionText)
		{
			List<BaseExpression> result = new List<BaseExpression>();
			expressionText = StripUnnecessaryWhiteSpaces(expressionText);

			while (expressionText.Length > 0)
			{
				BaseExpression expression = null;
				
				expression = ConstantExpression<string>.TryParse(expressionText);

				if (expression == null)
				{
					expression = TypeExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = LocalScopeExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = ConstructorExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = CastExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = MemberExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = IndexerExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = OperatorExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expression = ExceptionExpression.TryParse(expressionText);
				}

				if (expression == null)
				{
					expressionText = string.Empty;
				}
				else
				{
					result.Add(expression);

					if (expression.FoundExpressionLength > expressionText.Length)
					{
						expressionText = string.Empty;
					}
					else
					{
						expressionText = expressionText.Substring(expression.FoundExpressionLength);
					}
				}
			}

			return result;
		}
	}
}