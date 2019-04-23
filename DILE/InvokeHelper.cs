using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace Dile
{
	public static class InvokeHelper
	{
		public static void InvokeFormMethod(Control control, MethodInvoker invoker)
		{
			if (control.InvokeRequired)
			{
				if (!control.IsDisposed)
				{
					try
					{
						control.Invoke(invoker);
					}
					catch (InvalidOperationException)
					{
						if (!control.IsDisposed)
						{
							throw;
						}
					}
				}
			}
			else
			{
				invoker();
			}
		}

		public static void BeginInvokeFormMethod(Control control, MethodInvoker invoker)
		{
			if (control.InvokeRequired)
			{
				if (!control.IsDisposed)
				{
					try
					{
						control.BeginInvoke(invoker);
					}
					catch (InvalidOperationException)
					{
						if (!control.IsDisposed)
						{
							throw;
						}
					}
				}
			}
			else
			{
				invoker();
			}
		}
	}
}