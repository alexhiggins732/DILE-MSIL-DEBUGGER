using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class FrameRefresher
	{
		private ThreadWrapper thread;
		public ThreadWrapper Thread
		{
			get
			{
				return thread;
			}
			private set
			{
				thread = value;
			}
		}

		private int chainIndex;
		private int ChainIndex
		{
			get
			{
				return chainIndex;
			}
			set
			{
				chainIndex = value;
			}
		}

		private int frameIndex;
		private int FrameIndex
		{
			get
			{
				return frameIndex;
			}
			set
			{
				frameIndex = value;
			}
		}

		private bool isActiveFrame;
		private bool IsActiveFrame
		{
			get
			{
				return isActiveFrame;
			}
			set
			{
				isActiveFrame = value;
			}
		}

		public FrameRefresher(ThreadWrapper thread, int chainIndex, int frameIndex, bool isActiveFrame)
		{
			Thread = thread;
			ChainIndex = chainIndex;
			FrameIndex = frameIndex;
			IsActiveFrame = isActiveFrame;
		}

		private FrameWrapper FindFrame()
		{
			FrameWrapper result = null;

			if (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging)
			{
				result = Thread.Version3.StackWalk()[FrameIndex];
			}
			else
			{
				result = Thread.FindFrame(ChainIndex, FrameIndex);
			}

			return result;
		}

		public FrameWrapper GetRefreshedValue()
		{
			FrameWrapper result = null;

			if (IsActiveFrame)
			{
				if (DebugEventHandler.Instance.State == DebuggerState.DumpDebugging)
				{
					result = Thread.Version3.GetActiveFrame();
				}
				else
				{
					result = Thread.GetActiveFrame();
				}

				if (result == null)
				{
					result = FindFrame();
				}
			}
			else
			{
				result = FindFrame();
			}

			return result;
		}
	}
}