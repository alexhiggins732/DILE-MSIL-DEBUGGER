using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.Debug.Expressions
{
	public class DebugExpressionResult
	{
		private EvaluationContext context;
		private EvaluationContext Context
		{
			get
			{
				return context;
			}
			set
			{
				context = value;
			}
		}

		private ValueWrapper resultValue;
		public ValueWrapper ResultValue
		{
			get
			{
				return resultValue;
			}
			set
			{
				resultValue = value;
				resultClass = null;
			}
		}

		private ClassWrapper resultClass;
		public ClassWrapper ResultClass
		{
			get
			{
				if (resultClass == null && ResultValue != null && HelperFunctions.HasValueClass(ResultValue))
				{
					resultClass = HelperFunctions.GetClassOfValue(Context, ResultValue);
				}

				return resultClass;
			}
			set
			{
				resultClass = value;
			}
		}

		public DebugExpressionResult(EvaluationContext context) : this(context, null, null)
		{
		}

		public DebugExpressionResult(EvaluationContext context, ValueWrapper resultValue) : this(context, resultValue, null)
		{
		}

		public DebugExpressionResult(EvaluationContext context, ValueWrapper resultValue, ClassWrapper resultClass)
		{
			Context = context;
			ResultValue = resultValue;
			ResultClass = resultClass;
		}
	}
}