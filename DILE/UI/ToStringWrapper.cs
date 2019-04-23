using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.UI
{
	public class ToStringWrapper<T>
	{
		public T WrappedObject
		{
			get;
			private set;
		}

		public string StringValue
		{
			get;
			set;
		}

		public ToStringWrapper(T wrappedObject, string stringValue)
		{
			WrappedObject = wrappedObject;
			StringValue = stringValue;
		}

		public static implicit operator T(ToStringWrapper<T> wrapper)
		{
			return wrapper.WrappedObject;
		}

		public override string ToString()
		{
			return StringValue;
		}
	}
}