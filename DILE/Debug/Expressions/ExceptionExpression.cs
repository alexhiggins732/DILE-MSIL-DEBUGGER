using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.Debug.Expressions
{
	public class ExceptionExpression : BaseExpression
	{
		public ExceptionExpression()
			: base(Constants.CurrentExceptionName)
		{
		}

		public static BaseExpression TryParse(string expressionText)
		{
			BaseExpression result = null;

			if (expressionText.StartsWith(Constants.CurrentExceptionName))
			{
				result = new ExceptionExpression();
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			return new DebugExpressionResult(context, context.ThreadWrapper.GetCurrentException());
		}
	}
}