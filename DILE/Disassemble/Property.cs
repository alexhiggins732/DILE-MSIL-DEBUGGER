using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;

namespace Dile.Disassemble
{
	public class Property : TextTokenBase, IMultiLine, IHasSignature
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
				return SearchOptions.Property;
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

		private CorPropertyAttr flags;
		public CorPropertyAttr Flags
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

		private IntPtr signature;
		public IntPtr Signature
		{
			get
			{
				return signature;
			}
			private set
			{
				signature = value;
			}
		}

		private uint signatureLength;
		public uint SignatureLength
		{
			get
			{
				return signatureLength;
			}
			private set
			{
				signatureLength = value;
			}
		}

		private CorElementType elementType;
		public CorElementType ElementType
		{
			get
			{
				return elementType;
			}
			private set
			{
				elementType = value;
			}
		}

		private IntPtr defaultValue;
		public IntPtr DefaultValue
		{
			get
			{
				return defaultValue;
			}
			private set
			{
				defaultValue = value;
			}
		}

		private uint defaultValueLength;
		public uint DefaultValueLength
		{
			get
			{
				return defaultValueLength;
			}
			private set
			{
				defaultValueLength = value;
			}
		}

		private uint setterMethodToken;
		public uint SetterMethodToken
		{
			get
			{
				return setterMethodToken;
			}
			private set
			{
				setterMethodToken = value;
			}
		}

		private uint getterMethodToken;
		public uint GetterMethodToken
		{
			get
			{
				return getterMethodToken;
			}
			private set
			{
				getterMethodToken = value;
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

		private string definition;
		public string Definition
		{
			get
			{
				return definition;
			}
			private set
			{
				definition = value;
			}
		}

		private PropertySignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

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

		public BaseSignatureItem PropertyType
		{
			get
			{
				return signatureReader.ReturnType;
			}
		}

		public Property(IMetaDataImport2 import, uint token, TypeDefinition baseTypeDefinition, string name, uint flags, IntPtr signature, uint signatureLength, uint elementType, IntPtr defaultValue, uint defaultValueLength, uint setterMethodToken, uint getterMethodToken, uint[] otherMethods, uint otherMethodsCount)
		{
			Token = token;
			BaseTypeDefinition = baseTypeDefinition;
			Name = name;
			Flags = (CorPropertyAttr)flags;
			Signature = signature;
			SignatureLength = signatureLength;
			ElementType = (CorElementType)elementType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;

			if (DefaultValueLength > 0)
			{
				throw new NotImplementedException("Default value is given for the property.");
			}

			SetterMethodToken = setterMethodToken;
			GetterMethodToken = getterMethodToken;
			OtherMethods = otherMethods;
			OtherMethodsCount = otherMethodsCount;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new PropertySignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, Signature, SignatureLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			HelperFunctions.SetSignatureItemToken(allTokens, signatureReader.ReturnType);

			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			DefinitionBuilder.Length = 0;

			if ((Flags & CorPropertyAttr.prSpecialName) == CorPropertyAttr.prSpecialName)
			{
				DefinitionBuilder.Append("specialname ");
			}

			CorPropertyAttr reservedFlags = Flags & CorPropertyAttr.prReservedMask;

			if ((reservedFlags & CorPropertyAttr.prRTSpecialName) == CorPropertyAttr.prRTSpecialName)
			{
				DefinitionBuilder.Append("rtsspecialname ");
			}

			if ((signatureReader.CallingConvention & CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS) == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS)
			{
				DefinitionBuilder.Append("instance ");
			}

			DefinitionBuilder.Append(signatureReader.ReturnType);
			DefinitionBuilder.Append(" ");
			DefinitionBuilder.Append(Name);
			DefinitionBuilder.Append("(");

			if (signatureReader.Parameters != null)
			{
				for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
				{
					BaseSignatureItem parameter = signatureReader.Parameters[parametersIndex];
					HelperFunctions.SetSignatureItemToken(allTokens, parameter);
					DefinitionBuilder.Append(parameter);

					if (parametersIndex < signatureReader.Parameters.Count - 1)
					{
						DefinitionBuilder.Append(", ");
					}
				}
			}

			DefinitionBuilder.Append(")");
			Definition = DefinitionBuilder.ToString();
		}

		public void Initialize()
		{
			ReadMetadata();
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			CodeLines = new List<CodeLine>();
			CodeLine definitionLine = new CodeLine();
			definitionLine.Indentation = 0;

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

			AddMethodDefinitionLine(GetterMethodToken, ".get ");

			for (int index = 0; index < OtherMethodsCount; index++)
			{
				uint token = OtherMethods[index];

				AddMethodDefinitionLine(token, ".other ");
			}

			AddMethodDefinitionLine(SetterMethodToken, ".set ");

			Definition = ".property " + Definition;
			definitionLine.Text = Definition;

			CodeLines.Add(new CodeLine(0, string.Format("}} //end of property {0}::{1}", BaseTypeDefinition.Name, Name)));
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