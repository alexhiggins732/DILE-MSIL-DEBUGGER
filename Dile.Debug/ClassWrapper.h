#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "ModuleWrapper.h"
#include "ClassWrapper2.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		ref class FrameWrapper;
		ref class ValueWrapper;

		public ref class ClassWrapper : BaseWrapper
		{
		private:
			ICorDebugClass* classObject;
			ClassWrapper2^ class2ObjectWrapper;
			bool^ isVersion2;

		internal:
			property ICorDebugClass* ClassObject
			{
				ICorDebugClass* get()
				{
					return classObject;
				}

				void set (ICorDebugClass* value)
				{
					classObject = value;
				}
			}

			ClassWrapper()
			{
			}

			ClassWrapper(ICorDebugClass* classObject)
			{
				ClassWrapper();

				ClassObject = classObject;
			}

		public:
			property ClassWrapper2^ Version2
			{
				ClassWrapper2^ get()
				{
					if(class2ObjectWrapper == nullptr)
					{
						ICorDebugClass2* class2ObjectPointer;

						if (SUCCEEDED(classObject->QueryInterface(&class2ObjectPointer)))
						{
							class2ObjectWrapper = gcnew ClassWrapper2(class2ObjectPointer);
						}
					}

					return class2ObjectWrapper;
				}
			}

			property bool IsVersion2
			{
				virtual bool get()
				{
					bool result = false;

					if (isVersion2 == nullptr)
					{
						isVersion2 = (Version2 != nullptr);
						result = *isVersion2;
					}else
					{
						result = *isVersion2;
					}

					return result;
				}
			}

			UInt32 GetToken();

			ModuleWrapper^ GetModule();

			ValueWrapper^ GetStaticFieldValue(mdFieldDef fieldToken, FrameWrapper^ frame);
		};
	}
}