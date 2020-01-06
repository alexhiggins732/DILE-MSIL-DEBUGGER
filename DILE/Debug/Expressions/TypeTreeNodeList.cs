using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Debug.Expressions
{
	public class TypeTreeNodeList
	{
		private List<TypeTreeNode> typeTreeNodes;
		public List<TypeTreeNode> TypeTreeNodes
		{
			get
			{
				return typeTreeNodes;
			}
			private set
			{
				typeTreeNodes = value;
			}
		}

		public TypeTreeNodeList()
		{
			TypeTreeNodes = new List<TypeTreeNode>();
		}

		public TypeTreeNodeList(List<TypeTreeNode> typeTreeNodes)
		{
			TypeTreeNodes = typeTreeNodes;
		}

		public List<TypeWrapper> GetAsTypeWrapperList(EvaluationContext context)
		{
			List<TypeWrapper> result = new List<TypeWrapper>(TypeTreeNodes.Count);

			for (int index = 0; index < TypeTreeNodes.Count; index++)
			{
				TypeTreeNode typeArgument = TypeTreeNodes[index];

				result.Add(typeArgument.GetTreeAsTypeWrapper(context));
			}

			return result;
		}

		public GenericMethodParameter GetItemAsGenericParameter(int index)
		{
			GenericMethodParameter result = new GenericMethodParameter();
			TypeTreeNode typeNode = TypeTreeNodes[index];

			result.ElementType = HelperFunctions.GetElementTypeByName(typeNode.TypeDefinition.FullName);

			if (result.ElementType == CorElementType.ELEMENT_TYPE_END)
			{
				if (typeNode.TypeDefinition.IsValueType)
				{
					result.ElementType = CorElementType.ELEMENT_TYPE_VALUETYPE;
				}
				else if (typeNode.IsArray)
				{
					result.ElementType = CorElementType.ELEMENT_TYPE_SZARRAY;
				}
				else
				{
					result.ElementType = CorElementType.ELEMENT_TYPE_CLASS;
				}
			}

			if (typeNode.IsArray)
			{
				//result.ArrayElementType = new GenericMethodParameter(
			}

			result.TokenObject = typeNode.TypeDefinition;

			return result;
		}
	}
}