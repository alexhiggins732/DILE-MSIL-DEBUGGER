using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;

namespace Dile.Disassemble
{
	public static class ExceptionHandler
	{
		public static TToken ReadToken<TToken>(Func<TToken> readTokenFunction, TToken incorrectToken)
			where TToken : TokenBase
		{
			TToken result = incorrectToken;

			try
			{
				result = readTokenFunction();
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
			}

			return result;
		}
	}
}