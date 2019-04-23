using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class HandlerClause
	{
		public CorExceptionFlag Flags
		{
			get;
			private set;
		}

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

		public uint ClauseDependentData
		{
			get;
			private set;
		}

		public HandlerClause(CorExceptionFlag flags, uint offset, uint length, uint clauseDependentData)
		{
			Flags = flags;
			Offset = offset;
			Length = length;
			ClauseDependentData = clauseDependentData;
		}
	}
}