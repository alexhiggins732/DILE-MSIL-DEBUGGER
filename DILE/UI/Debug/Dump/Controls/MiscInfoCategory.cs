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
	public partial class MiscInfoCategory : UserControl, ICategoryControl
	{
		private DumpMiscInfo MiscInfo
		{
			get;
			set;
		}

		public MiscInfoCategory()
		{
			InitializeComponent();

			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Settings_DisplayHexaNumbersChanged);
		}

		private void Settings_DisplayHexaNumbersChanged()
		{
			if (MiscInfo != null)
			{
				DisplayProcessId();
				DisplayProcessorInfo();
			}
		}

		private void DisplayProcessId()
		{
			if ((MiscInfo.Type & MiscInfoType.ProcessId) == MiscInfoType.ProcessId)
			{
				lblProcessIdNA.Visible = false;
				txtProcessId.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessId);
			}
			else
			{
				lblProcessIdNA.Visible = true;
			}
		}

		private void DisplayProcessTimes()
		{
			if ((MiscInfo.Type & MiscInfoType.ProcessTimes) == MiscInfoType.ProcessTimes)
			{
				lblProcessInfoNA.Visible = false;
				txtProcessCreateTime.Text = Convert.ToString(MiscInfo.ProcessCreateTime.ToLocalTime());
				txtProcessUserTime.Text = Convert.ToString(MiscInfo.ProcessUserTime);
				txtProcessKernelTime.Text = Convert.ToString(MiscInfo.ProcessKernelTime);
			}
			else
			{
				lblProcessInfoNA.Visible = true;
			}
		}

		private void DisplayProcessorInfo()
		{
			if ((MiscInfo.Type & MiscInfoType.ProcessorPowerInfo) == MiscInfoType.ProcessorPowerInfo)
			{
				lblProcessorInfoNA.Visible = false;
				txtProcessorMaxMhz.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessorMaxMhz);
				txtProcessorCurrentMhz.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessorCurrentMhz);
				txtProcessorMhzLimit.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessorMhzLimit);
				txtProcessorMaxIdleState.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessorMaxIdleState);
				txtProcessorCurrentIdleState.Text = HelperFunctions.FormatNumber(MiscInfo.ProcessorCurrentIdleState);
			}
			else
			{
				lblProcessorInfoNA.Visible = true;
			}
		}

		public void Initialize(DumpDebugger dumpDebugger)
		{
			MiscInfo = dumpDebugger.GetMiscInfo();

			DisplayProcessId();
			DisplayProcessTimes();
			DisplayProcessorInfo();
		}
	}
}