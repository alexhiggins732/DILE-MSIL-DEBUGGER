using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class File : TokenBase, IMultiLine
	{
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

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.File;
			}
		}

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

		#endregion

		public bool DisplayInTree
		{
			get
			{
				return ((Flags & CorFileFlags.ffContainsNoMetaData) == CorFileFlags.ffContainsNoMetaData);
			}
		}

		private IntPtr hash;
		public IntPtr Hash
		{
			get
			{
				return hash;
			}
			private set
			{
				hash = value;
			}
		}

		private uint hashLength;
		public uint HashLength
		{
			get
			{
				return hashLength;
			}
			private set
			{
				hashLength = value;
			}
		}

		private CorFileFlags flags;
		public CorFileFlags Flags
		{
			get
			{
				return flags;
			}
			private set
			{
				flags = value;
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

		public File(Assembly assembly, uint token, string name, IntPtr hash, uint hashLength, uint flags)
		{
			Assembly = assembly;
			Token = token;
			Name = name;
			Hash = hash;
			HashLength = hashLength;
			Flags = (CorFileFlags)flags;
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();
			CodeLines.Add(new CodeLine(0, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			CodeLine definition = new CodeLine(0, "file " + Name);
			CodeLines.Add(definition);

			CodeLines.Add(new CodeLine(0, string.Format(".hash = {0}", HelperFunctions.ReadBlobAsString(Hash, HashLength))));
		}
	}
}