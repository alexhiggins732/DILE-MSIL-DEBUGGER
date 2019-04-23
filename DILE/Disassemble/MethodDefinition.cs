using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.Metadata.Signature;
using Dile.UI;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class MethodDefinition : TextTokenBase, IMultiLine, IHasSignature
	{
		//Signature token values start from 0x11000001. This default value seem to indicate that there's no LocalVarSig.
		private const int DefaultSignatureValue = 0x11000000;

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
				return SearchOptions.MethodDefinition;
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

		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = HelperFunctions.QuoteMethodName(value);
			}
		}

		private bool entryPoint = false;
		public bool EntryPoint
		{
			get
			{
				return entryPoint;
			}
			set
			{
				entryPoint = value;
			}
		}

		private CorMethodAttr flags;
		public CorMethodAttr Flags
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

		public bool IsStatic
		{
			get
			{
				return ((flags & CorMethodAttr.mdStatic) == CorMethodAttr.mdStatic);
			}
		}

		private IntPtr signaturePointer;
		public IntPtr SignaturePointer
		{
			get
			{
				return signaturePointer;
			}
			private set
			{
				signaturePointer = value;
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

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					ReadSignature();
					string returnTypeText = signatureReader.ReturnType.ToString();
					string callingConventionName = HelperFunctions.GetCallingConventionName(CallingConvention);

					StringBuilder parameterListBuilder = new StringBuilder();
					parameterListBuilder.Append("(");

					if (signatureReader.Parameters != null)
					{
						for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
						{
							BaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
							Parameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

							string parameterItemAsString = parameterItem.ToString();
							string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

							if (parametersIndex == signatureReader.Parameters.Count - 1)
							{
								parameterListBuilder.Append(parameterItemAsString);
							}
							else
							{
								parameterListBuilder.Append(parameterItemAsString);
								parameterListBuilder.Append(", ");
							}
						}
					}

					parameterListBuilder.Append(")");

					if (callingConventionName.Length > 0)
					{
						text = string.Format("{0} {1} {2}{3}{4}{5}", callingConventionName, returnTypeText, BaseTypeDefinition.FullName, (BaseTypeDefinition.FullName.Length == 0 ? string.Empty : "::"), Name, parameterListBuilder.ToString());
					}
					else
					{
						text = string.Format("{0} {1}{2}{3}{4}", returnTypeText, BaseTypeDefinition.FullName, (BaseTypeDefinition.FullName.Length == 0 ? string.Empty : "::"), Name, parameterListBuilder.ToString());
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

		private List<Parameter> parameters;
		public List<Parameter> Parameters
		{
			get
			{
				return parameters;
			}
			private set
			{
				parameters = value;
			}
		}

		private string displayName = null;
		public string DisplayName
		{
			get
			{
				if (displayName == null)
				{
					Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
					ReadSignature();
					string returnTypeText = signatureReader.ReturnType.ToString();
					Parameter returnParameter = FindParameterByOrdinalIndex(0);
					DisplayNameBuilder.Length = 0;
					DisplayNameBuilder.Append(Name);
					DisplayNameBuilder.Append(" : ");
					DisplayNameBuilder.Append(returnTypeText);

					if (GenericParameters != null)
					{
						DisplayNameBuilder.Append("<");

						for (int index = 0; index < GenericParameters.Count; index++)
						{
							GenericParameter genericParameter = GenericParameters[index];

							genericParameter.LazyInitialize(assembly.AllTokens);
							DisplayNameBuilder.Append(genericParameter.Text);

							if (index < GenericParameters.Count - 1)
							{
								DisplayNameBuilder.Append(", ");
							}
						}

						DisplayNameBuilder.Append(">");
					}

					DisplayNameBuilder.Append("(");

					if (signatureReader.Parameters != null)
					{
						for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
						{
							BaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
							Parameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

							string parameterItemAsString = parameterItem.ToString();
							string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

							if (parametersIndex == signatureReader.Parameters.Count - 1)
							{
								DisplayNameBuilder.Append(parameterItemAsString);
							}
							else
							{
								DisplayNameBuilder.Append(parameterItemAsString);
								DisplayNameBuilder.Append(", ");
							}
						}
					}

					DisplayNameBuilder.Append(")");
					DisplayName = DisplayNameBuilder.ToString();
				}

				return displayName;
			}
			private set
			{
				displayName = value;
			}
		}

		public string HeaderText
		{
			get
			{
				return string.Format("{0}.{1}", BaseTypeDefinition.Name, Name);
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

		private CorCallingConvention callingConvention;
		public CorCallingConvention CallingConvention
		{
			get
			{
				return callingConvention;
			}
			private set
			{
				callingConvention = value;
			}
		}

		private uint overrides = 0;
		public uint Overrides
		{
			get
			{
				return overrides;
			}
			set
			{
				overrides = value;
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
			set
			{
				pinvokeMap = value;
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

		private List<MethodSpec> methodSpecs;
		public List<MethodSpec> MethodSpecs
		{
			get
			{
				return methodSpecs;
			}
			private set
			{
				methodSpecs = value;
			}
		}

		private MethodSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		public MethodSignatureReader MethodSignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private Property ownerProperty = null;
		public Property OwnerProperty
		{
			get
			{
				return ownerProperty;
			}
			set
			{
				ownerProperty = value;
			}
		}

		private EventDefinition ownerEventDefinition = null;
		public EventDefinition OwnerEventDefinition
		{
			get
			{
				return ownerEventDefinition;
			}
			set
			{
				ownerEventDefinition = value;
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

		[ThreadStatic()]
		private static StringBuilder displayNameBuilder;
		private static StringBuilder DisplayNameBuilder
		{
			get
			{
				if (displayNameBuilder == null)
				{
					displayNameBuilder = new StringBuilder();
				}

				return displayNameBuilder;
			}
		}

		private uint localVarSigToken;
		private uint LocalVarSigToken
		{
			get
			{
				return localVarSigToken;
			}
			set
			{
				localVarSigToken = value;
			}
		}

		private ulong methodAddress;
		private ulong MethodAddress
		{
			get
			{
				return methodAddress;
			}
			set
			{
				methodAddress = value;
			}
		}

		public MethodDefinition(IMetaDataImport2 import, TypeDefinition typeDefinition, string name, uint token, uint flags, IntPtr signaturePointer, uint signatureLength, uint rva, uint implementationFlags)
		{
			BaseTypeDefinition = typeDefinition;
			Name = name;
			Token = token;
			Flags = (CorMethodAttr)flags;
			SignaturePointer = signaturePointer;
			SignatureLength = signatureLength;
			Rva = rva;
			ImplementationFlags = (CorMethodImpl)implementationFlags;

			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			HelperFunctions.GetMemberReferences(assembly, token);
			EnumParameters(import);
			GenericParameters = HelperFunctions.EnumGenericParameters(import, assembly, this);
			MethodSpecs = HelperFunctions.EnumMethodSpecs(import, assembly, this);

			if (assembly.ModuleScope.DebuggedModule != null)
			{
				FunctionWrapper debuggedFunction = assembly.ModuleScope.DebuggedModule.GetFunction(Token);

				try
				{
					LocalVarSigToken = debuggedFunction.GetLocalVarSigToken();

					if (LocalVarSigToken == DefaultSignatureValue)
					{
						LocalVarSigToken = 0;
					}

					MethodAddress = debuggedFunction.GetAddress();
				}
				catch (COMException comException)
				{
					//0x8013130a exception means that the method is native.
					if ((uint)comException.ErrorCode == 0x8013130a)
					{
						LocalVarSigToken = 0;
					}
					else
					{
						throw;
					}
				}
			}
		}

		private void EnumParameters(IMetaDataImport2 import)
		{
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			IntPtr enumHandle = IntPtr.Zero;
			uint[] parameterTokens = new uint[Project.DefaultArrayCount];
			uint count = 0;
			import.EnumParams(ref enumHandle, Token, parameterTokens, Convert.ToUInt32(parameterTokens.Length), out count);

			if (count > 0)
			{
				Parameters = new List<Parameter>();
			}

			while (count > 0)
			{
				for (uint parameterTokensIndex = 0; parameterTokensIndex < count; parameterTokensIndex++)
				{
					uint methodToken;
					uint parameterToken = parameterTokens[parameterTokensIndex];
					uint ordinalIndex;
					uint nameLength;
					uint attributeFlags;
					uint elementType;
					IntPtr defaultValue;
					uint defaultValueLength;

					import.GetParamProps(parameterToken, out methodToken, out ordinalIndex, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out attributeFlags, out elementType, out defaultValue, out defaultValueLength);

					if (nameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[nameLength];

						import.GetParamProps(parameterToken, out methodToken, out ordinalIndex, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, out attributeFlags, out elementType, out defaultValue, out defaultValueLength);
					}

					Parameter parameter = new Parameter(import, assembly.AllTokens, parameterToken, this, ordinalIndex, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameLength), attributeFlags, elementType, defaultValue, defaultValueLength);
					Parameters.Add(parameter);
					assembly.AllTokens[Token] = parameter;
				}

				import.EnumParams(ref enumHandle, Token, parameterTokens, Convert.ToUInt32(parameterTokens.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				import.CloseEnum(enumHandle);
			}

			if (Parameters != null)
			{
				Parameters.Sort();

				foreach (Parameter parameter in Parameters)
				{
					parameter.ReadMarshalInformation(import, assembly.AllTokens, Parameters.Count);
				}
			}
		}

		private string MemberAccessAsString()
		{
			string result = string.Empty;
			CorMethodAttr memberAccess = Flags & CorMethodAttr.mdMemberAccessMask;

			switch (memberAccess)
			{
				case CorMethodAttr.mdPrivateScope:
					result = "privatescope ";
					break;

				case CorMethodAttr.mdPrivate:
					result = "private ";
					break;

				case CorMethodAttr.mdFamANDAssem:
					result = "famandassem ";
					break;

				case CorMethodAttr.mdAssem:
					result = "assembly ";
					break;

				case CorMethodAttr.mdFamily:
					result = "family ";
					break;

				case CorMethodAttr.mdFamORAssem:
					result = "famorassem ";
					break;

				case CorMethodAttr.mdPublic:
					result = "public ";
					break;
			}

			return result.ToString();
		}

		private string MethodContractAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdHideBySig, "hidebysig "));

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdStatic, "static "));

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdFinal, "final "));

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdVirtual, "virtual "));

			return result.ToString();
		}

		private string VTableLayoutAsString()
		{
			string result = string.Empty;

			result = HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdNewSlot, "newslot ");

			return result;
		}

		private string MethodImplementationAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdAbstract, "abstract "));
			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdSpecialName, "specialname "));

			return result.ToString();
		}

		private string InteropAttributesAsString()
		{
			StringBuilder result = new StringBuilder();

			if ((Flags & CorMethodAttr.mdPinvokeImpl) == CorMethodAttr.mdPinvokeImpl)
			{
				result.Append("pinvokeimpl(");

				if (PinvokeMap == null)
				{
					result.Append("/* No map */");
				}
				else
				{
					result.Append(PinvokeMap);
				}

				result.Append(") ");
			}

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdUnmanagedExport, "unmanagedexp "));

			return result.ToString();
		}

		private string ReservedFlagsAsString()
		{
			StringBuilder result = new StringBuilder();

			if ((Flags & CorMethodAttr.mdReservedMask) == CorMethodAttr.mdReservedMask)
			{
				throw new NotSupportedException("Unknown method flag value (reserved mask).");
			}

			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdRTSpecialName, "rtspecialname "));
			result.Append(HelperFunctions.EnumAsString(Flags, CorMethodAttr.mdRequireSecObject, "reqsecobj "));

			return result.ToString();
		}

		private string CodeImplementationAsString()
		{
			string result = string.Empty;
			CorMethodImpl implementation = ImplementationFlags & CorMethodImpl.miCodeTypeMask;

			switch (implementation)
			{
				case CorMethodImpl.miIL:
					result = "cil ";
					break;

				case CorMethodImpl.miNative:
					result = "native ";
					break;

				case CorMethodImpl.miOPTIL:
					result = "optil ";
					break;

				case CorMethodImpl.miRuntime:
					result = "runtime ";
					break;
			}

			return result;
		}

		private string ManagedImplementationAsString()
		{
			string result = string.Empty;
			CorMethodImpl implementation = ImplementationFlags & CorMethodImpl.miManagedMask;

			switch (implementation)
			{
				case CorMethodImpl.miUnmanaged:
					result = "unmanaged ";
					break;

				case CorMethodImpl.miManaged:
					result = "managed ";
					break;
			}

			return result;
		}

		private string ImplementationInfoAsString()
		{
			StringBuilder result = new StringBuilder();

			result.Append(HelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miForwardRef, "forwardref "));

			result.Append(HelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miPreserveSig, "preservesig "));

			result.Append(HelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miInternalCall, "internalcall "));

			result.Append(HelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miSynchronized, "synchronized "));

			result.Append(HelperFunctions.EnumAsString(ImplementationFlags, CorMethodImpl.miNoInlining, "noinlining "));

			return result.ToString();
		}

		private CodeLine CreateMethodHead(string definition)
		{
			CodeLine result = new CodeLine();
			StringBuilder text = new StringBuilder();

			text.Append(".method ");
			HelperFunctions.AddWordToStringBuilder(text, MemberAccessAsString());
			HelperFunctions.AddWordToStringBuilder(text, MethodContractAsString());
			HelperFunctions.AddWordToStringBuilder(text, VTableLayoutAsString());
			HelperFunctions.AddWordToStringBuilder(text, MethodImplementationAsString());
			HelperFunctions.AddWordToStringBuilder(text, InteropAttributesAsString());
			HelperFunctions.AddWordToStringBuilder(text, ReservedFlagsAsString());
			text.Append(definition);
			HelperFunctions.AddWordToStringBuilder(text, CodeImplementationAsString());
			HelperFunctions.AddWordToStringBuilder(text, ManagedImplementationAsString());
			HelperFunctions.AddWordToStringBuilder(text, ImplementationInfoAsString());

			result.Text = text.ToString();

			return result;
		}

		private int FindCodeLine(uint offset)
		{
			int result = -1;
			bool found = false;
			int ilCodeIndex = 0;

			while (!found && ilCodeIndex < CodeLines.Count)
			{
				CodeLine codeLine = CodeLines[ilCodeIndex++];

				if (codeLine is BaseILCode)
				{
					BaseILCode ilCode = (BaseILCode)codeLine;

					if (ilCode.Offset == offset)
					{
						result = ilCodeIndex - 1;
						found = true;
					}
				}
			}

			return result;
		}

		private void AddExceptionCodeLines(uint offset, uint length, string name, string endComment, bool finallyClause)
		{
			int start = FindCodeLine(offset);

			if (start > -1)
			{
				int end = FindCodeLine(offset + length);

				if (end > -1)
				{
					int indentation = ((BaseILCode)CodeLines[start]).Indentation;

					for (int ilCodeIndex = start; ilCodeIndex < end; ilCodeIndex++)
					{
						CodeLines[ilCodeIndex].Indentation++;
					}

					CodeLine nameDefinition = new CodeLine(indentation, name);
					CodeLine bracket = new CodeLine(indentation, "{");

					CodeLines.Insert(start, bracket);
					CodeLines.Insert(start, nameDefinition);

					end += 2;
					if (finallyClause && indentation > 1)
					{
						indentation--;
					}
					bracket = new CodeLine(indentation, string.Format("}} {0}", endComment));
					CodeLines.Insert(end, bracket);
				}
			}
		}

		private void AddExceptionHandlingCodeLines(TryClause tryClause)
		{
			bool tryClauseAdded = false;
			for (int index = 0; index < tryClause.HandlerClauses.Count; index++)
			{
				HandlerClause handlerClause = tryClause.HandlerClauses[index];
				bool finallyClause = handlerClause.Flags.HasFlag(CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_FINALLY);

				if (!tryClauseAdded)
				{
					AddExceptionCodeLines(tryClause.Offset, tryClause.Length, ".try", "//end try", finallyClause);
					tryClauseAdded = true;
				}

				if (finallyClause)
				{
					AddExceptionCodeLines(handlerClause.Offset, handlerClause.Length, "finally", "//end finally", true);
				}
				else if (handlerClause.Flags.HasFlag(CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_FILTER))
				{
					AddExceptionCodeLines(handlerClause.ClauseDependentData, handlerClause.Offset - handlerClause.ClauseDependentData, "filter", "//end filter", false);
					AddExceptionCodeLines(handlerClause.Offset, handlerClause.Length, "catch", "//end filter handler", false);
				}
				else if (handlerClause.Flags.HasFlag(CorExceptionFlag.COR_ILEXCEPTION_CLAUSE_NONE))
				{
					string handlerName = "catch " + BaseTypeDefinition.ModuleScope.Assembly.AllTokens[handlerClause.ClauseDependentData].ToString();

					AddExceptionCodeLines(handlerClause.Offset, handlerClause.Length, handlerName, "//end handler", false);
				}
				else
				{
					throw new NotSupportedException("The following error handling clause type is not supported: " + handlerClause.Flags);
				}
			}
		}

		private void ReadMethodDataSections(BinaryReader assemblyReader)
		{
			bool moreSections = true;
			byte moreSectionsValue = (byte)CorILMethodSect.CorILMethod_Sect_MoreSects;
			byte fatFormatValue = (byte)CorILMethodSect.CorILMethod_Sect_FatFormat;
			byte exceptionHandlingTableValue = (byte)CorILMethodSect.CorILMethod_Sect_EHTable;

			while (moreSections)
			{
				int bytesToRead = Convert.ToInt32(assemblyReader.BaseStream.Position % 4);

				if (bytesToRead > 0)
				{
					assemblyReader.ReadBytes(4 - bytesToRead);
				}

				byte kind = assemblyReader.ReadByte();

				if ((kind & exceptionHandlingTableValue) != exceptionHandlingTableValue)
				{
					throw new NotImplementedException("The method data section is not an exception handling table.");
				}

				moreSections = ((kind & moreSectionsValue) == moreSectionsValue);
				int dataSize = 0;
				int clauseNumber = 0;
				bool fatFormat = ((kind & fatFormatValue) == fatFormatValue);

				if (fatFormat)
				{
					dataSize = assemblyReader.ReadByte() + assemblyReader.ReadByte() * 0x100 + assemblyReader.ReadByte() * 0x10000;
					clauseNumber = dataSize / 24;
				}
				else
				{
					dataSize = assemblyReader.ReadByte();
					//Read padding.
					assemblyReader.ReadBytes(2);
					clauseNumber = dataSize / 12;
				}

				List<TryClause> tryClauses = new List<TryClause>();
				for (int clauseIndex = 0; clauseIndex < clauseNumber; clauseIndex++)
				{
					CorExceptionFlag flags;
					uint tryOffset;
					uint tryLength;
					uint handlerOffset;
					uint handlerLength;
					if (fatFormat)
					{
						flags = (CorExceptionFlag)assemblyReader.ReadUInt32();
						tryOffset = assemblyReader.ReadUInt32();
						tryLength = assemblyReader.ReadUInt32();
						handlerOffset = assemblyReader.ReadUInt32();
						handlerLength = assemblyReader.ReadUInt32();
					}
					else
					{
						flags = (CorExceptionFlag)assemblyReader.ReadUInt16();
						tryOffset = assemblyReader.ReadUInt16();
						tryLength = assemblyReader.ReadByte();
						handlerOffset = assemblyReader.ReadUInt16();
						handlerLength = assemblyReader.ReadByte();
					}
					uint handlerClauseDependentData = assemblyReader.ReadUInt32();

					TryClause tryClause = tryClauses.FirstOrDefault(existingClause => existingClause.Offset == tryOffset && existingClause.Length == tryLength);
					if (tryClause == null)
					{
						tryClause = new TryClause(tryOffset, tryLength);
						tryClauses.Add(tryClause);
					}

					HandlerClause handlerClause = new HandlerClause(flags, handlerOffset, handlerLength, handlerClauseDependentData);
					tryClause.HandlerClauses.Add(handlerClause);
				}

				foreach (TryClause tryClause in tryClauses)
				{
					AddExceptionHandlingCodeLines(tryClause);
				}
			}
		}

		private ulong FindMethodHeader()
		{
			ulong result = MethodAddress;
			byte[] possibleLocalVarSigToken = BaseTypeDefinition.ModuleScope.Assembly.DebuggedProcess.ReadMemory(MethodAddress - 4, 4);

			if (BitConverter.ToUInt32(possibleLocalVarSigToken, 0) == LocalVarSigToken)
			{
				result = MethodAddress - 12;
			}
			else
			{
				result = MethodAddress - 1;
			}

			return result;
		}

		private void ReadILCode()
		{
			if (MethodSignatureReader.Parameters != null && MethodSignatureReader.Parameters.Count > 0
				&& (Parameters == null || Parameters.Count != MethodSignatureReader.Parameters.Count))
			{
				UIHandler.Instance.SetProgressText("Metadata was not found for all parameters of the following method: " + DisplayName + ".\n", false);
			}

			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;
			List<GenericParameter> allTypeParameters = BaseTypeDefinition.GetAllGenericParameters();
			CodeLine start = new CodeLine();
			start.Text = "{";
			CodeLines.Add(start);
			CodeLines.Add(new CodeLine(1, "//Token: 0x" + HelperFunctions.FormatAsHexNumber(Token, 8)));

			if (EntryPoint)
			{
				CodeLines.Add(new CodeLine(1, ".entrypoint"));
			}

			if (Overrides != 0)
			{
				TokenBase overridenMember = BaseTypeDefinition.ModuleScope.Assembly.AllTokens[Overrides];
				Type overridenMemberType = overridenMember.GetType();
				string memberName;

				if (GenericParameters != null && overridenMember is IHasSignature)
				{
					IHasSignature hasSignature = (IHasSignature)overridenMember;
					hasSignature.SignatureReader.SetGenericParametersOfMethod(allTypeParameters, GenericParameters);
				}

				if (overridenMemberType == typeof(MemberReference))
				{
					MemberReference memberReference = (MemberReference)overridenMember;
					memberName = memberReference.Text;
				}
				else if (overridenMemberType == typeof(MethodDefinition))
				{
					MethodDefinition methodDefinition = (MethodDefinition)overridenMember;
					memberName = string.Format("{0}::{1}", methodDefinition.BaseTypeDefinition.FullName, methodDefinition.Name);
				}
				else
				{
					throw new NotImplementedException(string.Format("Unhandled overriden member type ('{0}').", overridenMemberType.FullName));
				}

				CodeLines.Add(new CodeLine(1, ".override method " + memberName));
			}

			if (CustomAttributes != null)
			{
				foreach (CustomAttribute customAttribute in CustomAttributes)
				{
					CodeLines.Add(new CodeLine(1, customAttribute.Name));
				}
			}

			int codeSizePosition = 2;

			if (Parameters != null)
			{
				try
				{
					assembly.OpenMetadataInterfaces();

					foreach (Parameter parameter in Parameters)
					{
						parameter.LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);
						bool hasDefault = ((parameter.AttributeFlags & CorParamAttr.pdHasDefault) == CorParamAttr.pdHasDefault);

						if (hasDefault)
						{
							CodeLine defaultParameter = new CodeLine();
							defaultParameter.Indentation = 1;
							defaultParameter.Text = parameter.DefaultValueAsString;
							CodeLines.Add(defaultParameter);
							codeSizePosition++;
						}

						if (parameter.CustomAttributes != null)
						{
							if (!hasDefault)
							{
								CodeLine parameterLine = new CodeLine(1, string.Format(".param [{0}]", parameter.OrdinalIndex));
								CodeLines.Add(parameterLine);
								codeSizePosition++;
							}

							foreach (CustomAttribute customAttribute in parameter.CustomAttributes)
							{
								CodeLine customAttributeLine = new CodeLine(1, customAttribute.Name);
								CodeLines.Add(customAttributeLine);

								codeSizePosition++;
							}
						}
					}
				}
				finally
				{
					assembly.CloseMetadataInterfaces();
				}
			}

			if (PermissionSets != null)
			{
				foreach (PermissionSet permissionSet in PermissionSets)
				{
					permissionSet.SetText(assembly.AllTokens);
					CodeLine permissionSetLine = new CodeLine(1, permissionSet.Name);
					CodeLines.Add(permissionSetLine);

					codeSizePosition++;
				}
			}

			if (((Rva > 0) || BaseTypeDefinition.ModuleScope.Assembly.LoadedFromMemory) && ((ImplementationFlags & CorMethodImpl.miCodeTypeMask) != CorMethodImpl.miNative))
			{
				BinaryReader assemblyReader = null;

				if (assembly.LoadedFromMemory)
				{
					InMemoryMethodStream methodStream = new InMemoryMethodStream(assembly.DebuggedProcess, FindMethodHeader());
					assemblyReader = new BinaryReader(methodStream);
				}
				else
				{
					assembly.OpenAssemblyReader();
					assemblyReader = assembly.AssemblyReader;
					assemblyReader.BaseStream.Position = assembly.GetMethodAddress(Rva);
				}

				byte methodHeader = assemblyReader.ReadByte();
				int methodLength = 0;
				bool moreSects = false;

				if ((methodHeader & (byte)ILMethodHeader.CorILMethod_FatFormat) == (byte)ILMethodHeader.CorILMethod_FatFormat)
				{
					byte methodHeaderByte2 = assemblyReader.ReadByte();
					moreSects = ((methodHeader & (byte)CorILMethodFlags.CorILMethod_MoreSects) == (byte)CorILMethodFlags.CorILMethod_MoreSects);

					byte sizeOfHeader = Convert.ToByte((methodHeaderByte2 >> 4) * 4);
					ushort maxStack = assemblyReader.ReadUInt16();
					methodLength = assemblyReader.ReadInt32();
					uint localVarSigToken = assemblyReader.ReadUInt32();

					CodeLine maxStackLine = new CodeLine(1, string.Format(".maxstack {0}", maxStack));
					CodeLines.Add(maxStackLine);

					if (localVarSigToken != 0 && BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures != null && BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures.ContainsKey(localVarSigToken))
					{
						string initLocals = string.Empty;

						if ((methodHeader & (byte)CorILMethodFlags.CorILMethod_InitLocals) == (byte)CorILMethodFlags.CorILMethod_InitLocals)
						{
							initLocals = "init ";
						}

						StandAloneSignature standAloneSignature = BaseTypeDefinition.ModuleScope.Assembly.StandAloneSignatures[localVarSigToken];

						if (GenericParameters != null && standAloneSignature.SignatureReader.HasGenericMethodParameter)
						{
							standAloneSignature.SignatureReader.SetGenericParametersOfMethod(allTypeParameters, GenericParameters);
							standAloneSignature.LazyInitialize(BaseTypeDefinition.ModuleScope.Assembly.AllTokens);
						}

						CodeLine variablesLine = new CodeLine(1, string.Format(".locals {0}{1}", initLocals, standAloneSignature.Text));
						CodeLines.Add(variablesLine);
					}
				}
				else if ((methodHeader & (byte)ILMethodHeader.CorILMethod_TinyFormat) == (byte)ILMethodHeader.CorILMethod_TinyFormat)
				{
					methodLength = methodHeader >> 2;
				}

				CodeLine codeSize = new CodeLine(1, string.Format("//Code size {0} (0x{1:x})", methodLength, methodLength));

				if (CustomAttributes != null)
				{
					codeSizePosition++;
				}

				if (EntryPoint)
				{
					codeSizePosition++;
				}

				if (Overrides != 0)
				{
					codeSizePosition++;
				}

				CodeLines.Insert(codeSizePosition, codeSize);

				if ((ImplementationFlags & CorMethodImpl.miNative) != CorMethodImpl.miNative)
				{
					byte[] methodCode = new byte[methodLength];
					assemblyReader.Read(methodCode, 0, methodLength);
					int methodCodeIndex = 0;

					while (methodCodeIndex < methodCode.Length)
					{
						int offset = methodCodeIndex;
						short opCodeValue = methodCode[methodCodeIndex++];

						if (opCodeValue == 0xFE)
						{
							opCodeValue = (short)(opCodeValue * 0x100 + methodCode[methodCodeIndex++]);
						}

						if (OpCodeGroups.OpCodesByValue.ContainsKey(opCodeValue))
						{
							OpCode opCode = OpCodeGroups.OpCodesByValue[opCodeValue];
							OpCodeGroup opCodeGroup = OpCodeGroups.GetGroupOfOpCode(opCode);
							int parameterSize = 0;

							switch (opCodeGroup)
							{
								case OpCodeGroup.ByteArgumentParameter:
								case OpCodeGroup.ByteParameter:
								case OpCodeGroup.ByteVariableParameter:
								case OpCodeGroup.SbyteLocationParameter:
								case OpCodeGroup.SbyteParameter:
									parameterSize = 1;
									break;

								case OpCodeGroup.UshortArgumentParameter:
								case OpCodeGroup.UshortParameter:
								case OpCodeGroup.UshortVariableParameter:
									parameterSize = 2;
									break;

								case OpCodeGroup.FloatParameter:
								case OpCodeGroup.FieldParameter:
								case OpCodeGroup.IntLocationParameter:
								case OpCodeGroup.IntParameter:
								case OpCodeGroup.MethodParameter:
								case OpCodeGroup.StringParameter:
								case OpCodeGroup.TypeParameter:
								case OpCodeGroup.Calli:
								case OpCodeGroup.Ldtoken:
								case OpCodeGroup.Switch:
									parameterSize = 4;
									break;

								case OpCodeGroup.DoubleParameter:
								case OpCodeGroup.LongParameter:
									parameterSize = 8;
									break;
							}

							ulong parameter = 0;

							if (parameterSize > 0)
							{
								parameter = HelperFunctions.GetILCodeParameter(methodCode, methodCodeIndex, parameterSize);
							}

							BaseILCode code = null;

							switch (opCodeGroup)
							{
								case OpCodeGroup.Parameterless:
									code = new BaseILCode();
									break;

								case OpCodeGroup.FieldParameter:
									FieldILCode fieldILCode = new FieldILCode();
									code = fieldILCode;
									fieldILCode.Parameter = Convert.ToUInt32(parameter);
									fieldILCode.DecodedParameter = assembly.AllTokens[fieldILCode.Parameter];
									if (GenericParameters != null)
									{
										fieldILCode.SetGenericsMethodParameters(assembly.AllTokens, allTypeParameters, GenericParameters);
									}
									break;

								case OpCodeGroup.MethodParameter:
									MethodILCode methodILCode = new MethodILCode();
									code = methodILCode;
									methodILCode.Parameter = Convert.ToUInt32(parameter);
									methodILCode.DecodedParameter = assembly.AllTokens[methodILCode.Parameter];
									if (GenericParameters != null)
									{
										methodILCode.SetGenericsMethodParameters(assembly.AllTokens, allTypeParameters, GenericParameters);
									}
									break;

								case OpCodeGroup.StringParameter:
									StringILCode stringILCode = new StringILCode();
									code = stringILCode;
									stringILCode.Parameter = Convert.ToUInt32(parameter);
									stringILCode.DecodedParameter = assembly.UserStrings[stringILCode.Parameter];
									break;

								case OpCodeGroup.TypeParameter:
									TypeILCode typeILCode = new TypeILCode();
									code = typeILCode;
									typeILCode.Parameter = Convert.ToUInt32(parameter);
									typeILCode.DecodedParameter = assembly.AllTokens[typeILCode.Parameter];
									if (GenericParameters != null)
									{
										typeILCode.SetGenericsMethodParameters(assembly.AllTokens, allTypeParameters, GenericParameters);
									}
									break;

								case OpCodeGroup.IntLocationParameter:
									LocationILCode<int> intLocationILCode = new LocationILCode<int>();
									code = intLocationILCode;
									intLocationILCode.Parameter = (int)parameter;
									intLocationILCode.DecodedParameter = intLocationILCode.Parameter;
									break;

								case OpCodeGroup.SbyteLocationParameter:
									LocationILCode<sbyte> sbyteLocationILCode = new LocationILCode<sbyte>();
									code = sbyteLocationILCode;
									sbyteLocationILCode.Parameter = (sbyte)parameter;
									sbyteLocationILCode.DecodedParameter = sbyteLocationILCode.Parameter;
									break;

								case OpCodeGroup.ByteParameter:
									NumberILCode<byte> byteNumberILCode = new NumberILCode<byte>();
									code = byteNumberILCode;
									byteNumberILCode.Parameter = (byte)parameter;
									byteNumberILCode.DecodedParameter = byteNumberILCode.Parameter;
									break;

								case OpCodeGroup.UshortParameter:
									NumberILCode<ushort> ushortNumberILCode = new NumberILCode<ushort>();
									code = ushortNumberILCode;
									ushortNumberILCode.Parameter = (ushort)parameter;
									ushortNumberILCode.DecodedParameter = ushortNumberILCode.Parameter;
									break;

								case OpCodeGroup.SbyteParameter:
									NumberILCode<sbyte> sbyteNumberILCode = new NumberILCode<sbyte>();
									code = sbyteNumberILCode;
									sbyteNumberILCode.Parameter = (sbyte)parameter;
									sbyteNumberILCode.DecodedParameter = sbyteNumberILCode.Parameter;
									break;

								case OpCodeGroup.IntParameter:
									NumberILCode<int> intNumberILCode = new NumberILCode<int>();
									code = intNumberILCode;
									intNumberILCode.Parameter = (int)parameter;
									intNumberILCode.DecodedParameter = intNumberILCode.Parameter;
									break;

								case OpCodeGroup.LongParameter:
									NumberILCode<long> longNumberILCode = new NumberILCode<long>();
									code = longNumberILCode;
									longNumberILCode.Parameter = (long)parameter;
									longNumberILCode.DecodedParameter = longNumberILCode.Parameter;
									break;

								case OpCodeGroup.FloatParameter:
									NumberILCode<float> floatNumberILCode = new NumberILCode<float>();
									code = floatNumberILCode;
									floatNumberILCode.Parameter = HelperFunctions.ConvertToSingle(parameter);
									floatNumberILCode.DecodedParameter = floatNumberILCode.Parameter;
									break;

								case OpCodeGroup.DoubleParameter:
									NumberILCode<double> doubleNumberILCode = new NumberILCode<double>();
									code = doubleNumberILCode;
									doubleNumberILCode.Parameter = HelperFunctions.ConvertToDouble(parameter);
									doubleNumberILCode.DecodedParameter = doubleNumberILCode.Parameter;
									break;

								case OpCodeGroup.ByteArgumentParameter:
									ArgumentILCode<byte> byteArgumentILCode = new ArgumentILCode<byte>();
									code = byteArgumentILCode;
									byteArgumentILCode.Parameter = (byte)parameter;
									byteArgumentILCode.DecodedParameter = NameOfParameter(parameter);
									break;

								case OpCodeGroup.UshortArgumentParameter:
									ArgumentILCode<ushort> ushortArgumentILCode = new ArgumentILCode<ushort>();
									code = ushortArgumentILCode;
									ushortArgumentILCode.Parameter = (ushort)parameter;
									ushortArgumentILCode.DecodedParameter = NameOfParameter(parameter);
									break;

								case OpCodeGroup.ByteVariableParameter:
									VariableILCode<byte> byteVariableILCode = new VariableILCode<byte>();
									code = byteVariableILCode;
									byteVariableILCode.Parameter = (byte)parameter;
									byteVariableILCode.DecodedParameter = byteVariableILCode.Parameter;
									break;

								case OpCodeGroup.UshortVariableParameter:
									VariableILCode<ushort> ushortVariableILCode = new VariableILCode<ushort>();
									code = ushortVariableILCode;
									ushortVariableILCode.Parameter = (ushort)parameter;
									ushortVariableILCode.DecodedParameter = ushortVariableILCode.Parameter;
									break;

								case OpCodeGroup.Calli:
									CalliILCode calliILCode = new CalliILCode();
									code = calliILCode;
									calliILCode.Parameter = Convert.ToUInt32(parameter);
									calliILCode.DecodedParameter = assembly.StandAloneSignatures[calliILCode.Parameter];
									break;

								case OpCodeGroup.Ldtoken:
									LdtokenILCode ldtokenILCode = new LdtokenILCode();
									code = ldtokenILCode;
									ldtokenILCode.Parameter = Convert.ToUInt32(parameter);
									ldtokenILCode.DecodedParameter = assembly.AllTokens[ldtokenILCode.Parameter];

									if (GenericParameters != null)
									{
										ldtokenILCode.SetGenericsMethodParameters(assembly.AllTokens, allTypeParameters, GenericParameters);
									}
									break;

								case OpCodeGroup.Switch:
									SwitchILCode switchILCode = new SwitchILCode();
									code = switchILCode;
									ulong addressIndex = 0;
									switchILCode.Parameter = new int[parameter];
									parameterSize += Convert.ToInt32(parameter * 4);

									while (addressIndex < parameter)
									{
										int jumpAddress = (int)HelperFunctions.GetILCodeParameter(methodCode, methodCodeIndex + Convert.ToInt32((addressIndex + 1) * 4), 4);
										switchILCode.Parameter[addressIndex++] = jumpAddress;
									}
									break;
							}

							if (code != null)
							{
								code.Offset = offset;
								code.OpCode = opCode;
								code.DecodeParameter();
							}

							code.Indentation = 1;

							CodeLines.Add(code);
							methodCodeIndex += parameterSize;
						}
					}

					CodeLine end = new CodeLine();
					StringBuilder endText = new StringBuilder();
					endText.Append("} //end of method ");

					if (BaseTypeDefinition.FullName != null && BaseTypeDefinition.FullName.Length > 0)
					{
						endText.Append(BaseTypeDefinition.FullName);
						endText.Append("::");
					}

					endText.Append(Name);
					end.Text = endText.ToString();

					CodeLines.Add(end);

					if (moreSects)
					{
						ReadMethodDataSections(assemblyReader);
					}
				}

				if (assembly.LoadedFromMemory)
				{
					assemblyReader.Close();
				}
				else
				{
					assembly.CloseAssemblyReader();
				}
			}
			else
			{
				CodeLine end = new CodeLine();
				end.Text = string.Format("}} // end of method {0}{1}{2}", BaseTypeDefinition.Name, (BaseTypeDefinition.Name.Length > 0 ? "::" : string.Empty), Name);
				CodeLines.Add(end);
			}
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();

			ReadSignature();
			ReadMetadata();
			CodeLines.Insert(0, CreateMethodHead(GetDefinitionLine(BaseTypeDefinition.ModuleScope.Assembly.AllTokens)));
			ReadILCode();
		}

		private string NameOfParameter(ulong ordinalIndex)
		{
			string result;

			int parameterIndex = Convert.ToInt32(ordinalIndex);
			if (CallingConvention.HasFlag(CorCallingConvention.IMAGE_CEE_CS_CALLCONV_HASTHIS))
			{
				parameterIndex++;
			}

			Parameter parameter = FindParameterByOrdinalIndex(parameterIndex);
			if (parameter == null)
			{
				result = Convert.ToString(ordinalIndex);
			}
			else
			{
				result = parameter.Name;
			}

			return result;
		}

		private Parameter FindParameterByOrdinalIndex(int ordinalIndex)
		{
			Parameter result = null;

			if (Parameters != null)
			{
				int index = 0;

				while (index < Parameters.Count && result == null)
				{
					Parameter parameter = Parameters[index++];

					if (parameter.OrdinalIndex == ordinalIndex)
					{
						result = parameter;
					}
				}
			}

			return result;
		}

		private string GetDefinitionLine(Dictionary<uint, TokenBase> allTokens)
		{
			string callingConventionName = HelperFunctions.GetCallingConventionName(CallingConvention);
			ReadSignature();
			string returnTypeText = signatureReader.ReturnType.ToString();
			Parameter returnParameter = FindParameterByOrdinalIndex(0);
			DefinitionBuilder.Length = 0;

			if (GenericParameters != null)
			{
				DefinitionBuilder.Append("<");

				for (int index = 0; index < GenericParameters.Count; index++)
				{
					GenericParameter genericParameter = GenericParameters[index];
					DefinitionBuilder.Append(genericParameter.Text);

					if (index < GenericParameters.Count - 1)
					{
						DefinitionBuilder.Append(", ");
					}
				}

				DefinitionBuilder.Append(">");
			}

			DefinitionBuilder.Append("(");

			if (signatureReader.Parameters != null)
			{
				for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
				{
					BaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];
					Parameter parameter = FindParameterByOrdinalIndex(parametersIndex + 1);

					if (parametersIndex != 0)
					{
						DefinitionBuilder.Append("    ");
					}

					string parameterItemAsString = parameterItem.ToString();
					string attributeText = (parameter == null ? string.Empty : parameter.GetAttributeText());

					if (parametersIndex == signatureReader.Parameters.Count - 1)
					{
						DefinitionBuilder.Append(attributeText);
						DefinitionBuilder.Append(parameterItemAsString);

						if ((parameter != null) && (parameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.MarshalAsTypeString);
						}

						if (parameter == null || parameter.Name == null || parameter.Name.Length == 0)
						{
							DefinitionBuilder.Append(" A_");
							DefinitionBuilder.Append(parametersIndex);
						}
						else
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.Name);
						}
					}
					else
					{
						DefinitionBuilder.Append(attributeText);
						DefinitionBuilder.Append(parameterItemAsString);

						if ((parameter != null) && (parameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal)
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.MarshalAsTypeString);
						}

						if (parameter == null || parameter.Name == null || parameter.Name.Length == 0)
						{
							DefinitionBuilder.Append(" A_");
							DefinitionBuilder.Append(parametersIndex);
							DefinitionBuilder.Append("\n");
						}
						else
						{
							DefinitionBuilder.Append(" ");
							DefinitionBuilder.Append(parameter.Name);
							DefinitionBuilder.Append(",\n");
						}
					}
				}
			}

			DefinitionBuilder.Append(") ");
			DefinitionBuilder.Insert(0, Name);

			if ((returnParameter != null) && ((returnParameter.AttributeFlags & CorParamAttr.pdHasFieldMarshal) == CorParamAttr.pdHasFieldMarshal))
			{
				DefinitionBuilder.Insert(0, " ");
				DefinitionBuilder.Insert(0, returnParameter.MarshalAsTypeString);
			}

			DefinitionBuilder.Insert(0, " ");
			DefinitionBuilder.Insert(0, returnTypeText);

			if (callingConventionName.Length > 0)
			{
				DefinitionBuilder.Insert(0, " ");
				DefinitionBuilder.Insert(0, callingConventionName);
			}

			return DefinitionBuilder.ToString();
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				List<GenericParameter> allTypeParameters = BaseTypeDefinition.GetAllGenericParameters();
				signatureReader = new MethodSignatureReader(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, SignaturePointer, SignatureLength);
				signatureReader.ReadSignature();
				CallingConvention = signatureReader.CallingConvention;
				HelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, signatureReader.ReturnType, allTypeParameters, GenericParameters);

				if (signatureReader.Parameters != null)
				{
					for (int parametersIndex = 0; parametersIndex < signatureReader.Parameters.Count; parametersIndex++)
					{
						BaseSignatureItem parameterItem = signatureReader.Parameters[parametersIndex];

						HelperFunctions.SetSignatureItemToken(BaseTypeDefinition.ModuleScope.Assembly.AllTokens, parameterItem, allTypeParameters, GenericParameters);
					}
				}
			}
		}

		protected override void ReadMetadataInformation()
		{
			base.ReadMetadataInformation();
			Assembly assembly = BaseTypeDefinition.ModuleScope.Assembly;

			try
			{
				assembly.OpenMetadataInterfaces();

				CustomAttributes = HelperFunctions.EnumCustomAttributes(assembly.Import, assembly, this);

				if (CustomAttributes != null)
				{
					foreach (CustomAttribute customAttribute in CustomAttributes)
					{
						customAttribute.SetText(assembly.AllTokens);
					}
				}

				if ((Flags & CorMethodAttr.mdPinvokeImpl) == CorMethodAttr.mdPinvokeImpl)
				{
					PinvokeMap = HelperFunctions.ReadPinvokeMap(assembly.Import, assembly, Token, Name);
				}

				if (((Flags & CorMethodAttr.mdReservedMask) & CorMethodAttr.mdHasSecurity) == CorMethodAttr.mdHasSecurity)
				{
					PermissionSets = HelperFunctions.EnumPermissionSets(assembly.Import, assembly, Token);
				}
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