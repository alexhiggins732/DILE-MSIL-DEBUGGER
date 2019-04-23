#pragma once

#include <cordebug.h>
#include "BaseWrapper.h"
#include "IDebugEventHandler.h"
#include "managedeventhandler.h"

namespace Dile
{
	namespace Debug
	{
		public ref class Debugger : BaseWrapper
		{
		private:
			ICorDebug *pDebug;
			ManagedEventHandler *pManagedEventHandler;

		internal:
			Debugger(ICorDebug *pDebug);

		public:
			~Debugger();

			!Debugger();

			void Initialize(IDebugEventHandler^ debugEventHandler);

			void CreateProcess(String^ applicationName, String^ arguments, String^ currentDirectory);

			void DebugActiveProcess(DWORD id, BOOL win32Attach);

			ProcessWrapper^ GetProcess(UInt32 processID);
		};
	}
}