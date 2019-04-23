using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Dile.Configuration;
using Dile.Configuration.UI;
using Dile.Debug;
using Dile.Debug.Dump;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using WeifenLuo.WinFormsUI.Docking;

namespace Dile.UI
{
	partial class MainForm : Form
	{
		public event UpdateDebugInformationDelegate UpdateDebugInformation;

		private ProjectExplorer projectExplorer;
		private QuickSearchPanel quickSearchPanel;
		private OpCodeHelperPanel opCodeHelperPanel;
		private InformationPanel informationPanel;
		private DebugOutputPanel outputPanel;
		private CallStackPanel callStackPanel;
		private InformationPanel logMessagePanel;
		private ObjectsPanel localVariablesPanel;
		private ObjectsPanel argumentsPanel;
		private ObjectsPanel watchPanel;
		private ObjectsPanel autoObjectsPanel;
		private BreakpointsPanel breakpointsPanel;
		private ThreadsPanel threadsPanel;
		private ModulesPanel modulesPanel;
		private ObjectViewer objectViewer;
		private StartPage startPage;

		private Project projectToLoad;
		public Project ProjectToLoad
		{
			get
			{
				return projectToLoad;
			}
			set
			{
				projectToLoad = value;
			}
		}

		public AssemblyLoadRequest[] AssembliesToLoad
		{
			get;
			set;
		}

		public int? ProcessToAttachTo
		{
			get;
			set;
		}

		public ProjectExplorer ProjectExplorer
		{
			get
			{
				return projectExplorer;
			}
		}

		private Color readyLabelOriginalBackColor;
		private Color ReadyLabelOriginalBackColor
		{
			get
			{
				return readyLabelOriginalBackColor;
			}
			set
			{
				readyLabelOriginalBackColor = value;
			}
		}

		private Color readyLabelOriginalForeColor;
		private Color ReadyLabelOriginalForeColor
		{
			get
			{
				return readyLabelOriginalForeColor;
			}
			set
			{
				readyLabelOriginalForeColor = value;
			}
		}

		private List<CodeEditorForm> activeCodeEditors = new List<CodeEditorForm>();
		public List<CodeEditorForm> ActiveCodeEditors
		{
			get
			{
				return activeCodeEditors;
			}
			private set
			{
				activeCodeEditors = value;
			}
		}

		public IList<DocumentContent> ActiveDocuments
		{
			get;
			private set;
		}

		private NoArgumentsDelegate activateMethod;
		private NoArgumentsDelegate ActivateMethod
		{
			get
			{
				return activateMethod;
			}
			set
			{
				activateMethod = value;
			}
		}

		private ExtendedDialogResult loadUnloadedAssembly = ExtendedDialogResult.None;
		private ExtendedDialogResult LoadUnloadedAssembly
		{
			get
			{
				return loadUnloadedAssembly;
			}
			set
			{
				loadUnloadedAssembly = value;
			}
		}

		private DumpDebugger DumpDebugger
		{
			get;
			set;
		}

		private MemoryDumpInfoPage DumpInfoPage
		{
			get;
			set;
		}

		public MainForm()
		{
			ActiveDocuments = new List<DocumentContent>();

			InitializeComponent();

			MainForm_HandleCreated(null, null);
			ActivateMethod = new NoArgumentsDelegate(Activate);

			DebugEventHandler.Instance.StateChanged += new DebuggerStateChanged(DebugEventHandler_StateChanged);
		}

		private void AssignMenuFunctions()
		{
			//File menu
			newProjectMenu.Tag = new BaseMenuInformation(MenuFunction.NewProject);
			openProjectMenu.Tag = new BaseMenuInformation(MenuFunction.OpenProject);
			saveProjectMenu.Tag = new BaseMenuInformation(MenuFunction.SaveProject);
			saveProjectAsMenu.Tag = new BaseMenuInformation(MenuFunction.SaveProjectAs);
			settingsMenu.Tag = new BaseMenuInformation(MenuFunction.Settings);
			exitMenu.Tag = new BaseMenuInformation(MenuFunction.Exit);

			//Project menu
			projectPropertiesMenu.Tag = new BaseMenuInformation(MenuFunction.ProjectProperties);
			addAssemblyMenu.Tag = new BaseMenuInformation(MenuFunction.AddAssembly);
			removeAssemblyMenu.Tag = new BaseMenuInformation(MenuFunction.RemoveAssembly);
			openReferenceInProjectMenu.Tag = new BaseMenuInformation(MenuFunction.OpenReferenceInProject);

			//Debug menu
			attachToProcessMenu.Tag = new BaseMenuInformation(MenuFunction.AttachToProcess);
			runDebuggeeMenu.Tag = new BaseMenuInformation(MenuFunction.RunDebuggee);
			pauseDebuggeeMenu.Tag = new BaseMenuInformation(MenuFunction.PauseDebuggee);
			stopDebuggeeMenu.Tag = new BaseMenuInformation(MenuFunction.StopDebuggee);
			detachMenu.Tag = new BaseMenuInformation(MenuFunction.Detach);
			runToCursorMenu.Tag = new BaseMenuInformation(MenuFunction.RunToCursor);
			stepMenu.Tag = new BaseMenuInformation(MenuFunction.Step);
			stepIntoMenu.Tag = new BaseMenuInformation(MenuFunction.StepInto);
			stepOutMenu.Tag = new BaseMenuInformation(MenuFunction.StepOut);
			objectViewerMenu.Tag = new BaseMenuInformation(MenuFunction.ObjectViewer);

			//View menu
			wordWrapMenu.Tag = new BaseMenuInformation(MenuFunction.WordWrap);
			startPageMenu.Tag = new BaseMenuInformation(MenuFunction.StartPage);

			//Windows menu
			closeAllWindowsMenu.Tag = new BaseMenuInformation(MenuFunction.CloseAllWindows);

			//Help menu
			aboutMenu.Tag = new BaseMenuInformation(MenuFunction.About);
		}

		private void CreatePanelsMenu()
		{
			panelsMenu.MenuItems.Clear();

			foreach (PanelDisplayer panelDisplayer in Settings.Instance.Panels)
			{
				MenuItem menuItem = new MenuItem();

				if (panelDisplayer.MenuFunction == MenuFunction.ProjectExplorerPanel)
				{
					menuItem.Text = panelDisplayer.Panel.Text.Substring(0, panelDisplayer.Panel.Text.IndexOf(" -"));
				}
				else
				{
					menuItem.Text = panelDisplayer.Panel.Text;
				}

				menuItem.Tag = panelDisplayer;
				menuItem.Checked = (panelDisplayer.PanelVisible);
				menuItem.Click += new EventHandler(panelMenuItem_Click);
				panelDisplayer.Panel.MenuItem = menuItem;

				panelsMenu.MenuItems.Add(menuItem);
			}
		}

