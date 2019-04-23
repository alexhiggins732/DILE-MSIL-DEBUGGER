#pragma once

#include "Stdafx.h"

using namespace System;

namespace Dile
{
	namespace Debug
	{
		namespace Dump
		{
			public ref class Version
			{
			private:
				short major;
				short minor;
				short revision;
				short build;

			public:
				property short Major
				{
					short get()
					{
						return major;
					}

				private:
					void set(short value)
					{
						major = value;
					}
				}

				property short Minor
				{
					short get()
					{
						return minor;
					}

				private:
					void set(short value)
					{
						minor = value;
					}
				}

				property short Revision
				{
					short get()
					{
						return revision;
					}

				private:
					void set(short value)
					{
						revision = value;
					}
				}

				property short Build
				{
					short get()
					{
						return build;
					}

				private:
					void set(short value)
					{
						build = value;
					}
				}

				Version(short major, short minor, short revision, short build);

				Version(DWORD mostSignificantBits, DWORD leastSignificantBits);

				virtual String^ ToString() override;
			};
		}
	}
}