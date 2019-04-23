#include "Stdafx.h"
#include "MemoryDumpDataTarget.h"
#include <Windows.h>

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			MemoryDumpDataTarget::MemoryDumpDataTarget(DumpFileReader* pDumpFileReader)
			{
				this->pDumpFileReader = pDumpFileReader;
				referenceCount = 0;

			}

			HRESULT MemoryDumpDataTarget::QueryInterface(const IID& iid, void** obj)
			{
				HRESULT result = S_OK;

				if (iid == IID_ICorDebugDataTarget)
				{
					*obj = static_cast<ICorDebugDataTarget*>(this);
				}
				else
				{
					result = E_NOINTERFACE;
				}

				return result;
			}

			ULONG MemoryDumpDataTarget::AddRef()
			{
				return ++referenceCount;
			}

			ULONG MemoryDumpDataTarget::Release()
			{
				ULONG temp = --referenceCount;
				if (referenceCount == 0)
				{
					delete this;
				}

				return temp;
			}

			HRESULT MemoryDumpDataTarget::GetPlatform(CorDebugPlatform* pTargetPlatform)
			{
				*pTargetPlatform = pDumpFileReader->ReadPlatform();

				return S_OK;
			}

			HRESULT MemoryDumpDataTarget::ReadVirtual(CORDB_ADDRESS address, BYTE* pBuffer, ULONG32 bytesRequested, ULONG32* pBytesRead)
			{
				pDumpFileReader->ReadMemory(address, pBuffer, bytesRequested, pBytesRead);

				return S_OK;
			}

			HRESULT MemoryDumpDataTarget::GetThreadContext(DWORD dwThreadID, ULONG32 contextFlags, ULONG32 contextSize, BYTE* pContext)
			{
				HRESULT result = S_OK;

				pDumpFileReader->ReadThreadContext(dwThreadID, pContext, contextSize);

				return result;
			}
		}
	}
}