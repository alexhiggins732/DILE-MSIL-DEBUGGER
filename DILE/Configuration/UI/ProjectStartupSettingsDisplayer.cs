using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using System.Drawing;
using System.Windows.Forms;

namespace Dile.Configuration.UI
{
	public class ProjectStartupSettingsDisplayer : BaseSettingsDisplayer
	{
		private FolderBrowserDialog browseFolder;
		private FolderBrowserDialog BrowseFolder
		{
			get
			{
				if (browseFolder == null)
				{
					browseFolder = new FolderBrowserDialog();
					browseFolder.Description = "Choose a working directory...";
					browseFolder.ShowNewFolderButton = true;
				}

				return browseFolder;
			}
		}

		private OpenFileDialog browseProgram;
		private OpenFileDialog BrowseProgram
		{
			get
			{
				if (browseProgram == null)
				{
					browseProgram = new OpenFileDialog();
					browseProgram.AddExtension = true;
					browseProgram.CheckFileExists = true;
					browseProgram.CheckPathExists = true;
					browseProgram.Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*";
					browseProgram.Multiselect = false;
					browseProgram.ShowHelp = false;
					browseProgram.Title = "Choose a program...";
					browseProgram.ValidateNames = true;
				}

				return browseProgram;
			}
		}

		private RadioButton startAssembly;
		private RadioButton StartAssembly
		{
			get
			{
				return startAssembly;
			}
			set
			{
				startAssembly = value;
			}
		}

		private TextBox startAssemblyArguments;
		private TextBox StartAssemblyArguments
		{
			get
			{
				return startAssemblyArguments;
			}
			set
			{
				startAssemblyArguments = value;
			}
		}

		private TextBox startAssemblyWorkingDirectory;
		private TextBox StartAssemblyWorkingDirectory
		{
			get
			{
				return startAssemblyWorkingDirectory;
			}
			set
			{
				startAssemblyWorkingDirectory = value;
			}
		}

		private RadioButton startProgram;
		private RadioButton StartProgram
		{
			get
			{
				return startProgram;
			}
			set
			{
				startProgram = value;
			}
		}

		private TextBox startProgramExecutable;
		private TextBox StartProgramExecutable
		{
			get
			{
				return startProgramExecutable;
			}
			set
			{
				startProgramExecutable = value;
			}
		}

		private TextBox startProgramArguments;
		private TextBox StartProgramArguments
		{
			get
			{
				return startProgramArguments;
			}
			set
			{
				startProgramArguments = value;
			}
		}

		private TextBox startProgramWorkingDirectory;
		private TextBox StartProgramWorkingDirectory
		{
			get
			{
				return startProgramWorkingDirectory;
			}
			set
			{
				startProgramWorkingDirectory = value;
			}
		}

		#region ASP.NET Debugging
		//private RadioButton startBrowser;
		//private RadioButton StartBrowser
		//{
		//  get
		//  {
		//    return startBrowser;
		//  }
		//  set
		//  {
		//    startBrowser = value;
		//  }
		//}

		//private TextBox startBrowserUrl;
		//private TextBox StartBrowserUrl
		//{
		//  get
		//  {
		//    return startBrowserUrl;
		//  }
		//  set
		//  {
		//    startBrowserUrl = value;
		//  }
		//}

		//private CheckBox startBrowserAttachAspNet;
		//private CheckBox StartBrowserAttachAspNet
		//{
		//  get
		//  {
		//    return startBrowserAttachAspNet;
		//  }
		//  set
		//  {
		//    startBrowserAttachAspNet = value;
		//  }
		//}
		#endregion

		private RadioButton CreateRadioButton(TableLayoutPanel panel, Padding padding, string text, bool checkedState)
		{
			RadioButton result = new RadioButton();
			result.CheckAlign = ContentAlignment.MiddleLeft;
			result.Checked = checkedState;
			result.Dock = DockStyle.Fill;
			result.Padding = padding;
			result.Text = text;

			panel.Controls.Add(result);

			return result;
		}

		private CheckBox CreateCheckBox(TableLayoutPanel panel, Padding padding, string text, bool checkedState)
		{
			CheckBox result = new CheckBox();
			result.CheckAlign = ContentAlignment.MiddleLeft;
			result.Checked = checkedState;
			result.Dock = DockStyle.Fill;
			result.Padding = padding;
			result.Text = text;

			panel.Controls.Add(result);

			return result;
		}

		private TextBox CreateTextBox(TableLayoutPanel panel, Padding padding, string labelText, string text)
		{
			return CreateTextBox(panel, padding, labelText, text, null);
		}

		private TextBox CreateTextBox(TableLayoutPanel panel, Padding padding, string labelText, string text, Button browseButton)
		{
			Panel controlsPanel = new Panel();
			controlsPanel.Dock = DockStyle.Fill;

			if (browseButton != null)
			{
				controlsPanel.Height = browseButton.Height;
				controlsPanel.Controls.Add(browseButton);
			}

			TextBox result = new TextBox();
			result.Dock = DockStyle.Fill;
			result.Text = text;
			controlsPanel.Controls.Add(result);

			Label label = new Label();
			label.AutoSize = true;
			label.Dock = DockStyle.Left;
			label.Padding = padding;
			label.Text = labelText;
			controlsPanel.Controls.Add(label);

			if (browseButton == null)
			{
				controlsPanel.Height = Math.Max(result.Height, label.Height);
			}
			else
			{
				controlsPanel.Height = Math.Max(Math.Max(result.Height, label.Height), browseButton.Height);
			}

			panel.Controls.Add(controlsPanel);

			return result;
		}

