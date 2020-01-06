using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Metadata;

namespace Dile.UI.Debug
{
	public class FieldValueRefresher : BaseValueRefresher
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

		public FieldValueRefresher(string name, BaseValueRefresher parentObject, ClassWrapper classObject, uint fieldToken) : base(name)
		{
			ParentObject = parentObject;
			ClassObject = classObject;
			FieldToken = fieldToken;
			IsObjectValue = true;
		}

		public override ValueWrapper GetRefreshedValue()
		{
			ValueWrapper result = null;
			ValueWrapper parent = ParentObject.GetRefreshedValue();
			ValueWrapper dereferencedParent = parent;

			if (parent != null)
			{
				CorElementType parentElementType = (CorElementType)parent.ElementType;

				if (parentElementType == CorElementType.ELEMENT_TYPE_BYREF ||
					parentElementType == CorElementType.ELEMENT_TYPE_CLASS ||
					parentElementType == CorElementType.ELEMENT_TYPE_OBJECT ||
					parentElementType == CorElementType.ELEMENT_TYPE_PTR)
				{
					dereferencedParent = parent.DereferenceValue();
				}
			}

			if (dereferencedParent != null)
			{
				result = dereferencedParent.GetFieldValue(ClassObject, FieldToken);
			}

			return result;
		}
	}
}