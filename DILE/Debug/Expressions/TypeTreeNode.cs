using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;

namespace Dile.Debug.Expressions
{
	public class TypeTreeNode
	{
		private bool isArray;
		public bool IsArray
		{
			get
			{
				return isArray;
			}
			private set
			{
				isArray = value;
			}
		}

		private TypeDefinition typeDefinition;
		public TypeDefinition TypeDefinition
		{
			get
			{
				return typeDefinition;
			}
			private set
			{
				typeDefinition = value;
			}
		}

		private List<TypeTreeNode> childNodes;
		public List<TypeTreeNode> ChildNodes
		{
			get
			{
				if (childNodes == null)
				{
					childNodes = new List<TypeTreeNode>();
				}

				return childNodes;
			}
		}

		public TypeTreeNode(TypeDefinition typeDefinition, bool isArray)
		{
			TypeDefinition = typeDefinition;
			IsArray = isArray;
		}

		private void AddNodeAsString(StringBuilder treeBuilder)
		{
			treeBuilder.Append(TypeDefinition.Name);

			if (ChildNodes.Count > 0)
			{
				treeBuilder.Append("[");

				for (int index = 0; index < ChildNodes.Count; index++)
				{
					TypeTreeNode childNode = ChildNodes[index];
					childNode.AddNodeAsString(treeBuilder);

					if (index < ChildNodes.Count - 1)
					{
						treeBuilder.Append(", ");
					}
				}

				treeBuilder.Append("]");
			}
		}

		public string GetTreeAsString()
		{
			StringBuilder result = new StringBuilder();

			AddNodeAsString(result);

			return result.ToString();
		}

		public TypeWrapper GetTreeAsTypeWrapper(EvaluationContext context)
		{
			TypeWrapper result = null;
			List<TypeWrapper> typeArguments = new List<TypeWrapper>();

			if (ChildNodes.Count > 0)
			{
				for (int index = 0; index < ChildNodes.Count; index++)
				{
					TypeTreeNode childNode = ChildNodes[index];

					typeArguments.Add(childNode.GetTreeAsTypeWrapper(context));
				}
			}

			ClassWrapper classWrapper = HelperFunctions.FindClassOfTypeDefintion(context, TypeDefinition);

			if (classWrapper != null && classWrapper.IsVersion2)
			{
				CorElementType elementType = (TypeDefinition.IsValueType ? CorElementType.ELEMENT_TYPE_VALUETYPE : CorElementType.ELEMENT_TYPE_CLASS);

				result = classWrapper.Version2.GetParameterizedType((int)elementType, typeArguments);
			}

			return result;
		}
	}
}