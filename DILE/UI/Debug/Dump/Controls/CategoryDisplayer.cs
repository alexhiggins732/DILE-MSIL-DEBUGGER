using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug.Dump;
using System.Windows.Forms;

namespace Dile.UI.Debug.Dump.Controls
{
	public class CategoryDisplayer<TControl> : ICategoryDisplayer
		where TControl : Control, ICategoryControl, new()
	{
		public string Name
		{
			get;
			private set;
		}

		public CategoryDisplayer(string name)
		{
			Name = name;
		}

		#region ICategoryDisplayer Members
		public Control CreateControl(DumpDebugger dumpDebugger)
		{
			TControl result = null;

			try
			{
				TControl control = new TControl();
				control.Initialize(dumpDebugger);
				result = control;
			}
			catch (Exception exception)
			{
				UIHandler.Instance.ShowException(exception);
				UIHandler.Instance.DisplayUserWarning(exception.Message);

				throw;
			}

			return result;
		}
		#endregion

		public override string ToString()
		{
			return Name;
		}
	}
}