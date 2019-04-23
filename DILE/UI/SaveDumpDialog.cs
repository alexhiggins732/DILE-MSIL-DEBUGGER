using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Debug.Dump;
using System.Diagnostics;

namespace Dile.UI
{
	public partial class SaveDumpDialog : Form
	{
		public SaveDumpDialog(int defaultProcessId)
		{
			InitializeComponent();

			processIdSelector.Value = defaultProcessId;
		}

		private void InitializeDumpTypeListBox()
		{
			dumpTypeListBox.BeginUpdate();
			dumpTypeListBox.Items.Clear();
			foreach (DumpType dumpType in Enum.GetValues(typeof(DumpType)))
			{
				bool isChecked = false;

				switch(dumpType)
				{
					case DumpType.FilterMemory:
					case DumpType.FilterModulePaths:
					case DumpType.IgnoreInaccessibleMemory:
					case DumpType.Normal:
					case DumpType.ScanMemory:
					case DumpType.ValidTypeFlags:
					case DumpType.WithCodeSegs:
					case DumpType.WithFullAuxiliaryState:
					case DumpType.WithFullMemoryInfo:
					case DumpType.WithHandleData:
					case DumpType.WithIndirectlyReferencedMemory:
					case DumpType.WithPrivateReadWriteMemory:
					case DumpType.WithProcessThreadData:
					case DumpType.WithThreadInfo:
					case DumpType.WithUnloadedModules:
					case DumpType.WithoutAuxiliaryState:
					case DumpType.WithoutOptionalData:
					case DumpType.WithPrivateWriteCopyMemory:
					case DumpType.WithTokenInformation:
						break;

					case DumpType.WithDataSegs:
					case DumpType.WithFullMemory:
						isChecked = true;
						break;

					default:
						throw new NotSupportedException("The following dump type is not supported: " + dumpType);
				}

				dumpTypeListBox.Items.Add(dumpType, isChecked);
			}
			dumpTypeListBox.EndUpdate();
		}

		private void SaveDumpDialog_Load(object sender, EventArgs e)
		{
			InitializeDumpTypeListBox();
		}

		private void browseProcessButton_Click(object sender, EventArgs e)
		{
			AttachProcessDialog attachProcessDialog = new AttachProcessDialog(true);
			if (attachProcessDialog.ShowDialog() == DialogResult.OK)
			{
				processIdSelector.Value = attachProcessDialog.ProcessID;
			}
		}

		private void browseDumpFilePathButton_Click(object sender, EventArgs e)
		{
			saveDumpFileDialog.InitialDirectory = Settings.Instance.DefaultDumpFileDirectory ?? string.Empty;

			if (saveDumpFileDialog.ShowDialog() == DialogResult.OK)
			{
				dumpFilePathTextBox.Text = saveDumpFileDialog.FileName;
			}
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			DumpType dumpType = DumpType.Normal;
			foreach (DumpType checkedDumpType in dumpTypeListBox.CheckedItems)
			{
				dumpType |= checkedDumpType;
			}

			try
			{
				DumpFileWriter.WriteDumpFile(dumpFilePathTextBox.Text, Convert.ToInt32(processIdSelector.Value), dumpType);

				MessageBox.Show("The memory dump file has been successfully saved.");
			}
			catch(Exception exception)
			{
				MessageBox.Show("Failed to save the memory dump file for the following reason: " + Environment.NewLine + exception.Message);
				UIHandler.Instance.ShowException(exception);
			}

			DialogResult = DialogResult.OK;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void msdnLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://msdn.microsoft.com/en-us/library/ms680519(v=VS.85).aspx");
		}
	}
}