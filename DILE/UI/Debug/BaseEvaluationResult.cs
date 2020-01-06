using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class BaseEvaluationResult
	{
		private ValueWrapper result;
		public ValueWrapper Result
		{
			get
			{
				return result;
			}
			set
			{
				result = value;
			}
		}

		private EvaluationFinishedReason reason;
		public EvaluationFinishedReason Reason
		{
			get
			{
				return reason;
			}
			set
			{
				reason = value;
			}
		}

		private Exception exception;
		public Exception Exception
		{
			get
			{
				if (exception == null)
				{
					exception = GetExceptionAccordingToReason();
				}

				return exception;
			}
			set
			{
				exception = value;
			}
		}

		public bool IsSuccessful
		{
			get
			{
				return (Reason == EvaluationFinishedReason.Succeeded);
			}
		}

		public BaseEvaluationResult()
		{
		}

		public BaseEvaluationResult(ValueWrapper result, int hResult, EvaluationFinishedReason reason, Exception exception)
			: this()
		{
			Result = result;
			Reason = reason;
			Exception = exception;
		}

		private EvaluationHandlerException GetExceptionAccordingToReason()
		{
			EvaluationHandlerException result = null;

			switch (Reason)
			{
				case EvaluationFinishedReason.AbortFailed:
					result = new EvaluationHandlerException("The following exception occurred while trying to abort an evaluation: " + exception.Message, Exception);
					break;

				case EvaluationFinishedReason.AbortSucceeded:
					result = new EvaluationHandlerException("An evaluation has been successfully aborted because the given timeout period has expired.");
					break;

				case EvaluationFinishedReason.AbortTimeout:
					result = new EvaluationHandlerException("Failed to abort an evaluation, both the debugger and the debuggee can be in an unstable state.");
					break;

				case EvaluationFinishedReason.Failed:
					result = new EvaluationHandlerException("The following exception occurred while trying to run an evaluation: " + exception.Message, Exception);
					break;
			}

			return result;
		}

		public void ThrowExceptionAccordingToReason()
		{
			throw GetExceptionAccordingToReason();
		}
	}
}