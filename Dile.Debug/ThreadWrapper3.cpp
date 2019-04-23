#include "Stdafx.h"
#include "ThreadWrapper.h"

namespace Dile
{
	namespace Debug
	{
		ThreadWrapper3::ThreadWrapper3()
		{
		}

		ThreadWrapper3::ThreadWrapper3(ICorDebugThread3* thread3)
		{
			ThreadWrapper3();

			Thread3 = thread3;
		}

		List<FrameWrapper^>^ ThreadWrapper3::StackWalk()
		{
			List<FrameWrapper^>^ result = gcnew List<FrameWrapper^>();

			ICorDebugStackWalk* stackWalk = NULL;
			CheckHResult(Thread3->CreateStackWalk(&stackWalk));
			bool stackEndReached = false;
			int index = 0;

			do
			{
				HRESULT hResult = stackWalk->Next();
				switch(hResult)
				{
				case S_OK:
					break;

				case CORDBG_S_AT_END_OF_STACK:
				case CORDBG_E_PAST_END_OF_STACK:
					stackEndReached = true;
					break;

				default:
					CheckHResult(hResult);
					break;
				}

				if (!stackEndReached)
				{
					ICorDebugFrame* frame = NULL;
					hResult = stackWalk->GetFrame(&frame);
					FrameWrapper^ frameWrapper = nullptr;
					switch(hResult)
					{
					case S_OK:
					case S_FALSE:
						frameWrapper = gcnew FrameWrapper(0, index++, frame);
						result->Add(frameWrapper);
						break;

					default:
						CheckHResult(hResult);
						break;
					}
				}
			} while (!stackEndReached);

			return result;
		}

		FrameWrapper^ ThreadWrapper3::GetActiveFrame()
		{
			FrameWrapper^ result = nullptr;

			ICorDebugStackWalk* stackWalk = NULL;
			CheckHResult(Thread3->CreateStackWalk(&stackWalk));
			
			ICorDebugFrame* frame = NULL;
			HRESULT hResult = stackWalk->Next();
			switch(hResult)
			{
			case S_OK:
				hResult = stackWalk->GetFrame(&frame);
				switch(hResult)
				{
				case S_OK:
				case S_FALSE:
					result = gcnew FrameWrapper(0, 0, frame);
					break;

				default:
					CheckHResult(hResult);
					break;
				}
				break;

			case CORDBG_S_AT_END_OF_STACK:
			case CORDBG_E_PAST_END_OF_STACK:
				break;

			default:
				CheckHResult(hResult);
				break;
			}

			return result;
		}
	}
}