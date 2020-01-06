using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class LocalVariableRefresher : BaseValueRefresher
	{
		private FrameRefresher frameRefresher;
		public FrameRefresher FrameRefresher
		{
			get
			{
				return frameRefresher;
			}
			set
			{
				frameRefresher = value;
			}
		}

		private uint index;
		public uint Index
		{
			get
			{
				return index;
			}
			set
			{
				index = value;
			}
		}

		public LocalVariableRefresher(string name, FrameRefresher frameRefresher, uint index) : base(name)
		{
			FrameRefresher = frameRefresher;
			Index = index;
			IsObjectValue = false;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			ValueWrapper result = null;

			FrameWrapper frame = FrameRefresher.GetRefreshedValue();
			result = frame.GetLocalVariable(index);

			return result;
		}
	}
}