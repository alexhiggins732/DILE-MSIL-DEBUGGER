using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;

namespace Dile.UI.Debug
{
	public class FrameInformation
	{
		private MethodDefinition methodDefinition;
		public MethodDefinition MethodDefinition
		{
			get
			{
				return methodDefinition;
			}
			private set
			{
				methodDefinition = value;
			}
		}

		private int offset;
		public int Offset
		{
			get
			{
				return offset;
			}
			private set
			{
				offset = value;
			}
		}

		private bool isExactLocation;
		public bool IsExactLocation
		{
			get
			{
				return isExactLocation;
			}
			private set
			{
				isExactLocation = value;
			}
		}

		private bool isActiveFrame;
		public bool IsActiveFrame
		{
			get
			{
				return isActiveFrame;
			}
			private set
			{
				isActiveFrame = value;
			}
		}

		private FrameWrapper frame;
		public FrameWrapper Frame
		{
			get
			{
				return frame;
			}
			private set
			{
				frame = value;
			}
		}

		private FrameRefresher refresher;
		public FrameRefresher Refresher
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

		public FrameInformation(ThreadWrapper thread, MethodDefinition methodDefinition, bool isActiveFrame, FrameWrapper frame)
		{
			MethodDefinition = methodDefinition;
			IsActiveFrame = isActiveFrame;
			Frame = frame;

			if (frame.IsILFrame())
			{
				Offset = Convert.ToInt32(frame.GetIP(ref isExactLocation));
			}
			else
			{
				Offset = -1;
			}

			Refresher = new FrameRefresher(thread, Frame.ChainIndex, Frame.FrameIndex, IsActiveFrame);
		}

		public void RefreshFrame()
		{
			Frame = Refresher.GetRefreshedValue();
		}
	}
}