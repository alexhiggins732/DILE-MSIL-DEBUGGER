using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;

namespace Dile.Disassemble
{
	public class EventDefinition : TextTokenBase, IMultiLine
	{
		[ThreadStatic()]
		private static StringBuilder definitionBuilder;
		private static StringBuilder DefinitionBuilder
		{
			get
			{
				if (definitionBuilder == null)
				{
					definitionBuilder = new StringBuilder();
				}

				return definitionBuilder;
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
				return string.Format("{0}.{1}", BaseTypeDefinition.Name, Name);
			}
		}

		public bool LoadedFromMemory
		{
			get
			{
				return BaseTypeDefinition.ModuleScope.Assembly.LoadedFromMemory;
			}
		}

		public bool IsInMemory
		{
			get
			{
				return BaseTypeDefinition.ModuleScope.Assembly.IsInMemory;
			}
		}
		#endregion

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.EventDefinition;
			}
		}

		private TypeDefinition baseTypeDefinition;
		public TypeDefinition BaseTypeDefinition
		{
			get
			{
				return baseTypeDefinition;
			}
			private set
			{
				baseTypeDefinition = value;
			}
		}

		private CorEventAttr eventFlags;
		public CorEventAttr EventFlags
		{
			get
			{
				return eventFlags;
			}
			private set
			{
				eventFlags = value;
			}
		}

		private uint eventClassToken;
		public uint EventClassToken
		{
			get
			{
				return eventClassToken;
			}
			private set
			{
				eventClassToken = value;
			}
		}

		private uint addOnMethodToken;
		public uint AddOnMethodToken
		{
			get
			{
				return addOnMethodToken;
			}
			private set
			{
				addOnMethodToken = value;
			}
		}

		private uint removeOnMethodToken;
		public uint RemoveOnMethodToken
		{
			get
			{
				return removeOnMethodToken;
			}
			private set
			{
				removeOnMethodToken = value;
			}
		}

		private uint fireMethodToken;
		public uint FireMethodToken
		{
			get
			{
				return fireMethodToken;
			}
			private set
			{
				fireMethodToken = value;
			}
		}

		private uint[] otherMethods;
		public uint[] OtherMethods
		{
			get
			{
				return otherMethods;
			}
			private set
			{
				otherMethods = value;
			}
		}

		private uint otherMethodsCount;
		public uint OtherMethodsCount
		{
			get
			{
				return otherMethodsCount;
			}
			private set
			{
				otherMethodsCount = value;
			}
		}

		private List<CustomAttribute> customAttributes;
		public List<CustomAttribute> CustomAttributes
		{
			get
			{
				ReadMetadata();

				return customAttributes;
			}
			private set
			{
				customAttributes = value;
			}
		}

		public EventDefinition(IMetaDataImport2 import, uint token, TypeDefinition baseTypeDefinition, string name, uint eventFlags, uint eventClassToken, uint addOnMethodToken, uint removeOnMethodToken, uint fireMethodToken, uint[] otherMethods, uint otherMethodsCount)
		{
			Token = token;
			BaseTypeDefinition = baseTypeDefinition;
			Name = name;
			EventFlags = (CorEventAttr)eventFlags;
			EventClassToken = eventClassToken;
			AddOnMethodToken = addOnMethodToken;
			RemoveOnMethodToken = removeOnMethodToken;
			FireMethodToken = fireMethodToken;
			OtherMethods = otherMethods;
			OtherMethodsCount = otherMethodsCount;
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			CodeLine definitionLine = new CodeLine();

			definitionLine.Indentation = 0;
			definitionLine.Text = string.Format(".event {0} {1}", assembly.AllTokens[EventClassToken].Name, Name);

			CodeLines.Add(definitionLine);
			CodeLines.Add(new CodeLine(0, "{"));
			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			if (CustomAttributes != null)
			{
				foreach (CustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
					CodeLines.Add(new CodeLine(1, customAttribute.Name));
				}
			}

			AddMethodDefinitionLine(AddOnMethodToken, ".addon ");
			AddMethodDefinitionLine(FireMethodToken, ".fire ");

			for (int index = 0; index < OtherMethodsCount; index++)
			{
				uint token = OtherMethods[index];

				AddMethodDefinitionLine(token, ".other ");
			}

			AddMethodDefinitionLine(RemoveOnMethodToken, ".removeon ");

			CodeLines.Add(new CodeLine(0, string.Format("}} //end of event {0}::{1}", BaseTypeDefinition.Name, Name)));
		}

		private void AddMethodDefinitionLine(uint methodToken, string methodDefinitionName)
		{
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			if (assembly.AllTokens.ContainsKey(methodToken))
			{
				MethodDefinition getMethod = (MethodDefinition)assembly.AllTokens[methodToken];
				CodeLines.Add(new CodeLine(1, methodDefinitionName + getMethod.Text));
			}
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = HelperFunctions.EnumCustomAttributes(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, this);
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}
	}
}