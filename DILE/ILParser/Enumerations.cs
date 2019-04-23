using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.ILParser
{
	public enum LanguageElementType
	{
		Empty,
		ID,
		QString,
		SQString,
		Int32,
		Int64,
		Float64,
		ILInstruction,
		Start,
		Declaration
	}
}