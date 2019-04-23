#pragma once

#include <DbgHelp.h>

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			[Flags()]
			public enum class DumpType
			{
				Normal = MiniDumpNormal,
				WithDataSegs = MiniDumpWithDataSegs,
				WithFullMemory = MiniDumpWithFullMemory,
				WithHandleData =MiniDumpWithHandleData,
				FilterMemory = MiniDumpFilterMemory,
				ScanMemory = MiniDumpScanMemory,
				WithUnloadedModules = MiniDumpWithUnloadedModules,
				WithIndirectlyReferencedMemory = MiniDumpWithIndirectlyReferencedMemory,
				FilterModulePaths = MiniDumpFilterModulePaths,
				WithProcessThreadData = MiniDumpWithProcessThreadData,
				WithPrivateReadWriteMemory = MiniDumpWithPrivateReadWriteMemory,
				WithoutOptionalData = MiniDumpWithoutOptionalData,
				WithFullMemoryInfo = MiniDumpWithFullMemoryInfo,
				WithThreadInfo = MiniDumpWithThreadInfo,
				WithCodeSegs = MiniDumpWithCodeSegs,
				WithoutAuxiliaryState = MiniDumpWithoutAuxiliaryState,
				WithFullAuxiliaryState = MiniDumpWithFullAuxiliaryState,
				WithPrivateWriteCopyMemory = MiniDumpWithPrivateWriteCopyMemory,
				IgnoreInaccessibleMemory = MiniDumpIgnoreInaccessibleMemory,
				WithTokenInformation = MiniDumpWithTokenInformation,
				ValidTypeFlags = MiniDumpValidTypeFlags
			};

			[Flags()]
			public enum class FileType
			{
				Application = VFT_APP,
				Dll = VFT_DLL,
				DeviceDriver = VFT_DRV,
				Font = VFT_FONT,
				StaticLinkLibrary = VFT_STATIC_LIB,
				Unknown = VFT_UNKNOWN,
				VirtualDevice = VFT_VXD
			};

			[Flags()]
			public enum class FileFlags
			{
				Debug = VS_FF_DEBUG,
				InfoInferred = VS_FF_INFOINFERRED,
				Patched = VS_FF_PATCHED,
				PreRelease = VS_FF_PRERELEASE,
				PrivateBuild = VS_FF_PRIVATEBUILD,
				SpecialBuild = VS_FF_SPECIALBUILD
			};

			public enum class ExceptionCode
			{
				AccessViolation = EXCEPTION_ACCESS_VIOLATION,
				ArrayBoundsExceeded = EXCEPTION_ARRAY_BOUNDS_EXCEEDED,
				Breakpoint = EXCEPTION_BREAKPOINT,
				DataTypeMisalignment = EXCEPTION_DATATYPE_MISALIGNMENT,
				FloatingPointDenormalOperand = EXCEPTION_FLT_DENORMAL_OPERAND,
				FloatingPointDivideByZero = EXCEPTION_FLT_DIVIDE_BY_ZERO,
				FloatingPointInexactResult = EXCEPTION_FLT_INEXACT_RESULT,
				FloatingPointInvalidOperation = EXCEPTION_FLT_INVALID_OPERATION,
				FloatingPointOverflow = EXCEPTION_FLT_OVERFLOW,
				FloatingPointStackCheck = EXCEPTION_FLT_STACK_CHECK,
				FloatingPointUnderflow = EXCEPTION_FLT_UNDERFLOW,
				IllegalInstruction = EXCEPTION_ILLEGAL_INSTRUCTION,
				InPageError = EXCEPTION_IN_PAGE_ERROR,
				IntegerDivideByZero = EXCEPTION_INT_DIVIDE_BY_ZERO,
				IntegerOverflow = EXCEPTION_INT_OVERFLOW,
				InvalidDisposition = EXCEPTION_INVALID_DISPOSITION,
				NonContinuableException = EXCEPTION_NONCONTINUABLE_EXCEPTION,
				PrivInstruction = EXCEPTION_PRIV_INSTRUCTION,
				SingleStep = EXCEPTION_SINGLE_STEP,
				StackOverflow = EXCEPTION_STACK_OVERFLOW
			};

			public enum class ProcessorArchitecture
			{
				x64 = PROCESSOR_ARCHITECTURE_AMD64,
				Itanium = PROCESSOR_ARCHITECTURE_IA64,
				x86 = PROCESSOR_ARCHITECTURE_INTEL,
				Unknown = PROCESSOR_ARCHITECTURE_UNKNOWN
			};

			public enum class ProcessorLevel
			{
				IntelItanium = 1,
				Intel386 = 3,
				Intel486 = 4,
				IntelPentium = 5,
				IntelPentiumProOrPentiumII = 6
			};

			public enum class OperatingSystemType
			{
				DomainController = VER_NT_DOMAIN_CONTROLLER,
				Server = VER_NT_SERVER,
				Workstation = VER_NT_WORKSTATION
			};

			public enum class Platform
			{
				Win32s = VER_PLATFORM_WIN32s,
				NotSupported = VER_PLATFORM_WIN32_WINDOWS,
				NT = VER_PLATFORM_WIN32_NT
			};

			[Flags()]
			public enum class OperatingSystemSuite
			{
				BackOffice = VER_SUITE_BACKOFFICE,
				WindowsServer2003WebEdition = VER_SUITE_BLADE,
				WindowsServer2003ComputeClusterEdition = VER_SUITE_COMPUTE_SERVER,
				DataCenter = VER_SUITE_DATACENTER,
				Enterprise = VER_SUITE_ENTERPRISE,
				WindowsXPEmbedded = VER_SUITE_EMBEDDEDNT,
				WindowsXPHomeEdition = VER_SUITE_PERSONAL,
				SingleRemoteDesktopUser = VER_SUITE_SINGLEUSERTS,
				SmallBusiness = VER_SUITE_SMALLBUSINESS,
				SmallBusinessRestricted = VER_SUITE_SMALLBUSINESS_RESTRICTED,
				WindowsStorageServer2003R2 = VER_SUITE_STORAGE_SERVER,
				Terminal = VER_SUITE_TERMINAL
			};

			public enum class ThreadPriorityClass
			{
				Idle = IDLE_PRIORITY_CLASS,
				BelowNormal = BELOW_NORMAL_PRIORITY_CLASS,
				Normal = NORMAL_PRIORITY_CLASS,
				AboveNormal = ABOVE_NORMAL_PRIORITY_CLASS,
				High = HIGH_PRIORITY_CLASS,
				Realtime = REALTIME_PRIORITY_CLASS
			};

			public enum class ThreadPriority
			{
				Idle = THREAD_PRIORITY_IDLE,
				Lowest = THREAD_PRIORITY_LOWEST,
				BelowNormal = THREAD_PRIORITY_BELOW_NORMAL,
				Normal = THREAD_PRIORITY_NORMAL,
				AboveNormal = THREAD_PRIORITY_ABOVE_NORMAL,
				Highest = THREAD_PRIORITY_HIGHEST,
				TimeCritical = THREAD_PRIORITY_TIME_CRITICAL
			};

			[Flags()]
			public enum class MiscInfoType
			{
				None = 0,
				ProcessId = MINIDUMP_MISC1_PROCESS_ID,
				ProcessTimes = MINIDUMP_MISC1_PROCESS_TIMES,
				ProcessorPowerInfo = MINIDUMP_MISC1_PROCESSOR_POWER_INFO
			};
		}
	}
}