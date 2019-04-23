using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;

namespace Dile.Disassemble
{
	public class FieldDefinition : TextTokenBase, IMultiLine, IHasSignature
	{
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

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.FieldDefintion;
			}
		}

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					if (BaseTypeDefinition.FullName.Length > 0)
					{
						text = string.Format("{0} {1}::{2}", FieldTypeName, BaseTypeDefinition.FullName, Name);
					}
					else
					{
						text = string.Format("{0} {1}", FieldTypeName, Name);
					}
				}

				return text;
			}
			private set
			{
				text = value;
			}
		}

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

		private IntPtr signatureBlob;
		public IntPtr SignatureBlob
		{
			get
			{
				return signatureBlob;
			}
			private set
			{
				signatureBlob = value;
			}
		}

		private uint signatureBlobLength;
		public uint SignatureBlobLength
		{
			get
			{
				return signatureBlobLength;
			}
			private set
			{
				signatureBlobLength = value;
			}
		}

		private CorFieldAttr flags;
		public CorFieldAttr Flags
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

		private CorElementType defaultValueType;
		public CorElementType DefaultValueType
		{
			get
			{
				return defaultValueType;
			}
			private set
			{
				defaultValueType = value;
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

		private object defaultValueNumber = null;
		public object DefaultValueNumber
		{
			get
			{
				ReadMetadata();

				return defaultValueNumber;
			}
			private set
			{
				defaultValueNumber = value;
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

		private string fieldTypeName = null;
		public string FieldTypeName
		{
			get
			{
				if (fieldTypeName == null)
				{
					ReadSignature();

					if (signatureReader.GenericParameters != null)
					{
						FieldTypeNameBuilder.Length = 0;
						FieldTypeNameBuilder.Append("<");

						for (int index = 0; index < signatureReader.GenericParameters.Count; index++)
						{
							BaseSignatureItem genericParameter = signatureReader.GenericParameters[index];
							HelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, genericParameter);
							FieldTypeNameBuilder.Append(genericParameter);

							if (index < signatureReader.GenericParameters.Count - 1)
							{
								FieldTypeNameBuilder.Append(", ");
							}
						}

						FieldTypeNameBuilder.Append(">");

						fieldTypeName = FieldTypeNameBuilder.ToString();
					}
					else
					{
						HelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, signatureReader.Type);
						fieldTypeName = signatureReader.Type.ToString();
					}
				}

				return fieldTypeName;
			}
			private set
			{
				fieldTypeName = value;
			}
		}

		private uint rva;
		public uint Rva
		{
			get
			{
				return rva;
			}
			private set
			{
				rva = value;
			}
		}

		private CorMethodImpl implementationFlags;
		public CorMethodImpl ImplementationFlags
		{
			get
			{
				return implementationFlags;
			}
			private set
			{
				implementationFlags = value;
			}
		}

		private string defaultValueAsString;
		public string DefaultValueAsString
		{
			get
			{
				ReadMetadata();

				return defaultValueAsString;
			}
			private set
			{
				defaultValueAsString = value;
			}
		}

		private string marshalAsTypeString;
		public string MarshalAsTypeString
		{
			get
			{
				ReadMetadata();

				return marshalAsTypeString;
			}
			private set
			{
				marshalAsTypeString = value;
			}
		}

		private string pinvokeMap;
		public string PinvokeMap
		{
			get
			{
				ReadMetadata();

				return pinvokeMap;
			}
			private set
			{
				pinvokeMap = value;
			}
		}

		private FieldSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		[ThreadStatic()]
		private static StringBuilder fieldTypeNameBuilder;
		private static StringBuilder FieldTypeNameBuilder
		{
			get
			{
				if (fieldTypeNameBuilder == null)
				{
					fieldTypeNameBuilder = new StringBuilder();
				}

				return fieldTypeNameBuilder;
			}
		}

		public FieldDefinition(IMetaDataImport2 import, TypeDefinition typeDefinition, string name, uint token, IntPtr signatureBlob, uint signatureBlobLength, uint flags, uint defaultValueType, IntPtr defaultValue, uint defaultValueLength)
		{
			BaseTypeDefinition = typeDefinition;
			Name = name;
			Token = token;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
			Flags = (CorFieldAttr)flags;
			DefaultValueType = (CorElementType)defaultValueType;
			DefaultValue = defaultValue;
			DefaultValueLength = defaultValueLength;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new FieldSignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, SignatureBlob, SignatureBlobLength);
				signatureReader.ReadSignature();
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorFieldAttr memberAccess = Flags & CorFieldAttr.fdFieldAccessMask;

			switch (memberAccess)
			{
				case CorFieldAttr.fdPrivateScope:
					result = "privatescope ";
					break;

				case CorFieldAttr.fdPrivate:
					result = "private ";
					break;

				case CorFieldAttr.fdFamANDAssem:
					result = "famandassem ";
					break;

				case CorFieldAttr.fdAssembly:
					result = "assembly ";
					break;

				case CorFieldAttr.fdFamily:
					result = "family ";
					break;

				case CorFieldAttr.fdFamORAssem:
					result = "famorassem ";
					break;

				case CorFieldAttr.fdPublic:
					result = "public ";
					break;
			}

			return result;
		}

		private string ContractAttributesAsString()
		{
			string result = string.Empty;

			result = HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdStatic, "static ");
			result += HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdInitOnly, "initonly ");
			result += HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdLiteral, "literal ");
			result += HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdNotSerialized, "notserialized ");
			result += HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdSpecialName, "specialname ");

			if ((Flags & CorFieldAttr.fdPinvokeImpl) == CorFieldAttr.fdPinvokeImpl)
			{
				result += "pinvokeimpl(";

				if (PinvokeMap == null)
				{
					result += "/* No map */";
				}
				else
				{
					result += PinvokeMap;
				}

				result += ") ";
			}

			return result;
		}

		private string ReservedFlagsAsString()
		{
			string result = string.Empty;
			CorFieldAttr reservedFlags = Flags & CorFieldAttr.fdReservedMask;

			result = HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdRTSpecialName, "rtsspecialname ");

			result += HelperFunctions.EnumAsString(Flags, CorFieldAttr.fdHasFieldMarshal, "marshal ");

			return result;
		}

		public void Initialize()
		{
			ReadMetadata();
			CodeLines = new List<CodeLine>();
			CodeLines.Add(new CodeLine(0, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));
			StringBuilder definition = new StringBuilder();

			definition.Append(".field ");
			definition.Append(ContractAttributesAsString());
			definition.Append(MemberAccessAsString());
			definition.Append(ReservedFlagsAsString());
			definition.Append(FieldTypeName);
			definition.Append(" ");
			definition.Append(Name);

			if ((Flags & CorFieldAttr.fdHasDefault) == CorFieldAttr.fdHasDefault)
			{
				definition.Append(" = ");
				definition.Append(DefaultValueAsString);
			}

			if (((Flags & CorFieldAttr.fdReservedMask) & CorFieldAttr.fdHasFieldRVA) == CorFieldAttr.fdHasFieldRVA)
			{
				definition.Append(" at D_");
				definition.Append(HelperFunctions.FormatAsHexNumber(Rva, 8));
			}

			CodeLines.Add(new CodeLine(0, definition.ToString()));

			if (CustomAttributes != null)
			{
				Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				foreach (CustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(assembly.AllTokens);
					CodeLines.Add(new CodeLine(0, customAttribute.Name));
				}
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			BaseSignatureItem signatureItem = signatureReader.Type;
			HelperFunctions.SetSignatureItemToken(allTokens, signatureItem);

			if (((Flags & CorFieldAttr.fdReservedMask) & CorFieldAttr.fdHasFieldRVA) == CorFieldAttr.fdHasFieldRVA)
			{
				Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

				try
				{
					assembly.OpenMetadataInterfaces();
					uint rva;
					uint implFlags;

					assembly.Import.GetRVA(Token, out rva, out implFlags);

					Rva = rva;
					ImplementationFlags = (CorMethodImpl)implFlags;
				}
				finally
				{
					assembly.CloseMetadataInterfaces();
				}
			}
		}

		public bool IsReformattableDefaultValue()
		{
			return (DefaultValueNumber != null);
		}

		public string GetFormattedDefaultValue()
		{
			string result = null;

			if (IsReformattableDefaultValue())
			{
				result = HelperFunctions.FormatNumber(DefaultValueNumber);
			}
			else
			{
				result = DefaultValueAsString;
			}

			return result;
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				if ((Flags & CorFieldAttr.fdHasDefault) == CorFieldAttr.fdHasDefault)
				{
					object defaultValueNumber;
					DefaultValueAsString = HelperFunctions.ReadDefaultValue(DefaultValueType, DefaultValue, DefaultValueLength, out defaultValueNumber);
					DefaultValueNumber = defaultValueNumber;
				}

				if ((Flags & CorFieldAttr.fdHasFieldMarshal) == CorFieldAttr.fdHasFieldMarshal)
				{
					MarshalAsTypeString = string.Format("marshal({0})", HelperFunctions.ReadMarshalDescriptor(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly.AllTokens, Token, 0));
				}

				if ((Flags & CorFieldAttr.fdPinvokeImpl) == CorFieldAttr.fdPinvokeImpl)
				{
					PinvokeMap = HelperFunctions.ReadPinvokeMap(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, Token, Name);
				}

				CustomAttributes = HelperFunctions.EnumCustomAttributes(assembly.Import, BaseTypeDefinition.ModuleScope.Assembly, this);
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}

		public override string ToString()
		{
			LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);

			return Text;
		}
	}
}