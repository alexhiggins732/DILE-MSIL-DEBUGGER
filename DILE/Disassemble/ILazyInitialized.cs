using System;

using System.Collections.Generic;

namespace Dile.Disassemble
{
	public interface ILazyInitialized
	{
		void LazyInitialize(Dictionary<uint, TokenBase> allTokens);
	}
}