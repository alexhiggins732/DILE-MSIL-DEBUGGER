using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Metadata;
using System.Threading;

namespace Dile.UI.Debug
{
	public class PropertyValueRefresher : BaseValueRefresher
	{
		private EvaluationHandler propertyGetMethodCaller;
		private EvaluationHandler PropertyGetMethodCaller
		{
			get
			{
				return propertyGetMethodCaller;
			}
			set
			{
				propertyGetMethodCaller = value;
			}
		}

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

		private FunctionWrapper propertyGetMethod;
		private FunctionWrapper PropertyGetMethod
		{
			get
			{
				return propertyGetMethod;
			}
			set
			{
				propertyGetMethod = value;
			}
		}

		private List<ValueWrapper> propertyGetMethodArguments;
		private List<ValueWrapper> PropertyGetMethodArguments
		{
			get
			{
				return propertyGetMethodArguments;
			}
			set
			{
				propertyGetMethodArguments = value;
			}
		}

		public PropertyValueRefresher(string name, EvaluationHandler propertyGetMethodCaller, FunctionWrapper propertyGetMethod, List<ValueWrapper> propertyGetMethodArguments, BaseValueRefresher parentObject)
			: base(name)
		{
			PropertyGetMethodCaller = propertyGetMethodCaller;
			PropertyGetMethod = propertyGetMethod;
			PropertyGetMethodArguments = propertyGetMethodArguments;
			ParentObject = parentObject;
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

			if (parent != null)
			{
				List<TypeWrapper> typeArguments = null;

				if (parent.IsVersion2)
				{
					TypeWrapper parentExactType = parent.Version2.GetExactType();
					typeArguments = parentExactType.EnumerateTypeParameters();
				}

				BaseEvaluationResult propertyResult = PropertyGetMethodCaller.CallFunction(PropertyGetMethod, PropertyGetMethodArguments, typeArguments);

				if (propertyResult.IsSuccessful)
				{
					result = propertyResult.Result;
				}
			}

			return result;
		}
	}
}