using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.Disassemble
{
	public class TryClause
	{
		public uint Offset
		{
			get;
			private set;
		}

		public uint Length
		{
			get;
			private set;
		}

		public IList<HandlerClause> HandlerClauses
		{
			get;
			private set;
		}

		public TryClause(uint offset, uint length)
		{
			Offset = offset;
			Length = length;

			HandlerClauses = new List<HandlerClause>();
		}
	}
}