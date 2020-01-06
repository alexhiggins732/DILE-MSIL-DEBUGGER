using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Dile.Debug.Expressions
{
	public class EvaluationException : BaseDebugExpressionException
	{
		public EvaluationException()
			: base()
		{
		}

		public EvaluationException(string message)
			: base(message)
		{
		}

		protected EvaluationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public EvaluationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