		private void CreatePanelList()
		{
			Dictionary<MenuFunction, BasePanel> createdPanels = new Dictionary<MenuFunction, BasePanel>();
			createdPanels.Add(MenuFunction.ArgumentsPanel, argumentsPanel);
			createdPanels.Add(MenuFunction.AutoObjectsPanel, autoObjectsPanel);
			createdPanels.Add(MenuFunction.BreakpointsPanel, breakpointsPanel);
			createdPanels.Add(MenuFunction.CallStackPanel, callStackPanel);
			createdPanels.Add(MenuFunction.DebugOutputPanel, outputPanel);
			createdPanels.Add(MenuFunction.InformationPanel, informationPanel);
			createdPanels.Add(MenuFunction.LocalVariablesPanel, localVariablesPanel);
			createdPanels.Add(MenuFunction.LogMessagePanel, logMessagePanel);
			createdPanels.Add(MenuFunction.ModulesPanel, modulesPanel);
			createdPanels.Add(MenuFunction.ProjectExplorerPanel, ProjectExplorer);
			createdPanels.Add(MenuFunction.QuickSearchPanel, quickSearchPanel);
			createdPanels.Add(MenuFunction.OpCodeHelperPanel, opCodeHelperPanel);
			createdPanels.Add(MenuFunction.ThreadsPanel, threadsPanel);

			foreach (PanelDisplayer panelDisplayer in Settings.Instance.Panels)
			{
				if (createdPanels.ContainsKey(panelDisplayer.MenuFunction))
				{
					BasePanel panel = createdPanels[panelDisplayer.MenuFunction];
					panelDisplayer.Panel = panel;
					panel.Visible = panelDisplayer.PanelVisible;

					if (panelDisplayer.SerializableFont != null)
					{
						panel.Font = panelDisplayer.Font;
					}
					else
					{
						panelDisplayer.Font = panel.Font;
					}

					createdPanels.Remove(panelDisplayer.MenuFunction);
				}
			}

			if (Settings.Instance.CodeEditorFont == null)
			{
				Settings.Instance.CodeEditorFont = new CodeEditorFontSettings();
				Settings.Instance.CodeEditorFont.SerializableFont = Settings.DefaultCodeEditorFont;
			}

			Settings.Instance.CodeEditorFont.DefaultFont = Settings.DefaultCodeEditorFont;
			Settings.Instance.CodeEditorFont.Title = "Code editor";

			foreach (MenuFunction remainingPanelType in createdPanels.Keys)
			{
				BasePanel panel = createdPanels[remainingPanelType];
				PanelDisplayer panelDisplayer = new PanelDisplayer();

				panelDisplayer.Font = panel.Font;
				panelDisplayer.Panel = panel;
				panelDisplayer.MenuFunction = remainingPanelType;
				panelDisplayer.PanelVisible = true;
				Settings.Instance.Panels.Add(panelDisplayer);
			}

			CreatePanelsMenu();
		}

		private void MainForm_HandleCreated(object sender, EventArgs e)
		{
			projectExplorer = new ProjectExplorer();
			UIHandler.Instance.Initialize(this, dockPanel);
			CreatePanels();

			Project.ProjectChanged += new EventHandler(Project_ProjectChanged);
			Project.ProjectIsSavedChanged += new EventHandler(Project_ProjectIsSavedChanged);
			displayHexaNumbersButton.Checked = Settings.Instance.DisplayHexaNumbers;
			Settings.Instance.DisplayHexaNumbersChanged += new NoArgumentsDelegate(Instance_DisplayHexaNumbersChanged);
			ReadyLabelOriginalBackColor = readyLabel.BackColor;
			ReadyLabelOriginalForeColor = readyLabel.ForeColor;

			Settings.Instance.RecentAssembliesChanged += new NoArgumentsDelegate(RecentAssembliesChanged);
			Settings.Instance.RecentProjectsChanged += new NoArgumentsDelegate(RecentProjectsChanged);
			Settings.Instance.RecentDumpFilesChanged += new NoArgumentsDelegate(RecentDumpFilesChanged);
			RecentAssembliesChanged();
			RecentProjectsChanged();
			RecentDumpFilesChanged();
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			ProjectExplorer.Show(dockPanel, DockState.DockRight);
			quickSearchPanel.Show(dockPanel, DockState.DockRight);
			opCodeHelperPanel.Show(dockPanel, DockState.DockRight);

			informationPanel.Show(dockPanel, DockState.DockBottom);
			informationPanel.BringToFront();
			outputPanel.Show(dockPanel, DockState.DockBottom);
			outputPanel.BringToFront();

			logMessagePanel.Text = "Log Message Panel";
			logMessagePanel.TabText = "Log Message Panel";
			logMessagePanel.Show(dockPanel, DockState.DockBottom);
			logMessagePanel.BringToFront();

			threadsPanel.Show(dockPanel, DockState.DockBottom);
			modulesPanel.Show(dockPanel, DockState.DockBottom);
			callStackPanel.Show(dockPanel, DockState.DockBottom);
			breakpointsPanel.Show(dockPanel, DockState.DockBottom);

			localVariablesPanel.Mode = ObjectsPanelMode.LocalVariables;
			localVariablesPanel.Text = "Local Variables Panel";
			localVariablesPanel.TabText = localVariablesPanel.Text;
			localVariablesPanel.Show(dockPanel, DockState.DockBottom);

			argumentsPanel.Mode = ObjectsPanelMode.Arguments;
			argumentsPanel.Text = "Arguments Panel";
			argumentsPanel.TabText = argumentsPanel.Text;
			argumentsPanel.Show(dockPanel, DockState.DockBottom);

			autoObjectsPanel.Mode = ObjectsPanelMode.AutoObjects;
			autoObjectsPanel.Text = "Auto Objects Panel";
			autoObjectsPanel.TabText = autoObjectsPanel.Text;
			autoObjectsPanel.Show(dockPanel, DockState.DockBottom);

			watchPanel.Mode = ObjectsPanelMode.Watch;
			watchPanel.Text = "Watch Panel";
			watchPanel.TabText = watchPanel.Text;
			watchPanel.Show(dockPanel, DockState.DockBottom);

			informationPanel.Activate();
			ProjectExplorer.Activate();

			dockPanel.DockWindows[DockState.DockRight].SendToBack();

			Project.Instance = new Project();
			Project.Instance.IsSaved = true;
			Project.Instance.Name = "New project";

			ProjectExplorer.AddAssemblyDelegate = new NoArgumentsDelegate(AddAssembly);
			ProjectExplorer.ShowProject();

			CreatePanelList();
			AssignMenuFunctions();
			Settings.Instance.UpdateShortcuts(mainMenu);

			if (ProjectToLoad != null)
			{
				if (ProjectToLoad.FullPath == null || ProjectToLoad.FullPath.Length == 0)
				{
					AddAssembly(AssembliesToLoad);
					Settings.Instance.AddAssemblies(AssembliesToLoad.Select(requestedAssembly => requestedAssembly.FilePath)
						.ToArray());
					AssembliesToLoad = null;
				}
				else
				{
					OpenProject(ProjectToLoad.FullPath, true);
				}

				if (ProcessToAttachTo != null)
				{
					AttachToProcess(ProcessToAttachTo.Value);
				}
			}

			if (Settings.Instance.StartPageEnabled)
			{
				startPage = new StartPage();
				startPage.Show(dockPanel, DockState.Document);
			}
		}

		private void CreatePanels()
		{
			quickSearchPanel = new QuickSearchPanel();
			opCodeHelperPanel = new OpCodeHelperPanel();
			informationPanel = new InformationPanel();
			outputPanel = new DebugOutputPanel();
			callStackPanel = new CallStackPanel();
			logMessagePanel = new InformationPanel();
			localVariablesPanel = new ObjectsPanel();
			argumentsPanel = new ObjectsPanel();
			watchPanel = new ObjectsPanel();
			autoObjectsPanel = new ObjectsPanel();
			breakpointsPanel = new BreakpointsPanel();
			threadsPanel = new ThreadsPanel();
			modulesPanel = new ModulesPanel();
		}

		private void Project_ProjectChanged(object sender, EventArgs e)
		{
			ProjectExplorer.ShowProject();
			ClearLogMessagePanel();
			CloseCodeEditors();
			UpdateFormTitle();
		}

		private void UpdateFormTitle()
		{
			StringBuilder textBuilder = new StringBuilder("DILE (Dotnet IL Editor) v");
			textBuilder.Append(Settings.Instance.VersionNumber);

			if (Project.Instance != null)
			{
				textBuilder.Append(" - ");
				textBuilder.Append(Project.Instance.Name);

				textBuilder.Append(" [");
				textBuilder.Append(Project.Instance.FullPath);
				textBuilder.Append("]");

				if (!Project.Instance.IsSaved)
				{
					textBuilder.Append("*");
				}
			}

			Text = textBuilder.ToString();
		}

