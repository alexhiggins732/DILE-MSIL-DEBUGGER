using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.ExtensionMethods.ReaderWriterLockSlim;
using Dile.Metadata;
using System.IO;
using System.Threading;

namespace Dile.UI
{
	public class AssemblyLoader
	{
		public event AssembliesLoadedDelegate AssembliesLoaded;

		private static AssemblyLoader instance = new AssemblyLoader();
		public static AssemblyLoader Instance
		{
			get
			{
				return instance;
			}
		}

		private static Action<ModuleWrapper, Assembly> reopenMetadataInterfaces = new Action<ModuleWrapper, Assembly>((module, assembly) => assembly.ReopenMetadataInterfaces(module));
		private static Action<ModuleWrapper, Assembly> ReopenMetadataInterfaces
		{
			get
			{
				return reopenMetadataInterfaces;
			}
		}

		private bool ShuttingDown
		{
			get;
			set;
		}

		private ReaderWriterLockSlim RequestsLock
		{
			get;
			set;
		}

		private Queue<AssemblyLoadRequest> Requests
		{
			get;
			set;
		}

		private AutoResetEvent Signaller
		{
			get;
			set;
		}

		private Thread WorkerThread
		{
			get;
			set;
		}

		private AssemblyLoader()
		{
			RequestsLock = new ReaderWriterLockSlim();
			Requests = new Queue<AssemblyLoadRequest>();
			Signaller = new AutoResetEvent(false);

			WorkerThread = new Thread(WorkerThreadStart);
			WorkerThread.Name = "Assembly loading thread";
			WorkerThread.Start();
		}

		private void WorkerThreadStart()
		{
			do
			{
				if (Requests.Count > 0)
				{
					List<Assembly> loadedAssemblies = new List<Assembly>();
					AssemblyLoadRequest request = RequestsLock.Read(() => Requests.Dequeue());

					while (!ShuttingDown && request != null)
					{
						try
						{
							Assembly loadedAssembly = null;
							if (request.Module == null)
							{
								loadedAssembly = LoadAssembly(request.FilePath);
							}
							else
							{
								loadedAssembly = LoadAssembly(request.Module);
								UIHandler.Instance.MainForm.Invoke(new NoArgumentsDelegate(() => ReopenMetadataInterfaces(request.Module, loadedAssembly)));
							}

							loadedAssemblies.Add(loadedAssembly);
						}
						catch (Exception exception)
						{
							UIHandler.Instance.DisplayUserWarning("An error occurred while loading the assembly.");
							UIHandler.Instance.ShowException(exception);
						}

						UIHandler.Instance.SetProgressText("\n\n", true);
						if (Requests.Count > 0)
						{
							request = RequestsLock.Read(() => Requests.Dequeue());
						}
						else
						{
							request = null;
						}
					}

					UIHandler.Instance.ResetProgressBar();
					UIHandler.Instance.AssembliesLoaded(loadedAssemblies, true);

					if (!ShuttingDown && AssembliesLoaded != null)
					{
						AssembliesLoaded(loadedAssemblies, true);
					}
				}

				Signaller.WaitOne();
			} while (!ShuttingDown);
		}

		public void StartAsync(AssemblyLoadRequest[] requestedAssemblies)
		{
			RequestsLock.Write(() =>
			{
				foreach (AssemblyLoadRequest request in requestedAssemblies)
				{
					Requests.Enqueue(request);
				}
			});

			Signaller.Set();
		}

		public void ShutDown()
		{
			ShuttingDown = true;
			Signaller.Set();
		}

		private Assembly LoadAssembly(string filePath)
		{
			Assembly result = new Assembly(false, false);
			result.FullPath = filePath;
			result.LoadAssembly();

			return result;
		}

		private Assembly LoadAssembly(ModuleWrapper module)
		{
			Assembly result = null;
			string fileName = module.GetName();
			string name = fileName;

			try
			{
				name = Path.GetFileNameWithoutExtension(fileName);
			}
			catch
			{
			}

			result = new Assembly(true, module.IsInMemory());
			result.FileName = fileName;
			result.Name = name;
			MetaDataDispenserEx dispenser = new MetaDataDispenserEx();

			result.LoadAssemblyFromMetadataInterfaces(dispenser,
				(IMetaDataAssemblyImport)module.GetMetaDataAssemblyImport(),
				(IMetaDataImport2)module.GetMetaDataImport2(),
				module);

			return result;
		}
	}
}