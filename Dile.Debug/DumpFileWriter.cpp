#include "Stdafx.h"
#include "DumpFileWriter.h"
#include "LpctstrConverter.h"
#include <DbgHelp.h>

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			void DumpFileWriter::WriteDumpFile(String^ dumpFilePath, int processId, DumpType dumpType)
			{
				HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_VM_READ, FALSE, processId);
				if (hProcess == NULL)
				{
					throw gcnew InvalidOperationException("Failed to open the target process.");
				}

				LpctstrConverter lpDumpFilePath(dumpFilePath);
				HANDLE hFile = CreateFile(lpDumpFilePath, GENERIC_WRITE, 0, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
				if (hFile == INVALID_HANDLE_VALUE)
				{
					CloseHandle(hProcess);

					throw gcnew InvalidOperationException("Failed to create the memory dump file.");
				}

				if (!MiniDumpWriteDump(hProcess, processId, hFile, (MINIDUMP_TYPE)dumpType, NULL, NULL, NULL))
				{
					CloseHandle(hProcess);
					CloseHandle(hFile);

					throw gcnew InvalidOperationException("Failed to dump the memory of the selected process.");
				}

				CloseHandle(hProcess);
				CloseHandle(hFile);
			}
		}
	}
}