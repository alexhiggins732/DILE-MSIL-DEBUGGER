#pragma once

#include "DebugEventObjects.h"

namespace Dile
{
	namespace Debug
	{
		public interface class IDebugEventHandler
		{
		public:
			void Breakpoint(DebugEventObjects^ eventObjects);

			void StepComplete(DebugEventObjects^ eventObjects);

			void Break(DebugEventObjects^ eventObjects);

			void Exception(DebugEventObjects^ eventObjects);

			void EvalComplete(DebugEventObjects^ eventObjects);

			void EvalException(DebugEventObjects^ eventObjects);

			void CreateProcess(DebugEventObjects^ eventObjects);

			void ExitProcess(DebugEventObjects^ eventObjects);

			void CreateThread(DebugEventObjects^ eventObjects);

			void ExitThread(DebugEventObjects^ eventObjects);

			void LoadModule(DebugEventObjects^ eventObjects);

			void UnloadModule(DebugEventObjects^ eventObjects);

			void LoadClass(DebugEventObjects^ eventObjects);

			void UnloadClass(DebugEventObjects^ eventObjects);

			void DebuggerError(DebugEventObjects^ eventObjects);

			void LogMessage(DebugEventObjects^ eventObjects);

			void LogSwitch(DebugEventObjects^ eventObjects);

			void CreateAppDomain(DebugEventObjects^ eventObjects);

			void ExitAppDomain(DebugEventObjects^ eventObjects);

			void LoadAssembly(DebugEventObjects^ eventObjects);

			void UnloadAssembly(DebugEventObjects^ eventObjects);

			void ControlCTrap(DebugEventObjects^ eventObjects);

			void NameChange(DebugEventObjects^ eventObjects);

			void UpdateModuleSymbols(DebugEventObjects^ eventObjects);

			void EditAndContinueRemap(DebugEventObjects^ eventObjects);

			void BreakpointSetError(DebugEventObjects^ eventObjects);

			void FunctionRemapOpportunity(DebugEventObjects^ eventObjects);

			void CreateConnection(DebugEventObjects^ eventObjects);

			void ChangeConnection(DebugEventObjects^ eventObjects);

			void DestroyConnection(DebugEventObjects^ eventObjects);

			void Exception2(DebugEventObjects^ eventObjects);

			void ExceptionUnwind(DebugEventObjects^ eventObjects);

			void FunctionRemapComplete(DebugEventObjects^ eventObjects);

			void MDANotification(DebugEventObjects^ eventObjects);
		};
	}
}