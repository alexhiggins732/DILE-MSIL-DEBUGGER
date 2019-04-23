#include "stdafx.h"
#include "UnmanagedEventHandler.h"

namespace Dile
{
	namespace Debug
	{
		UnmanagedEventHandler::UnmanagedEventHandler()
		{
			referenceCount = 0;
		}

		UnmanagedEventHandler::~UnmanagedEventHandler()
		{
		}

		HRESULT STDMETHODCALLTYPE UnmanagedEventHandler::QueryInterface(const IID& iid, void** obj)
		{
			HRESULT result = S_OK;

			if (iid == IID_ICorDebugUnmanagedCallback)
			{
				*obj = static_cast<ICorDebugUnmanagedCallback*>(this);
			}
			else
			{
				result = E_NOINTERFACE;
			}

			return result;
		}

		ULONG STDMETHODCALLTYPE UnmanagedEventHandler::AddRef()
		{
			return ++referenceCount;
		}

		ULONG STDMETHODCALLTYPE UnmanagedEventHandler::Release()
		{
			ULONG temp = --referenceCount;
			if (referenceCount == 0)
			{
				delete this;
			}

			return temp;
		}

		HRESULT STDMETHODCALLTYPE UnmanagedEventHandler::DebugEvent(LPDEBUG_EVENT pDebugEvent, BOOL fOutOfBand)
		{
			process->Continue(fOutOfBand);

			return S_OK;
		}
	}
}