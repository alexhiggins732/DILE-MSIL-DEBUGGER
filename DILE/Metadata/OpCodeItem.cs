using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using System.Reflection.Emit;

namespace Dile.Metadata
{
	public class OpCodeItem : IComparable
	{
		public string OpCodesFieldName
		{
			get;
			private set;
		}

		public OpCode OpCode
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public OpCodeItem(string opCodesFieldName, OpCode opCode, string description)
		{
			OpCodesFieldName = opCodesFieldName;
			OpCode = opCode;
			Description = description;
		}

		public int CompareTo(object obj)
		{
			int result = 0;

			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			if (obj is OpCodeItem)
			{
				result = OpCode.Name.CompareTo(((OpCodeItem)obj).OpCode.Name);
			}
			else
			{
				throw new ArgumentException("Not supported type.", "obj");
			}

			return result;
		}

		public override string ToString()
		{
			return OpCode.Name;
		}
	}
}