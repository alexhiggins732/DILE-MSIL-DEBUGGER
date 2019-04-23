using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class ModuleScope : TokenBase, IMultiLine
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
				return SearchOptions.ModuleScope;
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

		private Guid mvid;
		public Guid Mvid
		{
			get
			{
				return mvid;
			}
			private set
			{
				mvid = value;
			}
		}

		private Dictionary<uint, TypeDefinition> typeDefinitions = new Dictionary<uint, TypeDefinition>();
		public Dictionary<uint, TypeDefinition> TypeDefinitions
		{
			get
			{
				return typeDefinitions;
			}
			private set
			{
				typeDefinitions = value;
			}
		}

		private ModuleWrapper debuggedModule;
		public ModuleWrapper DebuggedModule
		{
			get
			{
				return debuggedModule;
			}
			private set
			{
				debuggedModule = value;
			}
		}

		public ModuleScope(IMetaDataImport2 import, Assembly assembly)
		{
			Assembly = assembly;
			uint moduleNameLength;
			import.GetScopeProps(Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out moduleNameLength, ref mvid);

			if (moduleNameLength > Project.DefaultCharArray.Length)
			{
				Project.DefaultCharArray = new char[moduleNameLength];

				import.GetScopeProps(Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out moduleNameLength, ref mvid);
			}

			Name = HelperFunctions.GetString(Project.DefaultCharArray, 0, moduleNameLength);

			uint token;
			import.GetModuleFromScope(out token);
			Token = token;
			Assembly.AllTokens[Token] = this;
		}

		public void EnumerateTypeDefinitions(IMetaDataImport2 import, ModuleWrapper debuggedModule)
		{
			DebuggedModule = debuggedModule;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeDefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumTypeDefs(ref enumHandle, typeDefs, Convert.ToUInt32(typeDefs.Length), out count);

			while (count > 0)
			{
				for (uint typeDefsIndex = 0; typeDefsIndex < count; typeDefsIndex++)
				{
					uint token = typeDefs[typeDefsIndex];
					uint typeNameLength;
					uint typeDefFlags;
					uint baseTypeToken;

					import.GetTypeDefProps(token, Project.DefaultCharArray,
Convert.ToUInt32(Project.DefaultCharArray.Length), out typeNameLength, out typeDefFlags, out baseTypeToken);

					if (typeNameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[typeNameLength];

						import.GetTypeDefProps(token, Project.DefaultCharArray,
Convert.ToUInt32(Project.DefaultCharArray.Length), out typeNameLength, out typeDefFlags, out baseTypeToken);
					}

					TypeDefinition typeDefinition = new TypeDefinition(import, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, typeNameLength), token, (CorTypeAttr)typeDefFlags, baseTypeToken);
					TypeDefinitions[token] = typeDefinition;
					Assembly.AllTokens[token] = typeDefinition;
				}

				import.EnumTypeDefs(ref enumHandle, typeDefs, Convert.ToUInt32(typeDefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}

			foreach (TypeDefinition typeDefinition in TypeDefinitions.Values)
			{
				if (typeDefinition.IsNestedType)
				{
					typeDefinition.FindEnclosingType(import);
				}
			}

			DebuggedModule = null;
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();

			CodeLines.Add(new CodeLine(0, ".module " + Name));
			CodeLines.Add(new CodeLine(0, "//MVID: {" + Mvid.ToString() + "}"));
			CodeLines.Add(new CodeLine(0, ".imagebase 0x" + HelperFunctions.FormatAsHexNumber(Assembly.ImageBase, 8)));
			CodeLines.Add(new CodeLine(0, ".file alignment 0x" + HelperFunctions.FormatAsHexNumber(Assembly.FileAlignment, 8)));

			if (Assembly.IsPe32)
			{
				CodeLines.Add(new CodeLine(0, ".stackreserve 0x" + HelperFunctions.FormatAsHexNumber(Assembly.StackReserve, 8)));
			}
			else
			{
				CodeLines.Add(new CodeLine(0, ".stackreserve 0x" + HelperFunctions.FormatAsHexNumber(Assembly.StackReserve, 16)));
			}

			CodeLines.Add(new CodeLine(0, string.Format(".subsystem 0x{0} //{1}", HelperFunctions.FormatAsHexNumber(Assembly.Subsystem, 4), (Assembly.Subsystem == 2 ? "WINDOWS_CE" : "WINDOWS_GUI"))));
			CodeLines.Add(new CodeLine(0, ".corflags 0x" + HelperFunctions.FormatAsHexNumber(Assembly.CorFlags, 8)));

			string peFormat = "//PE Format: ";
			if (Assembly.IsPe32)
			{
				peFormat += "PE32 (32 bit assembly)";
			}
			else
			{
				peFormat += "PE32+ (64 bit assembly)";
			}
			CodeLines.Add(new CodeLine(0, peFormat));
		}
	}
}