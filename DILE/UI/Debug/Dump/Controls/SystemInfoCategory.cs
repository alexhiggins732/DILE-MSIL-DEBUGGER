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
	public partial class SystemInfoCategory : UserControl, ICategoryControl
	{
		private DumpSystemInfo SystemInfo
		{
			get;
			set;
		}

		public SystemInfoCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			txtModelNumber.Text = HelperFunctions.FormatNumber(SystemInfo.ModelNumber);
			txtStepping.Text = HelperFunctions.FormatNumber(SystemInfo.Stepping);
			txtNumberOfProcessors.Text = HelperFunctions.FormatNumber(SystemInfo.NumberOfProcessors);
			txtMajorVersion.Text = HelperFunctions.FormatNumber(SystemInfo.MajorVersion);
			txtMinorVersion.Text = HelperFunctions.FormatNumber(SystemInfo.MinorVersion);
			txtBuildNumber.Text = HelperFunctions.FormatNumber(SystemInfo.BuildNumber);
		}

		#region ICategoryControl Members
		public void Initialize(DumpDebugger dumpDebugger)
		{
			SystemInfo = dumpDebugger.GetSystemInfo();

			txtProcessorArchitecture.Text = Convert.ToString(SystemInfo.ProcessorArchitecture);
			txtProcessorLevel.Text = Convert.ToString(SystemInfo.ProcessorLevel);
			txtModelNumber.Text = HelperFunctions.FormatNumber(SystemInfo.ModelNumber);
			txtStepping.Text = HelperFunctions.FormatNumber(SystemInfo.Stepping);
			txtNumberOfProcessors.Text = HelperFunctions.FormatNumber(SystemInfo.NumberOfProcessors);
			txtOperationSystemType.Text = Convert.ToString(SystemInfo.OperatingSystemType);
			txtMajorVersion.Text = HelperFunctions.FormatNumber(SystemInfo.MajorVersion);
			txtMinorVersion.Text = HelperFunctions.FormatNumber(SystemInfo.MinorVersion);
			txtBuildNumber.Text = HelperFunctions.FormatNumber(SystemInfo.BuildNumber);
			txtPlatform.Text = Convert.ToString(SystemInfo.Platform);
			txtServicePack.Text = SystemInfo.ServicePack;
			txtOperatingSystemSuite.Text = Convert.ToString(SystemInfo.OperatingSystemSuite);
		}
		#endregion
	}
}
