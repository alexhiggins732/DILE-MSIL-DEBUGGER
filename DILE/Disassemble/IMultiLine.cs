using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;

namespace Dile.Disassemble
{
	public interface IMultiLine
	{
		List<CodeLine> CodeLines
		{
			get;
			set;
		}

		string HeaderText
		{
			get;
		}

		bool LoadedFromMemory
		{
			get;
		}

		bool IsInMemory
		{
			get;
		}

		void Initialize();
	}
}
