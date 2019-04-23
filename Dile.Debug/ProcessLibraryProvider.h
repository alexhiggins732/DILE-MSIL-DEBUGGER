#pragma once

#include <metahost.h>

namespace Dile
{
	namespace Debug
	{
		public class ProcessLibraryProvider : public ICLRDebuggingLibraryProvider
		{
		private:
			int referenceCount;

		public:
			ProcessLibraryProvider();

			virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** obj);

			virtual ULONG STDMETHODCALLTYPE AddRef();

			virtual ULONG STDMETHODCALLTYPE Release();

			virtual HRESULT STDMETHODCALLTYPE ProvideLibrary(const WCHAR* pwszFileName, DWORD dwTimestamp, DWORD dwSizeOfImage, HMODULE* hModule);
		};
	}
}