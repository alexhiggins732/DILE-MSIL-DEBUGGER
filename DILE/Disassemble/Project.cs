using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Debug;
using Dile.UI;
using Dile.UI.Debug;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Dile.Disassemble
{
	[Serializable()]
	public class Project : IDisposable
	{
		public static event EventHandler ProjectChanged;
		public static event EventHandler ProjectIsSavedChanged;

		#region Singleton pattern
		private static Project instance;
		public static Project Instance
		{
			get
			{
				return instance;
			}
			set
			{
				if (instance != null)
				{
					instance.Dispose();
				}

				instance = value;

				if (instance != null)
				{
					instance.AssociateBreakpointsWithMethods();
				}

				if (ProjectChanged != null)
				{
					ProjectChanged(value, new EventArgs());
				}
			}
		}
		#endregion

		public const int DefaultArrayCount = 16;
		private const int DefaultCharArrayCount = 1024;

		[ThreadStatic()]
		private static char[] defaultCharArray;
		public static char[] DefaultCharArray
		{
			get
			{
				if (defaultCharArray == null)
				{
					defaultCharArray = new char[DefaultCharArrayCount];
				}

				return defaultCharArray;
			}
			set
			{
				if (value.Length < defaultCharArray.Length)
				{
					throw new InvalidOperationException("Shorter array cannot be set.");
				}

				defaultCharArray = value;
			}
		}

		private List<Assembly> assemblies = new List<Assembly>();
		public List<Assembly> Assemblies
		{
			get
			{
				return assemblies;
			}
			set
			{
				assemblies = value;
				startupAssembly = null;

				SetStartupAssembly();
			}
		}

		private string name = "New project";
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		private bool isSaved = false;
		[XmlIgnore()]
		public bool IsSaved
		{
			get
			{
				return (isSaved && (FullPath.Length > 0 || Assemblies.Count == 0));
			}
			set
			{
				isSaved = value;

				if (ProjectIsSavedChanged != null)
				{
					ProjectIsSavedChanged(null, new EventArgs());
				}
			}
		}

		private string fullPath = string.Empty;
		[XmlIgnore()]
		public string FullPath
		{
			get
			{
				return fullPath;
			}
			set
			{
				fullPath = value;
			}
		}

		private Assembly startupAssembly = null;
		[XmlIgnore()]
		public Assembly StartupAssembly
		{
			get
			{
				return startupAssembly;
			}
			set
			{
				startupAssembly = value;

				if (value == null)
				{
					startupAssemblyPath = string.Empty;
				}
				else
				{
					startupAssemblyPath = startupAssembly.FullPath;
				}
			}
		}

		private string startupAssemblyPath = string.Empty;
		public string StartupAssemblyPath
		{
			get
			{
				return startupAssemblyPath;
			}
			set
			{
				startupAssemblyPath = value;

				SetStartupAssembly();
			}
		}

		private List<ExceptionInformation> exceptions = new List<ExceptionInformation>();
		public List<ExceptionInformation> Exceptions
		{
			get
			{
				return exceptions;
			}
			set
			{
				exceptions = value;
			}
		}

		private List<FunctionBreakpointInformation> functionBreakpoints = new List<FunctionBreakpointInformation>();
		public List<FunctionBreakpointInformation> FunctionBreakpoints
		{
			get
			{
				return functionBreakpoints;
			}
			set
			{
				functionBreakpoints = value;
			}
		}

		private List<SuspendableDebugEvent> suspendingDebugEvents = new List<SuspendableDebugEvent>();
		public List<SuspendableDebugEvent> SuspendingDebugEvents
		{
			get
			{
				return suspendingDebugEvents;
			}
			set
			{
				suspendingDebugEvents = value;
			}
		}

		private ProjectStartMode startMode;
		public ProjectStartMode StartMode
		{
			get
			{
				return startMode;
			}
			set
			{
				startMode = value;
			}
		}

		private string assemblyArguments;
		public string AssemblyArguments
		{
			get
			{
				return assemblyArguments;
			}
			set
			{
				assemblyArguments = value;
			}
		}

		private string assemblyWorkingDirectory;
		public string AssemblyWorkingDirectory
		{
			get
			{
				return assemblyWorkingDirectory;
			}
			set
			{
				assemblyWorkingDirectory = value;
			}
		}

		private string programExecutable;
		public string ProgramExecutable
		{
			get
			{
				return programExecutable;
			}
			set
			{
				programExecutable = value;
			}
		}

		private string programArguments;
		public string ProgramArguments
		{
			get
			{
				return programArguments;
			}
			set
			{
				programArguments = value;
			}
		}

		private string programWorkingDirectory;
		public string ProgramWorkingDirectory
		{
			get
			{
				return programWorkingDirectory;
			}
			set
			{
				programWorkingDirectory = value;
			}
		}

		#region ASP.NET Debugging
		//private string browserUrl;
		//public string BrowserUrl
		//{
		//  get
		//  {
		//    return browserUrl;
		//  }
		//  set
		//  {
		//    browserUrl = value;
		//  }
		//}

		//private bool autoAttachToAspNet;
		//public bool AutoAttachToAspNet
		//{
		//  get
		//  {
		//    return autoAttachToAspNet;
		//  }
		//  set
		//  {
		//    autoAttachToAspNet = value;
		//  }
		//}
		#endregion

		private FunctionBreakpointInformation runToCursorBreakpoint;
		[XmlIgnore()]
		public FunctionBreakpointInformation RunToCursorBreakpoint
		{
			get
			{
				return runToCursorBreakpoint;
			}
			set
			{
				runToCursorBreakpoint = value;
			}
		}

		private void AssociateBreakpointsWithMethods()
		{
			if (FunctionBreakpoints != null && FunctionBreakpoints.Count > 0 && Assemblies != null)
			{
				if (Assemblies.Count > 0)
				{
					int index = 0;

					while (index < FunctionBreakpoints.Count)
					{
						FunctionBreakpointInformation functionBreakpoint = FunctionBreakpoints[index];

						if (functionBreakpoint.AssociateWithMethod())
						{
							index++;
						}
						else
						{
							FunctionBreakpoints.Remove(functionBreakpoint);
						}
					}
				}
				else
				{
					FunctionBreakpoints.Clear();
				}
			}

			if (RunToCursorBreakpoint != null)
			{
				if (!RunToCursorBreakpoint.AssociateWithMethod())
				{
					RunToCursorBreakpoint.Remove();
					RunToCursorBreakpoint = null;
				}
			}
		}

		private void SetStartupAssembly()
		{
			if (StartupAssemblyPath == string.Empty)
			{
				StartupAssembly = null;
			}
			else if (Assemblies != null && Assemblies.Count > 0 && StartupAssembly == null)
			{
				Assembly foundAssembly = FindAssemblyByFullPath(Assemblies, StartupAssemblyPath);

				if (foundAssembly != null)
				{
					StartupAssembly = foundAssembly;
				}
			}
		}

		private Assembly FindAssemblyByFullPath(List<Assembly> assemblies, string assemblyFullPath)
		{
			Assembly result = null;
			assemblyFullPath = assemblyFullPath.ToUpperInvariant();
			int index = 0;

			while (result == null && index < assemblies.Count)
			{
				Assembly assembly = assemblies[index++];

				if (assembly.FullPath.ToUpperInvariant() == assemblyFullPath)
				{
					result = assembly;
				}
			}

			return result;
		}

		public bool IsAssemblyLoaded(string assemblyPath)
		{
			bool result = false;

			if (Assemblies != null)
			{
				result = Assemblies.Any(assembly => string.Equals(assembly.FullPath, assemblyPath, StringComparison.OrdinalIgnoreCase));

				if (!result)
				{
					result = assemblies.Any(assembly => (assembly.IsInMemory && string.Equals(assembly.Name, assemblyPath, StringComparison.Ordinal))
						|| (!assembly.IsInMemory && string.Equals(assembly.FullPath, assemblyPath, StringComparison.OrdinalIgnoreCase)));
				}
			}

			return result;
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (Assemblies != null && Assemblies.Count > 0)
			{
				foreach (Assembly assembly in Assemblies)
				{
					assembly.Dispose();
				}

				Assemblies = null;
			}
		}

		#endregion

		public void Save()
		{
			if (FullPath != null && FullPath.Length > 0)
			{
				Save(FullPath);
			}
		}

		public void Save(string path)
		{
			List<Assembly> inMemoryAssemblies = new List<Assembly>();

			if (Assemblies != null && Assemblies.Count > 0)
			{
				int index = 0;

				while (index < Assemblies.Count)
				{
					Assembly assembly = Assemblies[index];

					if (assembly.IsInMemory)
					{
						inMemoryAssemblies.Add(assembly);
						Assemblies.Remove(assembly);
					}
					else
					{
						index++;
					}
				}
			}

			using (FileStream projectFile = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Project));
				serializer.Serialize(projectFile, this);
				FullPath = path;
				IsSaved = true;
			}

			if (inMemoryAssemblies.Count > 0)
			{
				foreach (Assembly inMemoryAssembly in inMemoryAssemblies)
				{
					Assemblies.Add(inMemoryAssembly);
				}
			}
		}

		public bool SkipException(string assemblyPath, uint exceptionClassToken, uint throwingMethodToken, uint? currentIP)
		{
			bool result = false;
			int index = 0;

			while (!result && index < Exceptions.Count)
			{
				result = Exceptions[index++].Equals(assemblyPath, exceptionClassToken, throwingMethodToken, currentIP);
			}

			return result;
		}

		public FunctionBreakpointInformation FindFunctionBreakpoint(MethodDefinition methodDefinition, uint offset)
		{
			FunctionBreakpointInformation result = null;
			int index = 0;

			while (result == null && index < Project.Instance.FunctionBreakpoints.Count)
			{
				FunctionBreakpointInformation functionBreakpoint = Project.Instance.FunctionBreakpoints[index++];

				if (functionBreakpoint.MethodDefinition == methodDefinition && functionBreakpoint.Offset == offset)
				{
					result = functionBreakpoint;
				}
			}

			return result;
		}

		public bool HasBreakpointsInMethod(MethodDefinition methodDefinition)
		{
			bool result = false;
			int index = 0;

			while (!result && index < FunctionBreakpoints.Count)
			{
				FunctionBreakpointInformation functionBreakpoint = FunctionBreakpoints[index++];

				if (functionBreakpoint.MethodDefinition == methodDefinition)
				{
					result = true;
				}
			}

			return result;
		}

		public int FindExceptionInformationByIP(ExceptionInformation exceptionInformation)
		{
			int result = 0;
			bool found = false;

			while (!found && result < Exceptions.Count)
			{
				ExceptionInformation existingInformation = Exceptions[result];

				if (existingInformation.CompareTo(exceptionInformation) == 0 && existingInformation.ThrowingMethodToken == exceptionInformation.ThrowingMethodToken && existingInformation.IP == exceptionInformation.IP)
				{
					found = true;
				}
				else
				{
					result++;
				}
			}

			return (found ? result : -1);
		}

		public bool SuspendOnDebugEvent(SuspendableDebugEvent debugEvent)
		{
			return SuspendingDebugEvents.Contains(debugEvent);
		}

		public void RemoveInMemoryAssemblies()
		{
			if (Assemblies != null && Assemblies.Count > 0)
			{
				int index = 0;

				while (index < Assemblies.Count)
				{
					Assembly assembly = Assemblies[index];

					if (assembly.LoadedFromMemory)
					{
						RemoveAssemblyRelatedBreakpoints(assembly);
						Assemblies.Remove(assembly);
					}
					else
					{
						index++;
					}
				}
			}
		}

		public void RemoveAssemblyRelatedBreakpoints(Assembly assembly)
		{
			int index = 0;

			while (index < FunctionBreakpoints.Count)
			{
				FunctionBreakpointInformation breakpoint = FunctionBreakpoints[index];

				if (breakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly == assembly)
				{
					breakpoint.Remove();
					FunctionBreakpoints.Remove(breakpoint);
					UIHandler.Instance.RemoveBreakpoint(breakpoint);
				}
				else
				{
					index++;
				}
			}
		}
	}
}