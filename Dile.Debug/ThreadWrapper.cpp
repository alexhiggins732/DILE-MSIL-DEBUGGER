#include "stdafx.h"
#include "ThreadWrapper.h"
#include "ProcessWrapper.h"
#include "AppDomainWrapper.h"
#include "EvalWrapper.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;

namespace Dile
{
	namespace Debug
	{
		FrameWrapper^ ThreadWrapper::GetActiveFrame()
		{
			ICorDebugFrame* frame;
			CheckHResult(Thread->GetActiveFrame(&frame));

			FrameWrapper^ result = nullptr;

			if (frame != NULL)
			{
				result = gcnew FrameWrapper(frame);
				result->SetIsActiveFrame(true);
			}

			return result;
		}

		UInt32 ThreadWrapper::GetID()
		{
			DWORD id;
			CheckHResult(Thread->GetID(&id));

			return id;
		}

		List<FrameWrapper^>^ ThreadWrapper::GetCallStack()
		{
			List<FrameWrapper^>^ result = gcnew List<FrameWrapper^>();
			ICorDebugChainEnum* chainEnum;
			CheckHResult(Thread->EnumerateChains(&chainEnum));

			ULONG fetched;
			ICorDebugChain *chains[DefaultArrayCount];
			CheckHResult(chainEnum->Next(DefaultArrayCount, chains, &fetched));

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugChain* chain = chains[index];

					ICorDebugFrameEnum* frameEnum;
					CheckHResult(chain->EnumerateFrames(&frameEnum));

					ULONG fetchedFrames;
					ICorDebugFrame *frames[DefaultArrayCount];
					CheckHResult(frameEnum->Next(DefaultArrayCount, frames, &fetchedFrames));

					while (fetchedFrames <= DefaultArrayCount)
					{
						for(ULONG frameIndex = 0; frameIndex < fetchedFrames; frameIndex++)
						{
							ICorDebugFrame* frame = frames[frameIndex];

							FrameWrapper^ frameWrapper = gcnew FrameWrapper(index, frameIndex, frame);

							result->Add(frameWrapper);
						}

						if (fetchedFrames == DefaultArrayCount)
						{
							CheckHResult(frameEnum->Next(DefaultArrayCount, frames, &fetchedFrames));
						}
						else
						{
							fetchedFrames = DefaultArrayCount + 1;
						}
					}
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(chainEnum->Next(DefaultArrayCount, chains, &fetched));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}

			return result;
		}

		StepperWrapper^ ThreadWrapper::CreateStepper()
		{
			ICorDebugStepper* stepper;
			CheckHResult(Thread->CreateStepper(&stepper));

			StepperWrapper^ result = gcnew StepperWrapper();
			result->Stepper = stepper;

			return result;
		}

		ValueWrapper^ ThreadWrapper::GetCurrentException()
		{
			ICorDebugValue* exception;
			HRESULT hResult = Thread->GetCurrentException(&exception);

			ValueWrapper^ result = nullptr;

			if (SUCCEEDED(hResult) && exception != NULL)
			{
				result = gcnew ValueWrapper();
				result->Value = exception;
			}

			return result;
		}

		Int32 ThreadWrapper::CallFunction(FunctionWrapper^ function, List<ValueWrapper^>^ arguments, interior_ptr<EvalWrapper^> evalWrapper)
		{
			ICorDebugEval* eval;
			CheckHResult(Thread->CreateEval(&eval));

			*evalWrapper = gcnew EvalWrapper(eval);
			ICorDebugValue** args = new ICorDebugValue*[arguments->Count];

			for(int index = 0; index < arguments->Count; index++)
			{
				args[index] = arguments[index]->Value;
			}

			HRESULT hResult = eval->CallFunction(function->Function, arguments->Count, args);

			delete [] args;

			return hResult;
		}

		FrameWrapper^ ThreadWrapper::FindFrame(int chainIndex, int frameIndex)
		{
			FrameWrapper^ result = nullptr;
			ICorDebugChainEnum* chainEnum;
			CheckHResult(Thread->EnumerateChains(&chainEnum));
			ULONG count;
			chainEnum->GetCount(&count);

			if (chainIndex > 0)
			{
				CheckHResult(chainEnum->Skip(chainIndex));
			}

			ULONG fetched;
			ICorDebugChain *chains[1];
			CheckHResult(chainEnum->Next(1, chains, &fetched));

			if (fetched == 1)
			{
				ICorDebugChain* currentChain = chains[0];
				ICorDebugFrameEnum* frameEnum;
				CheckHResult(currentChain->EnumerateFrames(&frameEnum));
				frameEnum->GetCount(&count);

				if (frameIndex > 0)
				{
					CheckHResult(frameEnum->Skip(frameIndex));
				}

				ICorDebugFrame *frames[1];
				CheckHResult(frameEnum->Next(1, frames, &fetched));
				
				if (fetched == 1)
				{
					result = gcnew FrameWrapper(chainIndex, frameIndex, frames[0]);
				}
			}

			return result;
		}

