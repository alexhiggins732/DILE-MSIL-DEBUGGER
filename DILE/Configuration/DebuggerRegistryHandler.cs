using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;
using System.Diagnostics;
using System.Reflection;

namespace Dile.Configuration
{
	public class DebuggerRegistryHandler
	{
		private const string DefaultDebuggerKey = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\AeDebug";
		private const string DefaultDebuggerValue = "Debugger";
		private const string VS7JitFileName = "VS7JIT.EXE";
		private const string VSJitDebuggerFileName = "VSJITDEBUGGER.EXE";

		private string defaultDebugger;
		public string DefaultDebugger
		{
			get
			{
				if (string.IsNullOrEmpty(defaultDebugger))
				{
					RegistryKey defaultDebuggerKey = null;

					try
					{
						defaultDebuggerKey = Registry.LocalMachine.OpenSubKey(DefaultDebuggerKey);
						defaultDebugger = (string)defaultDebuggerKey.GetValue(DefaultDebuggerValue);
					}
					finally
					{
						if (defaultDebuggerKey != null)
						{
							defaultDebuggerKey.Close();
						}
					}
				}

				return defaultDebugger;
			}
		}

		public bool IsVsJitDefaultDebugger
		{
			get
			{
				string defaultDebuggerUpperCase = DefaultDebugger.ToUpperInvariant();

				return (defaultDebuggerUpperCase.Contains(VS7JitFileName) || defaultDebuggerUpperCase.Contains(VSJitDebuggerFileName));
			}
		}

		public DebuggerRegistryHandler()
		{
		}

		private string ExtractDefaultDebuggerPath()
		{
			string defaultDebuggerUpperCase = DefaultDebugger.ToUpperInvariant();
			int pathEndIndex = defaultDebuggerUpperCase.IndexOf(VSJitDebuggerFileName);

			if (pathEndIndex < 0)
			{
				pathEndIndex = defaultDebuggerUpperCase.IndexOf(VS7JitFileName);
				pathEndIndex += VS7JitFileName.Length - 1;
			}
			else
			{
				pathEndIndex += VSJitDebuggerFileName.Length - 1;
			}

			int pathStartIndex = defaultDebuggerUpperCase.LastIndexOf('"', pathEndIndex);

			if (pathStartIndex < 0)
			{
				pathStartIndex = 0;
			}

			return defaultDebuggerUpperCase.Substring(pathStartIndex, pathEndIndex + 1).Trim('"');
		}

		public void RegisterDileWithJitDebugger()
		{
			if (!IsVsJitDefaultDebugger)
			{
				throw new InvalidOperationException();
			}

			string defaultDebuggerPath = ExtractDefaultDebuggerPath();

			System.Diagnostics.Debug.WriteLine(defaultDebuggerPath);

			Process defaultDebuggerProcess = new Process();
			defaultDebuggerProcess.StartInfo.Arguments = string.Format("/registerold \"{0}\" \"DILE (Dotnet IL Editor) Path: {0}\"", Assembly.GetEntryAssembly().Location);
			defaultDebuggerProcess.StartInfo.CreateNoWindow = false;
			defaultDebuggerProcess.StartInfo.FileName = defaultDebuggerPath;

			defaultDebuggerProcess.Start();
		}
	}
}