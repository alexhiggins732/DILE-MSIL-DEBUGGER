using System;
using System.Collections.Generic;
using System.Text;

using Dile.UI;
using Dile.UI.Debug;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Dile.Configuration
{
	public class Settings
	{
		public event NoArgumentsDelegate RecentAssembliesChanged;
		public event NoArgumentsDelegate RecentProjectsChanged;
		public event NoArgumentsDelegate RecentDumpFilesChanged;
		public event NoArgumentsDelegate DisplayHexaNumbersChanged;
		public event NoArgumentsDelegate Changed;

		private const string ConfigurationFileName = "Dile.settings.xml";

		#region Singleton pattern
		private static readonly Settings instance = LoadConfiguration();
		public static Settings Instance
		{
			get
			{
				return instance;
			}
		}
		#endregion

		private static readonly object lockObject = new object();

		private static string configurationFilePath;
		private static string ConfigurationFilePath
		{
			get
			{
				return configurationFilePath;
			}
			set
			{
				configurationFilePath = value;
			}
		}

		public static SerializableFont DefaultFont
		{
			get
			{
				SerializableFont result = new SerializableFont();
				result.FamilyName = "Tahoma";
				result.FontStyle = FontStyle.Regular;
				result.GdiCharset = 1;
				result.GdiVerticalFont = false;
				result.GraphicsUnit = GraphicsUnit.World;
				result.Size = 11;

				return result;
			}
		}

		public static SerializableFont DefaultCodeEditorFont
		{
			get
			{
				SerializableFont result = new SerializableFont();
				result.FamilyName = "Courier New";
				result.FontStyle = FontStyle.Regular;
				result.GdiCharset = 0xee;
				result.GdiVerticalFont = false;
				result.GraphicsUnit = GraphicsUnit.Point;
				result.Size = 9;

				return result;
			}
		}

		private List<string> recentAssemblies = new List<string>();
		public List<string> RecentAssemblies
		{
			get
			{
				return recentAssemblies;
			}
			set
			{
				recentAssemblies = value;
			}
		}

		private List<string> recentProjects = new List<string>();
		public List<string> RecentProjects
		{
			get
			{
				return recentProjects;
			}
			set
			{
				recentProjects = value;
			}
		}

		public List<string> RecentDumpFiles
		{
			get;
			set;
		}

		private bool isLoadClassEnabled = true;
		public bool IsLoadClassEnabled
		{
			get
			{
				return isLoadClassEnabled;
			}
			set
			{
				isLoadClassEnabled = value;
			}
		}

		private bool warnUnloadedAssembly = true;
		public bool WarnUnloadedAssembly
		{
			get
			{
				return warnUnloadedAssembly;
			}
			set
			{
				warnUnloadedAssembly = value;
			}
		}

		private bool stopOnException = true;
		public bool StopOnException
		{
			get
			{
				return stopOnException;
			}
			set
			{
				stopOnException = value;
			}
		}

		private bool stopOnlyOnUnhandledException = false;
		public bool StopOnlyOnUnhandledException
		{
			get
			{
				return stopOnlyOnUnhandledException;
			}
			set
			{
				stopOnlyOnUnhandledException = value;
			}
		}

		private bool stopOnMdaNotification = true;
		public bool StopOnMdaNotification
		{
			get
			{
				return stopOnMdaNotification;
			}
			set
			{
				stopOnMdaNotification = value;
			}
		}

		private bool displayHexaNumbers = true;
		public bool DisplayHexaNumbers
		{
			get
			{
				return displayHexaNumbers;
			}
			set
			{
				bool valueChanged = (displayHexaNumbers != value);
				displayHexaNumbers = value;

				if (valueChanged && DisplayHexaNumbersChanged != null)
				{
					DisplayHexaNumbersChanged();
				}
			}
		}

		public bool EvaluateMethodsInObjectViewer
		{
			get;
			set;
		}

		private bool detachOnQuit = true;
		public bool DetachOnQuit
		{
			get
			{
				return detachOnQuit;
			}
			set
			{
				detachOnQuit = value;
			}
		}

		private int maxRecentAssembliesCount = 10;
		public int MaxRecentAssembliesCount
		{
			get
			{
				return maxRecentAssembliesCount;
			}
			set
			{
				bool isValueDecreased = (value < maxRecentAssembliesCount);

				maxRecentAssembliesCount = value;

				if (isValueDecreased && RecentAssemblies.Count > value)
				{
					RecentAssemblies.RemoveRange(value, RecentAssemblies.Count - value);
					OnRecentAssembliesChanged();
				}
			}
		}

		private int maxRecentProjectsCount = 10;
		public int MaxRecentProjectsCount
		{
			get
			{
				return maxRecentProjectsCount;
			}
			set
			{
				bool isValueDecreased = (value < maxRecentProjectsCount);

				maxRecentProjectsCount = value;

				if (isValueDecreased && RecentProjects.Count > value)
				{
					RecentProjects.RemoveRange(value, RecentProjects.Count - value);
					OnRecentProjectsChanged();
				}
			}
		}

		private int maxRecentDumpFilesCount = 10;
		public int MaxRecentDumpFilesCount
		{
			get
			{
				return maxRecentDumpFilesCount;
			}
			set
			{
				bool isValueDecreased = (value < maxRecentDumpFilesCount);

				maxRecentDumpFilesCount = value;

				if (isValueDecreased && RecentDumpFiles.Count > value)
				{
					RecentDumpFiles.RemoveRange(value, RecentDumpFiles.Count - value);
					OnRecentDumpFilesChanged();
				}
			}
		}

		private string defaultAssemblyDirectory = string.Empty;
		public string DefaultAssemblyDirectory
		{
			get
			{
				return defaultAssemblyDirectory;
			}
			set
			{
				defaultAssemblyDirectory = value;
			}
		}

		private string defaultProjectDirectory = string.Empty;
		public string DefaultProjectDirectory
		{
			get
			{
				return defaultProjectDirectory;
			}
			set
			{
				defaultProjectDirectory = value;
			}
		}

		public string DefaultDumpFileDirectory
		{
			get;
			set;
		}

		private List<MenuFunctionShortcut> shortcuts = new List<MenuFunctionShortcut>();
		public List<MenuFunctionShortcut> Shortcuts
		{
			get
			{
				return shortcuts;
			}
			set
			{
				shortcuts = value;
			}
		}

		private List<PanelDisplayer> panels = new List<PanelDisplayer>();
		public List<PanelDisplayer> Panels
		{
			get
			{
				return panels;
			}
			set
			{
				panels = value;
			}
		}

		private CodeEditorFontSettings codeEditorFont;
		public CodeEditorFontSettings CodeEditorFont
		{
			get
			{
				return codeEditorFont;
			}
			set
			{
				codeEditorFont = value;
			}
		}

		private DebugEventType displayedDebugEvents = DebugEventType.AllSet;
		public DebugEventType DisplayedDebugEvents
		{
			get
			{
				return displayedDebugEvents;
			}
			set
			{
				displayedDebugEvents = value;
			}
		}

		private int funcEvalTimeout = 5;
		public int FuncEvalTimeout
		{
			get
			{
				return funcEvalTimeout;
			}
			set
			{
				funcEvalTimeout = value;
			}
		}

		private int funcEvalAbortTimeout = 30;
		public int FuncEvalAbortTimeout
		{
			get
			{
				return funcEvalAbortTimeout;
			}
			set
			{
				funcEvalAbortTimeout = value;
			}
		}

		public int ProjectNewsFeedUpdatePeriod
		{
			get;
			set;
		}

		public DateTime? LastProjectNewsFeedUpdate
		{
			get;
			set;
		}

		public string LastProjectNewsFeed
		{
			get;
			set;
		}

		public int ReleasesFeedUpdatePeriod
		{
			get;
			set;
		}

		public DateTime? LastReleasesFeedUpdate
		{
			get;
			set;
		}

		public string LastReleasesFeed
		{
			get;
			set;
		}

		public int BlogFeedUpdatePeriod
		{
			get;
			set;
		}

		public DateTime? LastBlogFeedUpdate
		{
			get;
			set;
		}

		public string LastBlogFeed
		{
			get;
			set;
		}

		public bool StartPageEnabled
		{
			get;
			set;
		}

		public EscapingType NameEscapingType
		{
			get;
			set;
		}

		public bool CopyILAddresses
		{
			get;
			set;
		}

		public int AutomaticRefreshInterval
		{
			get;
			set;
		}

		public SearchOptions SearchOptions
		{
			get;
			set;
		}

		public bool SyncOpCodeHelper
		{
			get;
			set;
		}

		[XmlIgnore()]
		public string VersionNumber
		{
			get;
			private set;
		}

		private Settings()
		{
			EvaluateMethodsInObjectViewer = true;
			RecentDumpFiles = new List<string>();
			DefaultDumpFileDirectory = string.Empty;
			ProjectNewsFeedUpdatePeriod = 1;
			ReleasesFeedUpdatePeriod = 1;
			BlogFeedUpdatePeriod = 1;
			StartPageEnabled = true;
			NameEscapingType = EscapingType.CharacterCode;
			CopyILAddresses = false;
			AutomaticRefreshInterval = 2000;
			SearchOptions = SearchOptions.TypeDefinition;
			SyncOpCodeHelper = true;

			AssemblyName executingAssemblyName = Assembly.GetExecutingAssembly().GetName();
			VersionNumber = string.Format("{0}.{1}.{2}.{3}",
				executingAssemblyName.Version.Major,
				executingAssemblyName.Version.Minor,
				executingAssemblyName.Version.Build,
				executingAssemblyName.Version.Revision);
		}

		private static Settings LoadConfiguration()
		{
			Settings result = null;
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			ConfigurationFilePath = Path.Combine(Path.GetDirectoryName(executingAssembly.Location), ConfigurationFileName);

			if (File.Exists(ConfigurationFilePath))
			{
				FileStream fileStream = null;

				try
				{
					fileStream = new FileStream(ConfigurationFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
					XmlSerializer serializer = new XmlSerializer(typeof(Settings));
					result = (Settings)serializer.Deserialize(fileStream);
				}
				catch (Exception exception)
				{
					UIHandler.Instance.ShowException(exception);
					UIHandler.Instance.DisplayUserWarning("Unable to read the configuration file.");
					result = new Settings();
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
			else
			{
				result = new Settings();
			}

			return result;
		}

		public static void SaveConfiguration()
		{
			FileStream fileStream = null;

			lock (lockObject)
			{
				try
				{
					fileStream = new FileStream(ConfigurationFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
					XmlSerializer serializer = new XmlSerializer(typeof(Settings));
					serializer.Serialize(fileStream, Instance);
				}
				catch (Exception exception)
				{
					UIHandler.Instance.ShowException(exception);
					UIHandler.Instance.DisplayUserWarning("Unable to save the configuration file.");
				}
				finally
				{
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
		}

		private static void SaveShortcuts(MainMenu.MenuItemCollection menuItems)
		{
			foreach (MenuItem menuItem in menuItems)
			{
				BaseMenuInformation menuInformation = menuItem.Tag as BaseMenuInformation;

				if (menuInformation != null)
				{
					MenuFunction menuFunction = menuInformation.MenuFunction;
					MenuFunctionShortcut menuFunctionShortcut = new MenuFunctionShortcut();
					menuFunctionShortcut.MenuFunction = menuFunction;
					menuFunctionShortcut.Shortcut = menuItem.Shortcut;

					MenuFunctionShortcut existingShortcut = Instance.FindMenuFunctionShortcut(menuFunction);

					if (existingShortcut != null)
					{
						Instance.Shortcuts.Remove(existingShortcut);
					}

					Instance.Shortcuts.Add(menuFunctionShortcut);
				}

				if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
				{
					SaveShortcuts(menuItem.MenuItems);
				}
			}
		}

		public static void SaveConfiguration(MainMenu mainMenu)
		{
			SaveShortcuts(mainMenu.MenuItems);
			SaveConfiguration();
		}

		private void OnRecentAssembliesChanged()
		{
			if (RecentAssembliesChanged != null)
			{
				RecentAssembliesChanged();
			}
		}

		private void OnRecentProjectsChanged()
		{
			if (RecentProjectsChanged != null)
			{
				RecentProjectsChanged();
			}
		}

		private void OnRecentDumpFilesChanged()
		{
			if (RecentDumpFilesChanged != null)
			{
				RecentDumpFilesChanged();
			}
		}

		private void OnChanged()
		{
			if (Changed != null)
			{
				Changed();
			}
		}

		public void SettingsUpdated()
		{
			OnChanged();
		}

		public void AddProject(string projectFilePath)
		{
			HelperFunctions.AddItemToList<string>(RecentProjects, projectFilePath, MaxRecentProjectsCount);
			SaveConfiguration();
			OnRecentProjectsChanged();
		}

		public void AddAssembly(string assemblyFilePath)
		{
			HelperFunctions.AddItemToList<string>(RecentAssemblies, assemblyFilePath, MaxRecentAssembliesCount);
			SaveConfiguration();
			OnRecentAssembliesChanged();
		}

		public void AddDumpFile(string dumpFilePath)
		{
			HelperFunctions.AddItemToList(RecentDumpFiles, dumpFilePath, MaxRecentDumpFilesCount);
			SaveConfiguration();
			OnRecentDumpFilesChanged();
		}

		public void MoveAssemblyToFirst(string assemblyFilePath)
		{
			if (HelperFunctions.MoveItemInList<string>(RecentAssemblies, assemblyFilePath, 0))
			{
				SaveConfiguration();
				OnRecentAssembliesChanged();
			}
		}

		public void MoveProjectToFirst(string projectFilePath)
		{
			if (HelperFunctions.MoveItemInList<string>(RecentProjects, projectFilePath, 0))
			{
				SaveConfiguration();
				OnRecentProjectsChanged();
			}
		}

		public void MoveDumpFileToFirst(string dumpFilePath)
		{
			if (HelperFunctions.MoveItemInList(RecentDumpFiles, dumpFilePath, 0))
			{
				SaveConfiguration();
				OnRecentDumpFilesChanged();
			}
		}

		public void AddAssemblies(string[] assemblyFilePaths)
		{
			HelperFunctions.AddItemsToList<string>(RecentAssemblies, assemblyFilePaths, MaxRecentAssembliesCount);
			SaveConfiguration();
			OnRecentAssembliesChanged();
		}

		private MenuFunctionShortcut FindMenuFunctionShortcut(MenuFunction menuFunction)
		{
			MenuFunctionShortcut result = null;
			int index = 0;

			while (result == null && index < Shortcuts.Count)
			{
				MenuFunctionShortcut menuFunctionShortcut = Shortcuts[index++];

				if (menuFunctionShortcut.MenuFunction == menuFunction)
				{
					result = menuFunctionShortcut;
				}
			}

			return result;
		}

		private void UpdateShortcuts(MainMenu.MenuItemCollection menuItems)
		{
			foreach (MenuItem menuItem in menuItems)
			{
				BaseMenuInformation menuInformation = menuItem.Tag as BaseMenuInformation;

				if (menuInformation != null)
				{
					MenuFunction menuFunction = menuInformation.MenuFunction;
					MenuFunctionShortcut menuFunctionShortcut = FindMenuFunctionShortcut(menuFunction);

					if (menuFunctionShortcut != null)
					{
						menuItem.Shortcut = menuFunctionShortcut.Shortcut;
					}
				}

				if (menuItem.MenuItems != null && menuItem.MenuItems.Count > 0)
				{
					UpdateShortcuts(menuItem.MenuItems);
				}
			}
		}

		public void UpdateShortcuts(MainMenu mainMenu)
		{
			UpdateShortcuts(mainMenu.MenuItems);
		}

		public bool DisplayDebugEvent(DebugEventType debugEventType)
		{
			return ((DisplayedDebugEvents & debugEventType) == debugEventType);
		}
	}
}