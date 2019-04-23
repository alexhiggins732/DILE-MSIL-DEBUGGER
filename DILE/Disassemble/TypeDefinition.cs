using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Debug.Expressions;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;
using Dile.UI.Debug;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class TypeDefinition : TextTokenBase, IMultiLine
	{
		public bool LoadedFromMemory
		{
			get
			{
				return ModuleScope.Assembly.LoadedFromMemory;
			}
		}

		public bool IsInMemory
		{
			get
			{
				return ModuleScope.Assembly.IsInMemory;
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.TypeDefinition;
			}
		}

		private ModuleScope moduleScope;
		public ModuleScope ModuleScope
		{
			get
			{
				return moduleScope;
			}
			private set
			{
				moduleScope = value;
			}
		}

		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value.TrimEnd('\0');
			}
		}

		private CorTypeAttr flags;
		public CorTypeAttr Flags
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

		private uint baseTypeToken;
		public uint BaseTypeToken
		{
			get
			{
				return baseTypeToken;
			}
			private set
			{
				baseTypeToken = value;
			}
		}

		private Dictionary<uint, MethodDefinition> methodDefinitions;
		public Dictionary<uint, MethodDefinition> MethodDefinitions
		{
			get
			{
				return methodDefinitions;
			}
			private set
			{
				methodDefinitions = value;
			}
		}

		private Dictionary<uint, FieldDefinition> fieldDefinitions;
		public Dictionary<uint, FieldDefinition> FieldDefinitions
		{
			get
			{
				return fieldDefinitions;
			}
			private set
			{
				fieldDefinitions = value;
			}
		}

		private bool isNestedType = false;
		public bool IsNestedType
		{
			get
			{
				return isNestedType;
			}
			private set
			{
				isNestedType = value;
			}
		}

		private TypeDefinition enclosingType = null;
		public TypeDefinition EnclosingType
		{
			get
			{
				return enclosingType;
			}
			private set
			{
				enclosingType = value;
			}
		}

		private string shortName = null;
		public string ShortName
		{
			get
			{
				if (shortName == null)
				{
					CreateFullName();
				}

				return shortName;
			}
			private set
			{
				shortName = value;
			}
		}

		private string fullName = null;
		public string FullName
		{
			get
			{
				if (fullName == null)
				{
					CreateFullName();
				}

				return fullName;
			}
			private set
			{
				fullName = value;
			}
		}

		public string Namespace
		{
			get
			{
				string result = string.Empty;
				int lastDotPosition = ShortName.LastIndexOf('.');

				if (lastDotPosition > -1)
				{
					result = ShortName.Substring(0, lastDotPosition);
				}

				return result;
			}
		}

		private List<InterfaceImplementation> interfaceImplementations;
		public List<InterfaceImplementation> InterfaceImplementations
		{
			get
			{
				return interfaceImplementations;
			}
			private set
			{
				interfaceImplementations = value;
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

		private Dictionary<uint, Property> properties;
		public Dictionary<uint, Property> Properties
		{
			get
			{
				return properties;
			}
			private set
			{
				properties = value;
			}
		}

		private Dictionary<uint, EventDefinition> eventDefinitions;
		public Dictionary<uint, EventDefinition> EventDefinitions
		{
			get
			{
				return eventDefinitions;
			}
			private set
			{
				eventDefinitions = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return Name;
			}
		}

		private uint packSize;
		public uint PackSize
		{
			get
			{
				return packSize;
			}
			private set
			{
				packSize = value;
			}
		}

		private uint classSize;
		public uint ClassSize
		{
			get
			{
				return classSize;
			}
			private set
			{
				classSize = value;
			}
		}

		private bool layoutSpecified;
		public bool LayoutSpecified
		{
			get
			{
				return layoutSpecified;
			}
			private set
			{
				layoutSpecified = value;
			}
		}

		private List<PermissionSet> permissionSets;
		public List<PermissionSet> PermissionSets
		{
			get
			{
				ReadMetadata();

				return permissionSets;
			}
			private set
			{
				permissionSets = value;
			}
		}

		private List<GenericParameter> genericParameters;
		public List<GenericParameter> GenericParameters
		{
			get
			{
				return genericParameters;
			}
			private set
			{
				genericParameters = value;
			}
		}

		[ThreadStatic()]
		private static StringBuilder textBuilder;
		private static StringBuilder TextBuilder
		{
			get
			{
				if (textBuilder == null)
				{
					textBuilder = new StringBuilder();
				}

				return textBuilder;
			}
		}

		[ThreadStatic()]
		private static StringBuilder fullNameBuilder;
		private static StringBuilder FullNameBuilder
		{
			get
			{
				if (fullNameBuilder == null)
				{
					fullNameBuilder = new StringBuilder();
				}

				return fullNameBuilder;
			}
		}

		private List<TypeDefinition> nestedTypes = null;
		private List<TypeDefinition> NestedTypes
		{
			get
			{
				return nestedTypes;
			}
			set
			{
				nestedTypes = value;
			}
		}

		public bool IsValueType
		{
			get
			{
				bool result = false;

				if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
				{
					TokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
					TypeDefinition baseType = baseTypeTokenObject as TypeDefinition;

					if (baseType == null)
					{
						TypeReference baseTypeReference = baseTypeTokenObject as TypeReference;

						if (baseTypeReference != null)
						{
							baseType = HelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly);
						}
					}

					if (baseType != null && baseType.FullName == Dile.Constants.ValueTypeName)
					{
						result = true;
					}
				}

				return result;
			}
		}

		public TypeDefinition(IMetaDataImport2 import, ModuleScope moduleScope, uint token)
			: this(import, moduleScope, string.Empty, token, CorTypeAttr.tdAbstract, 0)
		{
		}

		public TypeDefinition(IMetaDataImport2 import, ModuleScope moduleScope, string name, uint token, CorTypeAttr flags, uint baseTypeToken)
		{
			ModuleScope = moduleScope;
			Name = name;
			Token = token;
			Flags = flags;
			BaseTypeToken = baseTypeToken;

			IsNestedType = (((Flags & CorTypeAttr.tdNestedAssembly) == CorTypeAttr.tdNestedAssembly) || ((Flags & CorTypeAttr.tdNestedFamANDAssem) == CorTypeAttr.tdNestedFamANDAssem) || ((Flags & CorTypeAttr.tdNestedFamily) == CorTypeAttr.tdNestedFamily) || ((Flags & CorTypeAttr.tdNestedFamORAssem) == CorTypeAttr.tdNestedFamORAssem) || ((Flags & CorTypeAttr.tdNestedPrivate) == CorTypeAttr.tdNestedPrivate) || ((Flags & CorTypeAttr.tdNestedPublic) == CorTypeAttr.tdNestedPublic));

			HelperFunctions.GetMemberReferences(ModuleScope.Assembly, Token);
			GetFieldDefinitions(import);
			GetMethodDefinitions(import);
			GetMethodImplementations(import);
			GetImplementedInterfaces(import);
			GetClassLayout(import);
			GetProperties(import);
			GetEvents(import);
			GenericParameters = HelperFunctions.EnumGenericParameters(import, ModuleScope.Assembly, this);
		}

		private void GetClassLayout(IMetaDataImport2 import)
		{
			CorTypeAttr classLayout = Flags & CorTypeAttr.tdLayoutMask;

			if (HelperFunctions.EnumContainsValue(classLayout, CorTypeAttr.tdExplicitLayout) || HelperFunctions.EnumContainsValue(classLayout, CorTypeAttr.tdSequentialLayout))
			{
				COR_FIELD_OFFSET[] fieldOffsets = new COR_FIELD_OFFSET[Project.DefaultArrayCount];
				uint count;

				try
				{
					import.GetClassLayout(Token, out packSize, fieldOffsets, Convert.ToUInt32(fieldOffsets.Length), out count, out classSize);

					LayoutSpecified = true;
				}
				catch (COMException)
				{
					LayoutSpecified = false;
				}
			}
			else
			{
				LayoutSpecified = false;
			}
		}

		private void GetMethodDefinitions(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] methodDefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumMethods(ref enumHandle, Token, methodDefs, Convert.ToUInt32(methodDefs.Length), out count);

			if (count > 0)
			{
				MethodDefinitions = new Dictionary<uint, MethodDefinition>();
			}

			while (count > 0)
			{
				for (uint methodDefsIndex = 0; methodDefsIndex < count; methodDefsIndex++)
				{
					uint token = methodDefs[methodDefsIndex];
					uint typeDefToken;
					uint methodNameLength;
					uint methodFlags;
					IntPtr signature;
					uint signatureLength;
					uint rva;
					uint implementationFlags;

					import.GetMethodProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out methodNameLength, out methodFlags, out signature, out signatureLength, out rva, out implementationFlags);

					if (methodNameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[methodNameLength];

						import.GetMethodProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out methodNameLength, out methodFlags, out signature, out signatureLength, out rva, out implementationFlags);
					}

					MethodDefinition methodDefinition = new MethodDefinition(import, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, methodNameLength), token, methodFlags, signature, signatureLength, rva, implementationFlags);
					MethodDefinitions[token] = methodDefinition;
					ModuleScope.Assembly.AllTokens[token] = methodDefinition;
				}

				import.EnumMethods(ref enumHandle, Token, methodDefs, Convert.ToUInt32(methodDefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		private void GetMethodImplementations(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] methodBodies = new uint[Project.DefaultArrayCount];
			uint[] methodImpls = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumMethodImpls(ref enumHandle, Token, methodBodies, methodImpls, Convert.ToUInt32(methodImpls.Length), out count);

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					MethodDefinitions[methodBodies[index]].Overrides = methodImpls[index];
				}

				import.EnumMethodImpls(ref enumHandle, Token, methodBodies, methodImpls, Convert.ToUInt32(methodImpls.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		private void GetFieldDefinitions(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] fieldDefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumFields(ref enumHandle, Token, fieldDefs, Convert.ToUInt32(fieldDefs.Length), out count);

			if (count > 0)
			{
				FieldDefinitions = new Dictionary<uint, FieldDefinition>();
			}

			while (count > 0)
			{
				for (uint fieldDefsIndex = 0; fieldDefsIndex < count; fieldDefsIndex++)
				{
					uint token = fieldDefs[fieldDefsIndex];
					uint typeDefToken;
					uint fieldNameLength;
					uint fieldFlags;
					IntPtr signature;
					uint signatureLength;
					uint defaultValueType;
					IntPtr defaultValue;
					uint defaultValueLength;

					import.GetFieldProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out fieldNameLength, out fieldFlags, out signature, out signatureLength, out defaultValueType, out defaultValue, out defaultValueLength);

					if (fieldNameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[fieldNameLength];

						import.GetFieldProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out fieldNameLength, out fieldFlags, out signature, out signatureLength, out defaultValueType, out defaultValue, out defaultValueLength);
					}

					FieldDefinition fieldDefinition = new FieldDefinition(import, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, fieldNameLength), token, signature, signatureLength, fieldFlags, defaultValueType, defaultValue, defaultValueLength);
					FieldDefinitions[token] = fieldDefinition;
					ModuleScope.Assembly.AllTokens[token] = fieldDefinition;
				}

				import.EnumFields(ref enumHandle, Token, fieldDefs, Convert.ToUInt32(fieldDefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		private void GetImplementedInterfaces(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] interfaces = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumInterfaceImpls(ref enumHandle, Token, interfaces, Convert.ToUInt32(interfaces.Length), out count);

			if (count > 0)
			{
				InterfaceImplementations = new List<InterfaceImplementation>();
			}

			while (count > 0)
			{
				for (uint interfacesIndex = 0; interfacesIndex < count; interfacesIndex++)
				{
					uint token = interfaces[interfacesIndex];
					uint typeDefToken;
					uint interfaceToken;

					import.GetInterfaceImplProps(token, out typeDefToken, out interfaceToken);

					InterfaceImplementation implementation = new InterfaceImplementation(import, token, this, interfaceToken);
					InterfaceImplementations.Add(implementation);
					ModuleScope.Assembly.AllTokens[token] = implementation;
				}

				import.EnumInterfaceImpls(ref enumHandle, Token, interfaces, Convert.ToUInt32(interfaces.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		private void GetProperties(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] propertyTokens = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumProperties(ref enumHandle, Token, propertyTokens, Convert.ToUInt32(propertyTokens.Length), out count);

			if (count > 0)
			{
				Properties = new Dictionary<uint, Property>();
			}

			while (count > 0)
			{
				for (uint propertyTokensIndex = 0; propertyTokensIndex < count; propertyTokensIndex++)
				{
					uint token = propertyTokens[propertyTokensIndex];
					uint typeDefToken;
					uint nameLength;
					uint flags;
					IntPtr signature;
					uint signatureLength;
					uint elementType;
					IntPtr defaultValue;
					uint defaultValueLength;
					uint setterMethodToken;
					uint getterMethodToken;
					uint[] otherMethods = new uint[Project.DefaultArrayCount];
					uint otherMethodsCount;

					import.GetPropertyProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out flags, out signature, out signatureLength, out elementType, out defaultValue, out defaultValueLength, out setterMethodToken, out getterMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);

					if (nameLength > Project.DefaultCharArray.Length || otherMethodsCount > otherMethods.Length)
					{
						Project.DefaultCharArray = new char[nameLength];
						otherMethods = new uint[otherMethodsCount];

						import.GetPropertyProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out flags, out signature, out signatureLength, out elementType, out defaultValue, out defaultValueLength, out setterMethodToken, out getterMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);
					}

					Property property = new Property(import, token, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameLength), flags, signature, signatureLength, elementType, defaultValue, defaultValueLength, setterMethodToken, getterMethodToken, otherMethods, otherMethodsCount);
					Properties[token] = property;
					ModuleScope.Assembly.AllTokens[token] = property;
				}

				import.EnumProperties(ref enumHandle, Token, propertyTokens, Convert.ToUInt32(propertyTokens.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		private void GetEvents(IMetaDataImport2 import)
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] eventTokens = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumEvents(ref enumHandle, Token, eventTokens, Convert.ToUInt32(eventTokens.Length), out count);

			if (count > 0)
			{
				EventDefinitions = new Dictionary<uint, EventDefinition>();
			}

			while (count > 0)
			{
				for (uint eventTokenIndex = 0; eventTokenIndex < count; eventTokenIndex++)
				{
					uint token = eventTokens[eventTokenIndex];
					uint typeDefToken;
					uint nameLength;
					uint flags;
					uint eventClassToken;
					uint addOnMethodToken;
					uint removeOnMethodToken;
					uint fireMethodToken;
					uint[] otherMethods = new uint[Project.DefaultArrayCount];
					uint otherMethodsCount;

					import.GetEventProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out flags, out eventClassToken, out addOnMethodToken, out removeOnMethodToken, out fireMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);

					if (nameLength > Project.DefaultCharArray.Length || otherMethodsCount > otherMethods.Length)
					{
						Project.DefaultCharArray = new char[nameLength];
						otherMethods = new uint[otherMethodsCount];

						import.GetEventProps(token, out typeDefToken, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out flags, out eventClassToken, out addOnMethodToken, out removeOnMethodToken, out fireMethodToken, otherMethods, Convert.ToUInt32(otherMethods.Length), out otherMethodsCount);
					}

					EventDefinition eventDefinition = new EventDefinition(import, token, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameLength), flags, eventClassToken, addOnMethodToken, removeOnMethodToken, fireMethodToken, otherMethods, otherMethodsCount);
					EventDefinitions[token] = eventDefinition;
					ModuleScope.Assembly.AllTokens[token] = eventDefinition;
				}

				import.EnumEvents(ref enumHandle, Token, eventTokens, Convert.ToUInt32(eventTokens.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}
		}

		public void FindEnclosingType(IMetaDataImport2 import)
		{
			if (IsNestedType)
			{
				uint enclosingTypeToken;
				import.GetNestedClassProps(Token, out enclosingTypeToken);

				EnclosingType = ModuleScope.TypeDefinitions[enclosingTypeToken];

				if (EnclosingType.NestedTypes == null)
				{
					EnclosingType.NestedTypes = new List<TypeDefinition>();
				}

				EnclosingType.NestedTypes.Add(this);
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorTypeAttr memberAccess = Flags & CorTypeAttr.tdVisibilityMask;

			switch (memberAccess)
			{
				case CorTypeAttr.tdNotPublic:
					result = "private ";
					break;

				case CorTypeAttr.tdPublic:
					result = "public ";
					break;

				case CorTypeAttr.tdNestedFamANDAssem:
					result = "nested famandassem ";
					break;

				case CorTypeAttr.tdNestedAssembly:
					result = "nested assembly ";
					break;

				case CorTypeAttr.tdNestedFamily:
					result = "nested family ";
					break;

				case CorTypeAttr.tdNestedFamORAssem:
					result = "nested famorassem ";
					break;

				case CorTypeAttr.tdNestedPrivate:
					result = "nested private ";
					break;

				case CorTypeAttr.tdNestedPublic:
					result = "nested public ";
					break;
			}

			return result;
		}

		private string LayoutAsString()
		{
			string result = string.Empty;
			CorTypeAttr layout = Flags & CorTypeAttr.tdLayoutMask;

			switch (layout)
			{
				case CorTypeAttr.tdAutoLayout:
					result = "auto ";
					break;

				case CorTypeAttr.tdSequentialLayout:
					result = "sequential ";
					break;

				case CorTypeAttr.tdExplicitLayout:
					result = "explicit ";
					break;
			}

			return result;
		}

		private string SpecialSemanticsAsString()
		{
			string result = string.Empty;

			if ((Flags & CorTypeAttr.tdAbstract) == CorTypeAttr.tdAbstract)
			{
				result = "abstract ";
			}
			else if ((Flags & CorTypeAttr.tdSealed) == CorTypeAttr.tdSealed)
			{
				result = "sealed ";
			}
			else if ((Flags & CorTypeAttr.tdSpecialName) == CorTypeAttr.tdSpecialName)
			{
				result = "specialname ";
			}

			return result;
		}

		private string ImplementationAsString()
		{
			string result = string.Empty;

			result = HelperFunctions.EnumAsString(Flags, CorTypeAttr.tdImport, "import ");
			result += HelperFunctions.EnumAsString(Flags, CorTypeAttr.tdSerializable, "serializable ");

			return result;
		}

		private string StringFormatAsString()
		{
			string result = string.Empty;
			CorTypeAttr stringFormat = Flags & CorTypeAttr.tdStringFormatMask;

			switch (stringFormat)
			{
				case CorTypeAttr.tdAnsiClass:
					result = "ansi ";
					break;

				case CorTypeAttr.tdUnicodeClass:
					result = "unicode ";
					break;

				case CorTypeAttr.tdAutoClass:
					result = "auto ";
					break;
			}

			return result;
		}

		private string ReservedFlagsAsString()
		{
			string result = string.Empty;

			result = HelperFunctions.EnumAsString(Flags, CorTypeAttr.tdBeforeFieldInit, "beforefieldinit ");

			if ((Flags & CorTypeAttr.tdForwarder) == CorTypeAttr.tdForwarder)
			{
				throw new NotImplementedException("Unknown type flag value (forwarder).");
			}

			CorTypeAttr reservedFlag = Flags & CorTypeAttr.tdReservedMask;

			result += HelperFunctions.EnumAsString(reservedFlag, CorTypeAttr.tdRTSpecialName, "rtspecialname ");

			return result;
		}

		public void Initialize()
		{
			Assembly assembly = ModuleScope.Assembly;
			ReadMetadata();
			CodeLines = new List<CodeLine>();
			TextBuilder.Length = 0;
			TextBuilder.Append(".class ");

			CorTypeAttr semantics = Flags & CorTypeAttr.tdClassSemanticsMask;

			if (semantics == CorTypeAttr.tdInterface)
			{
				TextBuilder.Append("interface ");
			}

			HelperFunctions.AddWordToStringBuilder(TextBuilder, MemberAccessAsString());
			HelperFunctions.AddWordToStringBuilder(TextBuilder, LayoutAsString());
			HelperFunctions.AddWordToStringBuilder(TextBuilder, StringFormatAsString());
			HelperFunctions.AddWordToStringBuilder(TextBuilder, SpecialSemanticsAsString());
			HelperFunctions.AddWordToStringBuilder(TextBuilder, ImplementationAsString());
			HelperFunctions.AddWordToStringBuilder(TextBuilder, ReservedFlagsAsString());
			TextBuilder.Append(Name);

			if (GenericParameters != null)
			{
				TextBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					GenericParameter genericParameter = GenericParameters[index];

					TextBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						TextBuilder.Append(", ");
					}
				}

				TextBuilder.Append(">");
			}

			CodeLine definition = new CodeLine(0, TextBuilder.ToString());
			CodeLines.Add(definition);

			if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				TextBuilder.Length = 0;
				TextBuilder.Append("extends ");
				TextBuilder.Append(ModuleScope.Assembly.AllTokens[BaseTypeToken]);

				CodeLine extends = new CodeLine(1, TextBuilder.ToString());
				CodeLines.Add(extends);
			}

			if (InterfaceImplementations != null && InterfaceImplementations.Count > 0)
			{
				TextBuilder.Length = 0;
				for (int index = 0; index < InterfaceImplementations.Count; index++)
				{
					InterfaceImplementation implementation = InterfaceImplementations[index];

					if (index == 0)
					{
						TextBuilder.Append("implements ");
					}
					else
					{
						TextBuilder.Append(", ");
					}

					TextBuilder.Append(ModuleScope.Assembly.AllTokens[implementation.InterfaceToken]);
				}

				CodeLine implements = new CodeLine(1, TextBuilder.ToString());
				CodeLines.Add(implements);
			}

			CodeLine bracket = new CodeLine(0, "{");
			CodeLines.Add(bracket);
			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			if (LayoutSpecified)
			{
				CodeLines.Add(new CodeLine(1, string.Format(".pack {0}", PackSize)));
				CodeLines.Add(new CodeLine(1, string.Format(".size {0}", ClassSize)));
			}

			if (CustomAttributes != null)
			{
				foreach (CustomAttribute customAttribute in CustomAttributes)
				{
					customAttribute.SetText(ModuleScope.Assembly.AllTokens);
					CodeLines.Add(new CodeLine(1, customAttribute.Name));
				}
			}

			if (PermissionSets != null)
			{
				foreach (PermissionSet permissionSet in PermissionSets)
				{
					permissionSet.SetText(assembly.AllTokens);
					CodeLines.Add(new CodeLine(1, permissionSet.Name));
				}
			}

			bracket = new CodeLine(0, "} //end of class " + FullName);
			CodeLines.Add(bracket);
		}

		private void CreateFullName()
		{
			FullNameBuilder.Length = 0;
			TypeDefinition typeDef = this;

			while (typeDef != null)
			{
				FullNameBuilder.Insert(0, string.Format("{0}/", typeDef.Name));
				typeDef = typeDef.EnclosingType;
			}
			FullNameBuilder.Remove(FullNameBuilder.Length - 1, 1);

			ShortName = FullNameBuilder.ToString();

			if (GenericParameters != null)
			{
				FullNameBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					GenericParameter genericParameter = GenericParameters[index];

					genericParameter.LazyInitialize(ModuleScope.Assembly.AllTokens);
					FullNameBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						FullNameBuilder.Append(", ");
					}
				}

				FullNameBuilder.Append(">");
			}

			FullName = FullNameBuilder.ToString();
		}

		public override string ToString()
		{
			return FullName;
		}

		private K FindTokenBaseByName<T, K>(Dictionary<T, K> tokenBaseObjects, string tokenBaseName, MemberType memberTypeToSearch) where K : TokenBase
		{
			K result = null;

			if (tokenBaseObjects != null && tokenBaseObjects.Count > 0)
			{
				Dictionary<T, K>.ValueCollection values = tokenBaseObjects.Values;
				Dictionary<T, K>.ValueCollection.Enumerator enumerator = values.GetEnumerator();

				while (result == null && enumerator.MoveNext())
				{
					K value = enumerator.Current;

					if (value.Name == tokenBaseName)
					{
						result = value;
					}
				}
			}

			if (result == null && ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				TokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
				TypeDefinition baseType = baseTypeTokenObject as TypeDefinition;

				if (baseType == null)
				{
					TypeReference baseTypeReference = baseTypeTokenObject as TypeReference;

					if (baseTypeReference != null)
					{
						baseType = HelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly);
					}
				}

				if (baseType != null)
				{
					switch (memberTypeToSearch)
					{
						case MemberType.Field:
							result = (K)(TokenBase)baseType.FindTokenBaseByName<uint, FieldDefinition>(baseType.FieldDefinitions, tokenBaseName, memberTypeToSearch);
							break;

						case MemberType.Method:
							result = (K)(TokenBase)baseType.FindTokenBaseByName<uint, MethodDefinition>(baseType.MethodDefinitions, tokenBaseName, memberTypeToSearch);
							break;

						case MemberType.Property:
							result = (K)(TokenBase)baseType.FindTokenBaseByName<uint, Property>(baseType.Properties, tokenBaseName, memberTypeToSearch);
							break;
					}
				}
			}

			return result;
		}

		public Property FindPropertyByName(string propertyName)
		{
			propertyName = HelperFunctions.QuoteName(propertyName);

			return FindTokenBaseByName<uint, Property>(Properties, propertyName, MemberType.Property);
		}

		public MethodDefinition FindMethodDefinitionByName(string methodName)
		{
			methodName = HelperFunctions.QuoteMethodName(methodName);

			return FindTokenBaseByName<uint, MethodDefinition>(MethodDefinitions, methodName, MemberType.Method);
		}

		public List<MethodDefinition> FindMethodDefinitionsByName(string methodName, int parameterCount)
		{
			List<MethodDefinition> result = new List<MethodDefinition>();

			if (MethodDefinitions != null && MethodDefinitions.Count > 0)
			{
				methodName = HelperFunctions.QuoteMethodName(methodName);
				Dictionary<uint, MethodDefinition>.ValueCollection values = MethodDefinitions.Values;
				Dictionary<uint, MethodDefinition>.ValueCollection.Enumerator enumerator = values.GetEnumerator();

				while (enumerator.MoveNext())
				{
					int expectedParameterCountTemp = (enumerator.Current.IsStatic || enumerator.Current.Name == Dile.Constants.ConstructorMethodName ? parameterCount : parameterCount - 1);

					if (enumerator.Current.Name == methodName)
					{
						if (enumerator.Current.Parameters == null)
						{
							if (expectedParameterCountTemp == 0)
							{
								result.Add(enumerator.Current);
							}
						}
						else if (enumerator.Current.Parameters.Count == expectedParameterCountTemp)
						{
							result.Add(enumerator.Current);
						}
						else if (enumerator.Current.Parameters.Count < expectedParameterCountTemp || enumerator.Current.Parameters.Count == expectedParameterCountTemp + 1)
						{
							Parameter lastParameter = enumerator.Current.Parameters[enumerator.Current.Parameters.Count - 1];

							if (lastParameter.ParamArrayAttributeExists())
							{
								result.Add(enumerator.Current);
							}
						}
					}
				}

				if (ModuleScope.Assembly.AllTokens.ContainsKey(BaseTypeToken))
				{
					TokenBase baseTypeTokenObject = ModuleScope.Assembly.AllTokens[BaseTypeToken];
					TypeDefinition baseType = baseTypeTokenObject as TypeDefinition;

					if (baseType == null)
					{
						TypeReference baseTypeReference = baseTypeTokenObject as TypeReference;

						if (baseTypeReference != null)
						{
							baseType = HelperFunctions.FindTypeByName(baseTypeReference.Name, baseTypeReference.ReferencedAssembly) as TypeDefinition;
						}
					}

					if (baseType != null)
					{
						result.AddRange(baseType.FindMethodDefinitionsByName(methodName, parameterCount));
					}
				}
			}

			return result;
		}

		public FieldDefinition FindFieldDefinitionByName(string fieldName)
		{
			fieldName = HelperFunctions.QuoteName(fieldName);
			return FindTokenBaseByName<uint, FieldDefinition>(FieldDefinitions, fieldName, MemberType.Field);
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			Assembly assembly = ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = HelperFunctions.EnumCustomAttributes(assembly.Import, ModuleScope.Assembly, this);

				if (((Flags & CorTypeAttr.tdReservedMask) & CorTypeAttr.tdHasSecurity) == CorTypeAttr.tdHasSecurity)
				{
					PermissionSets = HelperFunctions.EnumPermissionSets(assembly.Import, assembly, Token);
				}
			}
			finally
			{
				assembly.CloseMetadataInterfaces();
			}
		}

		public TypeDefinition FindNestedTypeByName(string nestedTypeName, int typeArgumentCount)
		{
			TypeDefinition result = null;

			if (typeArgumentCount > 0)
			{
				nestedTypeName = nestedTypeName + "`" + Convert.ToString(typeArgumentCount);
			}

			if (NestedTypes != null && NestedTypes.Count > 0)
			{
				int index = 0;

				while (result == null && index < NestedTypes.Count)
				{
					TypeDefinition nestedType = NestedTypes[index++];

					if (nestedType.Name == nestedTypeName)
					{
						result = nestedType;
					}
				}
			}

			return result;
		}

		public MethodDefinition FindImplicitCastOperator(EvaluationContext context, DebugExpressionResult parameter, IMethodParameter expectedParameter, bool isExpectedParameterArray)
		{
			if (HelperFunctions.IsArrayElementType(expectedParameter.ElementType))
			{
				expectedParameter = expectedParameter.ArrayElementType;
			}

			TypeDefinition expectedParameterType = expectedParameter.TokenObject as TypeDefinition;

			if (expectedParameterType == null)
			{
				TypeReference typeReference = expectedParameter.TokenObject as TypeReference;

				if (typeReference == null)
				{
					if (expectedParameter.ElementType != CorElementType.ELEMENT_TYPE_PTR)
					{
						expectedParameterType = HelperFunctions.GetTypeByElementType(expectedParameter.ElementType);

						if (expectedParameterType == null)
						{
							throw new InvalidOperationException(string.Format("The definition of the type ({0}) could not be found.", Enum.GetName(typeof(CorElementType), expectedParameter.ElementType)));
						}
					}
				}
				else
				{
					expectedParameterType = HelperFunctions.FindTypeByName(typeReference.Name, typeReference.ReferencedAssembly);

					if (expectedParameterType == null)
					{
						throw new InvalidOperationException(string.Format("The type definition of {0} could not be found. Perhaps the {1} assembly is not loaded.", typeReference.FullName, typeReference.ReferencedAssembly));
					}
				}
			}

			return FindImplicitCastOperator(context, parameter, expectedParameterType, isExpectedParameterArray);
		}

		public MethodDefinition FindImplicitCastOperator(EvaluationContext context, DebugExpressionResult parameter, TypeDefinition expectedParameterType, bool isExpectedParameterArray)
		{
			MethodDefinition result = null;

			List<DebugExpressionResult> parameters = new List<DebugExpressionResult>(1);
			parameters.Add(parameter);

			if (HelperFunctions.HasValueClass(parameter.ResultValue))
			{
				TypeDefinition parameterType = HelperFunctions.FindTypeOfValue(context, parameter);

				if (parameterType == null)
				{
					throw new InvalidOperationException("The type of a parameter value could not be determined while searching for implicit cast operators.");
				}

				if (parameterType != this)
				{
					result = parameterType.FindMethodDefinitionByParameter(context, "op_Implicit", null, null, parameters, expectedParameterType, isExpectedParameterArray);
				}
			}

			if (expectedParameterType != this)
			{
				MethodDefinition implicitOperatorMethod = expectedParameterType.FindMethodDefinitionByParameter(context, "op_Implicit", null, null, parameters, expectedParameterType, isExpectedParameterArray);

				if (implicitOperatorMethod != null)
				{
					if (result != null)
					{
						throw new InvalidOperationException(string.Format("Two suitable implicit cast operators have been found: {0}, {1}", result.Text, implicitOperatorMethod.Text));
					}

					result = implicitOperatorMethod;
				}
			}

			return result;
		}

		private bool CanConvert(CorElementType valueType, IMethodParameter expectedType)
		{
			bool result = false;

			switch (valueType)
			{
				case CorElementType.ELEMENT_TYPE_CHAR:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_U2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I1:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_I2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U1:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_U:
						case CorElementType.ELEMENT_TYPE_U2:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I2:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U2:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I:
						case CorElementType.ELEMENT_TYPE_U:
						case CorElementType.ELEMENT_TYPE_I4:
						case CorElementType.ELEMENT_TYPE_U4:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I4:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_U4:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_U8:
						case CorElementType.ELEMENT_TYPE_I8:
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_I8:
				case CorElementType.ELEMENT_TYPE_U8:
					switch (expectedType.ElementType)
					{
						case CorElementType.ELEMENT_TYPE_R4:
						case CorElementType.ELEMENT_TYPE_R8:
							result = true;
							break;

						case CorElementType.ELEMENT_TYPE_VALUETYPE:
							if (expectedType.TokenObject.Name == Dile.Constants.DecimalTypeName)
							{
								result = true;
							}
							break;
					}
					break;

				case CorElementType.ELEMENT_TYPE_R4:
					if (expectedType.ElementType == CorElementType.ELEMENT_TYPE_R8)
					{
						result = true;
					}
					break;
			}

			return result;
		}

		public bool IsSubclassOrImplements(TypeDefinition typeDefinition)
		{
			bool result = false;

			if (typeDefinition == this)
			{
				result = true;
			}
			else
			{
				TypeDefinition baseType = HelperFunctions.FindTypeByToken(ModuleScope.Assembly, BaseTypeToken);

				if (baseType != null)
				{
					result = baseType.IsSubclassOrImplements(typeDefinition);
				}

				if (!result && InterfaceImplementations != null)
				{
					int index = 0;

					while (!result && index < InterfaceImplementations.Count)
					{
						InterfaceImplementation interfaceImplementation = InterfaceImplementations[index++];
						TypeDefinition interfaceTypeDefinition = HelperFunctions.FindTypeByToken(ModuleScope.Assembly, interfaceImplementation.InterfaceToken);

						result = interfaceTypeDefinition.IsSubclassOrImplements(typeDefinition);
					}
				}
			}

			return result;
		}

		private bool ParameterTypesMatch(TypeDefinition currentParameterType, IMethodParameter definedParameter)
		{
			bool result = false;

			if (currentParameterType != null)
			{
				if (currentParameterType.FullName == definedParameter.GetTokenObjectName(false))
				{
					result = true;
				}
				else
				{
					CorElementType currentParameterElementType = HelperFunctions.GetElementTypeByName(currentParameterType.FullName);

					if (currentParameterElementType != CorElementType.ELEMENT_TYPE_END && currentParameterElementType == definedParameter.ElementType)
					{
						result = true;
					}
					else
					{
						TypeDefinition currentParameterBaseType = HelperFunctions.FindTypeByToken(currentParameterType.ModuleScope.Assembly, currentParameterType.BaseTypeToken);

						if (currentParameterBaseType != null)
						{
							result = ParameterTypesMatch(currentParameterBaseType, definedParameter);
						}

						if (!result && currentParameterType != null && currentParameterType.InterfaceImplementations != null)
						{
							int index = 0;

							while (!result && index < currentParameterType.InterfaceImplementations.Count)
							{
								InterfaceImplementation interfaceImplementation = currentParameterType.InterfaceImplementations[index++];
								TypeDefinition interfaceTypeDefinition = HelperFunctions.FindTypeByToken(currentParameterType.ModuleScope.Assembly, interfaceImplementation.InterfaceToken);

								result = ParameterTypesMatch(interfaceTypeDefinition, definedParameter);
							}
						}
					}
				}
			}

			return result;
		}

		private bool ParameterTypesMatch(EvaluationContext context, MethodDefinition methodDefinition, DebugExpressionResult currentParameter, int parameterIndex, bool automaticConversionEnabled, bool isParamsParameter, TypeTreeNodeList typeArguments, TypeTreeNodeList methodTypeArguments)
		{
			IMethodParameter definedParameter = null;
			BaseSignatureItem definedParameterItem = methodDefinition.MethodSignatureReader.Parameters[parameterIndex];
			VarSignatureItem genericParameter = definedParameterItem as VarSignatureItem;

			if (genericParameter != null)
			{
				if (genericParameter.MethodParameter)
				{
					definedParameter = methodTypeArguments.GetItemAsGenericParameter(genericParameter.Number);
				}
				else
				{
					definedParameter = typeArguments.GetItemAsGenericParameter(genericParameter.Number);
				}
			}
			else
			{
				definedParameter = (IMethodParameter)methodDefinition.MethodSignatureReader.Parameters[parameterIndex];
			}

			return ParameterTypesMatch(context, methodDefinition, currentParameter, definedParameter, automaticConversionEnabled, isParamsParameter);
		}

		private bool ParameterTypesMatch(EvaluationContext context, MethodDefinition methodDefinition, DebugExpressionResult currentParameter, IMethodParameter definedParameter, bool automaticConversionEnabled, bool isParamsParameter)
		{
			bool result = false;

			if (isParamsParameter)
			{
				IMethodParameter arrayElementItem = definedParameter.ArrayElementType;
				definedParameter = arrayElementItem;
			}

			CorElementType currentParameterType = (CorElementType)currentParameter.ResultValue.ElementType;
			CorElementType definedParameterType = definedParameter.ElementType;

			if (definedParameterType == CorElementType.ELEMENT_TYPE_CLASS ||
				definedParameterType == CorElementType.ELEMENT_TYPE_VALUETYPE)
			{
				TypeDefinition definedParameterTypeDefinition = definedParameter.TokenObject as TypeDefinition;

				if (definedParameterTypeDefinition != null && definedParameterTypeDefinition.GetIsEnum())
				{
					definedParameterType = definedParameterTypeDefinition.GetEnumElementType();
				}
			}

			if (HelperFunctions.HasValueClass(currentParameterType) && HelperFunctions.HasValueClass(definedParameterType))
			{
				result = true;
			}

			if (result || (currentParameterType == definedParameterType) ||
				(automaticConversionEnabled && (CanConvert(currentParameterType, definedParameter))))
			{
				if (result && (currentParameterType == CorElementType.ELEMENT_TYPE_OBJECT || definedParameterType == CorElementType.ELEMENT_TYPE_OBJECT))
				{
					result = true;
				}
				else if (HelperFunctions.HasValueClass(currentParameterType))
				{
					TypeDefinition currentParameterTypeDefinition = HelperFunctions.FindTypeOfValue(context, currentParameter);

					result = ParameterTypesMatch(currentParameterTypeDefinition, definedParameter);
				}
				else
				{
					result = true;
				}
			}

			if (!result && automaticConversionEnabled && methodDefinition.Name != Dile.Constants.ImplicitOperatorMethodName && FindImplicitCastOperator(context, currentParameter, definedParameter, HelperFunctions.IsArrayElementType(definedParameter.ElementType)) != null)
			{
				result = true;
			}

			return result;
		}

		public MethodDefinition FindMethodDefinitionByParameter(EvaluationContext context, string methodName, TypeTreeNodeList typeArguments, TypeTreeNodeList methodTypeArguments, List<DebugExpressionResult> parameters)
		{
			MethodDefinition result = null;

			if (MethodDefinitions != null && MethodDefinitions.Count > 0)
			{
				result = FindMethodDefinitionByParameter(context, methodName, typeArguments, methodTypeArguments, parameters, null, false);
			}

			return result;
		}

		public MethodDefinition FindMethodDefinitionByParameter(EvaluationContext context, string methodName, TypeTreeNodeList typeArguments, TypeTreeNodeList methodTypeArguments, List<DebugExpressionResult> parameters, TypeDefinition expectedReturnType, bool isArrayReturnTypeExpected)
		{
			List<MethodDefinition> possibleMethods = FindMethodDefinitionsByName(methodName, parameters.Count);
			MethodDefinition result = null;

			result = SearchPossibleMethods(context, typeArguments, methodTypeArguments, parameters, possibleMethods, expectedReturnType, isArrayReturnTypeExpected, false);

			return result;
		}

		private MethodDefinition SearchPossibleMethods(EvaluationContext context, TypeTreeNodeList typeArguments, TypeTreeNodeList methodTypeArguments, List<DebugExpressionResult> parameters, List<MethodDefinition> possibleMethods, TypeDefinition expectedReturnType, bool isArrayReturnTypeExpected, bool isRecursiveCall)
		{
			MethodDefinition result = null;
			int methodIndex = 0;
			bool paramsParameterFound = false;

			while (result == null && methodIndex < possibleMethods.Count)
			{
				paramsParameterFound = false;
				MethodDefinition method = possibleMethods[methodIndex++];
				int parameterIndexModifier = (method.IsStatic || method.Name == Dile.Constants.ConstructorMethodName ? 0 : 1);
				bool allParameterTypesMatch = true;

				if (method.Parameters == null)
				{
					if (parameters.Count > parameterIndexModifier)
					{
						allParameterTypesMatch = false;
					}
					else if (!method.IsStatic && method.Name != Dile.Constants.ConstructorMethodName)
					{
						if (parameters.Count == 0)
						{
							allParameterTypesMatch = false;
						}
						else
						{
							TypeDefinition thisTypeDefinition = HelperFunctions.FindTypeOfValue(context, parameters[0]);

							allParameterTypesMatch = HelperFunctions.TypeDefinitionsMatch(thisTypeDefinition, method.BaseTypeDefinition);
						}
					}
				}

				if (method.Parameters != null && allParameterTypesMatch)
				{
					int parameterIndex = parameterIndexModifier;
					int methodParameterIndex = 0;

					while (allParameterTypesMatch && parameterIndex < parameters.Count)
					{
						if (methodParameterIndex == method.Parameters.Count - 1 && !paramsParameterFound)
						{
							paramsParameterFound = method.Parameters[methodParameterIndex].ParamArrayAttributeExists();
						}

						if (ParameterTypesMatch(context, method, parameters[parameterIndex], methodParameterIndex, isRecursiveCall, paramsParameterFound, typeArguments, methodTypeArguments))
						{
							parameterIndex++;

							if (methodParameterIndex < method.Parameters.Count - 1)
							{
								methodParameterIndex++;
							}
						}
						else
						{
							allParameterTypesMatch = false;
						}
					}

					if (method.Parameters.Count != parameters.Count && !paramsParameterFound)
					{
						paramsParameterFound = method.Parameters[method.Parameters.Count - 1].ParamArrayAttributeExists();
					}
				}

				if (allParameterTypesMatch && expectedReturnType != null)
				{
					TypeSignatureItem returnSignature = (TypeSignatureItem)method.MethodSignatureReader.ReturnType;

					if (isArrayReturnTypeExpected)
					{
						if (HelperFunctions.IsArrayElementType(returnSignature.ElementType))
						{
							returnSignature = (TypeSignatureItem)returnSignature.NextItem;
						}
						else
						{
							allParameterTypesMatch = false;
						}
					}

					if (allParameterTypesMatch && !ParameterTypesMatch(expectedReturnType, returnSignature))
					{
						allParameterTypesMatch = false;
					}
				}

				if (allParameterTypesMatch)
				{
					result = method;

					if (isRecursiveCall || paramsParameterFound)
					{
						ExecuteAutomaticParameterConversion(context, parameters, method, parameterIndexModifier, paramsParameterFound);
					}
				}
			}

			if (result == null && !isRecursiveCall)
			{
				result = SearchPossibleMethods(context, typeArguments, methodTypeArguments, parameters, possibleMethods, expectedReturnType, isArrayReturnTypeExpected, true);
			}

			return result;
		}

		private void ExecuteAutomaticParameterConversion(EvaluationContext context, List<DebugExpressionResult> parameters, MethodDefinition method, int parameterIndexModifier, bool paramsParameterFound)
		{
			if (method.Parameters != null)
			{
				for (int index = 0; index < method.Parameters.Count; index++)
				{
					TypeSignatureItem methodSignatureParameter = (TypeSignatureItem)method.MethodSignatureReader.Parameters[index];

					if (index == method.Parameters.Count - 1 && paramsParameterFound)
					{
						TypeDefinition methodParameterTypeDef = null;
						TypeSignatureItem methodParameterArrayElementType = (TypeSignatureItem)methodSignatureParameter.NextItem;
						ClassWrapper parameterClass = null;

						if (methodParameterArrayElementType.TokenObject != null)
						{
							methodParameterTypeDef = methodParameterArrayElementType.TokenObject as TypeDefinition;

							if (methodParameterTypeDef == null)
							{
								TypeReference methodParameterTypeRef = methodParameterArrayElementType.TokenObject as TypeReference;

								if (methodParameterTypeRef != null)
								{
									methodParameterTypeDef = HelperFunctions.FindTypeByName(methodParameterTypeRef.Name, methodParameterTypeRef.ReferencedAssembly) as TypeDefinition;
								}
							}

							if (methodParameterTypeDef == null)
							{
								throw new InvalidOperationException(string.Format("The type definition of the {0} method's last parameter could not be found.", method.Text));
							}

							parameterClass = HelperFunctions.FindClassOfTypeDefintion(context, methodParameterTypeDef);
						}

						uint arrayLength = Convert.ToUInt32(parameters.Count - index - parameterIndexModifier);
						ValueWrapper paramsArrayValueWrapper = null;
						ArrayValueWrapper paramsArray = null;

						context.EvalWrapper.NewArray((int)methodParameterArrayElementType.ElementType, parameterClass, arrayLength);

						BaseEvaluationResult evaluationResult = context.EvaluationHandler.RunEvaluation(context.EvalWrapper);

						if (evaluationResult.IsSuccessful)
						{
							paramsArrayValueWrapper = evaluationResult.Result;
							paramsArray = paramsArrayValueWrapper.ConvertToArrayValue();
						}
						else
						{
							evaluationResult.ThrowExceptionAccordingToReason();
						}

						int parameterIndex = parameterIndexModifier + index;

						for (int remainingIndex = parameterIndex; remainingIndex < parameters.Count; remainingIndex++)
						{
							DebugExpressionResult parameterExpression = parameters[remainingIndex];

							ConvertParameter(context, parameterExpression, methodParameterArrayElementType);

							paramsArray = paramsArrayValueWrapper.ConvertToArrayValue();

							ValueWrapper paramsArrayElement = paramsArray.GetElementAtPosition(Convert.ToUInt32(remainingIndex - parameterIndex));

							if (HelperFunctions.HasValueClass(parameterExpression.ResultValue))
							{
								paramsArrayElement.SetValue(parameterExpression.ResultValue);
							}
							else
							{
								HelperFunctions.CastDebugValue(parameterExpression.ResultValue, paramsArrayElement);
							}
						}

						parameters.RemoveRange(parameterIndex, parameters.Count - parameterIndex);
						parameters.Add(new DebugExpressionResult(context, paramsArrayValueWrapper));
					}
					else
					{
						DebugExpressionResult parameterExpression = parameters[parameterIndexModifier + index];

						ConvertParameter(context, parameterExpression, methodSignatureParameter);
					}
				}
			}
		}

		private void ConvertParameter(EvaluationContext context, DebugExpressionResult parameterExpression, TypeSignatureItem methodParameter)
		{
			TypeDefinition parameterTypeDefinition = null;

			if (HelperFunctions.HasValueClass(parameterExpression.ResultValue))
			{
				parameterTypeDefinition = HelperFunctions.FindTypeOfValue(context, parameterExpression);
			}
			else
			{
				parameterTypeDefinition = HelperFunctions.GetTypeByElementType((CorElementType)parameterExpression.ResultValue.ElementType);
			}

			if (!ParameterTypesMatch(parameterTypeDefinition, methodParameter))
			{
				if (CanConvert((CorElementType)parameterExpression.ResultValue.ElementType, methodParameter))
				{
					if (methodParameter.TokenObject != null && methodParameter.TokenObject.Name == Dile.Constants.DecimalTypeName)
					{
						parameterExpression.ResultValue = HelperFunctions.CastToDecimal(context, parameterExpression.ResultValue);
					}
					else
					{
						ValueWrapper castedParameter = context.EvalWrapper.CreateValue((int)methodParameter.ElementType, null);
						HelperFunctions.CastDebugValue(parameterExpression.ResultValue, castedParameter);
						parameterExpression.ResultValue = castedParameter;
					}
				}
				else
				{
					MethodDefinition implicitOperatorMethod = FindImplicitCastOperator(context, parameterExpression, methodParameter, HelperFunctions.IsArrayElementType(methodParameter.ElementType));

					if (implicitOperatorMethod == null)
					{
						throw new InvalidOperationException(string.Format("No suitable implicit cast operator has been found to cast a value from {0} type to {1} type.", parameterTypeDefinition.FullName, methodParameter.GetTokenObjectName(true)));
					}

					List<ValueWrapper> arguments = new List<ValueWrapper>();
					arguments.Add(parameterExpression.ResultValue);

					BaseEvaluationResult evaluationResult = context.EvaluationHandler.CallMethod(context, implicitOperatorMethod, arguments);

					if (evaluationResult.IsSuccessful)
					{
						parameterExpression.ResultValue = evaluationResult.Result;
					}
					else
					{
						evaluationResult.ThrowExceptionAccordingToReason();
					}
				}
			}
		}

		public MethodDefinition SearchToStringMethod(TypeDefinition stringTypeDef)
		{
			MethodDefinition result = null;

			if (MethodDefinitions != null)
			{
				Dictionary<uint, MethodDefinition>.Enumerator methodDefEnumerator = MethodDefinitions.GetEnumerator();

				while (result == null && methodDefEnumerator.MoveNext())
				{
					MethodDefinition method = methodDefEnumerator.Current.Value;

					if (!method.IsStatic && method.Name == Dile.Constants.ToStringMethodName && ParameterTypesMatch(stringTypeDef, (TypeSignatureItem)method.MethodSignatureReader.ReturnType))
					{
						if (method.Parameters == null || method.Parameters.Count == 0 || (method.Parameters.Count == 1 && method.Parameters[0].ParamArrayAttributeExists()))
						{
							result = method;
						}
					}
				}
			}

			return result;
		}

		public List<GenericParameter> GetAllGenericParameters()
		{
			List<GenericParameter> result = GenericParameters;

			if (EnclosingType != null)
			{
				List<GenericParameter> enclosingTypeParameters = EnclosingType.GetAllGenericParameters();

				if (enclosingTypeParameters != null)
				{
					enclosingTypeParameters.AddRange(result);
				}
			}

			return result;
		}

		public bool GetIsEnum()
		{
			bool result = false;
			Assembly assembly = ModuleScope.Assembly;

			if (assembly.AllTokens.ContainsKey(BaseTypeToken))
			{
				if (assembly.AllTokens[BaseTypeToken].Name == Dile.Constants.EnumTypeName)
				{
					result = true;
				}
			}

			return result;
		}

		public CorElementType GetEnumElementType()
		{
			CorElementType result = CorElementType.ELEMENT_TYPE_END;
			FieldDefinition valueFieldDefinition = FindFieldDefinitionByName("value__");

			if (valueFieldDefinition != null)
			{
				result = HelperFunctions.GetShortNameElementType(valueFieldDefinition.FieldTypeName);
			}

			return result;
		}
	}
}