using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class ExportedType : TokenBase, IMultiLine
	{
		public List<CodeLine> CodeLines
		{
			get;
			set;
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
				throw new NotImplementedException();
			}
		}

		public bool IsInMemory
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.ExportedType;
			}
		}

		public Assembly Assembly
		{
			get;
			private set;
		}

		private uint ImplementationToken
		{
			get;
			set;
		}

		private CorTypeAttr Flags
		{
			get;
			set;
		}

		public ExportedType(Assembly assembly, uint token, string name, uint implementationToken, uint flags)
		{
			Assembly = assembly;
			Token = token;
			Name = name;
			ImplementationToken = implementationToken;
			Flags = (CorTypeAttr)flags;
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();

			if (!Flags.HasFlag(CorTypeAttr.tdForwarder))
			{
				throw new NotSupportedException("The exported type does not have the Forwarder flag set.");
			}

			CodeLines.Add(new CodeLine(0, ".class extern forwarder " + HelperFunctions.QuoteName(Name)));
			CodeLines.Add(new CodeLine(0, "{"));
			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			AssemblyReference assemblyReference;
			if (!Assembly.AssemblyReferences.TryGetValue(ImplementationToken, out assemblyReference))
			{
				throw new InvalidOperationException("The assembly reference the following type is forwarded to could not be found: " + Name);
			}

			CodeLines.Add(new CodeLine(1, ".assembly extern " + HelperFunctions.QuoteName(assemblyReference.Name)));
			CodeLines.Add(new CodeLine(0, "}"));
		}
	}
}