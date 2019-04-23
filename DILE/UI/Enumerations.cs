using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.UI
{
	[Flags()]
	public enum SearchOptions
	{
		None = 1,
		Assembly = 2,
		AssemblyReference = 4,
		ModuleScope = 8,
		ModuleReference = 16,
		File = 32,
		ManifestResource = 64,
		TypeDefinition = 128,
		MethodDefinition = 256,
		Property = 512,
		FieldDefintion = 1024,
		TokenValues = 2048,
		EventDefinition = 4096,
		ExportedType = 8192
	}

	public enum ValueFieldGroup : uint
	{
		PrivateScope = CorFieldAttr.fdPrivateScope,
		Private = CorFieldAttr.fdPrivate,
		FamilyAndAssembly = CorFieldAttr.fdFamANDAssem,
		Assembly = CorFieldAttr.fdAssembly,
		Family = CorFieldAttr.fdFamily,
		FamilyOrAssembly = CorFieldAttr.fdFamORAssem,
		Public = CorFieldAttr.fdPublic,
		ObjectInformation,
		EvaluationException,
		MissingModule
	}

	public enum MenuFunction
	{
		//File menu
		NewProject,
		OpenProject,
		SaveProject,
		SaveProjectAs,
		Settings,
		Exit,

		//Project menu
		ProjectProperties,
		AddAssembly,
		RemoveAssembly,
		OpenReferenceInProject,

		//Debug menu
		AttachToProcess,
		RunDebuggee,
		PauseDebuggee,
		StopDebuggee,
		Detach,
		Step,
		StepInto,
		StepOut,
		ObjectViewer,

		//View menu
		WordWrap,
		StartPage,

		//View/Panels menu
		InformationPanel = 1000,
		DebugOutputPanel = 1001,
		LogMessagePanel = 1002,
		ThreadsPanel = 1003,
		ModulesPanel = 1004,
		CallStackPanel = 1005,
		BreakpointsPanel = 1006,
		LocalVariablesPanel = 1007,
		ArgumentsPanel = 1008,
		AutoObjectsPanel = 1009,
		ProjectExplorerPanel = 1010,
		QuickSearchPanel = 1011,
		OpCodeHelperPanel = 1012,

		//Windows menu
		CloseAllWindows,

		//Help menu
		About,

		RunToCursor
	}

	public enum ObjectsPanelMode
	{
		LocalVariables,
		Arguments,
		AutoObjects,
		Watch
	}

	public enum ExtendedDialogResult
	{
		None,
		Yes,
		No,
		YesToAll,
		NoToAll
	}

	public enum TokenInformationClass
	{
		TokenUser = 1,
		TokenGroups,
		TokenPrivileges,
		TokenOwner,
		TokenPrimaryGroup,
		TokenDefaultDacl,
		TokenSource,
		TokenType,
		TokenImpersonationLevel,
		TokenStatistics,
		TokenRestrictedSids,
		TokenSessionId,
		TokenGroupsAndPrivileges,
		TokenSessionReference,
		TokenSandBoxInert,
		TokenAuditPolicy,
		TokenOrigin,
		TokenElevationType,
		TokenLinkedToken,
		TokenElevation,
		TokenHasRestrictions,
		TokenAccessInformation,
		TokenVirtualizationAllowed,
		TokenVirtualizationEnabled,
		TokenIntegrityLevel,
		TokenUIAccess,
		TokenMandatoryPolicy,
		TokenLogonSid,
		MaxTokenInfoClass
	}

	public enum DesiredAccess : uint
	{
		StandardRightsRequired = 0x000F0000,
		StandardRightsRead = 0x00020000,
		TokenAssignPrimary = 0x0001,
		TokenDuplicate = 0x0002,
		TokenImpersonate = 0x0004,
		TokenQuery = 0x0008,
		TokenQuerySource = 0x0010,
		TokenAdjustPrivileges = 0x0020,
		TokenAdjustGroups = 0x0040,
		TokenAdjustDefault = 0x0080,
		TokenAdjustSessionID = 0x0100,
		TokenRead = (StandardRightsRead | TokenQuery),
		TokenAllAccess = (StandardRightsRequired | TokenAssignPrimary |
				TokenDuplicate | TokenImpersonate | TokenQuery | TokenQuerySource |
				TokenAdjustPrivileges | TokenAdjustGroups | TokenAdjustDefault |
				TokenAdjustSessionID)
	}

	public enum SidNameUse
	{
		SidTypeUser = 1,
		SidTypeGroup,
		SidTypeDomain,
		SidTypeAlias,
		SidTypeWellKnownGroup,
		SidTypeDeletedAccount,
		SidTypeInvalid,
		SidTypeUnknown,
		SidTypeComputer
	}

	public enum RecentItemType
	{
		RecentProject,
		RecentAssembly,
		RecentDumpFile
	}
}