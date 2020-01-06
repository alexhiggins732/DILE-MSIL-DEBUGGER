using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using System.Windows.Forms;

namespace Dile.UI.Debug
{
	public class TreeNodeRefresherConnection
	{
		private TreeNode treeNode;
		public TreeNode TreeNode
		{
			get
			{
				return treeNode;
			}
			private set
			{
				treeNode = value;
			}
		}

		private BaseValueRefresher refresher;
		public BaseValueRefresher Refresher
		{
			get
			{
				return refresher;
			}
			private set
			{
				refresher = value;
			}
		}

		private FrameRefresher frameRefresher;
		public FrameRefresher FrameRefresher
		{
			get
			{
				return frameRefresher;
			}
			private set
			{
				frameRefresher = value;
			}
		}

		private bool reformattableNumber;
		public bool ReformattableNumber
		{
			get
			{
				return reformattableNumber;
			}
			private set
			{
				reformattableNumber = value;
			}
		}

		private object debugValue;
		private object DebugValue
		{
			get
			{
				return debugValue;
			}
			set
			{
				debugValue = value;
			}
		}

		public TreeNodeRefresherConnection(object debugValue, TreeNode treeNode, BaseValueRefresher refresher, FrameRefresher frame, bool reformattableNumber)
		{
			TreeNode = treeNode;
			Refresher = refresher;
			FrameRefresher = frameRefresher;
			ReformattableNumber = reformattableNumber;

			if (ReformattableNumber)
			{
				DebugValue = debugValue;
			}
		}

		public string GetReformattedNumber()
		{
			string result = string.Empty;

			if (ReformattableNumber)
			{
				result = HelperFunctions.FormatNumber(DebugValue);
			}

			return result;
		}
	}
}