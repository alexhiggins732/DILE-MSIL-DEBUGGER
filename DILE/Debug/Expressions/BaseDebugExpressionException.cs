using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Dile.Debug.Expressions
{
	public abstract class BaseDebugExpressionException : ApplicationException
	{
		public BaseDebugExpressionException()
			: base()
		{
		}

		public BaseDebugExpressionException(string message)
			: base(message)
		{
		}

		protected BaseDebugExpressionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public BaseDebugExpressionException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}