		void ThreadWrapper::ClearCurrentException()
		{
			Thread->ClearCurrentException();
		}

		void ThreadWrapper::InterceptCurrentException()
		{
			ICorDebugThread2* thread2;
			HRESULT	hResult = Thread->QueryInterface(&thread2);

			if (SUCCEEDED(hResult))
			{
				ICorDebugFrame* frame;
				Thread->GetActiveFrame(&frame);
				thread2->InterceptCurrentException(frame);
			}
		}

		ProcessWrapper^ ThreadWrapper::GetProcess()
		{
			ICorDebugProcess* process;
			CheckHResult(Thread->GetProcess(&process));

			ProcessWrapper^ result = gcnew ProcessWrapper();
			result->Process = process;

			return result;
		}

		ValueWrapper^ ThreadWrapper::GetObject()
		{
			ICorDebugValue* value;
			HRESULT hResult = Thread->GetObject(&value);

			ValueWrapper^ result = nullptr;

			if (SUCCEEDED(hResult))
			{
				result = gcnew ValueWrapper();
				result->Value = value;
			}

			return result;
		}

		void ThreadWrapper::EnumerateModules(String^ moduleName, bool verifyExtension, ICorDebugAssembly* assembly, List<ModuleWrapper^>^ foundModules)
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
					String^ moduleWrapperName = moduleWrapper->GetName();

					try
					{
						if (verifyExtension)
						{
							moduleWrapperName = Path::GetFileName(moduleWrapperName);
						}
						else
						{
							moduleWrapperName = Path::GetFileNameWithoutExtension(moduleWrapperName);
						}
					}
					catch (...)
					{
					}

					if (moduleName->Equals(moduleWrapperName, StringComparison::OrdinalIgnoreCase))
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

		void ThreadWrapper::EnumerateAssemblies(String^ moduleName, bool verifyExtension, ICorDebugAppDomain* appDomain, List<ModuleWrapper^>^ foundModules)
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
					EnumerateModules(moduleName, verifyExtension, assembly, foundModules);
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

		List<ModuleWrapper^>^ ThreadWrapper::FindModulesByName(String^ moduleName)
		{
			List<ModuleWrapper^>^ result = gcnew List<ModuleWrapper^>();
			ICorDebugAppDomain* appDomain;
			CheckHResult(Thread->GetAppDomain(&appDomain));

			EnumerateAssemblies(moduleName, true, appDomain, result);

			return result;
		}

		List<ModuleWrapper^>^ ThreadWrapper::FindModulesByNameWithoutExtension(String^ moduleName)
		{
			List<ModuleWrapper^>^ result = gcnew List<ModuleWrapper^>();
			ICorDebugAppDomain* appDomain;
			CheckHResult(Thread->GetAppDomain(&appDomain));

			EnumerateAssemblies(moduleName, false, appDomain, result);

			return result;
		}

		UInt32 ThreadWrapper::GetDebugState()
		{
			CorDebugThreadState threadState;
			CheckHResult(Thread->GetDebugState(&threadState));

			return threadState;
		}

		void ThreadWrapper::SetDebugState(UInt32 debugState)
		{
			CheckHResult(Thread->SetDebugState((CorDebugThreadState)debugState));
		}

		AppDomainWrapper^ ThreadWrapper::GetAppDomain()
		{
			ICorDebugAppDomain* appDomain;
			CheckHResult(Thread->GetAppDomain(&appDomain));

			AppDomainWrapper^ result = gcnew AppDomainWrapper();
			result->AppDomain = appDomain;

			return result;
		}

		EvalWrapper^ ThreadWrapper::CreateEval()
		{
			ICorDebugEval* eval;
			CheckHResult(Thread->CreateEval(&eval));

			EvalWrapper^ result = gcnew EvalWrapper(eval);

			return result;
		}
	}
}