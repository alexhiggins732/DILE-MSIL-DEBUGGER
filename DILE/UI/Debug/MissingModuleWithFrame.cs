using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class MissingModuleWithFrame : MissingModule
	{
		public FrameWrapper Frame
		{
			get;
			private set;
		}

		public FrameRefresher FrameRefresher
		{
			get;
			private set;
		}

		public MissingModuleWithFrame(ModuleWrapper module, ThreadWrapper thread, bool isActiveFrame, FrameWrapper frame)
			: base(module)
		{
			Frame = frame;
			FrameRefresher = new FrameRefresher(thread, frame.ChainIndex, frame.FrameIndex, isActiveFrame);
		}
	}
}