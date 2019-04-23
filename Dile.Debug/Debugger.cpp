#include "Stdafx.h"
#include "Debugger.h"
#include "BstrConverter.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		Debugger::Debugger(ICorDebug *pDebug)
		{
			this->pDebug = pDebug;

			pManagedEventHandler = NULL;
		}

		Debugger::~Debugger()
		{
			this->!Debugger();
		}

		Debugger::!Debugger()
		{
			CheckHResult(pManagedEventHandler->Release());
			CheckHResult(pDebug->Release());
		}

		void Debugger::Initialize(IDebugEventHandler^ debugEventHandler)
		{
			CheckHResult(pDebug->Initialize());

			if (pManagedEventHandler != NULL)
			{
				throw gcnew InvalidOperationException("The object has already been initialized.");
			}

			pManagedEventHandler = new ManagedEventHandler();
			pManagedEventHandler->debugEventHandler = debugEventHandler;
			CheckHResult(pDebug->SetManagedHandler(pManagedEventHandler));

			/*unmanagedEventHandler = new UnmanagedEventHandler();
			CheckHResult(pDebug->SetUnmanagedHandler(unmanagedEventHandler));*/
		}

		void Debugger::CreateProcess(String^ applicationName, String^ arguments, String^ currentDirectory)
		{
			STARTUPINFOW startupInfo;
			PROCESS_INFORMATION processInfo;
			ZeroMemory(&startupInfo, sizeof(startupInfo));
			ZeroMemory(&processInfo, sizeof(processInfo));
			startupInfo.cb = sizeof(STARTUPINFOW);

			//DWORD debugFlag = CREATE_NEW_CONSOLE;// | DEBUG_PROCESS;

			BstrConverter bstrApplicationName(applicationName);
			BstrConverter bstrArguments(arguments);
			BstrConverter bstrCurrentDirectory(currentDirectory);

			HRESULT hResult = pDebug->CreateProcess(bstrApplicationName,
				bstrArguments,
				NULL,
				NULL,
				TRUE,
				0,
				NULL,
				bstrCurrentDirectory,
				&startupInfo,
				&processInfo,
				DEBUG_NO_SPECIAL_OPTIONS,
				&pManagedEventHandler->process);

			CloseHandle(processInfo.hProcess);
			CloseHandle(processInfo.hThread);

			CheckHResult(hResult);
		}

		void Debugger::DebugActiveProcess(DWORD id, BOOL win32Attach)
		{
			CheckHResult(pDebug->DebugActiveProcess(id, win32Attach, &pManagedEventHandler->process));
		}

		ProcessWrapper^ Debugger::GetProcess(UInt32 processID)
		{
			ProcessWrapper^ result = nullptr;
			ICorDebugProcess* process;
			CheckHResult(pDebug->GetProcess(processID, &process));

			result = gcnew ProcessWrapper();
			result->Process = process;

			return result;
		}
	}
}