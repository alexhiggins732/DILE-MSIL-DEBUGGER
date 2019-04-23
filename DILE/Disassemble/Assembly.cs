using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Dile.Disassemble
{
	public class Assembly : TokenBase, IMultiLine, IDisposable
	{
		#region IMultiLine Members
		private List<CodeLine> codeLines;
		[XmlIgnore()]
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

		[XmlIgnore()]
		public string HeaderText
		{
			get
			{
				return FileName;
			}
		}

		[XmlIgnore()]
		public bool LoadedFromMemory
		{
			get;
			private set;
		}

		[XmlIgnore()]
		public bool IsInMemory
		{
			get;
			private set;
		}
		#endregion

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.Assembly;
			}
		}

		private string fullPath;
		public string FullPath
		{
			get
			{
				return fullPath;
			}

			set
			{
				fullPath = value;
				FileName = Path.GetFileName(FullPath);
			}
		}

		private string fileName;
		[XmlIgnore()]
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

		private ModuleScope moduleScope;
		[XmlIgnore()]
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

		private List<ModuleReference> moduleReferences;
		[XmlIgnore()]
		public List<ModuleReference> ModuleReferences
		{
			get
			{
				return moduleReferences;
			}

			private set
			{
				moduleReferences = value;
			}
		}

		private List<TypeReference> typeReferences;
		[XmlIgnore()]
		public List<TypeReference> TypeReferences
		{
			get
			{
				return typeReferences;
			}

			private set
			{
				typeReferences = value;
			}
		}

		private BinaryReader assemblyReader;
		[XmlIgnore()]
		public BinaryReader AssemblyReader
		{
			get
			{
				return assemblyReader;
			}

			private set
			{
				assemblyReader = value;
			}
		}

		private bool isPe32;
		[XmlIgnore()]
		public bool IsPe32
		{
			get
			{
				return isPe32;
			}
			private set
			{
				isPe32 = value;
			}
		}

		private uint entryPointToken = 0;
		[XmlIgnore()]
		public uint EntryPointToken
		{
			get
			{
				return entryPointToken;
			}
			private set
			{
				entryPointToken = value;
			}
		}

		private Dictionary<uint, UserString> userStrings;
		[XmlIgnore()]
		public Dictionary<uint, UserString> UserStrings
		{
			get
			{
				return userStrings;
			}
			private set
			{
				userStrings = value;
			}
		}

		private Dictionary<uint, TokenBase> allTokens = new Dictionary<uint, TokenBase>();
		[XmlIgnore()]
		public Dictionary<uint, TokenBase> AllTokens
		{
			get
			{
				return allTokens;
			}
			private set
			{
				allTokens = value;
			}
		}

		private Dictionary<uint, StandAloneSignature> standAloneSignatures;
		[XmlIgnore()]
		public Dictionary<uint, StandAloneSignature> StandAloneSignatures
		{
			get
			{
				return standAloneSignatures;
			}
			private set
			{
				standAloneSignatures = value;
			}
		}

		private Dictionary<uint, AssemblyReference> assemblyReferences;
		[XmlIgnore()]
		public Dictionary<uint, AssemblyReference> AssemblyReferences
		{
			get
			{
				return assemblyReferences;
			}
			private set
			{
				assemblyReferences = value;
			}
		}

		private List<CustomAttribute> assemblyCustomAttributes;
		[XmlIgnore()]
		public List<CustomAttribute> AssemblyCustomAttributes
		{
			get
			{
				return assemblyCustomAttributes;
			}
			private set
			{
				assemblyCustomAttributes = value;
			}
		}

		private TypeDefinition globalType;
		[XmlIgnore()]
		public TypeDefinition GlobalType
		{
			get
			{
				return globalType;
			}
			private set
			{
				globalType = value;
			}
		}

		private IMetaDataDispenserEx dispenser;
		[XmlIgnore()]
		public IMetaDataDispenserEx Dispenser
		{
			get
			{
				return dispenser;
			}
			private set
			{
				dispenser = value;
			}
		}

		private IMetaDataImport2 import;
		[XmlIgnore()]
		public IMetaDataImport2 Import
		{
			get
			{
				return import;
			}
			private set
			{
				import = value;
			}
		}

		private IMetaDataAssemblyImport assemblyImport;
		[XmlIgnore()]
		public IMetaDataAssemblyImport AssemblyImport
		{
			get
			{
				return assemblyImport;
			}
			private set
			{
				assemblyImport = value;
			}
		}

		private IntPtr publicKey;
		[XmlIgnore()]
		public IntPtr PublicKey
		{
			get
			{
				return publicKey;
			}
			private set
			{
				publicKey = value;
			}
		}

		private uint publicKeyLength;
		[XmlIgnore()]
		public uint PublicKeyLength
		{
			get
			{
				return publicKeyLength;
			}
			private set
			{
				publicKeyLength = value;
			}
		}

		private uint hashAlgorithmID;
		[XmlIgnore()]
		public uint HashAlgorithmID
		{
			get
			{
				return hashAlgorithmID;
			}
			private set
			{
				hashAlgorithmID = value;
			}
		}

		private AssemblyMetadata metadata;
		[XmlIgnore()]
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

		private CorAssemblyFlags flags;
		[XmlIgnore()]
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

		private uint imageBase;
		[XmlIgnore()]
		public uint ImageBase
		{
			get
			{
				return imageBase;
			}
			private set
			{
				imageBase = value;
			}
		}

		private uint fileAlignment;
		[XmlIgnore()]
		public uint FileAlignment
		{
			get
			{
				return fileAlignment;
			}
			private set
			{
				fileAlignment = value;
			}
		}

		private ulong stackReserve;
		[XmlIgnore()]
		public ulong StackReserve
		{
			get
			{
				return stackReserve;
			}
			private set
			{
				stackReserve = value;
			}
		}

		private ushort subsystem;
		[XmlIgnore()]
		public ushort Subsystem
		{
			get
			{
				return subsystem;
			}
			private set
			{
				subsystem = value;
			}
		}

		private uint corFlags;
		[XmlIgnore()]
		public uint CorFlags
		{
			get
			{
				return corFlags;
			}
			private set
			{
				corFlags = value;
			}
		}

		private List<SectionHeader> sectionHeaders = new List<SectionHeader>();
		[XmlIgnore()]
		public List<SectionHeader> SectionHeaders
		{
			get
			{
				return sectionHeaders;
			}
			private set
			{
				sectionHeaders = value;
			}
		}

		private List<ManifestResource> manifestResources;
		[XmlIgnore()]
		public List<ManifestResource> ManifestResources
		{
			get
			{
				return manifestResources;
			}
			private set
			{
				manifestResources = value;
			}
		}

		[XmlIgnore()]
		public List<ExportedType> ExportedTypes
		{
			get;
			private set;
		}

		private List<File> files;
		[XmlIgnore()]
		public List<File> Files
		{
			get
			{
				return files;
			}
			private set
			{
				files = value;
			}
		}

		private string frameworkVersion;
		[XmlIgnore()]
		public string FrameworkVersion
		{
			get
			{
				return frameworkVersion;
			}
			private set
			{
				frameworkVersion = value;
			}
		}

		private bool displayInTree;
		[XmlIgnore()]
		public bool DisplayInTree
		{
			get
			{
				return displayInTree;
			}
			private set
			{
				displayInTree = value;
			}
		}

		private List<PermissionSet> permissionSets;
		[XmlIgnore()]
		public List<PermissionSet> PermissionSets
		{
			get
			{
				return permissionSets;
			}
			private set
			{
				permissionSets = value;
			}
		}

		private IntPtr fileContentUnmanaged;
		[XmlIgnore()]
		private IntPtr FileContentUnmanaged
		{
			get
			{
				return fileContentUnmanaged;
			}
			set
			{
				fileContentUnmanaged = value;
			}
		}

		private uint fileContentLength;
		[XmlIgnore()]
		private uint FileContentLength
		{
			get
			{
				return fileContentLength;
			}
			set
			{
				fileContentLength = value;
			}
		}

		private ProcessWrapper debuggedProcess;
		[XmlIgnore()]
		public ProcessWrapper DebuggedProcess
		{
			get
			{
				return debuggedProcess;
			}
			private set
			{
				debuggedProcess = value;
			}
		}

		public Assembly()
			: this(false, false)
		{
		}

		public Assembly(bool loadedFromMemory, bool isInMemory)
		{
			LoadedFromMemory = loadedFromMemory;
			IsInMemory = isInMemory;
		}

		public Assembly(string fullPath, bool isDynamicAssembly)
			: this(true, isDynamicAssembly)
		{
			FullPath = fullPath;
			LoadAssembly();
		}

		~Assembly()
		{
			Dispose();
		}

		#region IDisposable Members

		public void Dispose()
		{
			try
			{
				if (AssemblyReader != null)
				{
					AssemblyReader.Close();
					AssemblyReader = null;
				}

				CloseMetadataInterfaces();
				//Todo The FileContentUnmanaged pointer and this point might be invalid thus the memory should be freed earlier/differently.
				Marshal.FreeHGlobal(FileContentUnmanaged);
			}
			catch
			{
			}
		}

		#endregion

		public void CloseMetadataInterfaces()
		{
			CloseMetadataInterfaces(false);
		}

		private void CloseMetadataInterfaces(bool forceClosing)
		{
			if (!LoadedFromMemory || forceClosing)
			{
				ReleaseObject(Dispenser);
				ReleaseObject(Import);
				ReleaseObject(AssemblyImport);
			}
		}

		private void ReleaseObject(object comObject)
		{
			if (comObject != null)
			{
				Marshal.ReleaseComObject(comObject);
				comObject = null;
			}
		}

		public void CloseAssemblyReader()
		{
			AssemblyReader.Close();
			AssemblyReader = null;
			GC.Collect();
		}

		public void OpenAssemblyReader()
		{
			if (AssemblyReader == null)
			{
				byte[] fileContent = new byte[FileContentLength];
				Marshal.Copy(FileContentUnmanaged, fileContent, 0, Convert.ToInt32(FileContentLength));

				AssemblyReader = new BinaryReader(new MemoryStream(fileContent));
			}

			AssemblyReader.BaseStream.Seek(0, SeekOrigin.Begin);
		}

		public void OpenMetadataInterfaces()
		{
			OpenMetadataInterfaces(false, null);
		}

		private void OpenMetadataInterfaces(bool forceOpening, ModuleWrapper module)
		{
			if (forceOpening || !LoadedFromMemory)
			{
				if (LoadedFromMemory)
				{
					Dispenser = new MetaDataDispenserEx();
					AssemblyImport = (IMetaDataAssemblyImport)module.GetMetaDataAssemblyImport();
					Import = (IMetaDataImport2)module.GetMetaDataImport2();
				}
				else
				{
					Dispenser = new MetaDataDispenserEx();
					object rawScope = null;
					Guid assemblyImportGuid = Guids.IID_IMetaDataAssemblyImport;
					Dispenser.OpenScopeOnMemory(FileContentUnmanaged, FileContentLength, (uint)CorOpenFlags.ofRead, ref assemblyImportGuid, out rawScope);
					AssemblyImport = (IMetaDataAssemblyImport)rawScope;

					object rawScope2 = null;
					Guid metaDataImportGuid = Guids.IID_IMetaDataImport2;
					Dispenser.OpenScopeOnMemory(FileContentUnmanaged, FileContentLength, (uint)CorOpenFlags.ofRead, ref metaDataImportGuid, out rawScope2);
					Import = (IMetaDataImport2)rawScope2;
				}
			}
		}

		public void ReopenMetadataInterfaces(ModuleWrapper module)
		{
			CloseMetadataInterfaces(true);
			OpenMetadataInterfaces(true, module);
		}

		public void LoadAssembly()
		{
			byte[] fileContent;
			using (FileStream fileStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read))
			{
				FileContentLength = Convert.ToUInt32(fileStream.Length);
				fileContent = new byte[FileContentLength];
				fileStream.Seek(0, SeekOrigin.Begin);
				fileStream.Read(fileContent, 0, Convert.ToInt32(FileContentLength));
			}

			FileContentUnmanaged = Marshal.AllocHGlobal(fileContent.Length);
			Marshal.Copy(fileContent, 0, FileContentUnmanaged, fileContent.Length);

			AssemblyReader = new BinaryReader(new MemoryStream(fileContent));
			OpenMetadataInterfaces();

			LoadAssemblyFromMetadataInterfaces();

			CloseMetadataInterfaces();
			CloseAssemblyReader();
		}

		public void LoadAssemblyFromMetadataInterfaces(IMetaDataDispenserEx dispenser, IMetaDataAssemblyImport assemblyImport, IMetaDataImport2 import, ModuleWrapper debuggedModule)
		{
			Dispenser = dispenser;
			AssemblyImport = assemblyImport;
			Import = import;

			uint bufferCount;
			Import.GetVersionString(Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out bufferCount);

			if (bufferCount > Project.DefaultCharArray.Length)
			{
				Project.DefaultCharArray = new char[bufferCount];
				Import.GetVersionString(Project.DefaultCharArray, bufferCount, out bufferCount);
			}

			FrameworkVersion = HelperFunctions.GetString(Project.DefaultCharArray, 0, bufferCount);
			FullPath = debuggedModule.GetName();
			FileName = Name;

			LoadAssemblyFromMetadataInterfaces(debuggedModule);
		}

		public void LoadAssemblyFromMetadataInterfaces()
		{
			LoadAssemblyFromMetadataInterfaces(null);
		}

		public void LoadAssemblyFromMetadataInterfaces(ModuleWrapper debuggedModule)
		{
			string assemblyPath = (LoadedFromMemory ? Name : FullPath);
			UIHandler.Instance.ResetProgressBar();

			if (LoadedFromMemory)
			{
				UIHandler.Instance.SetProgressBarMaximum(16);
				DebuggedProcess = debuggedModule.GetProcess();
			}
			else
			{
				UIHandler.Instance.SetProgressBarMaximum(17);
				UIHandler.Instance.SetProgressText(assemblyPath, "Reading header...", true);
				ReadHeader();
				UIHandler.Instance.StepProgressBar(1);
			}

			UIHandler.Instance.SetProgressText(assemblyPath, "Opening assembly references...", true);
			OpenAssemblyRefs();
			UIHandler.Instance.StepProgressBar(1);

			UIHandler.Instance.SetProgressText(assemblyPath, "Loading user strings...", true);
			GetUserStrings();
			UIHandler.Instance.StepProgressBar(1);

			UIHandler.Instance.SetProgressText(assemblyPath, "Loading manifest resources...", true);
			GetManifestResources();
			UIHandler.Instance.StepProgressBar(1);

			UIHandler.Instance.SetProgressText(assemblyPath, "Loading files...", true);
			GetFiles();
			UIHandler.Instance.StepProgressBar(1);

			UIHandler.Instance.SetProgressText(assemblyPath, "Loading module references...", true);
			GetModuleReferences();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading type references...", true);
			GetTypeReferences();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading global type's references...", true);
			HelperFunctions.GetMemberReferences(this, 0);
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading type specifications...", true);
			GetTypeSpecs();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading exported types...", true);
			GetExportedTypes();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading standalone signatures...", true);
			GetSignatures();
			UIHandler.Instance.StepProgressBar(1);

			UIHandler.Instance.SetProgressText(assemblyPath, "Loading module scope...", true);
			ModuleScope = new ModuleScope(Import, this);
			ModuleScope.EnumerateTypeDefinitions(Import, debuggedModule);
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading global type...", true);
			GlobalType = new TypeDefinition(Import, ModuleScope, 0);
			UIHandler.Instance.StepProgressBar(1);
			AllTokens[0] = GlobalType;

			UIHandler.Instance.SetProgressText(assemblyPath, "Associating properties with methods...", true);
			AssociateMethodsWithOwnerObjects();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Reading assembly properties...", true);

			DisplayInTree = false;
			try
			{
				ReadProperties();
				DisplayInTree = true;
			}
			catch (COMException comException)
			{
				unchecked
				{
					if (comException.ErrorCode != (int)0x80131130)
					{
						throw;
					}
				}
			}

			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Loading resolving resolution scopes...", true);
			ResolveResolutionScopes();
			UIHandler.Instance.StepProgressBar(1);
			UIHandler.Instance.SetProgressText(assemblyPath, "Searching for entry method...", true);
			SearchEntryPoint();
			UIHandler.Instance.StepProgressBar(1);
		}

		private void AssociatePropertyWithMethod(Property property, uint methodToken)
		{
			if (AllTokens.ContainsKey(methodToken))
			{
				MethodDefinition methodDefinition = (MethodDefinition)AllTokens[methodToken];
				methodDefinition.OwnerProperty = property;
			}
		}

		private void AssociateEventDefinitionWithMethod(EventDefinition eventDefinition, uint methodToken)
		{
			if (AllTokens.ContainsKey(methodToken))
			{
				MethodDefinition methodDefinition = (MethodDefinition)AllTokens[methodToken];
				methodDefinition.OwnerEventDefinition = eventDefinition;
			}
		}

		private void AssociateMethodsWithOwnerObjects()
		{
			foreach (TokenBase tokenObject in AllTokens.Values)
			{
				Property property = tokenObject as Property;

				if (property != null)
				{
					AssociatePropertyWithMethod(property, property.GetterMethodToken);
					AssociatePropertyWithMethod(property, property.SetterMethodToken);

					for (int index = 0; index < property.OtherMethodsCount; index++)
					{
						uint methodToken = property.OtherMethods[index];
						AssociatePropertyWithMethod(property, methodToken);
					}
				}
				else
				{
					EventDefinition eventDefinition = tokenObject as EventDefinition;

					if (eventDefinition != null)
					{
						AssociateEventDefinitionWithMethod(eventDefinition, eventDefinition.AddOnMethodToken);
						AssociateEventDefinitionWithMethod(eventDefinition, eventDefinition.FireMethodToken);
						AssociateEventDefinitionWithMethod(eventDefinition, eventDefinition.RemoveOnMethodToken);

						for (int index = 0; index < eventDefinition.OtherMethodsCount; index++)
						{
							uint methodToken = eventDefinition.OtherMethods[index];
							AssociateEventDefinitionWithMethod(eventDefinition, methodToken);
						}
					}
				}
			}
		}

		private void GetModuleReferences()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] moduleRefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			Import.EnumModuleRefs(ref enumHandle, moduleRefs, Convert.ToUInt32(moduleRefs.Length), out count);

			if (count > 0)
			{
				ModuleReferences = new List<ModuleReference>();
			}

			while (count > 0)
			{
				for (uint moduleRefsIndex = 0; moduleRefsIndex < count; moduleRefsIndex++)
				{
					uint token = moduleRefs[moduleRefsIndex];
					uint moduleRefNameLength;

					Import.GetModuleRefProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out moduleRefNameLength);

					if (moduleRefNameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[moduleRefNameLength];

						Import.GetModuleRefProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out moduleRefNameLength);
					}

					ModuleReference moduleReference = new ModuleReference(this, token, HelperFunctions.GetString(Project.DefaultCharArray, 0, moduleRefNameLength));
					ModuleReferences.Add(moduleReference);
					AllTokens[token] = moduleReference;
				}

				Import.EnumModuleRefs(ref enumHandle, moduleRefs, Convert.ToUInt32(moduleRefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				Import.CloseEnum(enumHandle);
			}
		}

		private void GetTypeReferences()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeRefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			Import.EnumTypeRefs(ref enumHandle, typeRefs, Convert.ToUInt32(typeRefs.Length), out count);

			if (count > 0)
			{
				TypeReferences = new List<TypeReference>();
			}

			while (count > 0)
			{
				for (uint typeRefsIndex = 0; typeRefsIndex < count; typeRefsIndex++)
				{
					uint token = typeRefs[typeRefsIndex];
					uint typeRefNameLength;
					uint resolutionScope;

					Import.GetTypeRefProps(token, out resolutionScope, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out typeRefNameLength);

					if (typeRefNameLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[typeRefNameLength];

						Import.GetTypeRefProps(token, out resolutionScope, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out typeRefNameLength);
					}

					TypeReference typeReference = new TypeReference(Import, this, HelperFunctions.GetString(Project.DefaultCharArray, 0, typeRefNameLength), token, resolutionScope);
					TypeReferences.Add(typeReference);
					AllTokens[token] = typeReference;
				}

				Import.EnumTypeRefs(ref enumHandle, typeRefs, Convert.ToUInt32(typeRefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				Import.CloseEnum(enumHandle);
			}
		}

		public void GetUserStrings()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] rStrings = new uint[Project.DefaultArrayCount];
			uint count = 0;
			Import.EnumUserStrings(ref enumHandle, rStrings, Convert.ToUInt32(rStrings.Length), out count);
			uint token = 0;
			Func<UserString> readUserString = null;

			if (count > 0)
			{
				UserStrings = new Dictionary<uint, UserString>();

				readUserString = delegate()
				{
					UserString result = null;
					uint userStringLength;

					Import.GetUserString(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out userStringLength);

					if (userStringLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[userStringLength];

						Import.GetUserString(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out userStringLength);
					}

					result = new UserString(token, HelperFunctions.GetString(Project.DefaultCharArray, 0, userStringLength));

					return result;
				};
			}

			while (count > 0)
			{
				for (uint rStringsIndex = 0; rStringsIndex < count; rStringsIndex++)
				{
					token = rStrings[rStringsIndex];
					UserString userStringObject = ExceptionHandler.ReadToken<UserString>(readUserString, UserString.IncorrectToken);

					UserStrings[token] = userStringObject;
					AllTokens[token] = userStringObject;
				}

				Import.EnumUserStrings(ref enumHandle, rStrings, Convert.ToUInt32(rStrings.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				Import.CloseEnum(enumHandle);
			}
		}

		private void GetTypeSpecs()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeSpecs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			Import.EnumTypeSpecs(ref enumHandle, typeSpecs, Convert.ToUInt32(typeSpecs.Length), out count);

			while (count > 0)
			{
				for (uint typeSpecsIndex = 0; typeSpecsIndex < count; typeSpecsIndex++)
				{
					uint token = typeSpecs[typeSpecsIndex];
					IntPtr signature;
					uint signatureLength;

					Import.GetTypeSpecFromToken(token, out signature, out signatureLength);
					TypeSpecification typeSpecification = new TypeSpecification(this, token, signature, signatureLength);
					AllTokens[token] = typeSpecification;
				}

				Import.EnumTypeSpecs(ref enumHandle, typeSpecs, Convert.ToUInt32(typeSpecs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				Import.CloseEnum(enumHandle);
			}
		}

		private void GetSignatures()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] signatures = new uint[Project.DefaultArrayCount];
			uint count = 0;
			Import.EnumSignatures(ref enumHandle, signatures, Convert.ToUInt32(signatures.Length), out count);

			if (count > 0)
			{
				StandAloneSignatures = new Dictionary<uint, StandAloneSignature>();
			}

			while (count > 0)
			{
				for (uint signaturesIndex = 0; signaturesIndex < count; signaturesIndex++)
				{
					uint token = signatures[signaturesIndex];
					IntPtr signature;
					uint signatureLength;

					Import.GetSigFromToken(token, out signature, out signatureLength);
					StandAloneSignature standAloneSignature = new StandAloneSignature(this, token, signature, signatureLength);
					StandAloneSignatures[token] = standAloneSignature;
					AllTokens[token] = standAloneSignature;
				}

				Import.EnumSignatures(ref enumHandle, signatures, Convert.ToUInt32(signatures.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				Import.CloseEnum(enumHandle);
			}
		}

		private void GetManifestResources()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] manifestResources = new uint[Project.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumManifestResources(ref enumHandle, manifestResources, Convert.ToUInt32(manifestResources.Length), out count);

			if (count > 0)
			{
				ManifestResources = new List<ManifestResource>();
			}

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					uint token = manifestResources[index];
					uint nameCount;
					uint providerToken;
					uint offset;
					uint flags;

					AssemblyImport.GetManifestResourceProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameCount, out providerToken, out offset, out flags);

					if (nameCount > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[nameCount];

						AssemblyImport.GetManifestResourceProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameCount, out providerToken, out offset, out flags);
					}

					ManifestResource manifestResource = new ManifestResource(this, token, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameCount), providerToken, offset, flags);
					ManifestResources.Add(manifestResource);
					AllTokens[token] = manifestResource;
				}

				AssemblyImport.EnumManifestResources(ref enumHandle, manifestResources, Convert.ToUInt32(manifestResources.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				AssemblyImport.CloseEnum(enumHandle);
			}
		}

		private void GetExportedTypes()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] exportedTypes = new uint[Project.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumExportedTypes(ref enumHandle, exportedTypes, Convert.ToUInt32(exportedTypes.Length), out count);

			if (count > 0)
			{
				ExportedTypes = new List<ExportedType>();
			}

			while (count > 0)
			{
				for (uint exportedTypesIndex = 0; exportedTypesIndex < count; exportedTypesIndex++)
				{
					uint token = exportedTypes[exportedTypesIndex];
					uint nameCount;
					uint implementationToken;
					uint typeDefinitionToken;
					uint flags;

					AssemblyImport.GetExportedTypeProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameCount, out implementationToken, out typeDefinitionToken, out flags);

					if (nameCount > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[nameCount];

						AssemblyImport.GetExportedTypeProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameCount, out implementationToken, out typeDefinitionToken, out flags);
					}

					ExportedType exportedType = new ExportedType(this, token, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameCount), implementationToken, flags);
					ExportedTypes.Add(exportedType);
					AllTokens[token] = exportedType;
				}

				AssemblyImport.EnumExportedTypes(ref enumHandle, exportedTypes, Convert.ToUInt32(exportedTypes.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				AssemblyImport.CloseEnum(enumHandle);
			}
		}

		private void GetFiles()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] files = new uint[Project.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumFiles(ref enumHandle, files, Convert.ToUInt32(files.Length), out count);

			if (count > 0)
			{
				Files = new List<File>();
			}

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					uint token = files[index];
					uint nameCount;
					IntPtr hash;
					uint hashLength;
					uint flags;

					AssemblyImport.GetFileProps(token, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameCount, out hash, out hashLength, out flags);

					if (nameCount > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[nameCount];

						AssemblyImport.GetFileProps(token, Project.DefaultCharArray, Convert.ToUInt32(Name.Length), out nameCount, out hash, out hashLength, out flags);
					}

					File file = new File(this, token, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameCount), hash, hashLength, flags);
					Files.Add(file);
					AllTokens[token] = file;
				}

				AssemblyImport.EnumFiles(ref enumHandle, files, Convert.ToUInt32(files.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				AssemblyImport.CloseEnum(enumHandle);
			}
		}

		private void ReadHeader()
		{
			AssemblyReader.BaseStream.Position = 0;
			byte[] dosHeader = AssemblyReader.ReadBytes(128);

			if (dosHeader[0] != 0x4d || dosHeader[1] != 0x5a)
			{
				throw new InvalidProgramException("The assembly doesn't contain a valid DOS header.");
			}

			uint lfanew = BitConverter.ToUInt32(dosHeader, 0x3c);
			if (lfanew == 0)
			{
				lfanew = 128;
			}

			AssemblyReader.BaseStream.Seek(lfanew, SeekOrigin.Begin);
			byte[] peSignature = AssemblyReader.ReadBytes(24);

			if (peSignature[0] != 0x50 || peSignature[1] != 0x45)
			{
				throw new InvalidProgramException("The assembly doesn't contain a valid PE signature.");
			}

			int numberOfSections = BitConverter.ToUInt16(peSignature, 6);

			AssemblyReader.BaseStream.Seek(lfanew + 24, SeekOrigin.Begin);
			IsPe32 = (AssemblyReader.ReadUInt16() == 0x010b);

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 224, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 240, SeekOrigin.Begin);
			}

			for (int sectionIndex = 0; sectionIndex < numberOfSections; sectionIndex++)
			{
				SectionHeader sectionHeader = new SectionHeader();

				AssemblyReader.BaseStream.Seek(12, SeekOrigin.Current);
				sectionHeader.VirtualAddress = AssemblyReader.ReadUInt32();
				sectionHeader.SizeOfRawData = AssemblyReader.ReadUInt32();
				sectionHeader.PointerToRawData = AssemblyReader.ReadUInt32();
				AssemblyReader.BaseStream.Seek(16, SeekOrigin.Current);

				SectionHeaders.Add(sectionHeader);
			}

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 28, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 24, SeekOrigin.Begin);
			}

			ImageBase = AssemblyReader.ReadUInt32();

			if (IsPe32)
			{
				AssemblyReader.ReadUInt32();
			}
			else
			{
				AssemblyReader.ReadUInt32();
				AssemblyReader.ReadUInt32();
			}

			FileAlignment = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(lfanew + 24 + 68, SeekOrigin.Begin);
			Subsystem = AssemblyReader.ReadUInt16();
			AssemblyReader.ReadUInt16();

			if (IsPe32)
			{
				StackReserve = AssemblyReader.ReadUInt32();
			}
			else
			{
				StackReserve = AssemblyReader.ReadUInt64();
			}

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 208, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 224, SeekOrigin.Begin);
			}

			uint cliHeaderAddress = GetMethodAddress(AssemblyReader.ReadUInt32());
			uint cliHeaderSize = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(cliHeaderAddress + 8, SeekOrigin.Begin);
			uint metadataRootRva = AssemblyReader.ReadUInt32();
			AssemblyReader.BaseStream.Seek(4, SeekOrigin.Current);
			CorFlags = AssemblyReader.ReadUInt32();
			EntryPointToken = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(GetMethodAddress(metadataRootRva) + 12, SeekOrigin.Begin);
			int frameworkVersionStringLength = AssemblyReader.ReadInt32();
			char[] frameworkVersionString = AssemblyReader.ReadChars(frameworkVersionStringLength);
			FrameworkVersion = HelperFunctions.TrimString(frameworkVersionString);
		}

		public uint GetMethodAddress(uint rva)
		{
			uint result = 0;
			bool found = false;
			int index = 0;

			while (!found && index < SectionHeaders.Count)
			{
				SectionHeader sectionHeader = SectionHeaders[index++];

				if (sectionHeader.ContainsRvaAddress(rva))
				{
					found = true;
					result = rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
				}
			}

			if (!found)
			{
				throw new InvalidOperationException("The given RVA does not seem to belong to any section. Perhaps this is not a valid .NET assembly.");
			}

			return result;
		}

		private void SearchEntryPoint()
		{
			if (AllTokens.ContainsKey(EntryPointToken))
			{
				TokenBase token = AllTokens[EntryPointToken];

				if (token is MethodDefinition)
				{
					((MethodDefinition)token).EntryPoint = true;
				}
			}
		}

		private string SafePathCombine(string path1, string path2)
		{
			string result;

			try
			{
				result = Path.Combine(path1, path2);
			}
			catch
			{
				result = string.Empty;
			}

			return result;
		}

		private void OpenAssemblyRefs()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] assemblyRefs = new uint[Project.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumAssemblyRefs(ref enumHandle, assemblyRefs, Convert.ToUInt32(assemblyRefs.Length), out count);

			uint systemDirectoryLength;
			Dispenser.GetCORSystemDirectory(Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.LongLength), out systemDirectoryLength);
			string systemDirectoryPath = HelperFunctions.GetString(Project.DefaultCharArray, 0, systemDirectoryLength);
			systemDirectoryPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(systemDirectoryPath)), FrameworkVersion) + "\\";
			string assemblyDirectoryPath = string.Format("{0}\\", Path.GetDirectoryName(FullPath));

			if (count > 0)
			{
				AssemblyReferences = new Dictionary<uint, AssemblyReference>();
			}

			while (count > 0)
			{
				for (uint assemblyRefsIndex = 0; assemblyRefsIndex < count; assemblyRefsIndex++)
				{
					uint token = assemblyRefs[assemblyRefsIndex];
					IntPtr PublicKeyOrToken;
					uint PublicKeyOrTokenLength;
					uint nameStringLength;
					AssemblyMetadata metadata = new AssemblyMetadata();
					IntPtr hashBlob;
					uint hashBlobLength;
					uint assemblyFlags;

					AssemblyImport.GetAssemblyRefProps(token, out PublicKeyOrToken, out PublicKeyOrTokenLength, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.LongLength), out nameStringLength, ref metadata, out hashBlob, out hashBlobLength, out assemblyFlags);

					if (nameStringLength > Project.DefaultCharArray.Length)
					{
						Project.DefaultCharArray = new char[nameStringLength];

						AssemblyImport.GetAssemblyRefProps(token, out PublicKeyOrToken, out PublicKeyOrTokenLength, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.LongLength), out nameStringLength, ref metadata, out hashBlob, out hashBlobLength, out assemblyFlags);
					}

					AssemblyReference assemblyRef = new AssemblyReference(this, token, HelperFunctions.GetString(Project.DefaultCharArray, 0, nameStringLength), PublicKeyOrToken, PublicKeyOrTokenLength, metadata, hashBlob, hashBlobLength, assemblyFlags);

					string exeAssemblyPath = string.Format("{0}.exe", assemblyRef.Name);
					string dllAssemblyPath = string.Format("{0}.dll", assemblyRef.Name);
					string exeAssemblyRefPath = SafePathCombine(assemblyDirectoryPath, exeAssemblyPath);
					string dllAssemblyRefPath = SafePathCombine(assemblyDirectoryPath, dllAssemblyPath);
					string dllSystemPath = SafePathCombine(systemDirectoryPath, dllAssemblyPath);
					string exeSystemPath = SafePathCombine(systemDirectoryPath, dllAssemblyPath);

					if (!string.IsNullOrEmpty(dllAssemblyRefPath) && System.IO.File.Exists(dllAssemblyRefPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = dllAssemblyRefPath;
					}
					else if (!string.IsNullOrEmpty(exeAssemblyRefPath) && System.IO.File.Exists(exeAssemblyRefPath))
					{
						assemblyRef.FileName = exeAssemblyPath;
						assemblyRef.FullPath = exeAssemblyRefPath;
					}
					else if (!string.IsNullOrEmpty(dllSystemPath) && System.IO.File.Exists(dllSystemPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = dllSystemPath;
					}
					else if (!string.IsNullOrEmpty(exeSystemPath) && System.IO.File.Exists(exeSystemPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = exeSystemPath;
					}
					else
					{
						AssemblyName assemblyName = new AssemblyName();
						assemblyName.Name = assemblyRef.Name;
						assemblyName.Version = new Version(assemblyRef.Metadata.usMajorVersion, assemblyRef.Metadata.usMinorVersion, assemblyRef.Metadata.usBuildNumber, assemblyRef.Metadata.usRevisionNumber);

						if (assemblyRef.Metadata.szLocale != IntPtr.Zero)
						{
							throw new NotImplementedException("The assembly local is different from default.");
						}

						assemblyName.CultureInfo = CultureInfo.InvariantCulture;

						byte[] publicKey = HelperFunctions.ReadBlobAsByteArray(assemblyRef.PublicKeyOrToken, assemblyRef.PublicKeyOrTokenLength);

						if ((assemblyRef.Flags & CorAssemblyFlags.afPublicKey) == CorAssemblyFlags.afPublicKey)
						{
							assemblyName.SetPublicKey(publicKey);
						}
						else
						{
							assemblyName.SetPublicKeyToken(publicKey);
						}

						try
						{
							System.Reflection.Assembly referencedAssembly = System.Reflection.Assembly.Load(assemblyName);

							assemblyRef.FullPath = Path.GetFullPath(new Uri(referencedAssembly.CodeBase).AbsolutePath);
							assemblyRef.FileName = Path.GetFileName(assemblyRef.FullPath);
						}
						catch
						{
						}
					}

					AssemblyReferences[assemblyRef.Token] = assemblyRef;
					AllTokens[assemblyRef.Token] = assemblyRef;
				}

				AssemblyImport.EnumAssemblyRefs(ref enumHandle, assemblyRefs, Convert.ToUInt32(assemblyRefs.Length), out count);
			}

			if (enumHandle != IntPtr.Zero)
			{
				AssemblyImport.CloseEnum(enumHandle);
			}
		}

		private void FindReferencedAssembly(TypeReference typeReference, uint resolutionScope, bool recursiveCall)
		{
			TokenBase tokenObject = AllTokens[resolutionScope];

			if (tokenObject is TypeReference)
			{
				TypeReference typeReferenceToken = (TypeReference)tokenObject;

				if (resolutionScope == typeReferenceToken.ResolutionScope)
				{
					typeReference.FullName = Constants.IncorrectTokenName;
				}
				else
				{
					FindReferencedAssembly(typeReference, typeReferenceToken.ResolutionScope, true);
					typeReference.FullName = string.Format("{0}/{1}", typeReferenceToken.FullName, typeReference.Name);
				}
			}
			else if (tokenObject is ModuleScope)
			{
				typeReference.FullName = typeReference.Name;
			}
			else
			{
				typeReference.ReferencedAssembly = tokenObject.Name;

				if (recursiveCall)
				{
					typeReference.FullName = string.Format("[{0}]{1}", typeReference.ReferencedAssembly, tokenObject.Name);
				}
				else
				{
					typeReference.FullName = string.Format("[{0}]{1}", typeReference.ReferencedAssembly, typeReference.Name);
				}
			}
		}

		private void ResolveResolutionScopes()
		{
			if (TypeReferences != null)
			{
				foreach (TypeReference typeReference in TypeReferences)
				{
					if (AllTokens.ContainsKey(typeReference.ResolutionScope))
					{
						FindReferencedAssembly(typeReference, typeReference.ResolutionScope, false);
					}
				}
			}
		}

		private void ReadProperties()
		{
			uint token;
			AssemblyImport.GetAssemblyFromScope(out token);
			Token = token;
			IntPtr publicKey;
			uint publicKeyLength;
			uint hashAlgorithmId;
			uint nameLength;
			AssemblyMetadata metadata = new AssemblyMetadata();
			uint flags;
			AssemblyImport.GetAssemblyProps(Token, out publicKey, out publicKeyLength, out hashAlgorithmId, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, ref metadata, out flags);

			if (nameLength > Project.DefaultCharArray.Length)
			{
				Project.DefaultCharArray = new char[nameLength];

				AssemblyImport.GetAssemblyProps(Token, out publicKey, out publicKeyLength, out hashAlgorithmId, Project.DefaultCharArray, Convert.ToUInt32(Project.DefaultCharArray.Length), out nameLength, ref metadata, out flags);
			}

			PublicKey = publicKey;
			PublicKeyLength = publicKeyLength;
			HashAlgorithmID = hashAlgorithmId;
			Name = HelperFunctions.GetString(Project.DefaultCharArray, 0, nameLength);
			Metadata = metadata;
			Flags = (CorAssemblyFlags)flags;

			if (Flags != CorAssemblyFlags.afPA_None
				&& Flags != CorAssemblyFlags.afPublicKey
				&& Flags != CorAssemblyFlags.afPA_NoPlatform
				&& Flags != (CorAssemblyFlags.afPA_NoPlatform | CorAssemblyFlags.afPublicKey))
			{
				throw new NotImplementedException("Unknown assembly flag value.");
			}

			AssemblyCustomAttributes = HelperFunctions.EnumCustomAttributes(Import, this, this);
			PermissionSets = HelperFunctions.EnumPermissionSets(Import, this, Token);
		}

		public void Initialize()
		{
			CodeLines = new List<CodeLine>();
			CodeLines.Add(new CodeLine(0, ".assembly " + Name));
			CodeLines.Add(new CodeLine(0, "{"));
			CodeLines.Add(new CodeLine(1, "//Full Path: " + FullPath));
			CodeLines.Add(new CodeLine(1, "//Metadata version: " + FrameworkVersion));

			if (AssemblyCustomAttributes != null)
			{
				foreach (CustomAttribute customAttribute in AssemblyCustomAttributes)
				{
					customAttribute.SetText(AllTokens);
					CodeLines.Add(new CodeLine(1, customAttribute.Name));
				}

				CodeLines.Add(new CodeLine(1, string.Empty));
			}

			if (PermissionSets != null)
			{
				foreach (PermissionSet permissionSet in PermissionSets)
				{
					permissionSet.SetText(AllTokens);
					CodeLines.Add(new CodeLine(1, permissionSet.Name));
				}

				CodeLines.Add(new CodeLine(1, string.Empty));
			}

			if (PublicKeyLength > 0)
			{
				CodeLines.Add(new CodeLine(1, ".publickey = " + HelperFunctions.ReadBlobAsString(PublicKey, PublicKeyLength)));
			}

			CodeLines.Add(new CodeLine(1, ".hash algorithm 0x" + HelperFunctions.FormatAsHexNumber(HashAlgorithmID, 8)));
			CodeLines.Add(new CodeLine(1, string.Format(".ver {0}:{1}:{2}:{3}", Metadata.usMajorVersion, Metadata.usMinorVersion, Metadata.usBuildNumber, Metadata.usRevisionNumber)));

			CodeLines.Add(new CodeLine(0, "} // end of assembly " + FileName));
		}
	}
}