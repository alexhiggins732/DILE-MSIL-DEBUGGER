#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "ValueWrapper.h"

using namespace System;
using namespace System::Collections::Generic;

namespace Dile
{
	namespace Debug
	{
		ref class ChainWrapper;
		ref class FunctionWrapper;

		public ref class FrameWrapper : BaseWrapper
		{
		private:
			ICorDebugFrame* frame;
			ICorDebugILFrame* ilFrame;
			int chainIndex;
			int frameIndex;
			bool isActiveFrame;

			ULONG32 MapToIL(ICorDebugCode* code, ULONG32 nativeIP);

		internal:
			property ICorDebugFrame* Frame
			{
				ICorDebugFrame* get()
				{
					return frame;
				}

				void set(ICorDebugFrame* value)
				{
					frame = value;

					if (frame == nullptr)
					{
						ilFrame = nullptr;
					}
					else
					{
						ICorDebugILFrame* tempILFrame;
						HRESULT hResult = frame->QueryInterface(&tempILFrame);

						if (SUCCEEDED(hResult))
						{
							ilFrame = tempILFrame;
						}
					}
				}
			}

			property ICorDebugILFrame* ILFrame
			{
				ICorDebugILFrame* get()
				{
					return ilFrame;
				}
			}

			void SetIsActiveFrame(bool isActiveFrame);

		public:
			property int ChainIndex
			{
				int get()
				{
					return chainIndex;
				}
			}

			property int FrameIndex
			{
				int get()
				{
					return frameIndex;
				}
			}

			property bool IsActiveFrame
			{
				bool get()
				{
					return isActiveFrame;
				}
			}

			FrameWrapper()
			{
				this->chainIndex = -1;
				this->frameIndex = -1;
				this->isActiveFrame = false;
			}

			FrameWrapper(int chainIndex, int frameIndex, ICorDebugFrame* frame)
			{
				this->chainIndex = chainIndex;
				this->frameIndex = frameIndex;
				Frame = frame;
			}

			FrameWrapper(ICorDebugFrame* frame)
			{
				Frame = frame;
			}

			bool IsILFrame();

			UInt32 GetFunctionToken();

			UInt32 GetIP(interior_ptr<bool> exactLocation);

			List<ValueWrapper^>^ GetLocalVariables();

			ValueWrapper^ GetLocalVariable(DWORD index);

			List<ValueWrapper^>^ GetArguments();

			ValueWrapper^ GetArgument(DWORD index);

			HRESULT CanSetIP(UInt32 offset);

			void SetIP(UInt32 offset);

			ChainWrapper^ GetChain();

			FunctionWrapper^ GetFunction();

			UInt32 GetLocalVariableCount();

			UInt32 GetArgumentCount();
		};
	}
}