		private Button CreateBrowseButton()
		{
			Button result = new Button();
			result.Dock = DockStyle.Right;
			result.Text = "Browse...";

			return result;
		}

		protected override void CreateControls(TableLayoutPanel panel)
		{
			panel.ColumnCount = 1;
			panel.RowCount = 11;

			panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

			Padding zeroPadding = new Padding(0, 0, 0, 0);
			Padding leftPadding = new Padding(20, 0, 0, 0);

			StartAssembly = CreateRadioButton(panel, zeroPadding, "Start selected assembly", (Project.Instance.StartMode == ProjectStartMode.StartAssembly));
			StartAssemblyArguments = CreateTextBox(panel, leftPadding, "Arguments:", Project.Instance.AssemblyArguments);
			Button browseAssemblyWorkingDirectory = CreateBrowseButton();
			StartAssemblyWorkingDirectory = CreateTextBox(panel, leftPadding, "Working directory:", Project.Instance.AssemblyWorkingDirectory, browseAssemblyWorkingDirectory);
			browseAssemblyWorkingDirectory.Tag = StartAssemblyWorkingDirectory;

			StartProgram = CreateRadioButton(panel, zeroPadding, "Start program", (Project.Instance.StartMode == ProjectStartMode.StartProgram));
			Button browseExecutable = CreateBrowseButton();
			StartProgramExecutable = CreateTextBox(panel, leftPadding, "Executable:", Project.Instance.ProgramExecutable, browseExecutable);
			browseExecutable.Tag = StartProgramExecutable;
			StartProgramArguments = CreateTextBox(panel, leftPadding, "Arguments:", Project.Instance.ProgramArguments);
			Button browseProgramWorkingDirectory = CreateBrowseButton();
			StartProgramWorkingDirectory = CreateTextBox(panel, leftPadding, "Working directory:", Project.Instance.ProgramWorkingDirectory, browseProgramWorkingDirectory);
			browseProgramWorkingDirectory.Tag = StartProgramWorkingDirectory;

			#region ASP.NET Debugging
			//StartBrowser = CreateRadioButton(panel, zeroPadding, "Start browser", (Project.Instance.StartMode == ProjectStartMode.StartBrowser));
			//StartBrowserUrl = CreateTextBox(panel, leftPadding, "URL:", Project.Instance.BrowserUrl);
			//StartBrowserAttachAspNet = CreateCheckBox(panel, leftPadding, "Auto-attach to the ASP.NET process", Project.Instance.AutoAttachToAspNet);
			#endregion

			browseAssemblyWorkingDirectory.Click += new EventHandler(browseWorkingDirectory_Click);
			browseProgramWorkingDirectory.Click += new EventHandler(browseWorkingDirectory_Click);
			browseExecutable.Click += new EventHandler(browseExecutable_Click);
		}

		private void browseExecutable_Click(object sender, EventArgs e)
		{
			Button browseButton = (Button)sender;
			TextBox executableTextBox = (TextBox)browseButton.Tag;
			BrowseProgram.FileName = executableTextBox.Text;

			if (BrowseProgram.ShowDialog() == DialogResult.OK)
			{
				executableTextBox.Text = BrowseProgram.FileName;
			}
		}

		private void browseWorkingDirectory_Click(object sender, EventArgs e)
		{
			Button browseButton = (Button)sender;
			TextBox directoryTextBox = (TextBox)browseButton.Tag;
			BrowseFolder.SelectedPath = directoryTextBox.Text;

			if (BrowseFolder.ShowDialog() == DialogResult.OK)
			{
				directoryTextBox.Text = BrowseFolder.SelectedPath;
			}
		}

		public override void ReadSettings()
		{
			if (StartAssembly.Checked)
			{
				Project.Instance.StartMode = ProjectStartMode.StartAssembly;
			}
			else if (StartProgram.Checked)
			{
				Project.Instance.StartMode = ProjectStartMode.StartProgram;
			}
			#region ASP.NET Debugging
			//else
			//{
			//  Project.Instance.StartMode = ProjectStartMode.StartBrowser;
			//}
			#endregion

			Project.Instance.AssemblyArguments = StartAssemblyArguments.Text;
			Project.Instance.AssemblyWorkingDirectory = StartAssemblyWorkingDirectory.Text;
			Project.Instance.ProgramExecutable = StartProgramExecutable.Text;
			Project.Instance.ProgramArguments = StartProgramArguments.Text;
			Project.Instance.ProgramWorkingDirectory = StartProgramWorkingDirectory.Text;
			#region ASP.NET Debugging
			//Project.Instance.BrowserUrl = StartBrowserUrl.Text;
			//Project.Instance.AutoAttachToAspNet = StartBrowserAttachAspNet.Checked;
			#endregion
		}

		public override string ToString()
		{
			return "Startup settings";
		}
	}
}