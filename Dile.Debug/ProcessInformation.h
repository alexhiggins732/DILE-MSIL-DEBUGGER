#pragma once

using namespace System;

namespace Dile
{
	namespace Debug
	{
		public ref class ProcessInformation : public IComparable
		{
		private:
			String^ name;
			unsigned int id;

		public:
			property String^ Name
			{
				String^ get()
				{
					return name;
				}

				void set(String^ value)
				{
					name = value;
				}
			}

			property unsigned int ID
			{
				unsigned int get()
				{
					return id;
				}

				void set(unsigned int value)
				{
					id = value;
				}
			}

			virtual int CompareTo(Object^ obj);
		};
	}
}