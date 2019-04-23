#pragma once

#include "stdafx.h"

namespace Dile
{
	namespace Debug
	{
		private class UnmanagedEventHandler : public ICorDebugUnmanagedCallback
		{
		public:
			ICorDebugProcess* process;

			UnmanagedEventHandler();
			~UnmanagedEventHandler();

		private:
			int referenceCount;

			virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** obj);

			virtual ULONG STDMETHODCALLTYPE AddRef();

			virtual ULONG STDMETHODCALLTYPE Release();

			virtual HRESULT STDMETHODCALLTYPE DebugEvent(LPDEBUG_EVENT pDebugEvent, BOOL fOutOfBand);
		};
	}
}