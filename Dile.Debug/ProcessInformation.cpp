#include "stdafx.h"
#include "ProcessInformation.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		int ProcessInformation::CompareTo(Object^ obj)
		{
			if (obj == nullptr)
			{
				throw gcnew ArgumentNullException("obj");
			}

			if (obj->GetType() != this->GetType())
			{
				throw gcnew ArgumentException("The object's type is different.", "obj");
			}

			return ID.CompareTo(((ProcessInformation^)obj)->ID);
		}
	}
}