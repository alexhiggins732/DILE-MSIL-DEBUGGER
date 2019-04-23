using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;
using WeifenLuo.WinFormsUI;

namespace Dile.UI
{
	public partial class InformationPanel : BasePanel
	{
		private Stopwatch counter;
		public Stopwatch Counter
		{
			get
			{
				return counter;
			}
			set
			{
				counter = value;
			}
		}

		public InformationPanel()
		{
			InitializeComponent();
		}

		protected override bool IsDebugPanel()
		{
			return false;
		}

		public void ResetCounter()
		{
			Counter = Stopwatch.StartNew();
		}

		public void AddInformation(string information)
		{
			informations.AppendText(information);
			informations.Focus();
		}

		public void AddException(Exception exception)
		{
			informations.AppendText("\n");
			informations.AppendText("Exception occurred while loading the assembly: ");
			informations.AppendText(exception.ToString() + " ");
			informations.Focus();
		}

		public void AddElapsedTime()
		{
			if (Counter != null)
			{
				Counter.Stop();

				informations.AppendText("(elapsed time = ");
				informations.AppendText(Counter.Elapsed.ToString());
				informations.AppendText(")\n");
			}
		}

		protected override void OnClearPanel()
		{
			base.OnClearPanel();

			informations.Clear();
		}
	}
}