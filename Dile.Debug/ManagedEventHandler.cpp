#include "stdafx.h"	
#include "ManagedEventHandler.h"
#include "DebugEventObjects.h"
#include "ValueWrapper.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		ManagedEventHandler::ManagedEventHandler()
		{
			referenceCount = 0;
		}

		ManagedEventHandler::~ManagedEventHandler()
		{
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::QueryInterface(const IID& iid, void** obj)
		{
			HRESULT result = S_OK;

			if (iid == IID_ICorDebugManagedCallback)
			{
				*obj = static_cast<ICorDebugManagedCallback*>(this);
			}
			else if (iid == IID_ICorDebugManagedCallback2)
			{
				*obj = static_cast<ICorDebugManagedCallback2*>(this);
			}
			else
			{
				result = E_NOINTERFACE;
			}

			return result;
		}

		ULONG STDMETHODCALLTYPE ManagedEventHandler::AddRef()
		{
			return ++referenceCount;
		}

		ULONG STDMETHODCALLTYPE ManagedEventHandler::Release()
		{
			ULONG temp = --referenceCount;
			if (referenceCount == 0)
			{
				delete this;
			}

			return temp;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::Breakpoint(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugBreakpoint *pBreakpoint)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Breakpoint = gcnew BreakpointWrapper(pBreakpoint);

			debugEventHandler->Breakpoint(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::StepComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugStepper *pStepper, CorDebugStepReason reason)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);

			if (pStepper != NULL)
			{
				eventObjects->Stepper = gcnew StepperWrapper(pStepper);
			}

			eventObjects->StepReason = Nullable<UInt32>(reason);

			debugEventHandler->StepComplete(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::Break(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);

			debugEventHandler->Break(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::Exception(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, BOOL unhandled)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->IsUnhandledException = Nullable<bool>(unhandled == TRUE ? true : false);

			debugEventHandler->Exception(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::EvalComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugEval *pEval)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Eval = gcnew EvalWrapper(pEval);

			debugEventHandler->EvalComplete(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::EvalException(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugEval *pEval)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Eval = gcnew EvalWrapper(pEval);

			debugEventHandler->EvalException(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::CreateProcess(ICorDebugProcess *pProcess)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);

			debugEventHandler->CreateProcess(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::ExitProcess(ICorDebugProcess *pProcess)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);

			debugEventHandler->ExitProcess(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::CreateThread(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);

			debugEventHandler->CreateThread(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::ExitThread(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);

			debugEventHandler->ExitThread(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::LoadModule(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Module = gcnew ModuleWrapper(pModule);

			debugEventHandler->LoadModule(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::UnloadModule(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Module = gcnew ModuleWrapper(pModule);

			debugEventHandler->UnloadModule(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::LoadClass(ICorDebugAppDomain *pAppDomain, ICorDebugClass *pC)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->ClassObject = gcnew ClassWrapper(pC);

			debugEventHandler->LoadClass(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::UnloadClass(ICorDebugAppDomain *pAppDomain, ICorDebugClass *pC)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->ClassObject = gcnew ClassWrapper(pC);

			debugEventHandler->UnloadClass(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::DebuggerError(ICorDebugProcess *pProcess, HRESULT errorHR, DWORD errorCode)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->ErrorHResult = Nullable<UInt32>(errorHR);
			eventObjects->ErrorCode = Nullable<UInt32>(errorCode);

			debugEventHandler->DebuggerError(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::LogMessage(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, LONG lLevel, WCHAR *pLogSwitchName, WCHAR *pMessage)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->LogLevel = Nullable<int>(lLevel);
			eventObjects->LogSwitchName = gcnew String(pLogSwitchName);
			eventObjects->Message = gcnew String(pMessage);

			debugEventHandler->LogMessage(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::LogSwitch(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, LONG lLevel, ULONG ulReason, WCHAR *pLogSwitchName, WCHAR *pParentName)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->LogLevel = Nullable<int>(lLevel);
			eventObjects->LogReason = Nullable<UInt32>(ulReason);
			eventObjects->LogSwitchName = gcnew String(pLogSwitchName);
			eventObjects->LogParentName = gcnew String(pParentName);

			debugEventHandler->LogSwitch(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::CreateAppDomain(ICorDebugProcess *pProcess, ICorDebugAppDomain *pAppDomain)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);

			debugEventHandler->CreateAppDomain(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::ExitAppDomain(ICorDebugProcess *pProcess, ICorDebugAppDomain *pAppDomain)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);

			debugEventHandler->ExitAppDomain(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::LoadAssembly(ICorDebugAppDomain *pAppDomain, ICorDebugAssembly *pAssembly)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Assembly = gcnew AssemblyWrapper(pAssembly);

			debugEventHandler->LoadAssembly(eventObjects);

			pAppDomain->Continue(FALSE);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::UnloadAssembly(ICorDebugAppDomain *pAppDomain, ICorDebugAssembly *pAssembly)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Assembly = gcnew AssemblyWrapper(pAssembly);

			debugEventHandler->UnloadAssembly(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::ControlCTrap(ICorDebugProcess *pProcess)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);

			debugEventHandler->ControlCTrap(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::NameChange(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();

			if (pAppDomain != NULL)
			{
				eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			}

			if (pThread != NULL)
			{
				eventObjects->Thread = gcnew ThreadWrapper(pThread);
				
				if (pAppDomain == NULL)
				{
					eventObjects->AppDomain = eventObjects->Thread->GetAppDomain();
				}
			}

			debugEventHandler->NameChange(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::UpdateModuleSymbols(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule, IStream *pSymbolStream)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Module = gcnew ModuleWrapper(pModule);

			debugEventHandler->UpdateModuleSymbols(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::EditAndContinueRemap(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pFunction, BOOL fAccurate)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Function = gcnew FunctionWrapper(pFunction);
			eventObjects->Accurate = Nullable<bool>(fAccurate == TRUE ? true : false);

			debugEventHandler->EditAndContinueRemap(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::BreakpointSetError(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugBreakpoint *pBreakpoint, DWORD dwError)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Breakpoint = gcnew BreakpointWrapper(pBreakpoint);
			eventObjects->ErrorCode = Nullable<UInt32>(dwError);

			debugEventHandler->BreakpointSetError(eventObjects);

			return S_OK;
		}

		HRESULT STDMETHODCALLTYPE ManagedEventHandler::FunctionRemapOpportunity(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pOldFunction, ICorDebugFunction *pNewFunction, ULONG32 oldILOffset)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Function = gcnew FunctionWrapper(pOldFunction);
			eventObjects->NewFunction = gcnew FunctionWrapper(pNewFunction);
			eventObjects->Offset = Nullable<UInt32>(oldILOffset);

			debugEventHandler->FunctionRemapOpportunity(eventObjects);

			return S_OK;
		}
      
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::CreateConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId, WCHAR *pConnName)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->ConnectionID = Nullable<UInt32>(dwConnectionId);
			eventObjects->ConnectionName = gcnew String(pConnName);

			debugEventHandler->CreateConnection(eventObjects);

			return S_OK;
		}
  
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::ChangeConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->ConnectionID = Nullable<UInt32>(dwConnectionId);

			debugEventHandler->ChangeConnection(eventObjects);

			return S_OK;
		}

    HRESULT STDMETHODCALLTYPE ManagedEventHandler::DestroyConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Process = gcnew ProcessWrapper(pProcess);
			eventObjects->ConnectionID = Nullable<UInt32>(dwConnectionId);

			debugEventHandler->DestroyConnection(eventObjects);

			return S_OK;
		}
  
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::Exception(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFrame *pFrame, ULONG32 nOffset, CorDebugExceptionCallbackType dwEventType, DWORD dwFlags)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);

			if (pFrame != NULL)
			{
				eventObjects->Frame = gcnew FrameWrapper(pFrame);
			}

			eventObjects->Offset = Nullable<UInt32>(nOffset);
			eventObjects->EventType = Nullable<UInt32>(dwEventType);
			eventObjects->Flags = Nullable<UInt32>(dwFlags);

			debugEventHandler->Exception2(eventObjects);
			//CheckHResult(pAppDomain->Continue(FALSE));

			return S_OK;
		}
  
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::ExceptionUnwind(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, CorDebugExceptionUnwindCallbackType dwEventType, DWORD dwFlags)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->EventType = Nullable<UInt32>(dwEventType);
			eventObjects->Flags = Nullable<UInt32>(dwFlags);

			debugEventHandler->ExceptionUnwind(eventObjects);

			return S_OK;
		}
  
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::FunctionRemapComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pFunction)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->AppDomain = gcnew AppDomainWrapper(pAppDomain);
			eventObjects->Thread = gcnew ThreadWrapper(pThread);
			eventObjects->Function = gcnew FunctionWrapper(pFunction);

			debugEventHandler->FunctionRemapComplete(eventObjects);

			return S_OK;
		}
  
    HRESULT STDMETHODCALLTYPE ManagedEventHandler::MDANotification(ICorDebugController *pController, ICorDebugThread *pThread, ICorDebugMDA *pMDA)
		{
			DebugEventObjects^ eventObjects = gcnew DebugEventObjects();
			eventObjects->Controller = gcnew ControllerWrapper(pController);

			if (pThread != NULL)
			{
				eventObjects->Thread = gcnew ThreadWrapper(pThread);
			}

			eventObjects->Mda = gcnew MdaWrapper(pMDA);

			debugEventHandler->MDANotification(eventObjects);

			return S_OK;
		}

		void ManagedEventHandler::CheckHResult(HRESULT hResult)
		{
			if (FAILED(hResult))
			{
				Marshal::ThrowExceptionForHR(hResult);
			}
		}
	}
}