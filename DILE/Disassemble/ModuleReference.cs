using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Disassemble.ILCodes;
using Dile.UI;

namespace Dile.Disassemble
{
	public class ModuleReference : TokenBase, IMultiLine
	{
		#region IMultiLine Members

		private List<CodeLine> codeLines;
		public List<CodeLine> CodeLines
		{
			get
			{
				return codeLines;
			}
			set
			{
				codeLines = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return Name;
			}
		}

		public bool LoadedFromMemory
		{
			get
			{
				return Assembly.LoadedFromMemory;
			}
		}

		public bool IsInMemory
		{
			get
			{
				return Assembly.IsInMemory;
			}
		}

		#endregion

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.ModuleReference;
			}
		}

		private Assembly assembly;
		public Assembly Assembly
		{
			get
			{
				return assembly;
			}
			private set
			{
				assembly = value;
			}
		}

		public ModuleReference(Assembly assembly, uint token, string name)
		{
			Token = token;
			Assembly = assembly;
			Name = name;
			HelperFunctions.GetMemberReferences(Assembly, Token);
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();
			CodeLines.Add(new CodeLine(0, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));
			CodeLines.Add(new CodeLine(0, ".module extern " + Name));
		}
	}
}