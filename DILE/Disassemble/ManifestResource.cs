using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class ManifestResource : TokenBase, IMultiLine
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
				return SearchOptions.ManifestResource;
			}
		}

		private uint providerToken;
		public uint ProviderToken
		{
			get
			{
				return providerToken;
			}
			private set
			{
				providerToken = value;
			}
		}

		private uint offset;
		public uint Offset
		{
			get
			{
				return offset;
			}
			private set
			{
				offset = value;
			}
		}

		private CorManifestResourceFlags flags;
		public CorManifestResourceFlags Flags
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

		public ManifestResource(Assembly assembly, uint token, string name, uint providerToken, uint offset, uint flags)
		{
			Assembly = assembly;
			Token = token;
			Name = name;
			ProviderToken = providerToken;
			Offset = offset;
			Flags = (CorManifestResourceFlags)flags;
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();

			StringBuilder definitionBuilder = new StringBuilder(".mresource ");

			if ((Flags & CorManifestResourceFlags.mrPublic) == CorManifestResourceFlags.mrPublic)
			{
				definitionBuilder.Append("public ");
			}
			else
			{
				definitionBuilder.Append("private ");
			}

			definitionBuilder.Append(Name);

			CodeLine definition = new CodeLine(0, definitionBuilder.ToString());
			CodeLines.Add(definition);

			CodeLines.Add(new CodeLine(0, "{"));

			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));
			CodeLines.Add(new CodeLine(1, string.Format(".file {0} at 0x{1}", Name, HelperFunctions.FormatAsHexNumber(Offset, 8))));

			CodeLines.Add(new CodeLine(0, "}"));
		}
	}
}