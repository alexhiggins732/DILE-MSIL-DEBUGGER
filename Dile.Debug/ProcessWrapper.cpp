#include "stdafx.h"
#include "ProcessWrapper.h"
#include "ModuleWrapper.h"
#include "BaseWrapper.h"
#include "CorError.h"
#include "Constants.h"
#include "ThreadWrapper.h"
#include "AppDomainWrapper.h"
#include "AssemblyWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::IO;

namespace Dile
{
	namespace Debug
	{
		void ProcessWrapper::EnableLogMessages(bool enable)
		{
			BOOL onOff = (enable ? TRUE : FALSE);

			CheckHResult(Process->EnableLogMessages(onOff));
		}

		UInt32 ProcessWrapper::GetHelperThreadID()
		{
			DWORD id;
			CheckHResult(Process->GetHelperThreadID(&id));

			return id;
		}

		UInt32 ProcessWrapper::GetID()
		{
			DWORD id;
			CheckHResult(Process->GetID(&id));

			return id;
		}

		void ProcessWrapper::ClearCurrentException(DWORD threadID)
		{
			CheckHResult(Process->ClearCurrentException(threadID));
		}

		void ProcessWrapper::EnumerateModules(String^ moduleName, ICorDebugAssembly* assembly, List<ModuleWrapper^>^ foundModules)
		{
			ICorDebugModuleEnum* moduleEnum;
			CheckHResult(assembly->EnumerateModules(&moduleEnum));

			ULONG fetched;
			ICorDebugModule* modules[DefaultArrayCount];
			CheckHResult(moduleEnum->Next(DefaultArrayCount, modules, &fetched));

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugModule* module = modules[index];
					ModuleWrapper^ moduleWrapper = gcnew ModuleWrapper();
					moduleWrapper->Module = module;

					if (moduleName != nullptr && moduleName->Length > 0)
					{
						String^ moduleWrapperName;

						if (moduleWrapper->IsInMemory())
						{
							moduleWrapperName = moduleWrapper->GetNameFromMetaData();
						}
						else
						{
							moduleWrapperName = moduleWrapper->GetName();

							try
							{
								moduleWrapperName = Path::GetFileName(moduleWrapperName);
							}
							catch (...)
							{
							}
						}

						if (moduleName->Equals(moduleWrapperName, StringComparison::OrdinalIgnoreCase))
						{
							foundModules->Add(moduleWrapper);
						}
					}
					else
					{
						foundModules->Add(moduleWrapper);
					}
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(moduleEnum->Next(DefaultArrayCount, modules, &fetched));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}
		}

		void ProcessWrapper::EnumerateAssemblies(String^ moduleName, ICorDebugAppDomain* appDomain, List<ModuleWrapper^>^ foundModules)
		{
			ICorDebugAssemblyEnum* assemblyEnum;
			CheckHResult(appDomain->EnumerateAssemblies(&assemblyEnum));

			ULONG fetched;
			ICorDebugAssembly* assemblies[DefaultArrayCount];
			CheckHResult(assemblyEnum->Next(DefaultArrayCount, assemblies, &fetched));

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugAssembly* assembly = assemblies[index];
					EnumerateModules(moduleName, assembly, foundModules);
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(assemblyEnum->Next(DefaultArrayCount, assemblies, &fetched));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}
		}

		void ProcessWrapper::EnumerateAppDomains(String^ moduleName, List<ModuleWrapper^>^ foundModules)
		{
			ICorDebugAppDomainEnum* appDomainEnum;
			CheckHResult(Process->EnumerateAppDomains(&appDomainEnum));

			ULONG fetched;
			ICorDebugAppDomain* appDomains[DefaultArrayCount];
			CheckHResult(appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched));

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugAppDomain* appDomain = appDomains[index];
					EnumerateAssemblies(moduleName, appDomain, foundModules);
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}
		}

		List<ModuleWrapper^>^ ProcessWrapper::FindModulesByName(String^ moduleName)
		{
			List<ModuleWrapper^>^ result = gcnew List<ModuleWrapper^>();

			if (moduleName != nullptr)
			{
				moduleName = moduleName->ToUpperInvariant();
			}

			EnumerateAppDomains(moduleName, result);

			return result;
		}

		List<ModuleWrapper^>^ ProcessWrapper::GetModules()
		{
			return FindModulesByName(nullptr);
		}

		ThreadWrapper^ ProcessWrapper::GetThread(UInt32 threadID)
		{
			ICorDebugThread* thread;
			CheckHResult(Process->GetThread(threadID, &thread));

			ThreadWrapper^ result = gcnew ThreadWrapper();
			result->Thread = thread;

			return result;
		}

		List<AppDomainWrapper^>^ ProcessWrapper::GetAppDomains()
		{
			List<AppDomainWrapper^>^ result = gcnew List<AppDomainWrapper^>();
			ICorDebugAppDomainEnum* appDomainEnum;
			CheckHResult(Process->EnumerateAppDomains(&appDomainEnum));

			ULONG fetched;
			ICorDebugAppDomain *appDomains[DefaultArrayCount];
			HRESULT hResult = appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugAppDomain* appDomain = appDomains[index];

						AppDomainWrapper^ appDomainWrapper = gcnew AppDomainWrapper();
						appDomainWrapper->AppDomain = appDomain;
						result->Add(appDomainWrapper);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}

			return result;
		}

		void ProcessWrapper::DeactivateSteppers()
		{
			ICorDebugAppDomainEnum* appDomainEnum;
			CheckHResult(Process->EnumerateAppDomains(&appDomainEnum));

			ULONG fetched;
			ICorDebugAppDomain *appDomains[16];
			HRESULT hResult = appDomainEnum->Next(16, appDomains, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugAppDomain* appDomain = appDomains[index];

						AppDomainWrapper^ appDomainWrapper = gcnew AppDomainWrapper();
						appDomainWrapper->AppDomain = appDomain;
						appDomainWrapper->DeactivateSteppers();
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}
		}

		void ProcessWrapper::ActivateBreakpoints(bool active)
		{
			ICorDebugAppDomainEnum* appDomainEnum;
			CheckHResult(Process->EnumerateAppDomains(&appDomainEnum));

			ULONG fetched;
			ICorDebugAppDomain *appDomains[DefaultArrayCount];
			HRESULT hResult = appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched);

			if (SUCCEEDED(hResult))
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugAppDomain* appDomain = appDomains[index];

						AppDomainWrapper^ appDomainWrapper = gcnew AppDomainWrapper();
						appDomainWrapper->AppDomain = appDomain;
						appDomainWrapper->ActivateBreakpoints(active);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(appDomainEnum->Next(DefaultArrayCount, appDomains, &fetched));
					}
					else
					{
						fetched = DefaultArrayCount + 1;
					}
				}
			}
			else
			{
				CheckHResult(hResult);
			}
		}

		void ProcessWrapper::Detach()
		{
			DeactivateSteppers();
			ActivateBreakpoints(false);
			ControllerWrapper::Detach();
		}

		array<Byte>^ ProcessWrapper::ReadMemory(CORDB_ADDRESS address, DWORD size)
		{
			BYTE *buffer = new BYTE[size];
			SIZE_T read;
			CheckHResult(Process->ReadMemory(address, size, buffer, &read));

			array<Byte>^ result = gcnew array<Byte>((unsigned long)read);

			for (unsigned int index = 0; index < read; index++)
			{
				result[index] = buffer[index];
			}

			delete [] buffer;

			return result;
		}
	}
}