using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public interface IMethodParameter
	{
		CorElementType ElementType
		{
			get;
			set;
		}

		IMethodParameter ArrayElementType
		{
			get;
			set;
		}

		TokenBase TokenObject
		{
			get;
			set;
		}

		string GetTokenObjectName(bool fullName);
	}
}