		private void Project_ProjectIsSavedChanged(object sender, EventArgs e)
		{
			UpdateFormTitle();
			ShowDebuggerState(DebugEventHandler.Instance.State);
		}

		private void Instance_DisplayHexaNumbersChanged()
		{
			displayHexaNumbersButton.Checked = Settings.Instance.DisplayHexaNumbers;
		}

		private void FillRecentMenu(MenuItem menuItem, List<string> recentItems)
		{
			menuItem.MenuItems.Clear();

			for (int index = 0; index < recentItems.Count; index++)
			{
				menuItem.MenuItems.Add(recentItems[index], new EventHandler(RecentMenuItem_Clicked));
			}
		}

		private void RecentMenuItem_Clicked(object sender, EventArgs e)
		{
			MenuItem menuItem = sender as MenuItem;

			if (menuItem != null)
			{
				if (menuItem.Parent == recentAssembliesMenu)
				{
					UIHandler.Instance.AddAssembly(menuItem.Text);
					Settings.Instance.MoveAssemblyToFirst(menuItem.Text);
				}
				else if (menuItem.Parent == recentProjectsMenu)
				{
					if (!WarningToSaveProject())
					{
						OpenProject(menuItem.Text, false);
						Settings.Instance.MoveProjectToFirst(menuItem.Text);
					}
				}
				else if (menuItem.Parent == recentDumpFilesMenu)
				{
					OpenDumpFile(menuItem.Text);
					Settings.Instance.MoveDumpFileToFirst(menuItem.Text);
				}
			}
		}

		private void RecentProjectsChanged()
		{
			FillRecentMenu(recentProjectsMenu, Settings.Instance.RecentProjects);
		}

		private void RecentAssembliesChanged()
		{
			FillRecentMenu(recentAssembliesMenu, Settings.Instance.RecentAssemblies);
		}

		private void RecentDumpFilesChanged()
		{
			FillRecentMenu(recentDumpFilesMenu, Settings.Instance.RecentDumpFiles);
		}

		public void RaiseUpdateDebugInformation(FrameRefresher activeFrameRefresher, FrameWrapper activeFrame, DebuggerState newState)
		{
			Activate();
			if (UpdateDebugInformation != null)
			{
				UpdateDebugInformation(activeFrameRefresher, activeFrame);
			}

			ShowDebuggerState(DebugEventHandler.Instance.State);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (!e.IsTerminating)
			{
				Exception thrownException = e.ExceptionObject as Exception;

				if (thrownException != null)
				{
					UIHandler.Instance.ShowException(thrownException);
				}
			}
		}

		private void exitMenu_Click(object sender, EventArgs e)
		{
			Close();
		}

		public void ShowObjectInObjectViewer(FrameWrapper frame, BaseValueRefresher valueRefresher, string initialExpression)
		{
			if (objectViewer == null)
			{
				objectViewer = new ObjectViewer();
			}

			objectViewer.Initialize();
			objectViewer.ShowValue(valueRefresher, frame, initialExpression);
		}

		public void AssembliesLoaded(List<Assembly> loadedAssemblies, bool isProjectChanged)
		{
			informationPanel.AddElapsedTime();
			readyLabel.Text = "Displaying loaded assemblies in the Project explorer... ";
			informationPanel.AddInformation(readyLabel.Text);
			informationPanel.ResetCounter();
			Application.DoEvents();

			if (ProjectToLoad != null)
			{
				ProjectToLoad.Assemblies = loadedAssemblies;
				ProjectToLoad.IsSaved = true;
				Project.Instance = ProjectToLoad;
				ProjectExplorer.ShowProject();
				breakpointsPanel.DisplayBreakpoints();
				outputPanel.ClearPanel();
			}
			else
			{
				for (int index = 0; index < loadedAssemblies.Count; index++)
				{
					Assembly loadedAssembly = loadedAssemblies[index];

					ProjectExplorer.AddAssemblyToProject(loadedAssembly);
					Settings.Instance.AddAssembly(loadedAssembly.FullPath);
				}
			}

			ProjectToLoad = null;
			progressBar.Visible = false;
			informationPanel.AddElapsedTime();
			ClearUserWarning();
			informationPanel.AddInformation(readyLabel.Text);
			informationPanel.AddInformation("\n\n");
			ResetPanels();

			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
			{
				ShowDebuggeeStoppedState();
			}
			else
			{
				ShowDebuggerState(DebugEventHandler.Instance.State);
				ClearDebugPanels(false);
				DebugEventHandler.Instance.DisplayCurrentCodeLocation();
			}

			GC.Collect(GC.MaxGeneration);

			if (isProjectChanged)
			{
				Project.Instance.IsSaved = false;
			}
		}

		public void ShowMessageBox(string caption, string text)
		{
			MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		public void ShowAssemblyMissingWarning(string assemblyPath)
		{
			if (!Project.Instance.IsAssemblyLoaded(assemblyPath))
			{
				bool loadAssembly = false;

				if (LoadUnloadedAssembly == ExtendedDialogResult.YesToAll)
				{
					loadAssembly = true;
				}
				else if (LoadUnloadedAssembly != ExtendedDialogResult.NoToAll)
				{
					Activate();
					string message = string.Format("The following assembly is not loaded: {0}\r\n\r\nWould you like to load it now?\r\n\r\nNote: if an assembly is not loaded in DILE then during the debugging you might not see correct call stack/object values as DILE will have not have enough information about the debuggee.", assemblyPath);

					ExtendedMessageBox messageBox = new ExtendedMessageBox();
					messageBox.ShowMessage("DILE - Load assembly?", message);
					LoadUnloadedAssembly = messageBox.ExtendedDialogResult;

					if (LoadUnloadedAssembly == ExtendedDialogResult.Yes || LoadUnloadedAssembly == ExtendedDialogResult.YesToAll)
					{
						loadAssembly = true;
					}
				}

				if (loadAssembly)
				{
					UIHandler.Instance.AddAssembly(assemblyPath);
				}
			}
		}

		public void AddBreakpoint(BreakpointInformation breakpoint)
		{
			breakpointsPanel.AddBreakpoint(breakpoint);
		}

		public void RemoveBreakpoint(BreakpointInformation breakpoint)
		{
			breakpointsPanel.RemoveBreakpoint(breakpoint);
		}

		public void DeactivateBreakpoint(BreakpointInformation breakpoint)
		{
			breakpointsPanel.DeactivateBreakpoint(breakpoint);
		}

		public void ClearSpecialLines()
		{
			foreach (CodeDisplayer codeDisplayer in ProjectExplorer.CodeDisplayers)
			{
				codeDisplayer.ClearSpecialLines();
			}
		}

		public void ClearCodeDisplayers(bool refreshDisplayers)
		{
			foreach (CodeDisplayer codeDisplayer in ProjectExplorer.CodeDisplayers)
			{
				codeDisplayer.ClearCurrentLine();
				codeDisplayer.ClearSpecialLines();

				if (refreshDisplayers)
				{
					codeDisplayer.Refresh();
				}
			}
		}

		public void AddModulesToPanel(ModuleWrapper[] modules)
		{
			modulesPanel.AddModules(modules);
		}

		public void ThreadChangedUpdate(ThreadWrapper thread)
		{
			DebugEventHandler.Instance.EventObjects.Thread = thread;
			DebugEventHandler.Instance.ChangeThread(thread);
		}

		public void FrameChangedUpdate(FrameRefresher frameRefresher, FrameWrapper frame)
		{
			DebugEventHandler.Instance.EventObjects.Frame = frame;
			DebugEventHandler.Instance.ChangeFrame(frameRefresher, DebugEventHandler.Instance.EventObjects.Frame);
		}

		public void ShowException(Exception exception)
		{
			informationPanel.ResetCounter();
			informationPanel.AddException(exception);
			DisplayUserWarning("Exception occurred in DILE.");
		}

		public void ResetProgressBar()
		{
			progressBar.Value = 0;
			informationPanel.Counter = null;
		}

		public void StepProgressBar(int incrementValue)
		{
			progressBar.Increment(incrementValue);
		}

		public void SetProgressBarMaximum(int maximum)
		{
			progressBar.Maximum = maximum;
		}

		public void SetProgressText(string text, bool addElapsedTime)
		{
			if (addElapsedTime)
			{
				informationPanel.AddElapsedTime();
				readyLabel.Text = text;
			}

			informationPanel.ResetCounter();
			informationPanel.AddInformation(text);
		}

		public void SetProgressBarVisible(bool visible)
		{
			progressBar.Visible = visible;
		}

		public void DisplayOutputInformation(DebugEventDescriptor debugEventDescriptor)
		{
			outputPanel.AddEvent(debugEventDescriptor);
		}

		public void ClearOutputPanel()
		{
			outputPanel.ClearPanel();
		}

		public void DisplayLogMessage(string logMessage)
		{
			logMessagePanel.AddInformation(logMessage);
		}

		public void ClearLogMessagePanel()
		{
			logMessagePanel.ClearPanel();
		}

		public void ClearDebugPanels(bool leaveThreads)
		{
			ClearSpecialLines();
			ClearUserWarning();
		}

		public void ClearUserWarning()
		{
			readyLabel.Text = "Ready";
		}

		public void DisplayUserWarning(string text)
		{
			readyLabel.Text = text;
		}

		private void AddAssembly()
		{
			if (Settings.Instance.DefaultAssemblyDirectory != null && Settings.Instance.DefaultAssemblyDirectory.Length > 0)
			{
				openFileDialog.InitialDirectory = Settings.Instance.DefaultAssemblyDirectory;
			}
			else
			{
				openFileDialog.InitialDirectory = string.Empty;
			}

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				AssemblyLoadRequest[] requestedAssemblies = openFileDialog.FileNames.Select(fileName => new AssemblyLoadRequest(fileName))
					.ToArray();

				AddAssembly(requestedAssemblies);
				Settings.Instance.AddAssemblies(openFileDialog.FileNames);
			}
		}

