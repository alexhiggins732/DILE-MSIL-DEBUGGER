using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dile.UI
{
	public class ProcessDescription
	{
		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		[DllImport("advapi32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool OpenProcessToken(IntPtr processHandle, DesiredAccess desiredAccess, out IntPtr tokenHandle);

		[DllImport("advapi32.dll", SetLastError = true)]
		static extern bool GetTokenInformation(IntPtr tokenHandle,
			TokenInformationClass tokenInformationClass,
			IntPtr tokenInformation,
			uint tokenInformationLength,
			out uint returnLength);

		[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool LookupAccountSid(string lpSystemName,
			IntPtr sid,
			StringBuilder lpName,
			ref uint cchName,
			StringBuilder referencedDomainName,
			ref uint cchReferencedDomainName,
			out SidNameUse peUse);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool CloseHandle(IntPtr hHandle);

		private int id;
		public int ID
		{
			get
			{
				return id;
			}
			private set
			{
				id = value;
			}
		}

		private string name;
		public string Name
		{
			get
			{
				return name;
			}
			private set
			{
				name = value;
			}
		}

		private string fileName;
		public string FileName
		{
			get
			{
				return fileName;
			}
			private set
			{
				fileName = value;
			}
		}

		private string mainWindowTitle;
		public string MainWindowTitle
		{
			get
			{
				return mainWindowTitle;
			}
			private set
			{
				mainWindowTitle = value;
			}
		}

		private string userName;
		public string UserName
		{
			get
			{
				return userName;
			}
			private set
			{
				userName = value;
			}
		}

		private string framework;
		public string Framework
		{
			get
			{
				return framework;
			}
			private set
			{
				framework = value;
			}
		}

		public bool IsManaged
		{
			get;
			private set;
		}

		public ProcessDescription(MetaHost metaHost, Process process)
		{
			ID = process.Id;
			Name = process.ProcessName;
			MainWindowTitle = process.MainWindowTitle;
			FileName = TryRetrieve(() => process.MainModule.FileName, "N/A");
			UserName = TryRetrieve(() => GetUsername(process.Handle), "N/A");
			Framework = TryRetrieve(() => GetFrameworks(metaHost, process), "N/A");
		}

		private T TryRetrieve<T>(Func<T> retrieval, T defaultValue)
		{
			T result = defaultValue;

			try
			{
				result = retrieval();
			}
			catch
			{
			}

			return result;
		}

		private string GetFrameworks(MetaHost metaHost, Process process)
		{
			StringBuilder result = new StringBuilder();
			RuntimeInfo[] runtimeInfos = null;

			try
			{
				runtimeInfos = metaHost.EnumerateLoadedRuntimes(process.Handle);

				for (int index = 0; index < runtimeInfos.Length; index++)
				{
					RuntimeInfo runtimeInfo = runtimeInfos[index];
					IsManaged = true;

					if (index > 0)
					{
						result.Append(", ");
					}

					result.Append(runtimeInfo.GetVersionString());
				}
			}
			finally
			{
				if (runtimeInfos != null)
				{
					foreach (RuntimeInfo runtimeInfo in runtimeInfos)
					{
						try
						{
							runtimeInfo.Dispose();
						}
						catch
						{
						}
					}
				}
			}

			return result.ToString();
		}

		private string GetUsername(IntPtr processHandle)
		{
			string result = string.Empty;
			IntPtr tokenHandle;

			if (OpenProcessToken(processHandle, DesiredAccess.TokenQuery, out tokenHandle))
			{
				const int bufferLength = 256;
				IntPtr tokenInformation = Marshal.AllocHGlobal(bufferLength);
				uint returnLength;

				if (GetTokenInformation(tokenHandle, TokenInformationClass.TokenUser, tokenInformation, bufferLength, out returnLength))
				{
					TokenUser tokenUser = (TokenUser)Marshal.PtrToStructure(tokenInformation, typeof(TokenUser));

					StringBuilder name = new StringBuilder();
					uint nameCapacity = (uint)name.Capacity;
					StringBuilder domainName = new StringBuilder();
					uint domainNameCapacity = (uint)name.Capacity;
					SidNameUse sidNameUse;

					int errorCode = 0;

					if (!LookupAccountSid(null, tokenUser.User.Sid, name, ref nameCapacity, domainName, ref domainNameCapacity, out sidNameUse))
					{
						errorCode = Marshal.GetLastWin32Error();

						if (errorCode == ERROR_INSUFFICIENT_BUFFER)
						{
							name.EnsureCapacity((int)nameCapacity);
							domainName.EnsureCapacity((int)domainNameCapacity);

							if (!LookupAccountSid(null, tokenUser.User.Sid, name, ref nameCapacity, domainName, ref domainNameCapacity, out sidNameUse))
							{
								errorCode = Marshal.GetLastWin32Error();
							}
						}
					}

					if (errorCode == 0)
					{
						result = domainName + "\\" + name;
					}
					else
					{
						result = "Error code: " + errorCode;
					}
				}
			}

			return result;
		}
	}
}