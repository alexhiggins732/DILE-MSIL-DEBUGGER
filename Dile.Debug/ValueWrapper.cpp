#include "stdafx.h"
#include "ValueWrapper.h"

namespace Dile
{
	namespace Debug
	{
		generic<typename T> T ValueWrapper::GetGenericValue()
		{
			T result;

			ICorDebugGenericValue* genericValue;
			CheckHResult(value->QueryInterface(&genericValue));
			CheckHResult(genericValue->GetValue(&result));

			return result;
		}

		generic<typename T> void ValueWrapper::SetGenericValue(T newValue)
		{
			ICorDebugGenericValue* genericValue;
			CheckHResult(value->QueryInterface(&genericValue));
			CheckHResult(genericValue->SetValue(&newValue));
		}

		void ValueWrapper::SetGenericValue(ValueWrapper ^newValue)
		{
			ICorDebugGenericValue* genericValue;
			CheckHResult(newValue->Value->QueryInterface(&genericValue));

			LONG newValueToSet = 0;
			CheckHResult(genericValue->GetValue(&newValueToSet));

			CheckHResult(value->QueryInterface(&genericValue));
			CheckHResult(genericValue->SetValue(&newValueToSet));
		}

		String^ ValueWrapper::GetStringValue()
		{
			ICorDebugStringValue* stringValue;
			CheckHResult(value->QueryInterface(&stringValue));

			ULONG32 stringLength;
			CheckHResult(stringValue->GetLength(&stringLength));
			stringLength++;

			WCHAR* string = new WCHAR[stringLength];
			ULONG32 expectedLength;
			HRESULT hResult = stringValue->GetString(stringLength, &expectedLength, string);

			if (FAILED(hResult))
			{
				delete [] string;
			}

			CheckHResult(hResult);

			String^ result = gcnew String(string);

			delete [] string;

			return result;
		}

		bool ValueWrapper::IsBoxedValue()
		{
			ICorDebugBoxValue* boxValue;
			return (FAILED(value->QueryInterface(&boxValue)) ? false : true);
		}

		ValueWrapper^ ValueWrapper::UnboxValue()
		{
			ICorDebugBoxValue* boxValue;
			CheckHResult(value->QueryInterface(&boxValue));
			
			ICorDebugObjectValue* unboxValue;
			CheckHResult(boxValue->GetObject(&unboxValue));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = unboxValue;

			return result;
		}

		ValueWrapper^ ValueWrapper::DereferenceValue()
		{
			ValueWrapper^ result = nullptr;
			ICorDebugReferenceValue* referenceValue;
			
			if (SUCCEEDED(value->QueryInterface(&referenceValue)))
			{
				BOOL isNull;
				CheckHResult(referenceValue->IsNull(&isNull));

				if (isNull == FALSE)
				{
					ICorDebugValue* debugValue;
					HRESULT hResult = referenceValue->Dereference(&debugValue);

					if (SUCCEEDED(hResult) && debugValue != NULL)
					{
						result = gcnew ValueWrapper();
						result->Value = debugValue;
					}
				}
			}

			return result;
		}

		ValueWrapper^ ValueWrapper::DereferenceStrong()
		{
			ICorDebugReferenceValue* referenceValue;
			CheckHResult(value->QueryInterface(&referenceValue));

			ICorDebugValue* dereferencedValue;
			CheckHResult(referenceValue->DereferenceStrong(&dereferencedValue));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = dereferencedValue;

			return result;
		}

		ValueWrapper^ ValueWrapper::CreateHandle(bool strongHandle)
		{
			ICorDebugHeapValue2* heap;
			HRESULT hResult = value->QueryInterface(&heap);

			ValueWrapper^ result = nullptr;

			if (SUCCEEDED(hResult))
			{
				ICorDebugHandleValue* handle;
				CheckHResult(heap->CreateHandle((strongHandle ? HANDLE_STRONG : HANDLE_WEAK_TRACK_RESURRECTION), &handle));

				result = gcnew ValueWrapper();
				result->Value = handle;
			}

			return result;
		}

		ClassWrapper^ ValueWrapper::GetClassInformation()
		{
			ICorDebugClass* classObject;
			CheckHResult(GetObjectValue()->GetClass(&classObject));

			ClassWrapper^ result = gcnew ClassWrapper();
			result->ClassObject = classObject;

			return result;
		}

		ValueWrapper^ ValueWrapper::GetFieldValue(ClassWrapper^ classObject, UInt32 fieldToken)
		{
			ICorDebugValue* fieldValue;
			CheckHResult(GetObjectValue()->GetFieldValue(classObject->ClassObject, fieldToken, &fieldValue));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = fieldValue;

			return result;
		}

		ArrayValueWrapper^ ValueWrapper::ConvertToArrayValue()
		{
			ICorDebugReferenceValue* referenceValue;
			CheckHResult(value->QueryInterface(&referenceValue));

			BOOL isNull;
			CheckHResult(referenceValue->IsNull(&isNull));

			ArrayValueWrapper^ result = nullptr;

			if (!isNull)
			{
				ICorDebugValue* dereferencedValue;
				CheckHResult(referenceValue->Dereference(&dereferencedValue));

				ICorDebugArrayValue* arrayValue;
				CheckHResult(dereferencedValue->QueryInterface(&arrayValue));

				result = gcnew ArrayValueWrapper();
				result->ArrayValue = arrayValue;
			}

			return result;
		}

		bool ValueWrapper::IsNull()
		{
			ICorDebugReferenceValue* referenceValue;
			CheckHResult(value->QueryInterface(&referenceValue));

			BOOL isNull;
			CheckHResult(referenceValue->IsNull(&isNull));

			return (isNull == TRUE ? true : false);
		}

		UInt32 ValueWrapper::GetDereferenceError()
		{
			HRESULT result = S_OK;
			ICorDebugReferenceValue* referenceValue;
			result = value->QueryInterface(&referenceValue);

			if (SUCCEEDED(result))
			{
				BOOL isNull;
				result = referenceValue->IsNull(&isNull);

				if (SUCCEEDED(result) && isNull == FALSE)
				{
					ICorDebugValue* debugValue;
					result = referenceValue->Dereference(&debugValue);
				}
			}

			return result;
		}

		CORDB_ADDRESS ValueWrapper::GetValue()
		{
			CORDB_ADDRESS result;
			ICorDebugReferenceValue* referenceValue;
			
			if (SUCCEEDED(value->QueryInterface(&referenceValue)))
			{
				referenceValue->GetValue(&result);
			}

			return result;
		}

		void ValueWrapper::SetValue(Dile::Debug::ValueWrapper ^valueWrapper)
		{
			ICorDebugReferenceValue* referenceValue;
			
			if (SUCCEEDED(value->QueryInterface(&referenceValue)))
			{
				CORDB_ADDRESS sourceAddress = valueWrapper->GetValue();
				referenceValue->SetValue(sourceAddress);
			}
		}
	}
}