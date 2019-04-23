#include "stdafx.h"
#include "FrameWrapper.h"
#include "ChainWrapper.h"
#include "Constants.h"
#include "FunctionWrapper.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ULONG32 FrameWrapper::MapToIL(ICorDebugCode* code, ULONG32 nativeIP)
		{
			ULONG32 result = -4;
			ULONG32 count;
			COR_DEBUG_IL_TO_NATIVE_MAP* mappings = new COR_DEBUG_IL_TO_NATIVE_MAP[0];
			CheckHResult(code->GetILToNativeMapping(0, &count, mappings));

			delete [] mappings;
			mappings = new COR_DEBUG_IL_TO_NATIVE_MAP[count];
			CheckHResult(code->GetILToNativeMapping(count, &count, mappings));
			bool found = false;

			if (count > 0)
			{
				ULONG32 index = 0;

				while (!found && index < count)
				{
					COR_DEBUG_IL_TO_NATIVE_MAP mapping = mappings[index++];

					if (mapping.nativeStartOffset <= nativeIP && mapping.nativeEndOffset >= nativeIP && mapping.ilOffset != NO_MAPPING && mapping.ilOffset != PROLOG && mapping.ilOffset != EPILOG)
					{
						result = mapping.ilOffset;
						found = true;
					}
				}
			}

			delete [] mappings;

			return result;
		}

		void FrameWrapper::SetIsActiveFrame(bool isActiveFrame)
		{
			this->isActiveFrame = isActiveFrame;
		}

		bool FrameWrapper::IsILFrame()
		{
			return (ILFrame != nullptr);
		}

		UInt32 FrameWrapper::GetFunctionToken()
		{
			mdMethodDef result;

			CheckHResult(Frame->GetFunctionToken(&result));

			return result;
		}

		UInt32 FrameWrapper::GetIP(interior_ptr<bool> exactLocation)
		{
			ULONG32 result;
			CorDebugMappingResult mappingResult;

			CheckHResult(ILFrame->GetIP(&result, &mappingResult));
			*exactLocation = (mappingResult == MAPPING_EXACT);

			ICorDebugNativeFrame* nativeFrame;
			HRESULT hResult = Frame->QueryInterface(&nativeFrame);

			if (SUCCEEDED(hResult))
			{
				ULONG32 nativeIP;
				CheckHResult(nativeFrame->GetIP(&nativeIP));

				ICorDebugCode* code;
				CheckHResult(nativeFrame->GetCode(&code));

				ULONG32 mappedIL = MapToIL(code, nativeIP);

				if (mappedIL >= 0 && mappedIL != (ULONG32)-4)
				{
					result = mappedIL;
				}
			}

			return result;
		}

		List<ValueWrapper^>^ FrameWrapper::GetLocalVariables()
		{
			List<ValueWrapper^>^ result = gcnew List<ValueWrapper^>();
			ICorDebugValueEnum* localVariableEnum;
			CheckHResult(ILFrame->EnumerateLocalVariables(&localVariableEnum));

			ULONG fetched;
			ICorDebugValue *values[DefaultArrayCount];
			HRESULT hResult = localVariableEnum->Next(DefaultArrayCount, values, &fetched);

			if (SUCCEEDED(hResult) || hResult == 0x80131304)
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugValue* value = values[index];

						ValueWrapper^ valueWrapper = gcnew ValueWrapper();
						valueWrapper->Value = value;
						result->Add(valueWrapper);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(localVariableEnum->Next(DefaultArrayCount, values, &fetched));
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

		ValueWrapper^ FrameWrapper::GetLocalVariable(DWORD index)
		{
			ICorDebugValue* localVariable;
			CheckHResult(ILFrame->GetLocalVariable(index, &localVariable));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = localVariable;

			return result;
		}

		List<ValueWrapper^>^ FrameWrapper::GetArguments()
		{
			List<ValueWrapper^>^ result = gcnew List<ValueWrapper^>();
			ICorDebugValueEnum* argumentEnum;
			CheckHResult(ILFrame->EnumerateArguments(&argumentEnum));

			ULONG fetched;
			ICorDebugValue *values[DefaultArrayCount];
			HRESULT hResult = argumentEnum->Next(DefaultArrayCount, values, &fetched);

			if (SUCCEEDED(hResult) || hResult == 0x80131304)
			{
				while (fetched <= DefaultArrayCount)
				{
					for(ULONG index = 0; index < fetched; index++)
					{
						ICorDebugValue* value = values[index];

						ValueWrapper^ valueWrapper = gcnew ValueWrapper();
						valueWrapper->Value = value;
						result->Add(valueWrapper);
					}

					if (fetched == DefaultArrayCount)
					{
						CheckHResult(argumentEnum->Next(DefaultArrayCount, values, &fetched));
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

		ValueWrapper^ FrameWrapper::GetArgument(DWORD index)
		{
			ICorDebugValue* argument;
			CheckHResult(ILFrame->GetArgument(index, &argument));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = argument;

			return result;
		}

		HRESULT FrameWrapper::CanSetIP(UInt32 offset)
		{
			return ILFrame->CanSetIP(offset);
		}

		void FrameWrapper::SetIP(UInt32 offset)
		{
			CheckHResult(ILFrame->SetIP(offset));
		}

		ChainWrapper^ FrameWrapper::GetChain()
		{
			ICorDebugChain* chain;
			CheckHResult(Frame->GetChain(&chain));

			ChainWrapper^ result = gcnew ChainWrapper();
			result->Chain = chain;

			return result;
		}

		FunctionWrapper^ FrameWrapper::GetFunction()
		{
			ICorDebugFunction* function;
			CheckHResult(Frame->GetFunction(&function));

			FunctionWrapper^ result = gcnew FunctionWrapper();
			result->Function = function;

			return result;
		}

		UInt32 FrameWrapper::GetLocalVariableCount()
		{
			ICorDebugValueEnum* localVariableEnum;
			CheckHResult(ILFrame->EnumerateLocalVariables(&localVariableEnum));

			ULONG result;
			CheckHResult(localVariableEnum->GetCount(&result));

			return result;
		}

		UInt32 FrameWrapper::GetArgumentCount()
		{
			ICorDebugValueEnum* argumentEnum;
			CheckHResult(ILFrame->EnumerateArguments(&argumentEnum));

			ULONG result;
			CheckHResult(argumentEnum->GetCount(&result));

			return result;
		}
	}
}