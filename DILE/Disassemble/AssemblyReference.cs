using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class AssemblyReference : TokenBase, IMultiLine
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
				return SearchOptions.AssemblyReference;
			}
		}

		private string fileName = string.Empty;
		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
			}
		}

		private string fullPath = string.Empty;
		public string FullPath
		{
			get
			{
				return fullPath;
			}
			set
			{
				fullPath = value;
			}
		}

		private IntPtr publicKeyOrToken;
		public IntPtr PublicKeyOrToken
		{
			get
			{
				return publicKeyOrToken;
			}
			private set
			{
				publicKeyOrToken = value;
			}
		}

		private uint publicKeyOrTokenLength;
		public uint PublicKeyOrTokenLength
		{
			get
			{
				return publicKeyOrTokenLength;
			}
			private set
			{
				publicKeyOrTokenLength = value;
			}
		}

		private AssemblyMetadata metadata;
		public AssemblyMetadata Metadata
		{
			get
			{
				return metadata;
			}
			private set
			{
				metadata = value;
			}
		}

		private IntPtr hashBlob;
		public IntPtr HashBlob
		{
			get
			{
				return hashBlob;
			}
			private set
			{
				hashBlob = value;
			}
		}

		private uint hashBlobLength;
		public uint HashBlobLength
		{
			get
			{
				return hashBlobLength;
			}
			private set
			{
				hashBlobLength = value;
			}
		}

		private CorAssemblyFlags flags;
		public CorAssemblyFlags Flags
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

		public AssemblyReference(Assembly assembly, uint token, string name, IntPtr publicKeyOrToken, uint publicKeyOrTokenLength, AssemblyMetadata metadata, IntPtr hashBlob, uint hashBlobLength, uint flags)
		{
			Assembly = assembly;
			Token = token;
			Name = name;
			PublicKeyOrToken = publicKeyOrToken;
			PublicKeyOrTokenLength = publicKeyOrTokenLength;
			Metadata = metadata;
			HashBlob = hashBlob;
			HashBlobLength = hashBlobLength;
			Flags = (CorAssemblyFlags)flags;
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();

			StringBuilder definition = new StringBuilder();
			definition.Append(".assembly extern ");
			definition.Append(HelperFunctions.EnumAsString(Flags, CorAssemblyFlags.afRetargetable, "retargetable "));
			definition.Append(Name);

			CodeLine definitionLine = new CodeLine(0, definition.ToString());
			CodeLines.Add(definitionLine);

			CodeLines.Add(new CodeLine(0, "{"));
			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			if (FullPath != null && FullPath.Length > 0)
			{
				CodeLines.Add(new CodeLine(1, "//Full Path: " + FullPath));
			}
			else
			{
				CodeLines.Add(new CodeLine(1, "//Full Path: exact location not found. "));
			}

			CodeLines.Add(new CodeLine(1, ".publickeytoken = " + HelperFunctions.ReadBlobAsString(PublicKeyOrToken, PublicKeyOrTokenLength)));

			if (HashBlobLength > 0)
			{
				CodeLines.Add(new CodeLine(1, ".hash = " + HelperFunctions.ReadBlobAsString(HashBlob, HashBlobLength)));
			}

			CodeLines.Add(new CodeLine(1, string.Format(".ver {0}:{1}:{2}:{3}", Metadata.usMajorVersion, Metadata.usMinorVersion, Metadata.usBuildNumber, Metadata.usRevisionNumber)));

			CodeLines.Add(new CodeLine(0, "} //end of assembly reference " + Name));
		}
	}
}