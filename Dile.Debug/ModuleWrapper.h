#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class AssemblyWrapper;
		ref class ClassWrapper;
		ref class FunctionWrapper;
		ref class ProcessWrapper;

		public ref class ModuleWrapper : BaseWrapper
		{
		private:
			ICorDebugModule* module;

		internal:
			property ICorDebugModule* Module
			{
				ICorDebugModule* get()
				{
					return module;
				}

				void set (ICorDebugModule* value)
				{
					module = value;
				}
			}

			ModuleWrapper()
			{
			}

			ModuleWrapper(ICorDebugModule* module)
			{
				ModuleWrapper();

				Module = module;
			}

		public:
			String^ GetName();

			UInt32 GetSize();

			bool IsDynamic();

			void EnableClassLoadBacks(bool enable);

			void EnableJitDebugging(bool jitDebugging, bool allowJitOptimization);

			FunctionWrapper^ GetFunction(mdMethodDef token);

			ClassWrapper^ GetClass(mdTypeDef typeDef);

			AssemblyWrapper^ GetAssembly();

			UInt32 GetToken();

			UInt64 GetBaseAddress();

			bool IsInMemory();

			Object^ GetMetaDataImport2();

			Object^ GetMetaDataAssemblyImport();

			ProcessWrapper^ GetProcess();

			String^ GetNameFromMetaData();
		};
	}
}