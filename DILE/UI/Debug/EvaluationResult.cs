using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class EvaluationResult : BaseEvaluationResult
	{
		private BaseValueRefresher resultRefresher;
		public BaseValueRefresher ResultRefresher
		{
			get
			{
				return resultRefresher;
			}
			set
			{
				resultRefresher = value;
			}
		}

		private string expression;
		public string Expression
		{
			get
			{
				return expression;
			}
			private set
			{
				expression = value;
			}
		}

		private ValueFieldGroup group;
		public ValueFieldGroup Group
		{
			get
			{
				return group;
			}
			private set
			{
				group = value;
			}
		}

		public EvaluationResult(string expression, ValueFieldGroup group)
		{
			Expression = expression;
			Group = group;
		}

		public EvaluationResult(BaseValueRefresher resultRefresher, ValueWrapper result, string expression, ValueFieldGroup group) : this(expression, group)
		{
			ResultRefresher = resultRefresher;
			Result = result;
		}

		public void LoadBaseEvaluationResult(BaseEvaluationResult evaluationResult)
		{
			Result = evaluationResult.Result;
			Exception = evaluationResult.Exception;
			Reason = evaluationResult.Reason;
		}
	}
}