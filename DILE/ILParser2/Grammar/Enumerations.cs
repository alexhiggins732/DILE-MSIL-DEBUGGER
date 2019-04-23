using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser2.Grammar
{
	public enum TokenKind
	{
		Colon,
		ConstantString,
		DottedName,
		Empty,
		EOF,
		Float64,
		ID,
		Instruction,
		Int32,
		Int64,
		Name,
		Pipe,
		QString,
		SemiColon,
		SQString,
		Start
	}
}