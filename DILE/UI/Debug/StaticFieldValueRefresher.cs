using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class StaticFieldValueRefresher : BaseValueRefresher
	{
		private ClassWrapper classObject;
		private ClassWrapper ClassObject
		{
			get
			{
				return classObject;
			}
			set
			{
				classObject = value;
			}
		}

		private FrameRefresher frameRefresher;
		private FrameRefresher FrameRefresher
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

		private uint fieldToken;
		private uint FieldToken
		{
			get
			{
				return fieldToken;
			}
			set
			{
				fieldToken = value;
			}
		}

		public StaticFieldValueRefresher(string name, ClassWrapper classObject, FrameRefresher frameRefresher, uint fieldToken)
			: base(name)
		{
			ClassObject = classObject;
			FrameRefresher = frameRefresher;
			FieldToken = fieldToken;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			FrameWrapper frame = FrameRefresher.GetRefreshedValue();
			ValueWrapper result = ClassObject.GetStaticFieldValue(FieldToken, frame);

			return result;
		}
	}
}