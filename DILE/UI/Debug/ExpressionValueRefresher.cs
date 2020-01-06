using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Debug.Expressions;

namespace Dile.UI.Debug
{
	public class ExpressionValueRefresher : BaseValueRefresher
	{
		private List<BaseExpression> expressions;
		private List<BaseExpression> Expressions
		{
			get
			{
				return expressions;
			}
			set
			{
				expressions = value;
			}
		}

		private FrameRefresher frameRefresher;
		private FrameRefresher FrameRefresher
		{
			get
			{
				return frameRefresher;
			}
			set
			{
				frameRefresher = value;
			}
		}

		private EvaluationHandler evaluationHandler;
		private EvaluationHandler EvaluationHandler
		{
			get
			{
				return evaluationHandler;
			}
			set
			{
				evaluationHandler = value;
			}
		}

		public ExpressionValueRefresher(List<BaseExpression> expressions, FrameRefresher frameRefresher, EvaluationHandler evaluationHandler, string parsedExpression)
			: base(parsedExpression)
		{
			Expressions = expressions;
			FrameRefresher = frameRefresher;
			EvaluationHandler = evaluationHandler;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			DebugExpressionResult result = null;

			EvalWrapper evalWrapper = FrameRefresher.Thread.CreateEval();
			ProcessWrapper processWrapper = FrameRefresher.Thread.GetProcess();

			EvaluationContext context = new EvaluationContext(processWrapper, EvaluationHandler, evalWrapper, FrameRefresher.Thread, true);
			result = new DebugExpressionResult(context);

			for (int index = 0; index < Expressions.Count; index++)
			{
				BaseExpression expression = Expressions[index];
				result = expression.Evaluate(context, result);

				OperatorExpression operatorExpression = expression as OperatorExpression;

				if (operatorExpression != null)
				{
					while (!operatorExpression.IsEvaluationComplete)
					{
						result = operatorExpression.Evaluate(context, result);
					}
				}
			}

			return result.ResultValue;
		}
	}
}