		public void AddAssembly(AssemblyLoadRequest[] requestedAssemblies)
		{
			ClearUserWarning();
			readyLabel.Text = string.Empty;
			progressBar.Visible = true;
			AssemblyLoader.Instance.StartAsync(requestedAssemblies);
		}

		private void SaveProject()
		{
			bool saveProject = true;
			string projectFullPath = string.Empty;

			if (Project.Instance.FullPath != null && Project.Instance.FullPath.Length > 0)
			{
				projectFullPath = Project.Instance.FullPath;
			}
			else
			{
				if (Settings.Instance.DefaultProjectDirectory != null && Settings.Instance.DefaultProjectDirectory.Length > 0)
				{
					saveProjectDialog.InitialDirectory = Settings.Instance.DefaultProjectDirectory;
				}
				else
				{
					saveProjectDialog.InitialDirectory = string.Empty;
				}

				saveProject = (saveProjectDialog.ShowDialog() == DialogResult.OK);

				if (saveProject)
				{
					projectFullPath = saveProjectDialog.FileName;
					Settings.Instance.AddProject(projectFullPath);
				}
			}

			if (saveProject)
			{
				Project.Instance.Save(projectFullPath);
			}
		}

		private void CloseCodeEditors()
		{
			if (ActiveCodeEditors != null)
			{
				while (ActiveCodeEditors.Count != 0)
				{
					CodeEditorForm codeEditorForm = ActiveCodeEditors[0];

					codeEditorForm.Close();
					codeEditorForm.Dispose();
				}
			}
		}

		public void CloseDynamicModuleDocuments()
		{
			int index = 0;

			while (index < ActiveCodeEditors.Count)
			{
				CodeEditorForm codeEditorForm = ActiveCodeEditors[index];

				if (codeEditorForm.CodeObject.LoadedFromMemory)
				{
					codeEditorForm.Close();
					codeEditorForm.Dispose();
				}
				else
				{
					index++;
				}
			}
		}

		private void addAssemblyMenu_Click(object sender, EventArgs e)
		{
			AddAssembly();
		}

		private void viewWordWrap_Click(object sender, EventArgs e)
		{
			wordWrapMenu.Checked = !wordWrapMenu.Checked;

			for (int index = 0; index < dockPanel.Panes.Count; index++)
			{
				DockPane pane = dockPanel.Panes[index];
				CodeEditorForm codeEditor = pane.ActiveContent as CodeEditorForm;

				if (codeEditor != null)
				{
					codeEditor.SetWordWrap(wordWrapMenu.Checked);
				}
			}

			ProjectExplorer.WordWrap = wordWrapMenu.Checked;
		}

		private void saveProjectMenu_Click(object sender, EventArgs e)
		{
			SaveProject();
		}

		private void openProjectMenu_Click(object sender, EventArgs e)
		{
			if (!WarningToSaveProject())
			{
				if (Settings.Instance.DefaultProjectDirectory != null && Settings.Instance.DefaultProjectDirectory.Length > 0)
				{
					openProjectDialog.InitialDirectory = Settings.Instance.DefaultProjectDirectory;
				}
				else
				{
					openProjectDialog.InitialDirectory = string.Empty;
				}

				if (openProjectDialog.ShowDialog() == DialogResult.OK)
				{
					CloseCodeEditors();
					ClearOutputPanel();
					OpenProject(openProjectDialog.FileName, true);
				}
			}
		}

		private bool IsProjectFile(string fileName)
		{
			return fileName.EndsWith(".dileproj");
		}

		private bool IsAssemblyFile(string fileName)
		{
			return (fileName.EndsWith(".exe") || fileName.EndsWith(".dll"));
		}

		private bool IsDumpFile(string fileName)
		{
			return fileName.EndsWith(".dmp");
		}

		public void OpenFiles(StringCollection fileNames)
		{
			int index = 0;
			bool foundProjectFile = false;
			List<AssemblyLoadRequest> requestedAssemblies = new List<AssemblyLoadRequest>();

			while (!foundProjectFile && index < fileNames.Count)
			{
				string fileName = fileNames[index++].ToLowerInvariant();

				if (IsProjectFile(fileName))
				{
					OpenProject(fileName, true);
					foundProjectFile = true;
				}
				else if (IsAssemblyFile(fileName))
				{
					requestedAssemblies.Add(new AssemblyLoadRequest(fileName));
				}
				else if (IsDumpFile(fileName))
				{
					OpenDumpFile(fileName);
				}
			}

			if (!foundProjectFile && requestedAssemblies.Count > 0)
			{
				UIHandler.Instance.AddAssembly(requestedAssemblies.ToArray());
			}
		}

