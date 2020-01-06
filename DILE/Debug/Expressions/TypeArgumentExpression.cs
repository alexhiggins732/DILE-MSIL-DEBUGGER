using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Debug.Expressions
{
	public class TypeArgumentExpression : BaseExpression
	{
		private TypeTreeNodeList typeArguments;
		public TypeTreeNodeList TypeArguments
		{
			get
			{
				return typeArguments;
			}
			set
			{
				typeArguments = value;
			}
		}

		public TypeArgumentExpression(string foundExpression, List<TypeTreeNode> typeArguments)
			: base(foundExpression)
		{
			TypeArguments = new TypeTreeNodeList(typeArguments);
		}

		public static BaseExpression TryParse(string expressionText)
		{
			TypeArgumentExpression result = null;

			if (expressionText.Length > 0 && expressionText[0] == '<')
			{
				int textIndex = 1;
				List<TypeTreeNode> typeArguments = new List<TypeTreeNode>();
				TypeExpression typeArgument = null;

				do
				{
					typeArgument = TypeExpression.TryParse(expressionText.Substring(textIndex)) as TypeExpression;

					if (typeArgument != null)
					{
						textIndex += typeArgument.FoundExpressionLength;
						TypeTreeNode typeTreeNode = new TypeTreeNode(typeArgument.TypeDefinition, typeArgument.IsArray);
						typeArguments.Add(typeTreeNode);
					}

					if (textIndex < expressionText.Length && expressionText[textIndex] == ',')
					{
						textIndex++;
					}

				} while (typeArgument != null && textIndex < expressionText.Length && expressionText[textIndex] != '>');

				if (typeArguments.Count > 0)
				{
					result = new TypeArgumentExpression(expressionText.Substring(0,textIndex + 1), typeArguments);
				}
			}

			return result;
		}

		public void Merge(TypeArgumentExpression typeArgumentExpression)
		{
			TypeArguments.TypeTreeNodes.AddRange(typeArgumentExpression.TypeArguments.TypeTreeNodes);
		}
	}
}