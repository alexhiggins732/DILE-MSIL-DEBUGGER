#pragma once

#include "Stdafx.h"
#include "DumpFileReader.h"

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public class MemoryDumpDataTarget : public ICorDebugDataTarget
			{
			private:
				DumpFileReader* pDumpFileReader;
				int referenceCount;

			public:
				MemoryDumpDataTarget(DumpFileReader* pDumpFileReader);

				virtual HRESULT STDMETHODCALLTYPE QueryInterface(const IID& iid, void** obj);

				virtual ULONG STDMETHODCALLTYPE AddRef();

				virtual ULONG STDMETHODCALLTYPE Release();

				virtual HRESULT STDMETHODCALLTYPE GetPlatform(CorDebugPlatform* pTargetPlatform);

				virtual HRESULT STDMETHODCALLTYPE ReadVirtual(CORDB_ADDRESS address, BYTE* pBuffer, ULONG32 bytesRequested, ULONG32* pBytesRead);

				virtual HRESULT STDMETHODCALLTYPE GetThreadContext(DWORD dwThreadID, ULONG32 contextFlags, ULONG32 contextSize, BYTE* pContext);
			};
		}
	}
}