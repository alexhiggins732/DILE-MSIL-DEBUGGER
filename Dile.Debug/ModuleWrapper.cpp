#include "stdafx.h"
#include "ModuleWrapper.h"
#include "ClassWrapper.h"
#include "AssemblyWrapper.h"
#include "FunctionWrapper.h"
#include "ProcessWrapper.h"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		String^ ModuleWrapper::GetName()
		{
			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG32 expectedSize;

			HRESULT hResult = Module->GetName(DefaultCharArraySize, &expectedSize, name);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = Module->GetName(expectedSize, &expectedSize, name);

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

		UInt32 ModuleWrapper::GetSize()
		{
			ULONG32 size;
			CheckHResult(Module->GetSize(&size));

			return size;
		}

		bool ModuleWrapper::IsDynamic()
		{
			BOOL isDynamic;
			CheckHResult(Module->IsDynamic(&isDynamic));

			return (isDynamic == TRUE ? true : false);
		}

		void ModuleWrapper::EnableClassLoadBacks(bool enable)
		{
			Module->EnableClassLoadCallbacks((enable ? TRUE : FALSE));
		}

		void ModuleWrapper::EnableJitDebugging(bool jitDebugging, bool allowJitOptimization)
		{
			Module->EnableJITDebugging((jitDebugging ? TRUE : FALSE), (allowJitOptimization ? TRUE : FALSE));
		}

		ClassWrapper^ ModuleWrapper::GetClass(mdTypeDef typeDef)
		{
			ICorDebugClass* classObject;
			CheckHResult(Module->GetClassFromToken(typeDef, &classObject));

			ClassWrapper^ result = gcnew ClassWrapper();
			result->ClassObject = classObject;

			return result;
		}

		FunctionWrapper^ ModuleWrapper::GetFunction(mdMethodDef token)
		{
			ICorDebugFunction* function;
			CheckHResult(Module->GetFunctionFromToken(token, &function));

			FunctionWrapper^ result = gcnew FunctionWrapper();
			result->Function = function;

			return result;
		}

		AssemblyWrapper^ ModuleWrapper::GetAssembly()
		{
			ICorDebugAssembly* assembly;
			CheckHResult(Module->GetAssembly(&assembly));

			AssemblyWrapper^ result = gcnew AssemblyWrapper();
			result->Assembly = assembly;

			return result;
		}

		UInt32 ModuleWrapper::GetToken()
		{
			mdModule token;
			CheckHResult(Module->GetToken(&token));

			return token;
		}

		UInt64 ModuleWrapper::GetBaseAddress()
		{
			CORDB_ADDRESS baseAddress;
			CheckHResult(Module->GetBaseAddress(&baseAddress));

			return baseAddress;
		}

		bool ModuleWrapper::IsInMemory()
		{
			BOOL isInMemory;
			CheckHResult(Module->IsInMemory(&isInMemory));

			return (isInMemory == TRUE ? true : false);
		}

		Object^ ModuleWrapper::GetMetaDataImport2()
		{
			IMetaDataImport2* import2;
			CheckHResult(Module->GetMetaDataInterface(IID_IMetaDataImport2, (IUnknown**)&import2));

			IntPtr^ import2Pointer = gcnew IntPtr(import2);
			return Marshal::GetObjectForIUnknown(*import2Pointer);
		}

		Object^ ModuleWrapper::GetMetaDataAssemblyImport()
		{
			IMetaDataAssemblyImport* assemblyImport;
			CheckHResult(Module->GetMetaDataInterface(IID_IMetaDataAssemblyImport, (IUnknown**)&assemblyImport));

			IntPtr^ assemblyImportPointer = gcnew IntPtr(assemblyImport);
			return Marshal::GetObjectForIUnknown(*assemblyImportPointer);
		}

		ProcessWrapper^ ModuleWrapper::GetProcess()
		{
			ICorDebugProcess* process;
			CheckHResult(Module->GetProcess(&process));

			ProcessWrapper^ result = gcnew ProcessWrapper();
			result->Process = process;

			return result;
		}

		String^ ModuleWrapper::GetNameFromMetaData()
		{
			IMetaDataImport* import;
			CheckHResult(Module->GetMetaDataInterface(IID_IMetaDataImport, (IUnknown**)&import));

			WCHAR *name = new WCHAR[DefaultCharArraySize];
			ULONG expectedSize;
			GUID mvid;

			HRESULT hResult = import->GetScopeProps(name, DefaultCharArraySize, &expectedSize, &mvid);

			if (FAILED(hResult))
			{
				delete [] name;
			}

			CheckHResult(hResult);

			if (expectedSize > DefaultCharArraySize)
			{
				delete [] name;

				name = new WCHAR[expectedSize];

				hResult = import->GetScopeProps(name, DefaultCharArraySize, &expectedSize, &mvid);

				if (FAILED(hResult))
				{
					delete [] name;
				}

				CheckHResult(hResult);
			}

			String^ result = gcnew String(name);

			delete [] name;

			import->Release();

			return result;
		}
	}
}