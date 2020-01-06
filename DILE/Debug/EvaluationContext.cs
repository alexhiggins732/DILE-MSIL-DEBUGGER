using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.UI.Debug;

namespace Dile.Debug
{
	public class EvaluationContext
	{
		private ProcessWrapper processWrapper;
		public ProcessWrapper ProcessWrapper
		{
			get
			{
				return processWrapper;
			}
			private set
			{
				processWrapper = value;
			}
		}

		private EvaluationHandler evaluationHandler;
		public EvaluationHandler EvaluationHandler
		{
			get
			{
				return evaluationHandler;
			}
			private set
			{
				evaluationHandler = value;
			}
		}

		private EvalWrapper evalWrapper;
		public EvalWrapper EvalWrapper
		{
			get
			{
				return evalWrapper;
			}
			private set
			{
				evalWrapper = value;
			}
		}

		private ThreadWrapper threadWrapper;
		public ThreadWrapper ThreadWrapper
		{
			get
			{
				return threadWrapper;
			}
			set
			{
				threadWrapper = value;
			}
		}

		public FrameWrapper FrameWrapper
		{
			get
			{
				return EvaluationHandler.Frame;
			}
		}

		private TypeTreeNodeList classTypeArguments;
		public TypeTreeNodeList ClassTypeArguments
		{
			get
			{
				return classTypeArguments;
			}
			set
			{
				classTypeArguments = value;
			}
		}

		public bool MethodCallsEnabled
		{
			get;
			set;
		}

		public EvaluationContext(ProcessWrapper processWrapper, EvaluationHandler evaluationHandler, EvalWrapper evalWrapper, ThreadWrapper threadWrapper, bool methodCallsEnabled)
		{
			ProcessWrapper = processWrapper;
			EvaluationHandler = evaluationHandler;
			EvalWrapper = evalWrapper;
			ThreadWrapper = threadWrapper;
			MethodCallsEnabled = methodCallsEnabled;
		}
	}
}