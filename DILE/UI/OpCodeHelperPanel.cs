using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Metadata;
using System.Diagnostics;

namespace Dile.UI
{
	public partial class OpCodeHelperPanel : BasePanel
	{
		private const string MsdnUrl = "http://msdn.microsoft.com/en-us/library/system.reflection.emit.opcodes.{0}.aspx";

		public OpCodeHelperPanel()
		{
			InitializeComponent();
		}

		private void OpCodeHelperPanel_Load(object sender, EventArgs e)
		{
			cmbOpCodes.BeginUpdate();
			cmbOpCodes.Items.Clear();
			cmbOpCodes.Sorted = false;

			foreach (OpCodeItem opCodeItem in OpCodeGroups.OpCodeItemsByOpCode.Values)
			{
				cmbOpCodes.Items.Add(opCodeItem);
			}

			cmbOpCodes.Sorted = true;
			cmbOpCodes.EndUpdate();
		}

		private void cmbOpCodes_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmbOpCodes.SelectedIndex > -1)
			{
				OpCodeItem selectedItem = (OpCodeItem)cmbOpCodes.Items[cmbOpCodes.SelectedIndex];
				lblDescription.Text = selectedItem.Description;
				lblDescription.Visible = true;
				linkMsdn.Visible = true;
			}
			else
			{
				lblDescription.Visible = false;
				linkMsdn.Visible = false;
			}
		}

		private void linkMsdn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (cmbOpCodes.SelectedIndex > -1)
			{
				OpCodeItem selectedItem = (OpCodeItem)cmbOpCodes.Items[cmbOpCodes.SelectedIndex];
				Process.Start(string.Format(MsdnUrl, selectedItem.OpCodesFieldName));
			}
		}

		public void Display(OpCodeItem opCodeItem)
		{
			cmbOpCodes.SelectedItem = opCodeItem;
		}
	}
}