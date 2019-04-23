#pragma once

#include "stdafx.h"
#include "IDebugEventHandler.h"
#include <vcclr.h>

namespace Dile
{
	namespace Debug
	{
		private class ManagedEventHandler : public ICorDebugManagedCallback, public ICorDebugManagedCallback2
		{
		public:
			ICorDebugProcess* process;
			gcroot<IDebugEventHandler^> debugEventHandler;

		public:
			ManagedEventHandler();

			~ManagedEventHandler();

			virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** obj);

			virtual ULONG STDMETHODCALLTYPE AddRef();

			virtual ULONG STDMETHODCALLTYPE Release();

		private:
			int referenceCount;

			virtual HRESULT STDMETHODCALLTYPE Breakpoint(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugBreakpoint *pBreakpoint);

			virtual HRESULT STDMETHODCALLTYPE StepComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugStepper *pStepper, CorDebugStepReason reason);

			virtual HRESULT STDMETHODCALLTYPE Break(ICorDebugAppDomain *pAppDomain, ICorDebugThread *thread);

			virtual HRESULT STDMETHODCALLTYPE Exception(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, BOOL unhandled);

			virtual HRESULT STDMETHODCALLTYPE EvalComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugEval *pEval);

			virtual HRESULT STDMETHODCALLTYPE EvalException(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugEval *pEval);

			virtual HRESULT STDMETHODCALLTYPE CreateProcess(ICorDebugProcess *pProcess);

			virtual HRESULT STDMETHODCALLTYPE ExitProcess(ICorDebugProcess *pProcess);

			virtual HRESULT STDMETHODCALLTYPE CreateThread(ICorDebugAppDomain *pAppDomain, ICorDebugThread *thread);

			virtual HRESULT STDMETHODCALLTYPE ExitThread(ICorDebugAppDomain *pAppDomain, ICorDebugThread *thread);

			virtual HRESULT STDMETHODCALLTYPE LoadModule(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule);

			virtual HRESULT STDMETHODCALLTYPE UnloadModule(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule);

			virtual HRESULT STDMETHODCALLTYPE LoadClass(ICorDebugAppDomain *pAppDomain, ICorDebugClass *c);

			virtual HRESULT STDMETHODCALLTYPE UnloadClass(ICorDebugAppDomain *pAppDomain, ICorDebugClass *c);

			virtual HRESULT STDMETHODCALLTYPE DebuggerError(ICorDebugProcess *pProcess, HRESULT errorHR, DWORD errorCode);

			virtual HRESULT STDMETHODCALLTYPE LogMessage(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, LONG lLevel, WCHAR *pLogSwitchName, WCHAR *pMessage);

			virtual HRESULT STDMETHODCALLTYPE LogSwitch(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, LONG lLevel, ULONG ulReason, WCHAR *pLogSwitchName, WCHAR *pParentName);

			virtual HRESULT STDMETHODCALLTYPE CreateAppDomain(ICorDebugProcess *pProcess, ICorDebugAppDomain *pAppDomain);

			virtual HRESULT STDMETHODCALLTYPE ExitAppDomain(ICorDebugProcess *pProcess, ICorDebugAppDomain *pAppDomain);

			virtual HRESULT STDMETHODCALLTYPE LoadAssembly(ICorDebugAppDomain *pAppDomain, ICorDebugAssembly *pAssembly);

			virtual HRESULT STDMETHODCALLTYPE UnloadAssembly(ICorDebugAppDomain *pAppDomain, ICorDebugAssembly *pAssembly);

			virtual HRESULT STDMETHODCALLTYPE ControlCTrap(ICorDebugProcess *pProcess);

			virtual HRESULT STDMETHODCALLTYPE NameChange(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread);

			virtual HRESULT STDMETHODCALLTYPE UpdateModuleSymbols(ICorDebugAppDomain *pAppDomain, ICorDebugModule *pModule, IStream *pSymbolStream);

			virtual HRESULT STDMETHODCALLTYPE EditAndContinueRemap(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pFunction, BOOL fAccurate);

			virtual HRESULT STDMETHODCALLTYPE BreakpointSetError(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugBreakpoint *pBreakpoint, DWORD dwError);

			virtual HRESULT STDMETHODCALLTYPE FunctionRemapOpportunity(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pOldFunction, ICorDebugFunction *pNewFunction, ULONG32 oldILOffset);
      
      virtual HRESULT STDMETHODCALLTYPE CreateConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId, WCHAR *pConnName);
      
      virtual HRESULT STDMETHODCALLTYPE ChangeConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId);

      virtual HRESULT STDMETHODCALLTYPE DestroyConnection(ICorDebugProcess *pProcess, CONNID dwConnectionId);
      
      virtual HRESULT STDMETHODCALLTYPE Exception(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFrame *pFrame, ULONG32 nOffset, CorDebugExceptionCallbackType dwEventType, DWORD dwFlags);
      
      virtual HRESULT STDMETHODCALLTYPE ExceptionUnwind(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, CorDebugExceptionUnwindCallbackType dwEventType, DWORD dwFlags);
      
      virtual HRESULT STDMETHODCALLTYPE FunctionRemapComplete(ICorDebugAppDomain *pAppDomain, ICorDebugThread *pThread, ICorDebugFunction *pFunction);
      
      virtual HRESULT STDMETHODCALLTYPE MDANotification(ICorDebugController *pController, ICorDebugThread *pThread, ICorDebugMDA *pMDA);

			void CheckHResult(HRESULT hResult);
		};
	}
}