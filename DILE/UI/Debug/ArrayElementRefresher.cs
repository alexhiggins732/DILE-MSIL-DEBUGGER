using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;

namespace Dile.UI.Debug
{
	public class ArrayElementRefresher : BaseValueRefresher
	{
		private BaseValueRefresher parentObject;
		private BaseValueRefresher ParentObject
		{
			get
			{
				return parentObject;
			}
			set
			{
				parentObject = value;
			}
		}

		private uint elementIndex;
		private uint ElementIndex
		{
			get
			{
				return elementIndex;
			}
			set
			{
				elementIndex = value;
			}
		}

		public ArrayElementRefresher(string name, BaseValueRefresher parentObject, uint elementIndex) : base(name)
		{
			ParentObject = parentObject;
			ElementIndex = elementIndex;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			ValueWrapper parent = ParentObject.GetRefreshedValue();
			ArrayValueWrapper array = parent.ConvertToArrayValue();
			ValueWrapper element = array.GetElementAtPosition(ElementIndex);

			return element;
		}
	}
}