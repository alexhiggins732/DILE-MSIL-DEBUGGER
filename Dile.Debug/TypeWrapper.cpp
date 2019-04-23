#include "stdafx.h"
#include "TypeWrapper.h"
#include "ClassWrapper.h"
#include "Constants.h"

namespace Dile
{
	namespace Debug
	{
		TypeWrapper^ TypeWrapper::GetBase()
		{
			TypeWrapper^ result = nullptr;

			ICorDebugType* baseType;
			HRESULT hResult = Type->GetBase(&baseType);

			if (hResult != E_INVALIDARG)
			{
				CheckHResult(hResult);

				if (baseType != NULL)
				{
					result = gcnew TypeWrapper(baseType);
				}
			}

			return result;
		}

		ClassWrapper^ TypeWrapper::GetClass()
		{
			ClassWrapper^ result = nullptr;

			ICorDebugClass* classPointer;
			CheckHResult(Type->GetClass(&classPointer));

			result = gcnew ClassWrapper(classPointer);

			return result;
		}

		List<TypeWrapper^>^ TypeWrapper::EnumerateTypeParameters()
		{
			List<TypeWrapper^>^ result = gcnew List<TypeWrapper^>();

			ICorDebugTypeEnum* typeEnum;
			CheckHResult(Type->EnumerateTypeParameters(&typeEnum));

			ULONG fetched;
			ICorDebugType* types[DefaultArrayCount];
			CheckHResult(typeEnum->Next(DefaultArrayCount, types, &fetched));

			while (fetched <= DefaultArrayCount)
			{
				for(ULONG index = 0; index < fetched; index++)
				{
					ICorDebugType* type = types[index];
					TypeWrapper^ typeWrapper = gcnew TypeWrapper(type);

					result->Add(typeWrapper);
				}

				if (fetched == DefaultArrayCount)
				{
					CheckHResult(Type->EnumerateTypeParameters(&typeEnum));
				}
				else
				{
					fetched = DefaultArrayCount + 1;
				}
			}

			return result;
		}

		List<TypeWrapper^>^ TypeWrapper::EnumerateAllTypeParameters()
		{
			List<TypeWrapper^>^ result = EnumerateTypeParameters();
			TypeWrapper^ baseTypeWrapper = GetBase();

			while (baseTypeWrapper != nullptr)
			{
				result->AddRange(baseTypeWrapper->EnumerateTypeParameters());

				baseTypeWrapper = baseTypeWrapper->GetBase();
			}

			return result;
		}
	}
}