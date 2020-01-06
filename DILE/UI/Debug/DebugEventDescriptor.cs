using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public class DebugEventDescriptor
	{
		private Dictionary<string, string> eventValueParameters = null;
		private Dictionary<string, string> EventValueParameters
		{
			get
			{
				return eventValueParameters;
			}
			set
			{
				eventValueParameters = value;
			}
		}

		private Dictionary<string, Dictionary<string, string>> eventObjectParameters = null;
		private Dictionary<string, Dictionary<string, string>> EventObjectParameters
		{
			get
			{
				return eventObjectParameters;
			}
			set
			{
				eventObjectParameters = value;
			}
		}

		private DebugEventType debugEventType;
		public DebugEventType DebugEventType
		{
			get
			{
				return debugEventType;
			}
			private set
			{
				debugEventType = value;
			}
		}

		private string AsString
		{
			get;
			set;
		}

		public DebugEventDescriptor(DebugEventType debugEventType)
		{
			DebugEventType = debugEventType;

			DateTime now = DateTime.Now;
			AsString = "[" + now.ToString("HH:mm:ss.fff") + "]: " + Convert.ToString(DebugEventType);
			FillEventObjectParameters();
			FillEventValueParameters();
		}

		private void AddEventValueParameter<T>(string valueName, Nullable<T> value) where T : struct
		{
			if (value != null && value.HasValue)
			{
				if (EventValueParameters == null)
				{
					EventValueParameters = new Dictionary<string, string>();
				}

				EventValueParameters[valueName] = value.ToString();
			}
		}

		private void AddEventValueParameter(string valueName, string value)
		{
			if (value != null)
			{
				if (EventValueParameters == null)
				{
					EventValueParameters = new Dictionary<string, string>();
				}

				EventValueParameters[valueName] = value;
			}
		}

		private void FillEventValueParameters()
		{
			AddEventValueParameter<bool>("Accurate", DebugEventHandler.Instance.EventObjects.Accurate);
			AddEventValueParameter<uint>("Connection ID", DebugEventHandler.Instance.EventObjects.ConnectionID);
			AddEventValueParameter("Connection Name", DebugEventHandler.Instance.EventObjects.ConnectionName);
			AddEventValueParameter<uint>("Error Code", DebugEventHandler.Instance.EventObjects.ErrorCode);
			AddEventValueParameter<uint>("Error HRESULT", DebugEventHandler.Instance.EventObjects.ErrorHResult);
			AddEventValueParameter<uint>("Event Type", DebugEventHandler.Instance.EventObjects.EventType);
			AddEventValueParameter<uint>("Flags", DebugEventHandler.Instance.EventObjects.Flags);
			AddEventValueParameter<bool>("Unhandled Exception", DebugEventHandler.Instance.EventObjects.IsUnhandledException);
			AddEventValueParameter<int>("Log Level", DebugEventHandler.Instance.EventObjects.LogLevel);
			AddEventValueParameter("Log Parent Name", DebugEventHandler.Instance.EventObjects.LogParentName);
			AddEventValueParameter<uint>("Log Reason", DebugEventHandler.Instance.EventObjects.LogReason);
			AddEventValueParameter("Log Switch Name", DebugEventHandler.Instance.EventObjects.LogSwitchName);
			AddEventValueParameter("Message", DebugEventHandler.Instance.EventObjects.Message);
			AddEventValueParameter<uint>("Offset", DebugEventHandler.Instance.EventObjects.Offset);
			AddEventValueParameter<uint>("Step Reason", DebugEventHandler.Instance.EventObjects.StepReason);
		}

		private void FillEventObjectParameters()
		{
			//ToDo Display information about: breakpoint, eval, stepper.
			//ToDo Display enum names, not uint values.
			EventObjectParameters = new Dictionary<string, Dictionary<string, string>>();

			if (DebugEventHandler.Instance.EventObjects.AppDomain != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["ID"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.AppDomain.GetID());
				objectInfo["Name"] = DebugEventHandler.Instance.EventObjects.AppDomain.GetName();
				EventObjectParameters["AppDomain"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Assembly != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Name"] = DebugEventHandler.Instance.EventObjects.Assembly.GetName();
				EventObjectParameters["Assembly"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.ClassObject != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Module Name"] = DebugEventHandler.Instance.EventObjects.ClassObject.GetModule().GetName();
				objectInfo["Token"] = string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(DebugEventHandler.Instance.EventObjects.ClassObject.GetToken(), 8));
				EventObjectParameters["Class"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Frame != null && DebugEventHandler.Instance.EventObjects.Frame.IsILFrame())
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Function Token"] = string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(DebugEventHandler.Instance.EventObjects.Frame.GetFunctionToken(), 8));
				objectInfo["IL Frame"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Frame.IsILFrame());
				EventObjectParameters["Frame"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Function != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Module Name"] = DebugEventHandler.Instance.EventObjects.Function.GetModule().GetName();
				objectInfo["Token"] = string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(DebugEventHandler.Instance.EventObjects.Function.GetToken(), 8));
				EventObjectParameters["Function"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Mda != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Name"] = DebugEventHandler.Instance.EventObjects.Mda.GetName();
				objectInfo["Description"] = DebugEventHandler.Instance.EventObjects.Mda.GetDescription();
				objectInfo["Flags"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Mda.GetFlags());
				objectInfo["OS Thread ID"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Mda.GetOSThreadID());
				objectInfo["Xml"] = DebugEventHandler.Instance.EventObjects.Mda.GetXml();
				EventObjectParameters["MDA"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Module != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Token"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Module.GetToken());
				objectInfo["Name"] = DebugEventHandler.Instance.EventObjects.Module.GetName();
				objectInfo["Base Address"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Module.GetBaseAddress());
				objectInfo["Size"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Module.GetSize());
				objectInfo["Dynamic"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Module.IsDynamic());
				objectInfo["In Memory"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Module.IsInMemory());
				EventObjectParameters["Module"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.NewFunction != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["Module Name"] = DebugEventHandler.Instance.EventObjects.NewFunction.GetModule().GetName();
				objectInfo["Token"] = string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(DebugEventHandler.Instance.EventObjects.NewFunction.GetToken(), 8));
				EventObjectParameters["New Function"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Process != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				try
				{
					objectInfo["Helper Thread ID"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Process.GetHelperThreadID());
				}
				catch (COMException comException)
				{
					if ((uint)comException.ErrorCode != 0x80131c49)
					{
						throw;
					}
				}
				objectInfo["ID"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Process.GetID());
				EventObjectParameters["Process"] = objectInfo;
			}

			if (DebugEventHandler.Instance.EventObjects.Thread != null)
			{
				Dictionary<string, string> objectInfo = new Dictionary<string, string>();
				objectInfo["ID"] = Convert.ToString(DebugEventHandler.Instance.EventObjects.Thread.GetID());
				EventObjectParameters["Thread"] = objectInfo;
			}
		}

		private TreeNode AddTreeNode(TreeNodeCollection parentNodeCollection, string nodeName, string nodeValue)
		{
			string nodeText = string.Format("{0}: {1}", nodeName, nodeValue);

			return AddTreeNode(parentNodeCollection, nodeText);
		}

		private TreeNode AddTreeNode(TreeNodeCollection parentNodeCollection, string nodeText)
		{
			TreeNode result = new TreeNode(nodeText);
			parentNodeCollection.Add(result);

			return result;
		}

		private void AddTreeNodes(TreeNodeCollection parentNodeCollection, Dictionary<string, string> subNodeInfo)
		{
			if (subNodeInfo != null)
			{
				foreach (KeyValuePair<string, string> objectProperty in subNodeInfo)
				{
					AddTreeNode(parentNodeCollection, objectProperty.Key, objectProperty.Value);
				}
			}
		}

		public void CreateTree(TreeNodeCollection parentNodeCollection)
		{
			if (EventObjectParameters != null)
			{
				foreach (KeyValuePair<string, Dictionary<string, string>> objectInfo in EventObjectParameters)
				{
					TreeNode objectInfoNode = AddTreeNode(parentNodeCollection, objectInfo.Key);
					AddTreeNodes(objectInfoNode.Nodes, objectInfo.Value);
				}
			}

			if (EventValueParameters != null)
			{
				AddTreeNodes(parentNodeCollection, EventValueParameters);
			}
		}

		public override string ToString()
		{
			return AsString;
		}
	}
}