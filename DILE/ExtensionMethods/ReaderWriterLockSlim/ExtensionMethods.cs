using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Threading = System.Threading;

namespace Dile.ExtensionMethods.ReaderWriterLockSlim
{
	public static class ExtensionMethods
	{
		public static void Read(this Threading.ReaderWriterLockSlim obj, Action operation)
		{
			obj.EnterReadLock();

			try
			{
				operation();
			}
			finally
			{
				obj.ExitReadLock();
			}
		}

		public static T Read<T>(this Threading.ReaderWriterLockSlim obj, Func<T> operation)
		{
			T result;
			obj.EnterReadLock();

			try
			{
				result = operation();
			}
			finally
			{
				obj.ExitReadLock();
			}

			return result;
		}

		public static void Write(this Threading.ReaderWriterLockSlim obj, Action operation)
		{
			obj.EnterWriteLock();

			try
			{
				operation();
			}
			finally
			{
				obj.ExitWriteLock();
			}
		}

		public static T Write<T>(this Threading.ReaderWriterLockSlim obj, Func<T> operation)
		{
			T result;
			obj.EnterWriteLock();

			try
			{
				result = operation();
			}
			finally
			{
				obj.ExitWriteLock();
			}

			return result;
		}
	}
}