		public void OpenProject(string projectFilePath, bool addToConfiguration)
		{
			try
			{
				using (FileStream projectFile = new FileStream(projectFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Project));
					Project project = (Project)serializer.Deserialize(projectFile);

					string[] fileNames = new string[project.Assemblies.Count];

					for (int index = 0; index < project.Assemblies.Count; index++)
					{
						fileNames[index] = project.Assemblies[index].FullPath;
					}

					project.FullPath = projectFilePath;
					ProjectToLoad = project;

					UIHandler.Instance.AddAssembly(fileNames.Select(fileName => new AssemblyLoadRequest(fileName))
						.ToArray());

					if (addToConfiguration)
					{
						Settings.Instance.AddProject(projectFilePath);
					}
				}
			}
			catch (InvalidOperationException invalidOperationException)
			{
				if (invalidOperationException.InnerException is XmlException)
				{
					informationPanel.AddInformation(string.Format("'{0}' is not a valid xml file.\n", projectFilePath));
				}
				else
				{
					informationPanel.AddInformation(string.Format("Could not load '{0}' file. (exception message: '{1}')\n", projectFilePath, invalidOperationException.Message));
				}

				ProjectToLoad = null;
			}
			catch (Exception exception)
			{
				informationPanel.AddInformation(string.Format("Could not load '{0}' file. (exception message: '{1}')\n", projectFilePath, exception.Message));

				ProjectToLoad = null;
			}
		}

		private void newProjectMenu_Click(object sender, EventArgs e)
		{
			if (!WarningToSaveProject())
			{
				ProjectProperties properties = new ProjectProperties();
				Project.Instance = new Project();

				properties.DisplaySettings();

				ClearOutputPanel();
				breakpointsPanel.ForceClearPanel();
				ProjectExplorer.ShowProject();
				ResetPanels();
				ShowDebuggerState(DebuggerState.DebuggeeStopped);
			}
		}

		private void ResetPanels()
		{
			threadsPanel.Reset();
		}

