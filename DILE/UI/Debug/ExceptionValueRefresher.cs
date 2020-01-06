using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Metadata;

namespace Dile.UI.Debug
{
	public class ExceptionValueRefresher : BaseValueRefresher
	{
		private ValueWrapper exceptionObject;
		private ValueWrapper ExceptionObject
		{
			get
			{
				return exceptionObject;
			}
			set
			{
				exceptionObject = value;
			}
		}

		public ExceptionValueRefresher(string name, ThreadWrapper thread) : base(name)
		{
			//TODO Seems to be working but I don't trust this. According to the documentation: "GetCurrentException: Returns the exception object which is currently being thrown by the thread. This will only exist for the duration of an exception callback."
			//The problem is that if the DereferenceStrong() is called on an ICorDebugReferenceValue then it will really dereference the value thus it can't be passed as "this" parameter for methods etc.
			ValueWrapper currentException = thread.GetCurrentException();
			ValueWrapper dereferencedException = currentException.DereferenceValue();

			if (dereferencedException != null)
			{
				try
				{
					ExceptionObject = dereferencedException.CreateHandle(true);
				}
				catch
				{
				}

				if (ExceptionObject == null)
				{
					ExceptionObject = currentException;
				}
			}

			IsObjectValue = true;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			return ExceptionObject;
		}
	}
}