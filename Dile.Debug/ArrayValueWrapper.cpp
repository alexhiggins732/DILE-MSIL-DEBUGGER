#include "stdafx.h"
#include "ArrayValueWrapper.h"
#include "ValueWrapper.h"

namespace Dile
{
	namespace Debug
	{
		ValueWrapper^ ArrayValueWrapper::GetElementAtPosition(ULONG32 position)
		{
			ICorDebugValue* value;
			CheckHResult(ArrayValue->GetElementAtPosition(position, &value));

			ValueWrapper^ result = gcnew ValueWrapper();
			result->Value = value;

			return result;
		}

		ValueWrapper^ ArrayValueWrapper::GetElement(List<UInt32>^ indices)
		{
			ULONG32 *positions = new ULONG32[indices->Count];

			for(int index = 0; index < indices->Count; index++)
			{
				positions[index] = indices[index];
			}

			ICorDebugValue* value;
			HRESULT hResult = ArrayValue->GetElement(indices->Count, positions, &value);

			delete [] positions;
			ValueWrapper^ result = nullptr;

			if (FAILED(hResult))
			{
				CheckHResult(hResult);
			}
			else
			{
				result = gcnew ValueWrapper();
				result->Value = value;
			}

			return result;
		}

		List<UInt32>^ ArrayValueWrapper::GetBaseIndicies()
		{
			GetRank();

			ULONG32 *indicies = new ULONG32[rank];
			HRESULT hResult = ArrayValue->GetBaseIndicies(rank, indicies);

			List<ULONG32>^ result = nullptr;

			if (FAILED(hResult))
			{
				delete [] indicies;
				CheckHResult(hResult);
			}
			else
			{
				result = gcnew List<ULONG32>();

				for(ULONG32 index = 0; index < rank; index++)
				{
					result->Add(indicies[index]);
				}

				delete [] indicies;
			}

			return result;
		}

		UInt32 ArrayValueWrapper::GetCount()
		{
			ULONG32 count;

			CheckHResult(ArrayValue->GetCount(&count));

			return count;
		}

		List<UInt32>^ ArrayValueWrapper::GetDimensions()
		{
			GetRank();

			ULONG32 *dimensions = new ULONG32[rank];
			HRESULT hResult = ArrayValue->GetDimensions(rank, dimensions);

			List<ULONG32>^ result = nullptr;

			if (FAILED(hResult))
			{
				delete [] dimensions;
				CheckHResult(hResult);
			}
			else
			{
				result = gcnew List<ULONG32>();

				for(ULONG32 index = 0; index < rank; index++)
				{
					result->Add(dimensions[index]);
				}

				delete [] dimensions;
			}

			return result;
		}

		UInt32 ArrayValueWrapper::GetRank()
		{
			if (rank == -1)
			{
				ULONG32 tempRank;
				CheckHResult(ArrayValue->GetRank(&tempRank));

				rank = tempRank;
			}

			return rank;
		}

		bool ArrayValueWrapper::HasBaseIndicies()
		{
			BOOL result;
			CheckHResult(ArrayValue->HasBaseIndicies(&result));

			return (result == TRUE ? true : false);
		}

		int ArrayValueWrapper::GetElementType()
		{
			CorElementType result;

			CheckHResult(arrayValue->GetElementType(&result));

			return result;
		}
	}
}