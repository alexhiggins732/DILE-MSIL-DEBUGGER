#pragma once

#include "stdafx.h"
#include "BaseWrapper.h"
#include "ClassWrapper.h"
#include "ArrayValueWrapper.h"
#include "ValueWrapper2.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class ValueWrapper : BaseWrapper
		{
		private:
			bool disposed;
			ICorDebugValue* value;
			ICorDebugObjectValue* cachedObjectValue;
			CorElementType elementType;
			ValueWrapper2^ value2Wrapper;
			bool^ isVersion2;

			ICorDebugObjectValue* GetObjectValue()
			{
				if (cachedObjectValue == NULL)
				{
					ICorDebugObjectValue* objectValue;
					CheckHResult(value->QueryInterface(&objectValue));

					cachedObjectValue = objectValue;
				}

				return cachedObjectValue;
			}

			~ValueWrapper()
			{
				if (!disposed)
				{
					ICorDebugHandleValue* handle;

					HRESULT hResult = value->QueryInterface(&handle);

					if (SUCCEEDED(hResult))
					{
						handle->Dispose();
					}

					disposed = true;
					GC::SuppressFinalize(this);
				}
			}

		internal:
			property ICorDebugValue* Value
			{
				ICorDebugValue* get()
				{
					return value;
				}

				void set(ICorDebugValue* value)
				{
					this->value = value;

					CorElementType type;
					CheckHResult(value->GetType(&type));
					elementType = type;
					cachedObjectValue = NULL;
				}
			}

			ValueWrapper()
			{
				disposed = false;
			}

			ValueWrapper(ICorDebugValue* value)
			{
				ValueWrapper();

				Value = value;
			}

		public:
			property ValueWrapper2^ Version2
			{
				ValueWrapper2^ get()
				{
					if(value2Wrapper == nullptr)
					{
						ICorDebugValue2* value2Pointer;

						if (SUCCEEDED(value->QueryInterface(&value2Pointer)))
						{
							value2Wrapper = gcnew ValueWrapper2(value2Pointer);
						}
					}

					return value2Wrapper;
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
					}
					else
					{
						result = *isVersion2;
					}

					return result;
				}
			}

			property int ElementType
			{
				int get()
				{
					return elementType;
				}
			}

			generic<typename T> T GetGenericValue();

			generic<typename T> void SetGenericValue(T newValue);
			
			void SetGenericValue(ValueWrapper^ newValue);

			String^ GetStringValue();

			bool IsBoxedValue();

			ValueWrapper^ UnboxValue();

			ValueWrapper^ DereferenceValue();

			ValueWrapper^ DereferenceStrong();

			ValueWrapper^ CreateHandle(bool strongHandle);

			ClassWrapper^ GetClassInformation();

			ValueWrapper^ GetFieldValue(ClassWrapper^ classObject, UInt32 fieldToken);

			ArrayValueWrapper^ ConvertToArrayValue();

			bool IsNull();

			UInt32 GetDereferenceError();

			CORDB_ADDRESS GetValue();

			void SetValue(ValueWrapper^ valueWrapper);
		};
	}
}