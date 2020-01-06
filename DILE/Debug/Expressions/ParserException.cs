using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization;

namespace Dile.Debug.Expressions
{
	public class ParserException : BaseDebugExpressionException
	{
		public ParserException()
			: base()
		{
		}

		public ParserException(string message)
			: base(message)
		{
		}

		protected ParserException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public ParserException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}