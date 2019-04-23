using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ILParser2.Code
{
	public enum TokenKind
	{
		Id,
		QString,
		SQString,
		Int32,
		Int64,
		Float64,
		Instruction,
		Text,
		Character
	}
}