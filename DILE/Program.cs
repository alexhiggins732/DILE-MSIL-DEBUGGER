using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Dile.Disassemble;
using Dile.UI;
using System.Linq;

namespace Dile
{
	public static class Program
	{
		private const string Instructions = @"Usage:
dile [/p ""Project name""] [/a ""assembly path""] [/l ""project name.dileproj""] [/i process_id]

	/p	Optional. When DILE is loaded a new project will be created with the given name.
	/a	Optional, can be repeated. When DILE is loaded a new project will be created and the given assemblies will be added to it.
	/l	Optional. DILE will load the given dileproj file. If this parameter is given then /p and /a will be ignored.
	/i	Optional. DILE will attach to the process with given process id.

If a parameter is followed by a name/path which contains spaces then it should be written between quotes.

Examples:
Create a new project with the name Test project:
dile /p ""Test project""

Create a new project called Test project and add the TestAssembly.exe to it:
dile /p ""Test project"" /a TestAssembly.exe

Create a new project and add the TestAssembly.exe and another My test.dll from a different directory:
dile /a TestAssembly.exe /a ""c:\assemblies\My test.dll""

Load an existing project:
dile /l TestProject.dileproj

Attach to process with id 1234:
dile /i 1234";

		private static void ShowParameters()
		{
			MessageBox.Show(Instructions);
		}

		private static void ProcessArguments(string[] arguments, out Project project, out AssemblyLoadRequest[] assembliesToLoad, out int? processToAttachTo)
		{
			project = new Project();
			processToAttachTo = null;
			assembliesToLoad = null;
			bool wrongArgumentFound = false;
			int index = 1;
			bool addAssembly = false;
			bool loadProject = false;
			bool attach = false;
			List<string> pathOfAssemblies = new List<string>();

			while (!wrongArgumentFound && index < arguments.Length)
			{
				string argument = arguments[index++].ToLowerInvariant();

				if (argument == "/a")
				{
					addAssembly = true;
				}
				else if (argument == "/p")
				{
					addAssembly = false;
				}
				else if (argument == "/l")
				{
					loadProject = true;
				}
				else if (argument == "/i")
				{
					attach = true;
				}
				else
				{
					wrongArgumentFound = true;
				}

				if (!wrongArgumentFound)
				{
					if (index < arguments.Length)
					{
						if (loadProject)
						{
							project.FullPath = arguments[index++];
							project.IsSaved = true;
						}
						else if (addAssembly)
						{
							pathOfAssemblies.Add(arguments[index++]);
						}
						else if (attach)
						{
							processToAttachTo = int.Parse(arguments[index++]);
						}
						else
						{
							project.Name = arguments[index++];
						}
					}
					else
					{
						wrongArgumentFound = true;
					}
				}
			}

			if (wrongArgumentFound)
			{
				ShowParameters();
				project = null;
			}
			else
			{
				assembliesToLoad = pathOfAssemblies.Select(assemblyPath => new AssemblyLoadRequest(assemblyPath))
					.ToArray();

				if (!loadProject && string.IsNullOrEmpty(project.Name))
				{
					project.Name = "New project";
				}
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Dile.Metadata.OpCodeGroups.Initialize();
			Application.EnableVisualStyles();

			MainForm mainForm = new MainForm();
			string[] arguments = Environment.GetCommandLineArgs();

#if DEBUG
			try
			{
#endif
				if (arguments.Length > 1)
				{
					Project project;
					AssemblyLoadRequest[] assembliesToLoad;
					int? processToAttachTo;
					ProcessArguments(arguments, out project, out assembliesToLoad, out processToAttachTo);

					if (project != null)
					{
						mainForm.ProjectToLoad = project;
						mainForm.AssembliesToLoad = assembliesToLoad;
						mainForm.ProcessToAttachTo = processToAttachTo;

						Application.Run(mainForm);
					}
				}
				else
				{
					Application.Run(mainForm);
				}
#if DEBUG
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception);
			}
#endif
		}
	}
}