		private void saveProjectAsMenu_Click(object sender, EventArgs e)
		{
			if (Settings.Instance.DefaultProjectDirectory != null && Settings.Instance.DefaultProjectDirectory.Length > 0)
			{
				saveProjectDialog.InitialDirectory = Settings.Instance.DefaultProjectDirectory;
			}
			else
			{
				saveProjectDialog.InitialDirectory = string.Empty;
			}

			if (saveProjectDialog.ShowDialog() == DialogResult.OK)
			{
				using (FileStream projectFile = new FileStream(saveProjectDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					XmlSerializer serializer = new XmlSerializer(typeof(Project));
					serializer.Serialize(projectFile, Project.Instance);
					Project.Instance.FullPath = saveProjectDialog.FileName;
				}
			}
		}

		private void ShowMenuItems(bool removeAssembly, bool openReferenceInProject)
		{
			removeAssemblyMenu.Enabled = removeAssembly;
			openReferenceInProjectMenu.Enabled = openReferenceInProject;
		}

		private void ShowWindow(DockContent window, MenuItem menuItem)
		{
			menuItem.Checked = !menuItem.Checked;

			if (menuItem.Checked)
			{
				window.Show(dockPanel);
			}
			else
			{
				window.Hide();
			}
		}

		private void panelMenuItem_Click(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;
			PanelDisplayer panelDisplayer = (PanelDisplayer)menuItem.Tag;

			ShowWindow(panelDisplayer.Panel, menuItem);
			Settings.SaveConfiguration();
		}

		private void projectMenu_Popup(object sender, EventArgs e)
		{
			TreeNode selectedNode = ProjectExplorer.ProjectElements.SelectedNode;
			bool removeAssembly = false;
			bool openReferenceInProject = false;

			if (selectedNode != null)
			{
				if (selectedNode.Tag is AssemblyReference)
				{
					openReferenceInProject = true;
				}
				else if (selectedNode.Nodes.Count > 0 && selectedNode.Nodes[0].Tag is Assembly)
				{
					removeAssembly = true;
				}
			}

			ShowMenuItems(removeAssembly, openReferenceInProject);
		}

		private void projectProperties_Click(object sender, EventArgs e)
		{
			ProjectProperties properties = new ProjectProperties();
			if (properties.DisplaySettings() == DialogResult.OK)
			{
				Project.Instance.IsSaved = false;
				ShowDebuggerState(DebuggerState.DebuggeeStopped);
			}

			ProjectExplorer.ProjectElements.Nodes[0].Text = Project.Instance.Name;
		}

		private void removeAssemblyMenu_Click(object sender, EventArgs e)
		{
			if (ProjectExplorer.ProjectElements.SelectedNode != null && ProjectExplorer.ProjectElements.SelectedNode.Nodes.Count > 0 && ProjectExplorer.ProjectElements.SelectedNode.Nodes[0].Tag is Assembly)
			{
				Assembly assembly = (Assembly)ProjectExplorer.ProjectElements.SelectedNode.Nodes[0].Tag;

				Project.Instance.Assemblies.Remove(assembly);
				Project.Instance.IsSaved = false;
				ProjectExplorer.ProjectElements.Nodes.Remove(ProjectExplorer.ProjectElements.SelectedNode);
			}
		}

		private void openReferenceInProjectMenu_Click(object sender, EventArgs e)
		{
			if (ProjectExplorer.ProjectElements.SelectedNode != null && ProjectExplorer.ProjectElements.SelectedNode.Tag is AssemblyReference)
			{
				AssemblyReference assemblyReference = (AssemblyReference)ProjectExplorer.ProjectElements.SelectedNode.Tag;

				UIHandler.Instance.AddAssembly(assemblyReference.FullPath);
			}
		}

		private void aboutMenu_Click(object sender, EventArgs e)
		{
			AboutBox aboutBox = new AboutBox();

			aboutBox.ShowDialog();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = WarningToSaveProject();

			if (!e.Cancel && Settings.Instance.DetachOnQuit)
			{
				DetachFromDebuggee();
				AssemblyLoader.Instance.ShutDown();
			}
		}

		private bool WarningToSaveProject()
		{
			bool result = false;

			if (!Project.Instance.IsSaved)
			{
				DialogResult dialogResult = MessageBox.Show("Do you want to save the changes of the project?", "Dotnet IL Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);

				switch (dialogResult)
				{
					case DialogResult.Yes:
						SaveProject();
						break;

					case DialogResult.Cancel:
						result = true;
						break;
				}
			}

			return result;
		}

		private void AttachToProcess(int processId)
		{
			try
			{
				quickSearchPanel.ClearPanel();
				ClearCodeDisplayers(true);
				BreakpointHandler.Instance.DeleteRemovedBreakpoints();
				System.Diagnostics.Process debuggeeProcess = System.Diagnostics.Process.GetProcessById(processId);

				using (MetaHost metaHost = new MetaHost())
				{
					RuntimeInfo[] runtimeInfos = metaHost.EnumerateLoadedRuntimes(debuggeeProcess.Handle);
					RuntimeInfo latestRuntimeInfo = FindLatestRuntimeInfo(runtimeInfos);

					DebugEventHandler.Instance.Debugger = latestRuntimeInfo.GetDebugger();
					DebugEventHandler.Instance.Debugger.Initialize(DebugEventHandler.Instance);
					DebugEventHandler.Instance.Debugger.DebugActiveProcess(Convert.ToUInt32(processId), 0);
					DisplayLogMessage("\n---------------Attached to debuggee--------------------\n");

					foreach (RuntimeInfo runtimeInfo in runtimeInfos)
					{
						runtimeInfo.Dispose();
					}
				}
			}
			catch (Exception exception)
			{
				ShowException(exception);
				DisplayUserWarning("Exception occurred while trying to start the debuggee.");
			}
		}

		private void attachToProcessMenu_Click(object sender, EventArgs e)
		{
			using (AttachProcessDialog attachProcessDialog = new AttachProcessDialog(false))
			{
				if (attachProcessDialog.ShowDialog() == DialogResult.OK)
				{
					AttachToProcess(attachProcessDialog.ProcessID);
				}
			}
		}

		private RuntimeInfo FindLatestRuntimeInfo(RuntimeInfo[] runtimeInfos)
		{
			RuntimeInfo result = null;

			if (runtimeInfos.Length == 1)
			{
				result = runtimeInfos[0];
			}
			else
			{
				List<RuntimeVersion> runtimeVersions = new List<RuntimeVersion>(runtimeInfos.Length);

				foreach (RuntimeInfo runtimeInfo in runtimeInfos)
				{
					runtimeVersions.Add(new RuntimeVersion(runtimeInfo));
				}

				result = runtimeVersions.Max().RuntimeInfo;
			}

			return result;
		}

		private void DebugEventHandler_StateChanged(DebuggerState oldState, DebuggerState newState)
		{
			UIHandler.Instance.ShowDebuggerState(newState);
		}

		public void ShowDebuggerState(DebuggerState newState)
		{
			switch (newState)
			{
				case DebuggerState.DebuggeeRunning:
					runButton.Enabled = false;
					pauseButton.Enabled = true;
					stopButton.Enabled = true;
					detachButton.Enabled = true;
					doNotStopButton.Enabled = false;
					doNotStopHereButton.Enabled = false;

					attachToProcessMenu.Enabled = false;
					runDebuggeeMenu.Enabled = false;
					pauseDebuggeeMenu.Enabled = true;
					stopDebuggeeMenu.Enabled = true;
					detachMenu.Enabled = true;
					runToCursorMenu.Enabled = true;
					stepMenu.Enabled = false;
					stepIntoMenu.Enabled = false;
					stepOutMenu.Enabled = false;
					objectViewerMenu.Enabled = false;
					openDumpMenu.Enabled = false;
					break;

				case DebuggerState.DebuggeeStopped:
					ShowDebuggeeStoppedState();
					doNotStopButton.Enabled = false;
					doNotStopHereButton.Enabled = false;
					openDumpMenu.Enabled = true;
					CloseDynamicModuleDocuments();
					break;

				case DebuggerState.DebuggeePaused:
				case DebuggerState.DebuggeeSuspended:
					runButton.Enabled = true;
					pauseButton.Enabled = false;
					stopButton.Enabled = true;
					detachButton.Enabled = true;
					doNotStopButton.Enabled = false;
					doNotStopHereButton.Enabled = false;

					attachToProcessMenu.Enabled = false;
					runDebuggeeMenu.Enabled = true;
					pauseDebuggeeMenu.Enabled = false;
					stopDebuggeeMenu.Enabled = true;
					detachMenu.Enabled = true;
					runToCursorMenu.Enabled = true;
					stepMenu.Enabled = true;
					stepIntoMenu.Enabled = true;
					stepOutMenu.Enabled = true;
					objectViewerMenu.Enabled = true;
					openDumpMenu.Enabled = false;
					break;

				case DebuggerState.EvaluatingExpression:
					runButton.Enabled = false;
					pauseButton.Enabled = false;
					stopButton.Enabled = false;
					detachButton.Enabled = false;
					doNotStopButton.Enabled = false;
					doNotStopHereButton.Enabled = false;

					attachToProcessMenu.Enabled = false;
					runDebuggeeMenu.Enabled = false;
					pauseDebuggeeMenu.Enabled = false;
					stopDebuggeeMenu.Enabled = false;
					detachMenu.Enabled = false;
					runToCursorMenu.Enabled = false;
					stepMenu.Enabled = false;
					stepIntoMenu.Enabled = false;
					stepOutMenu.Enabled = false;
					objectViewerMenu.Enabled = false;
					openDumpMenu.Enabled = false;
					break;

				case DebuggerState.DebuggeeThrewException:
					runButton.Enabled = true;
					pauseButton.Enabled = false;
					stopButton.Enabled = true;
					detachButton.Enabled = true;
					doNotStopButton.Enabled = true;
					doNotStopHereButton.Enabled = true;

					attachToProcessMenu.Enabled = false;
					runDebuggeeMenu.Enabled = true;
					pauseDebuggeeMenu.Enabled = false;
					stopDebuggeeMenu.Enabled = true;
					detachMenu.Enabled = true;
					runToCursorMenu.Enabled = true;
					stepMenu.Enabled = true;
					stepIntoMenu.Enabled = true;
					stepOutMenu.Enabled = true;
					objectViewerMenu.Enabled = true;
					openDumpMenu.Enabled = false;
					break;

				case DebuggerState.DumpDebugging:
					runButton.Enabled = false;
					pauseButton.Enabled = false;
					stopButton.Enabled = true;
					detachButton.Enabled = false;
					doNotStopButton.Enabled = false;
					doNotStopHereButton.Enabled = false;

					attachToProcessMenu.Enabled = false;
					runDebuggeeMenu.Enabled = false;
					pauseDebuggeeMenu.Enabled = false;
					stopDebuggeeMenu.Enabled = true;
					detachMenu.Enabled = true;
					runToCursorMenu.Enabled = false;
					stepMenu.Enabled = false;
					stepIntoMenu.Enabled = false;
					stepOutMenu.Enabled = false;
					objectViewerMenu.Enabled = true;
					openDumpMenu.Enabled = true;
					break;
			}
		}

		private void ShowDebuggeeStoppedState()
		{
			runButton.Enabled = IsStartupExecutableSpecified();
			pauseButton.Enabled = false;
			stopButton.Enabled = false;
			detachButton.Enabled = false;

			attachToProcessMenu.Enabled = true;
			runDebuggeeMenu.Enabled = runButton.Enabled;
			pauseDebuggeeMenu.Enabled = false;
			stopDebuggeeMenu.Enabled = false;
			detachMenu.Enabled = false;
			runToCursorMenu.Enabled = runButton.Enabled;
			stepMenu.Enabled = runButton.Enabled;
			stepIntoMenu.Enabled = runButton.Enabled;
			stepOutMenu.Enabled = runButton.Enabled;
			objectViewerMenu.Enabled = false;
		}

		private static bool IsStartupExecutableSpecified()
		{
			bool result = false;

			switch (Project.Instance.StartMode)
			{
				case ProjectStartMode.StartAssembly:
					result = (Project.Instance.StartupAssembly != null);
					break;

				case ProjectStartMode.StartProgram:
					result = !string.IsNullOrEmpty(Project.Instance.ProgramExecutable);
					break;
			}
			return result;
		}

		private void runDebuggeeMenu_Click(object sender, EventArgs e)
		{
			RunDebuggee();
		}

		private void RunDebuggee()
		{
			if (DebugEventHandler.Instance.State != DebuggerState.DebuggeeStopped)
			{
				ClearDebugPanels(false);
				ClearCodeDisplayers(true);
				DebugEventHandler.Instance.ContinueProcess();
			}
			else if (!IsStartupExecutableSpecified())
			{
				DisplayUserWarning("No startup assembly/program is specified.");
			}
			else
			{
				try
				{
					DisplayLogMessage("\n---------------Debuggee started--------------------\n");
					LoadUnloadedAssembly = ExtendedDialogResult.None;
					ClearDebugPanels(false);
					ClearCodeDisplayers(true);
					string executable = string.Empty;
					string arguments = string.Empty;
					string workingDirectory = string.Empty;

					switch (Project.Instance.StartMode)
					{
						case ProjectStartMode.StartAssembly:
							executable = Project.Instance.StartupAssembly.FullPath;
							arguments = Project.Instance.AssemblyArguments;
							workingDirectory = Project.Instance.AssemblyWorkingDirectory;
							break;

						case ProjectStartMode.StartProgram:
							executable = Project.Instance.ProgramExecutable;
							arguments = Project.Instance.ProgramArguments;
							workingDirectory = Project.Instance.ProgramWorkingDirectory;
							break;
					}

					if (string.IsNullOrEmpty(workingDirectory))
					{
						workingDirectory = Path.GetDirectoryName(executable);
					}

					if (string.IsNullOrEmpty(arguments))
					{
						arguments = string.Format("\"{0}\"", executable);
					}
					else
					{
						arguments = string.Format("\"{0}\" {1}", executable, arguments);
					}

					BreakpointHandler.Instance.DeleteRemovedBreakpoints();
					using (MetaHost metaHost = new MetaHost())
					{
						string frameworkVersion = metaHost.GetVersionFromFile(executable);

						using (RuntimeInfo runtimeInfo = metaHost.GetRuntime(frameworkVersion))
						{
							DebugEventHandler.Instance.Debugger = runtimeInfo.GetDebugger();
							DebugEventHandler.Instance.Debugger.Initialize(DebugEventHandler.Instance);
							DebugEventHandler.Instance.Debugger.CreateProcessA(executable, arguments, workingDirectory);
						}
					}
				}
				catch (Exception exception)
				{
					ShowException(exception);
					DisplayUserWarning("Exception occurred while trying to start the debuggee.");
				}
			}
		}

		private void SetBreakpointOnFirstInstruction()
		{
			if (Project.Instance.StartupAssembly != null)
			{
				bool entryMethodNotFound = false;

				if (Project.Instance.StartupAssembly.AllTokens.ContainsKey(Project.Instance.StartupAssembly.EntryPointToken))
				{
					MethodDefinition entryMethod = Project.Instance.StartupAssembly.AllTokens[Project.Instance.StartupAssembly.EntryPointToken] as MethodDefinition;

					if (entryMethod == null)
					{
						entryMethodNotFound = true;
					}
					else
					{
						BreakpointHandler.Instance.RunToCursor(entryMethod, 0, false);
						RunDebuggee();
					}
				}
				else
				{
					entryMethodNotFound = true;
				}

				if (entryMethodNotFound)
				{
					UIHandler.Instance.DisplayUserWarning("The entry method is not found in the startup assembly.");
				}
			}
		}

		private void stepMenu_Click(object sender, EventArgs e)
		{
			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
			{
				SetBreakpointOnFirstInstruction();
			}
			else
			{
				BreakpointHandler.Instance.Step(StepType.StepOver);
			}
		}

		private void stepIntoMenu_Click(object sender, EventArgs e)
		{
			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
			{
				SetBreakpointOnFirstInstruction();
			}
			else
			{
				BreakpointHandler.Instance.Step(StepType.StepIn);
			}
		}

		private void detachMenu_Click(object sender, EventArgs e)
		{
			DetachFromDebuggee();
		}

		private void DetachFromDebuggee()
		{
			DebugEventHandler.Instance.Detach();
		}

		private void runButton_Click(object sender, EventArgs e)
		{
			RunDebuggee();
		}

		private void PauseDebuggee()
		{
			try
			{
				DebugEventHandler.Instance.RefreshAndSuspendProcess();
				List<ThreadWrapper> debugeeThreads = DebugEventHandler.Instance.EventObjects.Controller.EnumerateThreads();

				if (debugeeThreads.Count > 0)
				{
					DebugEventHandler.Instance.EventObjects.Thread = debugeeThreads[0];

					DebugEventHandler.Instance.DisplayAllInformation(DebuggerState.DebuggeePaused);
				}
			}
			catch (Exception exception)
			{
				ShowException(exception);
			}
		}

		private void pauseButton_Click(object sender, EventArgs e)
		{
			PauseDebuggee();
		}

		private void StopDebuggee()
		{
			try
			{
				if (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging)
				{
					DumpDebugger.Dispose();
					DumpDebugger = null;
					DebugEventHandler.Instance.OnProcessExited();
				}
				else
				{
					DebugEventHandler.Instance.RefreshAndSuspendProcess();
					DebugEventHandler.Instance.EventObjects.Process.Stop();
				}
			}
			catch (Exception exception)
			{
				ShowException(exception);
			}
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			StopDebuggee();
		}

		private void detachButton_Click(object sender, EventArgs e)
		{
			DetachFromDebuggee();
		}

		private void stepOutMenu_Click(object sender, EventArgs e)
		{
			if (DebugEventHandler.Instance.State == DebuggerState.DebuggeeStopped)
			{
				SetBreakpointOnFirstInstruction();
			}
			else
			{
				BreakpointHandler.Instance.Step(StepType.StepOut);
			}
		}

		private void MainForm_DragDrop(object sender, DragEventArgs e)
		{
			DataObject draggedObject = e.Data as DataObject;

			if (draggedObject != null)
			{
				StringCollection fileDropList = draggedObject.GetFileDropList();

				if (fileDropList != null && fileDropList.Count > 0)
				{
					e.Effect = DragDropEffects.All;
					OpenFiles(fileDropList);
				}
			}
		}

		private bool CanBeDropped(IDataObject draggedObject)
		{
			bool result = false;
			DataObject dataObject = draggedObject as DataObject;

			if (dataObject != null)
			{
				StringCollection fileDropList = dataObject.GetFileDropList();

				if (fileDropList != null && fileDropList.Count > 0)
				{
					result = true;
				}
			}

			return result;
		}

		private void MainForm_DragEnter(object sender, DragEventArgs e)
		{
			if (CanBeDropped(e.Data))
			{
				e.Effect = DragDropEffects.All;
			}
		}

		private void MainForm_DragOver(object sender, DragEventArgs e)
		{
			if (CanBeDropped(e.Data))
			{
				e.Effect = DragDropEffects.All;
			}
		}

		private void UpdateCodeEditorFonts()
		{
			Font font = Settings.Instance.CodeEditorFont.Font;

			foreach (CodeEditorForm codeEditor in ActiveCodeEditors)
			{
				codeEditor.UpdateFont(font);
			}
		}

		private void settingsMenu_Click(object sender, EventArgs e)
		{
			SettingsDialog settingsDialog = new SettingsDialog();

			if (settingsDialog.DisplaySettings(mainMenu) == DialogResult.OK)
			{
				Settings.SaveConfiguration(mainMenu);
				UpdateCodeEditorFonts();
			}
		}

		private void doNotStopButton_Click(object sender, EventArgs e)
		{
			AddExceptionInformation(false);

			doNotStopButton.Enabled = false;
			doNotStopHereButton.Enabled = false;
		}

		private void AddExceptionInformation(bool addIPCheck)
		{
			ValueWrapper currentException = DebugEventHandler.Instance.EventObjects.Thread.GetCurrentException();

			if (currentException != null && !currentException.IsNull())
			{
				ValueWrapper dereferencedException = currentException.DereferenceValue();

				if (dereferencedException != null)
				{
					ClassWrapper currentExceptionClass = dereferencedException.GetClassInformation();
					ModuleWrapper module = currentExceptionClass.GetModule();
					ExceptionInformation exceptionInformation = new ExceptionInformation(module.GetName(), currentExceptionClass.GetToken());
					uint throwingMethodToken = 0;
					uint currentIP = 0;

					if (addIPCheck && DebugEventHandler.Instance.EventObjects.Thread != null)
					{
						FrameWrapper activeFrame = DebugEventHandler.Instance.EventObjects.Thread.GetActiveFrame();

						if (activeFrame != null && activeFrame.IsILFrame())
						{
							bool exactLocation = false;
							currentIP = activeFrame.GetIP(ref exactLocation);
							exceptionInformation.IP = currentIP;

							throwingMethodToken = activeFrame.GetFunctionToken();
							exceptionInformation.ThrowingMethodToken = throwingMethodToken;
						}
					}

					int indexOfExisting = 0;

					if (addIPCheck)
					{
						indexOfExisting = Project.Instance.FindExceptionInformationByIP(exceptionInformation);
					}
					else
					{
						indexOfExisting = Project.Instance.Exceptions.IndexOf(exceptionInformation);
					}

					if (indexOfExisting >= 0)
					{
						ExceptionInformation existingExceptionInformation = Project.Instance.Exceptions[indexOfExisting];
						existingExceptionInformation.Skip = true;
						Project.Instance.IsSaved = false;

						if (addIPCheck)
						{
							existingExceptionInformation.ThrowingMethodToken = throwingMethodToken;
							existingExceptionInformation.IP = currentIP;
						}
					}
					else
					{
						Project.Instance.Exceptions.Add(exceptionInformation);
						Project.Instance.IsSaved = false;
					}
				}
			}
		}

		private void doNotStopHereButton_Click(object sender, EventArgs e)
		{
			AddExceptionInformation(true);
			doNotStopButton.Enabled = false;
			doNotStopHereButton.Enabled = false;
		}

		private void displayHexaNumbersButton_Click(object sender, EventArgs e)
		{
			displayHexaNumbersButton.Checked = !displayHexaNumbersButton.Checked;
			Settings.Instance.DisplayHexaNumbers = displayHexaNumbersButton.Checked;
			Settings.SaveConfiguration();
		}

		private void toggleBreakpointMenu_Click(object sender, EventArgs e)
		{
			CodeEditorForm codeEditor = dockPanel.ActiveContent as CodeEditorForm;

			if (codeEditor != null)
			{
				codeEditor.SetBreakpointAtSelection();
			}
		}

		private void windowMenuItem_Click(object sender, EventArgs e)
		{
			MenuItem menuItem = (MenuItem)sender;

			if (dockPanel.Documents != null)
			{
				IDockContent[] documents = dockPanel.DocumentsToArray();

				if (documents != null && documents.Length > menuItem.Index)
				{
					documents[menuItem.Index].DockHandler.Activate();
				}
			}
		}

		private void FillWindowMenu()
		{
			bool separatorFound = false;

			while (!separatorFound && windowMenu.MenuItems.Count > 0)
			{
				if (windowMenu.MenuItems[0].Text.Equals("-", StringComparison.OrdinalIgnoreCase))
				{
					separatorFound = true;
				}
				else
				{
					windowMenu.MenuItems.RemoveAt(0);
				}
			}

			if (ActiveCodeEditors != null)
			{
				int index = 0;

				while (index < 10 && index < ActiveCodeEditors.Count)
				{
					CodeEditorForm codeEditorForm = ActiveCodeEditors[index];

					MenuItem menuItem = new MenuItem();
					menuItem.Text = string.Format("{0} {1}", index + 1, codeEditorForm.TabText);
					menuItem.Click += new EventHandler(windowMenuItem_Click);

					if (codeEditorForm == dockPanel.ActiveDocument)
					{
						menuItem.Checked = true;
					}

					windowMenu.MenuItems.Add(index++, menuItem);
				}
			}
		}

		private void closeAllWindowsMenu_Click(object sender, EventArgs e)
		{
			CloseCodeEditors();

			if (startPage != null && !startPage.IsDisposed)
			{
				startPage.Close();
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if (msg.Msg == Constants.WM_KEYDOWN || msg.Msg == Constants.WM_SYSKEYDOWN)
			{
				if (keyData == (Keys.Control | Keys.Tab))
				{
					UIHandler.Instance.DisplayDocumentSelector();
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		public void DocumentActivated(DocumentContent documentContent)
		{
			int index = ActiveDocuments.IndexOf(documentContent);

			if (index >= 0)
			{
				ActiveDocuments.RemoveAt(index);
				ActiveDocuments.Insert(0, documentContent);
			}
		}

		private void dockPanel_ContentAdded(object sender, DockContentEventArgs e)
		{
			DocumentContent documentContent = e.Content as DocumentContent;

			if (documentContent != null)
			{
				if (!ActiveDocuments.Contains(documentContent))
				{
					ActiveDocuments.Add(documentContent);
				}

				CodeEditorForm codeEditor = e.Content as CodeEditorForm;

				if (codeEditor != null && !ActiveCodeEditors.Contains(codeEditor))
				{
					ActiveCodeEditors.Add(codeEditor);
				}
			}
		}

		private void dockPanel_ContentRemoved(object sender, DockContentEventArgs e)
		{
			DocumentContent documentContent = e.Content as DocumentContent;

			if (documentContent != null)
			{
				if (ActiveDocuments.Contains(documentContent))
				{
					ActiveDocuments.Remove(documentContent);
				}

				CodeEditorForm codeEditor = e.Content as CodeEditorForm;

				if (codeEditor != null && ActiveCodeEditors.Contains(codeEditor))
				{
					ActiveCodeEditors.Remove(codeEditor);
				}
			}
		}

		private void windowMenu_Popup(object sender, EventArgs e)
		{
			FillWindowMenu();
		}

		private void pauseDebuggeeMenu_Click(object sender, EventArgs e)
		{
			PauseDebuggee();
		}

		private void stopDebuggeeMenu_Click(object sender, EventArgs e)
		{
			StopDebuggee();
		}

		private void runToCursorMenu_Click(object sender, EventArgs e)
		{
			CodeEditorForm codeEditor = dockPanel.ActiveContent as CodeEditorForm;

			if (codeEditor != null)
			{
				codeEditor.SetRunToCursorAtSelection();
				RunDebuggee();
			}
		}

		private void objectViewerMenu_Click(object sender, EventArgs e)
		{
			ShowObjectInObjectViewer(DebugEventHandler.Instance.EventObjects.Frame, null, string.Empty);
		}

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{
			if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.F10)
			{
				e.SuppressKeyPress = true;
			}
		}

		private void startPageMenu_Click(object sender, EventArgs e)
		{
			if (startPage.IsDisposed)
			{
				startPage = new StartPage();
			}

			startPage.Show(dockPanel, DockState.Document);
		}

		private void openDumpMenu_Click(object sender, EventArgs e)
		{
			openDumpFileDialog.InitialDirectory = Settings.Instance.DefaultDumpFileDirectory ?? string.Empty;

			if (openDumpFileDialog.ShowDialog() == DialogResult.OK)
			{
				OpenDumpFile(openDumpFileDialog.FileName);
			}
		}

		public void OpenDumpFile(string dumpFilePath)
		{
			if (DumpInfoPage != null)
			{
				DumpInfoPage.Dispose();
			}

			if (DumpDebugger != null)
			{
				DumpDebugger.Dispose();
			}

			try
			{
				DumpDebugger = new DumpDebugger(dumpFilePath);
				ProcessWrapper processWrapper = DumpDebugger.OpenDumpFile();

				DebugEventHandler.Instance.EventObjects.Process = processWrapper;
				DebugEventHandler.Instance.EventObjects.Controller = processWrapper;
				List<ThreadWrapper> debugeeThreads = DebugEventHandler.Instance.EventObjects.Controller.EnumerateThreads();

				if (debugeeThreads.Count > 0)
				{
					DebugEventHandler.Instance.EventObjects.Thread = debugeeThreads[0];

					DebugEventHandler.Instance.DisplayAllInformation(DebuggerState.DumpDebugging);
				}

				DumpInfoPage = new MemoryDumpInfoPage(DumpDebugger);
				DumpInfoPage.Show(dockPanel, DockState.Document);
				Settings.Instance.AddDumpFile(dumpFilePath);
			}
			catch (Exception exception)
			{
				ShowException(exception);
			}
		}

		private void saveDumpMenu_Click(object sender, EventArgs e)
		{
			SaveDumpDialog saveDumpDialog = new SaveDumpDialog(Convert.ToInt32(DebugEventHandler.Instance.DebugeeProcessID));
			saveDumpDialog.ShowDialog();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			WindowState = FormWindowState.Maximized;
		}

		public void DisplayOpCodeHelp(OpCodeItem opCodeItem)
		{
			opCodeHelperPanel.Display(opCodeItem);
		}
	}
}