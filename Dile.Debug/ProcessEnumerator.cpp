#include "stdafx.h"
#include "ProcessEnumerator.h"
#include "Constants.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		String^ ProcessEnumerator::GetProcessName(ICorPublishProcess* process)
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = process->GetDisplayName(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = process->GetDisplayName(expectedSize, &expectedSize, name);

				if (FAILED(hResult))
				{
					delete [] name;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(name);

			delete [] name;

			return result;
		}

		List<ProcessInformation^>^ ProcessEnumerator::GetManagedProcesses()
		{
			List<ProcessInformation^>^ result;
			ICorPublish* publish;

			CheckHResult(CoCreateInstance(CLSID_CorpubPublish, NULL, CLSCTX_INPROC_SERVER, IID_ICorPublish, (LPVOID*)&publish));

			ICorPublishProcessEnum* processEnum;
			CheckHResult(publish->EnumProcesses(COR_PUB_MANAGEDONLY, &processEnum));

			ICorPublishProcess* processes[DefaultArrayCount];
			ULONG fetched;
			CheckHResult(processEnum->Next(DefaultArrayCount, &processes[0], &fetched));

			if (fetched > 0)
			{
				result = gcnew List<ProcessInformation^>(fetched);
			}

			while (fetched > 0)
			{
				for (ULONG index = 0; index < fetched; index++)
				{
					ICorPublishProcess* process = processes[index];
					ProcessInformation^ processInformation = gcnew ProcessInformation();

					unsigned int id;
					process->GetProcessID(&id);
					processInformation->ID = id;
					processInformation->Name = GetProcessName(process);

					result->Add(processInformation);
				}

				if (fetched < DefaultArrayCount)
				{
					fetched = 0;
				}
				else
				{
					CheckHResult(processEnum->Next(DefaultArrayCount, &processes[0], &fetched));
				}
			}

			return result;
		}
	}
}