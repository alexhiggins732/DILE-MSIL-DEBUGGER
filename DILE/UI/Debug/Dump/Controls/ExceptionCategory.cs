using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug.Dump;

namespace Dile.UI.Debug.Dump.Controls
{
	public partial class ExceptionCategory : UserControl, ICategoryControl
	{
		private DumpExceptionInfo ExceptionInfo
		{
			get;
			set;
		}

		public ExceptionCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			if (ExceptionInfo != null)
			{
				txtRecord.Text = HelperFunctions.FormatNumber(ExceptionInfo.Record);
				txtAddress.Text = HelperFunctions.FormatNumber(ExceptionInfo.Address);
			}
		}

		#region ICategoryControl Members
		public void Initialize(DumpDebugger dumpDebugger)
		{
			ExceptionInfo = dumpDebugger.GetException();

			if (ExceptionInfo != null)
			{
				txtCode.Text = Convert.ToString(ExceptionInfo.Code);
				txtContinuable.Text = Convert.ToString(ExceptionInfo.Continuable);
				txtRecord.Text = HelperFunctions.FormatNumber(ExceptionInfo.Record);
				txtAddress.Text = HelperFunctions.FormatNumber(ExceptionInfo.Address);
				txtClrException.Text = Convert.ToString(ExceptionInfo.Record == 0xe0434f4d);
			}
		}
		#endregion
	}
}
