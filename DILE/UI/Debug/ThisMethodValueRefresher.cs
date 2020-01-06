using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;

namespace Dile.UI.Debug
{
	public class ThisMethodValueRefresher : BaseValueRefresher
	{
		private EvaluationContext evaluationContext;
		private EvaluationContext EvaluationContext
		{
			get
			{
				return evaluationContext;
			}
			set
			{
				evaluationContext = value;
			}
		}

		private BaseValueRefresher parentObject;
		private BaseValueRefresher ParentObject
		{
			get
			{
				return parentObject;
			}
			set
			{
				parentObject = value;
			}
		}

		private MethodDefinition method;
		private MethodDefinition Method
		{
			get
			{
				return method;
			}
			set
			{
				method = value;
			}
		}

		public ThisMethodValueRefresher(string name, EvaluationContext evaluationContext, BaseValueRefresher parentObject, MethodDefinition method)
			: base(name)
		{
			EvaluationContext = evaluationContext;
			ParentObject = parentObject;
			Method = method;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			ValueWrapper result = null;
			ValueWrapper parentValueWrapper = ParentObject.GetRefreshedValue();

			List<ValueWrapper> arguments = new List<ValueWrapper>(1);
			arguments.Add(parentValueWrapper);

			List<TypeWrapper> typeArguments = null;

			if (parentValueWrapper != null && parentValueWrapper.IsVersion2)
			{
				TypeWrapper parentExactType = parentValueWrapper.Version2.GetExactType();
				typeArguments = parentExactType.EnumerateTypeParameters();
			}

			BaseEvaluationResult evaluationResult = EvaluationContext.EvaluationHandler.CallMethod(EvaluationContext, Method, arguments, typeArguments);

			if (evaluationResult.IsSuccessful)
			{
				result = evaluationResult.Result;
			}

			return result;
		}
	}
}