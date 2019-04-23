using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection.Emit;

namespace Dile.Disassemble.ILCodes
{
	public class BaseILCode : CodeLine
	{
		private int offset = 0;
		public int Offset
		{
			get
			{
				return offset;
			}

			set
			{
				offset = value;
			}
		}

		public string Address
		{
			get
			{
				return string.Format("IL_{0}:", HelperFunctions.FormatAsHexNumber(Offset, 4));
			}
		}

		private OpCode opCode;
		public OpCode OpCode
		{
			get
			{
				return opCode;
			}

			set
			{
				opCode = value;

				if (Text.Length == 0)
				{
					Text = OpCode.Name;
				}
			}
		}

		public BaseILCode()
		{
		}

		public virtual void DecodeParameter()
		{
			Text = OpCode.Name;
		}
	}
}