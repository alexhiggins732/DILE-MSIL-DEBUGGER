using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata
{
	public enum OpCodeGroup
	{
		Parameterless = 1,
		FieldParameter = 2,
		MethodParameter = 4,
		StringParameter = 8,
		TypeParameter = 16,
		SbyteLocationParameter = 32,
		IntLocationParameter = 64,
		ByteParameter = 128,
		UshortParameter = 256,
		SbyteParameter = 512,
		IntParameter = 1024,
		LongParameter = 2048,
		FloatParameter = 4096,
		DoubleParameter = 8192,
		ByteArgumentParameter = 16384,
		UshortArgumentParameter = 32768,
		ByteVariableParameter = 65536,
		UshortVariableParameter = 131072,
		Calli = 262144,
		Switch = 524288,
		Ldtoken = 1048576
	}

	[Flags()]
	public enum ILMethodHeader : byte
	{
		CorILMethod_FatFormat = 0x3,
		CorILMethod_TinyFormat = 0x2,
		CorILMethod_MoreSects = 0x8,
		CorILMethod_InitLocals = 0x10
	}

	[Flags()]
	public enum CorTypeAttr : uint
	{
		// Use this mask to retrieve the type visibility information.
		tdVisibilityMask = 0x00000007,
		tdNotPublic = 0x00000000,     // Class is not public scope.
		tdPublic = 0x00000001,     // Class is public scope.
		tdNestedPublic = 0x00000002,     // Class is nested with public visibility.
		tdNestedPrivate = 0x00000003,     // Class is nested with private visibility.
		tdNestedFamily = 0x00000004,     // Class is nested with family visibility.
		tdNestedAssembly = 0x00000005,     // Class is nested with assembly visibility.
		tdNestedFamANDAssem = 0x00000006,     // Class is nested with family and assembly visibility.
		tdNestedFamORAssem = 0x00000007,     // Class is nested with family or assembly visibility.

		// Use this mask to retrieve class layout information
		tdLayoutMask = 0x00000018,
		tdAutoLayout = 0x00000000,     // Class fields are auto-laid out
		tdSequentialLayout = 0x00000008,     // Class fields are laid out sequentially
		tdExplicitLayout = 0x00000010,     // Layout is supplied explicitly
		// end layout mask

		// Use this mask to retrieve class semantics information.
		tdClassSemanticsMask = 0x00000060,
		tdClass = 0x00000000,     // Type is a class.
		tdInterface = 0x00000020,     // Type is an interface.
		// end semantics mask

		// Special semantics in addition to class semantics.
		tdAbstract = 0x00000080,     // Class is abstract
		tdSealed = 0x00000100,     // Class is concrete and may not be extended
		tdSpecialName = 0x00000400,     // Class name is special.  Name describes how.

		// Implementation attributes.
		tdImport = 0x00001000,     // Class / interface is imported
		tdSerializable = 0x00002000,     // The class is Serializable.

		// Use tdStringFormatMask to retrieve string information for native interop
		tdStringFormatMask = 0x00030000,
		tdAnsiClass = 0x00000000,     // LPTSTR is interpreted as ANSI in this class
		tdUnicodeClass = 0x00010000,     // LPTSTR is interpreted as UNICODE
		tdAutoClass = 0x00020000,     // LPTSTR is interpreted automatically
		// end string format mask

		tdBeforeFieldInit = 0x00100000,     // Initialize the class any time before first static field access.
		tdForwarder = 0x00200000,     // This ExportedType is a type forwarder.

		// Flags reserved for runtime use.
		tdReservedMask = 0x00040800,
		tdRTSpecialName = 0x00000800,     // Runtime should check name encoding.
		tdHasSecurity = 0x00040000,     // Class has security associate with it.
	}

	public enum CorSaveSize : uint
	{
		cssAccurate = 0x0000,               // Find exact save size, accurate but slower.
		cssQuick = 0x0001,               // Estimate save size, may pad estimate, but faster.
		cssDiscardTransientCAs = 0x0002,               // remove all of the CAs of discardable types
	}

	[Flags()]
	// MethodImpl attr bits, used by DefineMethodImpl.
	public enum CorMethodImpl : uint
	{
		// code impl mask
		miCodeTypeMask = 0x0003,   // Flags about code type.
		miIL = 0x0000,   // Method impl is IL.
		miNative = 0x0001,   // Method impl is native.
		miOPTIL = 0x0002,   // Method impl is OPTIL
		miRuntime = 0x0003,   // Method impl is provided by the runtime.
		// end code impl mask

		// managed mask
		miManagedMask = 0x0004,   // Flags specifying whether the code is managed or unmanaged.
		miUnmanaged = 0x0004,   // Method impl is unmanaged, otherwise managed.
		miManaged = 0x0000,   // Method impl is managed.
		// end managed mask

		// implementation info and interop
		miForwardRef = 0x0010,   // Indicates method is defined; used primarily in merge scenarios.
		miPreserveSig = 0x0080,   // Indicates method sig is not to be mangled to do HRESULT conversion.

		miInternalCall = 0x1000,   // Reserved for internal use.

		miSynchronized = 0x0020,   // Method is single threaded through the body.
		miNoInlining = 0x0008,   // Method may not be inlined.
		miMaxMethodImplVal = 0xffff,   // Range check value
	}

	public enum CorValidatorModuleType : uint
	{
		ValidatorModuleTypeInvalid = 0x0,
		ValidatorModuleTypeMin = 0x00000001,
		ValidatorModuleTypePE = 0x00000001,
		ValidatorModuleTypeObj = 0x00000002,
		ValidatorModuleTypeEnc = 0x00000003,
		ValidatorModuleTypeIncr = 0x00000004,
		ValidatorModuleTypeMax = 0x00000004,
	}

	[Flags()]
	public enum CorMethodAttr : uint
	{
		// member access mask - Use this mask to retrieve accessibility information.
		mdMemberAccessMask = 0x0007,
		mdPrivateScope = 0x0000,     // Member not referenceable.
		mdPrivate = 0x0001,     // Accessible only by the parent type.
		mdFamANDAssem = 0x0002,     // Accessible by sub-types only in this Assembly.
		mdAssem = 0x0003,     // Accessibly by anyone in the Assembly.
		mdFamily = 0x0004,     // Accessible only by type and sub-types.
		mdFamORAssem = 0x0005,     // Accessibly by sub-types anywhere, plus anyone in assembly.
		mdPublic = 0x0006,     // Accessibly by anyone who has visibility to this scope.
		// end member access mask

		// method contract attributes.
		mdStatic = 0x0010,     // Defined on type, else per instance.
		mdFinal = 0x0020,     // Method may not be overridden.
		mdVirtual = 0x0040,     // Method virtual.
		mdHideBySig = 0x0080,     // Method hides by name+sig, else just by name.

		// vtable layout mask - Use this mask to retrieve vtable attributes.
		mdVtableLayoutMask = 0x0100,
		mdReuseSlot = 0x0000,     // The default.
		mdNewSlot = 0x0100,     // Method always gets a new slot in the vtable.
		// end vtable layout mask

		// method implementation attributes.
		mdCheckAccessOnOverride = 0x0200,     // Overridability is the same as the visibility.
		mdAbstract = 0x0400,     // Method does not provide an implementation.
		mdSpecialName = 0x0800,     // Method is special.  Name describes how.

		// interop attributes
		mdPinvokeImpl = 0x2000,     // Implementation is forwarded through pinvoke.
		mdUnmanagedExport = 0x0008,     // Managed method exported via thunk to unmanaged code.

		// Reserved flags for runtime use only.
		mdReservedMask = 0xd000,
		mdRTSpecialName = 0x1000,     // Runtime should check name encoding.
		mdHasSecurity = 0x4000,     // Method has security associate with it.
		mdRequireSecObject = 0x8000,     // Method calls another method containing security code.
	}

	// FieldDef attr bits, used by DefineField.
	[Flags()]
	public enum CorFieldAttr : uint
	{
		// member access mask - Use this mask to retrieve accessibility information.
		fdFieldAccessMask = 0x0007,
		fdPrivateScope = 0x0000,     // Member not referenceable.
		fdPrivate = 0x0001,     // Accessible only by the parent type.
		fdFamANDAssem = 0x0002,     // Accessible by sub-types only in this Assembly.
		fdAssembly = 0x0003,     // Accessibly by anyone in the Assembly.
		fdFamily = 0x0004,     // Accessible only by type and sub-types.
		fdFamORAssem = 0x0005,     // Accessibly by sub-types anywhere, plus anyone in assembly.
		fdPublic = 0x0006,     // Accessibly by anyone who has visibility to this scope.
		// end member access mask

		// field contract attributes.
		fdStatic = 0x0010,     // Defined on type, else per instance.
		fdInitOnly = 0x0020,     // Field may only be initialized, not written to after init.
		fdLiteral = 0x0040,     // Value is compile time constant.
		fdNotSerialized = 0x0080,     // Field does not have to be serialized when type is remoted.

		fdSpecialName = 0x0200,     // field is special.  Name describes how.

		// interop attributes
		fdPinvokeImpl = 0x2000,     // Implementation is forwarded through pinvoke.

		// Reserved flags for runtime use only.
		fdReservedMask = 0x9500,
		fdRTSpecialName = 0x0400,     // Runtime(metadata internal APIs) should check name encoding.
		fdHasFieldMarshal = 0x1000,     // Field has marshalling information.
		fdHasDefault = 0x8000,     // Field has default.
		fdHasFieldRVA = 0x0100,     // Field has RVA.
	}

	// Param attr bits, used by DefineParam.
	[Flags()]
	public enum CorParamAttr : uint
	{
		pdIn = 0x0001,     // Param is [In]
		pdOut = 0x0002,     // Param is [out]
		pdOptional = 0x0010,     // Param is optional

		// Reserved flags for Runtime use only.
		pdReservedMask = 0xf000,
		pdHasDefault = 0x1000,     // Param has default value.
		pdHasFieldMarshal = 0x2000,     // Param has FieldMarshal.

		pdUnused = 0xcfe0,
	}

	// Property attr bits, used by DefineProperty.
	public enum CorPropertyAttr : uint
	{
		prSpecialName = 0x0200,     // property is special.  Name describes how.

		// Reserved flags for Runtime use only.
		prReservedMask = 0xf400,
		prRTSpecialName = 0x0400,     // Runtime(metadata internal APIs) should check name encoding.
		prHasDefault = 0x1000,     // Property has default

		prUnused = 0xe9ff,
	}

	// Event attr bits, used by DefineEvent.
	public enum CorEventAttr : uint
	{
		evSpecialName = 0x0200,     // event is special.  Name describes how.

		// Reserved flags for Runtime use only.
		evReservedMask = 0x0400,
		evRTSpecialName = 0x0400,     // Runtime(metadata internal APIs) should check name encoding.
	}

	// MethodSemantic attr bits, used by DefineProperty, DefineEvent.
	public enum CorMethodSemanticsAttr : uint
	{
		msSetter = 0x0001,     // Setter for property
		msGetter = 0x0002,     // Getter for property
		msOther = 0x0004,     // other method for property or event
		msAddOn = 0x0008,     // AddOn method for event
		msRemoveOn = 0x0010,     // RemoveOn method for event
		msFire = 0x0020,     // Fire method for event
	}
	// DeclSecurity attr bits, used by DefinePermissionSet.
	public enum CorDeclSecurity : uint
	{
		dclActionMask = 0x001f,     // Mask allows growth of enum.
		dclActionNil = 0x0000,     //
		dclRequest = 0x0001,     //
		dclDemand = 0x0002,     //
		dclAssert = 0x0003,     //
		dclDeny = 0x0004,     //
		dclPermitOnly = 0x0005,     //
		dclLinktimeCheck = 0x0006,     //
		dclInheritanceCheck = 0x0007,     //
		dclRequestMinimum = 0x0008,     //
		dclRequestOptional = 0x0009,     //
		dclRequestRefuse = 0x000a,     //
		dclPrejitGrant = 0x000b,     // Persisted grant set at prejit time
		dclPrejitDenied = 0x000c,     // Persisted denied set at prejit time
		dclNonCasDemand = 0x000d,     //
		dclNonCasLinkDemand = 0x000e,     //
		dclNonCasInheritance = 0x000f,     //
		dclLinkDemandChoice = 0x0010,     //
		dclInheritanceDemandChoice = 0x0011,     //
		dclDemandChoice = 0x0012,     //
		dclMaximumValue = 0x0012,     // Maximum legal value
	}

	// PinvokeMap attr bits, used by DefinePinvokeMap.
	public enum CorPinvokeMap : uint
	{
		pmNoMangle = 0x0001,   // Pinvoke is to use the member name as specified.

		// Use this mask to retrieve the CharSet information.
		pmCharSetMask = 0x0006,
		pmCharSetNotSpec = 0x0000,
		pmCharSetAnsi = 0x0002,
		pmCharSetUnicode = 0x0004,
		pmCharSetAuto = 0x0006,


		pmBestFitUseAssem = 0x0000,
		pmBestFitEnabled = 0x0010,
		pmBestFitDisabled = 0x0020,
		pmBestFitMask = 0x0030,

		pmThrowOnUnmappableCharUseAssem = 0x0000,
		pmThrowOnUnmappableCharEnabled = 0x1000,
		pmThrowOnUnmappableCharDisabled = 0x2000,
		pmThrowOnUnmappableCharMask = 0x3000,

		pmSupportsLastError = 0x0040,   // Information about target function. Not relevant for fields.

		// None of the calling convention flags is relevant for fields.
		pmCallConvMask = 0x0700,
		pmCallConvWinapi = 0x0100,   // Pinvoke will use native callconv appropriate to target windows platform.
		pmCallConvCdecl = 0x0200,
		pmCallConvStdcall = 0x0300,
		pmCallConvThiscall = 0x0400,   // In M9, pinvoke will raise exception.
		pmCallConvFastcall = 0x0500,

		pmMaxValue = 0xFFFF,
	}

	// Assembly attr bits, used by DefineAssembly.
	public enum CorAssemblyFlags : uint
	{
		afPublicKey = 0x0001,     // The assembly ref holds the full (unhashed) public key.

		afPA_None = 0x0000,     // Processor Architecture unspecified
		afPA_MSIL = 0x0010,     // Processor Architecture: neutral (PE32)
		afPA_x86 = 0x0020,     // Processor Architecture: x86 (PE32)
		afPA_IA64 = 0x0030,     // Processor Architecture: Itanium (PE32+)
		afPA_AMD64 = 0x0040,     // Processor Architecture: AMD X64 (PE32+)
		afPA_NoPlatform = 0x0070,      // applies to any platform but cannot run on any (e.g. reference assembly), should not have "specified" set
		afPA_Specified = 0x0080,     // Propagate PA flags to AssemblyRef record
		afPA_Mask = 0x0070,     // Bits describing the processor architecture
		afPA_FullMask = 0x00F0,     // Bits describing the PA incl. Specified
		afPA_Shift = 0x0004,     // NOT A FLAG, shift count in PA flags <--> index conversion

		afEnableJITcompileTracking = 0x8000, // From "DebuggableAttribute".
		afDisableJITcompileOptimizer = 0x4000, // From "DebuggableAttribute".

		afRetargetable = 0x0100,     // The assembly can be retargeted (at runtime) to an
		//  assembly from a different publisher.
	}

	// ManifestResource attr bits, used by DefineManifestResource.
	public enum CorManifestResourceFlags : uint
	{
		mrVisibilityMask = 0x0007,
		mrPublic = 0x0001,     // The Resource is exported from the Assembly.
		mrPrivate = 0x0002,     // The Resource is private to the Assembly.
	}

	// File attr bits, used by DefineFile.
	public enum CorFileFlags : uint
	{
		ffContainsMetaData = 0x0000,     // This is not a resource file
		ffContainsNoMetaData = 0x0001,     // This is a resource file or other non-metadata-containing file
	}
	// PE file kind bits, returned by IMetaDataImport2::GetPEKind()
	public enum CorPEKind : uint
	{
		peNot = 0x00000000,   // not a PE file
		peILonly = 0x00000001,   // flag IL_ONLY is set in COR header
		pe32BitRequired = 0x00000002,  // flag 32BIT_REQUIRED is set in COR header
		pe32Plus = 0x00000004,   // PE32+ file (64 bit)
		pe32Unmanaged = 0x00000008    // PE32 without COR header
	}

	// GenericParam bits, used by DefineGenericParam.
	public enum CorGenericParamAttr : uint
	{
		// Variance of type parameters, only applicable to generic parameters 
		// for generic interfaces and delegates
		gpVarianceMask = 0x0003,
		gpNonVariant = 0x0000,
		gpCovariant = 0x0001,
		gpContravariant = 0x0002,

		// Special constraints, applicable to any type parameters
		gpSpecialConstraintMask = 0x001C,
		gpNoSpecialConstraint = 0x0000,
		gpReferenceTypeConstraint = 0x0004,      // type argument must be a reference type
		gpValueTypeConstraint = 0x0008,      // type argument must be a value type
		gpDefaultConstructorConstraint = 0x0010, // type argument must have a public nullary constructor
	}

	public enum CorElementType : uint
	{
		ELEMENT_TYPE_END = 0x0,
		ELEMENT_TYPE_VOID = 0x1,
		ELEMENT_TYPE_BOOLEAN = 0x2,
		ELEMENT_TYPE_CHAR = 0x3,
		ELEMENT_TYPE_I1 = 0x4,
		ELEMENT_TYPE_U1 = 0x5,
		ELEMENT_TYPE_I2 = 0x6,
		ELEMENT_TYPE_U2 = 0x7,
		ELEMENT_TYPE_I4 = 0x8,
		ELEMENT_TYPE_U4 = 0x9,
		ELEMENT_TYPE_I8 = 0xa,
		ELEMENT_TYPE_U8 = 0xb,
		ELEMENT_TYPE_R4 = 0xc,
		ELEMENT_TYPE_R8 = 0xd,
		ELEMENT_TYPE_STRING = 0xe,

		// every type above PTR will be simple type
		ELEMENT_TYPE_PTR = 0xf,      // PTR <type>
		ELEMENT_TYPE_BYREF = 0x10,     // BYREF <type>

		// Please use ELEMENT_TYPE_VALUETYPE. ELEMENT_TYPE_VALUECLASS is deprecated.
		ELEMENT_TYPE_VALUETYPE = 0x11,     // VALUETYPE <class Token>
		ELEMENT_TYPE_CLASS = 0x12,     // CLASS <class Token>
		ELEMENT_TYPE_VAR = 0x13,     // a class type variable VAR <U1>
		ELEMENT_TYPE_ARRAY = 0x14,     // MDARRAY <type> <rank> <bcount> <bound1> ... <lbcount> <lb1> ...
		ELEMENT_TYPE_GENERICINST = 0x15,     // instantiated type
		ELEMENT_TYPE_TYPEDBYREF = 0x16,     // This is a simple type.

		ELEMENT_TYPE_I = 0x18,     // native integer size
		ELEMENT_TYPE_U = 0x19,     // native unsigned integer size
		ELEMENT_TYPE_FNPTR = 0x1B,     // FNPTR <complete sig for the function including calling convention>
		ELEMENT_TYPE_OBJECT = 0x1C,     // Shortcut for System.Object
		ELEMENT_TYPE_SZARRAY = 0x1D,     // Shortcut for single dimension zero lower bound array
		// SZARRAY <type>
		ELEMENT_TYPE_MVAR = 0x1e,     // a method type variable MVAR <U1>

		// This is only for binding
		ELEMENT_TYPE_CMOD_REQD = 0x1F,     // required C modifier : E_T_CMOD_REQD <mdTypeRef/mdTypeDef>
		ELEMENT_TYPE_CMOD_OPT = 0x20,     // optional C modifier : E_T_CMOD_OPT <mdTypeRef/mdTypeDef>

		// This is for signatures generated internally (which will not be persisted in any way).
		ELEMENT_TYPE_INTERNAL = 0x21,     // INTERNAL <typehandle>

		// Note that this is the max of base type excluding modifiers
		ELEMENT_TYPE_MAX = 0x22,     // first invalid element type


		ELEMENT_TYPE_MODIFIER = 0x40,
		ELEMENT_TYPE_SENTINEL = 0x01 | ELEMENT_TYPE_MODIFIER, // sentinel for varargs
		ELEMENT_TYPE_PINNED = 0x05 | ELEMENT_TYPE_MODIFIER,
		ELEMENT_TYPE_R4_HFA = 0x06 | ELEMENT_TYPE_MODIFIER, // used only internally for R4 HFA types
		ELEMENT_TYPE_R8_HFA = 0x07 | ELEMENT_TYPE_MODIFIER, // used only internally for R8 HFA types

	}

	public enum CorSerializationType : uint
	{
		SERIALIZATION_TYPE_UNDEFINED = 0,
		SERIALIZATION_TYPE_BOOLEAN = CorElementType.ELEMENT_TYPE_BOOLEAN,
		SERIALIZATION_TYPE_CHAR = CorElementType.ELEMENT_TYPE_CHAR,
		SERIALIZATION_TYPE_I1 = CorElementType.ELEMENT_TYPE_I1,
		SERIALIZATION_TYPE_U1 = CorElementType.ELEMENT_TYPE_U1,
		SERIALIZATION_TYPE_I2 = CorElementType.ELEMENT_TYPE_I2,
		SERIALIZATION_TYPE_U2 = CorElementType.ELEMENT_TYPE_U2,
		SERIALIZATION_TYPE_I4 = CorElementType.ELEMENT_TYPE_I4,
		SERIALIZATION_TYPE_U4 = CorElementType.ELEMENT_TYPE_U4,
		SERIALIZATION_TYPE_I8 = CorElementType.ELEMENT_TYPE_I8,
		SERIALIZATION_TYPE_U8 = CorElementType.ELEMENT_TYPE_U8,
		SERIALIZATION_TYPE_R4 = CorElementType.ELEMENT_TYPE_R4,
		SERIALIZATION_TYPE_R8 = CorElementType.ELEMENT_TYPE_R8,
		SERIALIZATION_TYPE_STRING = CorElementType.ELEMENT_TYPE_STRING,
		SERIALIZATION_TYPE_SZARRAY = CorElementType.ELEMENT_TYPE_SZARRAY, // Shortcut for single dimension zero lower bound array
		SERIALIZATION_TYPE_TYPE = 0x50,
		SERIALIZATION_TYPE_TAGGED_OBJECT = 0x51,
		SERIALIZATION_TYPE_FIELD = 0x53,
		SERIALIZATION_TYPE_PROPERTY = 0x54,
		SERIALIZATION_TYPE_ENUM = 0x55
	}

	[Flags()]
	public enum CorCallingConvention : uint
	{
		IMAGE_CEE_CS_CALLCONV_DEFAULT = 0x0,

		IMAGE_CEE_UNMANAGED_CALLCONV_C = 0x1,
		IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL = 0x2,
		IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL = 0x3,
		IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL = 0x4,

		IMAGE_CEE_CS_CALLCONV_VARARG = 0x5,
		IMAGE_CEE_CS_CALLCONV_FIELD = 0x6,
		IMAGE_CEE_CS_CALLCONV_LOCAL_SIG = 0x7,
		IMAGE_CEE_CS_CALLCONV_PROPERTY = 0x8,
		IMAGE_CEE_CS_CALLCONV_UNMGD = 0x9,
		IMAGE_CEE_CS_CALLCONV_GENERICINST = 0x0a,  // generic method instantiation
		IMAGE_CEE_CS_CALLCONV_MAX = 0x0b,  // first invalid calling convention


		// The high bits of the calling convention convey additional info
		IMAGE_CEE_CS_CALLCONV_MASK = 0x0f,  // Calling convention is bottom 4 bits
		IMAGE_CEE_CS_CALLCONV_HASTHIS = 0x20,  // Top bit indicates a 'this' parameter
		IMAGE_CEE_CS_CALLCONV_EXPLICITTHIS = 0x40,  // This parameter is explicitly in the signature
		IMAGE_CEE_CS_CALLCONV_GENERIC = 0x10,  // Generic method sig with explicit number of type arguments (precedes ordinary parameter count)
	}

	public enum CorUnmanagedCallingConvention : uint
	{
		IMAGE_CEE_UNMANAGED_CALLCONV_C = 0x1,
		IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL = 0x2,
		IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL = 0x3,
		IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL = 0x4,

		IMAGE_CEE_CS_CALLCONV_C = IMAGE_CEE_UNMANAGED_CALLCONV_C,
		IMAGE_CEE_CS_CALLCONV_STDCALL = IMAGE_CEE_UNMANAGED_CALLCONV_STDCALL,
		IMAGE_CEE_CS_CALLCONV_THISCALL = IMAGE_CEE_UNMANAGED_CALLCONV_THISCALL,
		IMAGE_CEE_CS_CALLCONV_FASTCALL = IMAGE_CEE_UNMANAGED_CALLCONV_FASTCALL,

	}

	public enum CorArgType : uint
	{
		IMAGE_CEE_CS_END = 0x0,
		IMAGE_CEE_CS_VOID = 0x1,
		IMAGE_CEE_CS_I4 = 0x2,
		IMAGE_CEE_CS_I8 = 0x3,
		IMAGE_CEE_CS_R4 = 0x4,
		IMAGE_CEE_CS_R8 = 0x5,
		IMAGE_CEE_CS_PTR = 0x6,
		IMAGE_CEE_CS_OBJECT = 0x7,
		IMAGE_CEE_CS_STRUCT4 = 0x8,
		IMAGE_CEE_CS_STRUCT32 = 0x9,
		IMAGE_CEE_CS_BYVALUE = 0xA,
	}

	/***********************************************************************************/
	// a COR_ILMETHOD_SECT is a generic container for attributes that are private
	// to a particular method.  The COR_ILMETHOD structure points to one of these
	// (see GetSect()).  COR_ILMETHOD_SECT can decode the Kind of attribute (but not
	// its internal data layout, and can skip past the current attibute to find the
	// Next one.   The overhead for COR_ILMETHOD_SECT is a minimum of 2 bytes.

	public enum CorILMethodSect : uint                            // codes that identify attributes
	{
		CorILMethod_Sect_Reserved = 0,
		CorILMethod_Sect_EHTable = 1,
		CorILMethod_Sect_OptILTable = 2,

		CorILMethod_Sect_KindMask = 0x3F,        // The mask for decoding the type code
		CorILMethod_Sect_FatFormat = 0x40,        // fat format
		CorILMethod_Sect_MoreSects = 0x80,        // there is another attribute after this one
	}

	/***********************************************************************************/
	/* If COR_ILMETHOD_SECT_HEADER::Kind() = CorILMethod_Sect_EHTable then the attribute
		 is a list of exception handling clauses.  There are two formats, fat or small
	*/
	public enum CorExceptionFlag : uint                      // defintitions for the Flags field below (for both big and small)
	{
		COR_ILEXCEPTION_CLAUSE_NONE,                    // This is a typed handler
		COR_ILEXCEPTION_CLAUSE_OFFSETLEN = 0x0000,      // Deprecated
		COR_ILEXCEPTION_CLAUSE_DEPRECATED = 0x0000,     // Deprecated
		COR_ILEXCEPTION_CLAUSE_FILTER = 0x0001,        // If this bit is on, then this EH entry is for a filter
		COR_ILEXCEPTION_CLAUSE_FINALLY = 0x0002,        // This clause is a finally clause
		COR_ILEXCEPTION_CLAUSE_FAULT = 0x0004,          // Fault clause (finally that is called on exception only)
		COR_ILEXCEPTION_CLAUSE_DUPLICATED = 0x0008,     // duplicated clase..  this clause was duplicated down to a funclet which was pulled out of line
	}

	public enum CorILMethodFlags : uint
	{
		CorILMethod_InitLocals = 0x0010,           // call default constructor on all local vars
		CorILMethod_MoreSects = 0x0008,           // there is another attribute after this one

		CorILMethod_CompressedIL = 0x0040,           // FIX Remove this and do it on a per Module basis

		// Indicates the format for the COR_ILMETHOD header
		CorILMethod_FormatShift = 3,
		CorILMethod_FormatMask = ((1 << (int)CorILMethod_FormatShift) - 1),
		CorILMethod_TinyFormat = 0x0002,         // use this code if the code size is even
		CorILMethod_SmallFormat = 0x0000,
		CorILMethod_FatFormat = 0x0003,
		CorILMethod_TinyFormat1 = 0x0006,         // use this code if the code size is odd
	}

	//*****************************************************************************
	//
	// Enums for SetOption API.
	//
	//*****************************************************************************

	// flags for MetaDataCheckDuplicatesFor
	public enum CorCheckDuplicatesFor : uint
	{
		MDDupAll = 0xffffffff,
		MDDupENC = MDDupAll,
		MDNoDupChecks = 0x00000000,
		MDDupTypeDef = 0x00000001,
		MDDupInterfaceImpl = 0x00000002,
		MDDupMethodDef = 0x00000004,
		MDDupTypeRef = 0x00000008,
		MDDupMemberRef = 0x00000010,
		MDDupCustomAttribute = 0x00000020,
		MDDupParamDef = 0x00000040,
		MDDupPermission = 0x00000080,
		MDDupProperty = 0x00000100,
		MDDupEvent = 0x00000200,
		MDDupFieldDef = 0x00000400,
		MDDupSignature = 0x00000800,
		MDDupModuleRef = 0x00001000,
		MDDupTypeSpec = 0x00002000,
		MDDupImplMap = 0x00004000,
		MDDupAssemblyRef = 0x00008000,
		MDDupFile = 0x00010000,
		MDDupExportedType = 0x00020000,
		MDDupManifestResource = 0x00040000,
		MDDupGenericParam = 0x00080000,
		MDDupMethodSpec = 0x00100000,
		MDDupGenericParamConstraint = 0x00200000,
		// gap for debug junk
		MDDupAssembly = 0x10000000,

		// This is the default behavior on metadata. It will check duplicates for TypeRef, MemberRef, Signature, TypeSpec and MethodSpec.
		MDDupDefault = MDNoDupChecks | MDDupTypeRef | MDDupMemberRef | MDDupSignature | MDDupTypeSpec | MDDupMethodSpec,
	}

	// flags for MetaDataRefToDefCheck
	public enum CorRefToDefCheck : uint
	{
		// default behavior is to always perform TypeRef to public and MemberRef to MethodDef/FieldDef optimization
		MDRefToDefDefault = 0x00000003,
		MDRefToDefAll = 0xffffffff,
		MDRefToDefNone = 0x00000000,
		MDTypeRefToDef = 0x00000001,
		MDMemberRefToDef = 0x00000002
	}


	// MetaDataNotificationForTokenMovement
	public enum CorNotificationForTokenMovement : uint
	{
		// default behavior is to notify TypeRef, MethodDef, MemberRef, and FieldDef token remaps
		MDNotifyDefault = 0x0000000f,
		MDNotifyAll = 0xffffffff,
		MDNotifyNone = 0x00000000,
		MDNotifyMethodDef = 0x00000001,
		MDNotifyMemberRef = 0x00000002,
		MDNotifyFieldDef = 0x00000004,
		MDNotifyTypeRef = 0x00000008,

		MDNotifyTypeDef = 0x00000010,
		MDNotifyParamDef = 0x00000020,
		MDNotifyInterfaceImpl = 0x00000040,
		MDNotifyProperty = 0x00000080,
		MDNotifyEvent = 0x00000100,
		MDNotifySignature = 0x00000200,
		MDNotifyTypeSpec = 0x00000400,
		MDNotifyCustomAttribute = 0x00000800,
		MDNotifySecurityValue = 0x00001000,
		MDNotifyPermission = 0x00002000,
		MDNotifyModuleRef = 0x00004000,

		MDNotifyNameSpace = 0x00008000,

		MDNotifyAssemblyRef = 0x01000000,
		MDNotifyFile = 0x02000000,
		MDNotifyExportedType = 0x04000000,
		MDNotifyResource = 0x08000000,
	}


	public enum CorSetENC : uint
	{
		MDSetENCOn = 0x00000001,   // Deprecated name.
		MDSetENCOff = 0x00000002,   // Deprecated name.

		MDUpdateENC = 0x00000001,   // ENC mode.  Tokens don't move; can be updated.
		MDUpdateFull = 0x00000002,   // "Normal" update mode.
		MDUpdateExtension = 0x00000003,   // Extension mode.  Tokens don't move, adds only.
		MDUpdateIncremental = 0x00000004,   // Incremental compilation
		MDUpdateDelta = 0x00000005,   // If ENC on, save only deltas.
		MDUpdateMask = 0x00000007,


	}


	// flags used in SetOption when pair with MetaDataErrorIfEmitOutOfOrder guid
	public enum CorErrorIfEmitOutOfOrder : uint
	{
		MDErrorOutOfOrderDefault = 0x00000000,   // default not to generate any error
		MDErrorOutOfOrderNone = 0x00000000,   // do not generate error for out of order emit
		MDErrorOutOfOrderAll = 0xffffffff,   // generate out of order emit for method, field, param, property, and event
		MDMethodOutOfOrder = 0x00000001,   // generate error when methods are emitted out of order
		MDFieldOutOfOrder = 0x00000002,   // generate error when fields are emitted out of order
		MDParamOutOfOrder = 0x00000004,   // generate error when params are emitted out of order
		MDPropertyOutOfOrder = 0x00000008,   // generate error when properties are emitted out of order
		MDEventOutOfOrder = 0x00000010,   // generate error when events are emitted out of order
	}


	// flags used in SetOption when pair with MetaDataImportOption guid
	public enum CorImportOptions : uint
	{
		MDImportOptionDefault = 0x00000000,   // default to skip over deleted records
		MDImportOptionAll = 0xFFFFFFFF,   // Enumerate everything
		MDImportOptionAllTypeDefs = 0x00000001,   // all of the typedefs including the deleted public
		MDImportOptionAllMethodDefs = 0x00000002,   // all of the methoddefs including the deleted ones
		MDImportOptionAllFieldDefs = 0x00000004,   // all of the fielddefs including the deleted ones
		MDImportOptionAllProperties = 0x00000008,   // all of the properties including the deleted ones
		MDImportOptionAllEvents = 0x00000010,   // all of the events including the deleted ones
		MDImportOptionAllCustomAttributes = 0x00000020, // all of the custom attributes including the deleted ones
		MDImportOptionAllExportedTypes = 0x00000040,   // all of the ExportedTypes including the deleted ones

	}


	// flags for MetaDataThreadSafetyOptions
	public enum CorThreadSafetyOptions : uint
	{
		// default behavior is to have thread safety turn off. This means that MetaData APIs will not take reader/writer
		// lock. Clients is responsible to make sure the properly thread synchornization when using MetaData APIs.
		MDThreadSafetyDefault = 0x00000000,
		MDThreadSafetyOff = 0x00000000,
		MDThreadSafetyOn = 0x00000001,
	}


	// flags for MetaDataLinkerOptions
	public enum CorLinkerOptions : uint
	{
		// default behavior is not to keep private types
		MDAssembly = 0x00000000,
		MDNetModule = 0x00000001,
	}

	//
	// Token tags.
	//
	public enum CorTokenType : uint
	{
		mdtModule = 0x00000000,       //
		mdtTypeRef = 0x01000000,       //
		mdtTypeDef = 0x02000000,       //
		mdtFieldDef = 0x04000000,       //
		mdtMethodDef = 0x06000000,       //
		mdtParamDef = 0x08000000,       //
		mdtInterfaceImpl = 0x09000000,       //
		mdtMemberRef = 0x0a000000,       //
		mdtCustomAttribute = 0x0c000000,       //
		mdtPermission = 0x0e000000,       //
		mdtSignature = 0x11000000,       //
		mdtEvent = 0x14000000,       //
		mdtProperty = 0x17000000,       //
		mdtModuleRef = 0x1a000000,       //
		mdtTypeSpec = 0x1b000000,       //
		mdtAssembly = 0x20000000,       //
		mdtAssemblyRef = 0x23000000,       //
		mdtFile = 0x26000000,       //
		mdtExportedType = 0x27000000,       //
		mdtManifestResource = 0x28000000,       //
		mdtGenericParam = 0x2a000000,       //

		mdtMethodSpec = 0x2b000000,       //
		mdtGenericParamConstraint = 0x2c000000,

		mdtString = 0x70000000,       //
		mdtName = 0x71000000,       //
		mdtBaseType = 0x72000000,       // Leave this on the high end value. This does not correspond to metadata table
	}

	//
	// Open bits.
	//
	public enum CorOpenFlags : uint
	{
		ofRead = 0x00000000,     // Open scope for read
		ofWrite = 0x00000001,     // Open scope for write.
		ofReadWriteMask = 0x00000001,     // Mask for read/write bit.

		ofCopyMemory = 0x00000002,     // Open scope with memory. Ask metadata to maintain its own copy of memory.

		// These are obsolete and are ignored.
		ofCacheImage = 0x00000004,     // EE maps but does not do relocations or verify image
		ofNoTypeLib = 0x00000080,     // Don't OpenScope on a typelib.

		// Internal bits
		ofReserved1 = 0x00000100,     // Reserved for internal use.
		ofReserved2 = 0x00000200,     // Reserved for internal use.
		ofReserved = 0xffffff78      // All the reserved bits.

	}

	// Note that this must be kept in sync with System.AttributeTargets.
	public enum CorAttributeTargets : uint
	{
		catAssembly = 0x0001,
		catModule = 0x0002,
		catClass = 0x0004,
		catStruct = 0x0008,
		catEnum = 0x0010,
		catConstructor = 0x0020,
		catMethod = 0x0040,
		catProperty = 0x0080,
		catField = 0x0100,
		catEvent = 0x0200,
		catInterface = 0x0400,
		catParameter = 0x0800,
		catDelegate = 0x1000,
		catGenericParameter = 0x4000,

		catAll = catAssembly | catModule | catClass | catStruct | catEnum | catConstructor |
			catMethod | catProperty | catField | catEvent | catInterface | catParameter | catDelegate | catGenericParameter,
		catClassMembers = catClass | catStruct | catEnum | catConstructor | catMethod | catProperty | catField | catEvent | catDelegate | catInterface,

	}

	public enum NGenHintEnum : uint
	{
		NGenDefault = 0x0000, // No preference specified

		NGenEager = 0x0001, // NGen at install time
		NGenLazy = 0x0002, // NGen after install time
		NGenNever = 0x0003  // Assembly should not be ngened      
	}

	public enum LoadHintEnum : uint
	{
		LoadDefault = 0x0000, // No preference specified

		LoadAlways = 0x0001, // Dependency is always loaded
		LoadSometimes = 0x0002, // Dependency is sometimes loaded
		LoadNever = 0x0003  // Dependency is never loaded
	}

	public enum CorNativeType : uint
	{
		NATIVE_TYPE_END = 0x0,
		NATIVE_TYPE_VOID = 0x1,
		NATIVE_TYPE_BOOLEAN = 0x2,
		NATIVE_TYPE_I1 = 0x3,
		NATIVE_TYPE_U1 = 0x4,
		NATIVE_TYPE_I2 = 0x5,
		NATIVE_TYPE_U2 = 0x6,
		NATIVE_TYPE_I4 = 0x7,
		NATIVE_TYPE_U4 = 0x8,
		NATIVE_TYPE_I8 = 0x9,
		NATIVE_TYPE_U8 = 0xa,
		NATIVE_TYPE_R4 = 0xb,
		NATIVE_TYPE_R8 = 0xc,
		NATIVE_TYPE_SYSCHAR = 0xd,
		NATIVE_TYPE_VARIANT = 0xe,
		NATIVE_TYPE_CURRENCY = 0xf,
		NATIVE_TYPE_PTR = 0x10,

		NATIVE_TYPE_DECIMAL = 0x11,
		NATIVE_TYPE_DATE = 0x12,
		NATIVE_TYPE_BSTR = 0x13,
		NATIVE_TYPE_LPSTR = 0x14,
		NATIVE_TYPE_LPWSTR = 0x15,
		NATIVE_TYPE_LPTSTR = 0x16,
		NATIVE_TYPE_FIXEDSYSSTRING = 0x17,
		NATIVE_TYPE_OBJECTREF = 0x18,
		NATIVE_TYPE_IUNKNOWN = 0x19,
		NATIVE_TYPE_IDISPATCH = 0x1a,
		NATIVE_TYPE_STRUCT = 0x1b,
		NATIVE_TYPE_INTF = 0x1c,
		NATIVE_TYPE_SAFEARRAY = 0x1d,
		NATIVE_TYPE_FIXEDARRAY = 0x1e,
		NATIVE_TYPE_INT = 0x1f,
		NATIVE_TYPE_UINT = 0x20,

		NATIVE_TYPE_NESTEDSTRUCT = 0x21,
		NATIVE_TYPE_BYVALSTR = 0x22,
		NATIVE_TYPE_ANSIBSTR = 0x23,
		NATIVE_TYPE_TBSTR = 0x24,
		NATIVE_TYPE_VARIANTBOOL = 0x25,
		NATIVE_TYPE_FUNC = 0x26,

		NATIVE_TYPE_ASANY = 0x28,
		NATIVE_TYPE_ARRAY = 0x2a,
		NATIVE_TYPE_LPSTRUCT = 0x2b,
		NATIVE_TYPE_CUSTOMMARSHALER = 0x2c,
		NATIVE_TYPE_IINSPECTABLE = 0x2e,
		NATIVE_TYPE_HSTRING = 0x2f,

		NATIVE_TYPE_ERROR = 0x2d,

		NATIVE_TYPE_MAX = 0x50
	}

	public enum VariantType
	{
		VT_EMPTY = 0,
		VT_NULL = 1,
		VT_I2 = 2,
		VT_I4 = 3,
		VT_R4 = 4,
		VT_R8 = 5,
		VT_CY = 6,
		VT_DATE = 7,
		VT_BSTR = 8,
		VT_DISPATCH = 9,
		VT_ERROR = 10,
		VT_BOOL = 11,
		VT_VARIANT = 12,
		VT_UNKNOWN = 13,
		VT_DECIMAL = 14,
		VT_I1 = 16,
		VT_UI1 = 17,
		VT_UI2 = 18,
		VT_UI4 = 19,
		VT_I8 = 20,
		VT_UI8 = 21,
		VT_INT = 22,
		VT_UINT = 23,
		VT_VOID = 24,
		VT_HRESULT = 25,
		VT_PTR = 26,
		VT_SAFEARRAY = 27,
		VT_CARRAY = 28,
		VT_USERDEFINED = 29,
		VT_LPSTR = 30,
		VT_LPWSTR = 31,
		VT_RECORD = 36,
		VT_INT_PTR = 37,
		VT_UINT_PTR = 38,
		VT_FILETIME = 64,
		VT_BLOB = 65,
		VT_STREAM = 66,
		VT_STORAGE = 67,
		VT_STREAMED_OBJECT = 68,
		VT_STORED_OBJECT = 69,
		VT_BLOB_OBJECT = 70,
		VT_CF = 71,
		VT_CLSID = 72,
		VT_VERSIONED_STREAM = 73,
		VT_BSTR_BLOB = 0xfff,
		VT_VECTOR = 0x1000,
		VT_ARRAY = 0x2000,
		VT_BYREF = 0x4000,
		VT_RESERVED = 0x8000,
		VT_ILLEGAL = 0xffff,
		VT_ILLEGALMASKED = 0xfff,
		VT_TYPEMASK = 0xfff
	}
}