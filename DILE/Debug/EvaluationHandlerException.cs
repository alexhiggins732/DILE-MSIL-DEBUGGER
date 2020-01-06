using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Dile.Debug
{
	public class EvaluationHandlerException : ApplicationException
	{
		public EvaluationHandlerException()
			: this(string.Empty, null)
		{
		}

		public EvaluationHandlerException(string message)
			: this(message, null)
		{
		}

		public EvaluationHandlerException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected EvaluationHandlerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}