using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using Dile.UI.Debug;
using System.Globalization;

namespace Dile.Debug.Expressions
{
	public class IndexerExpression : BaseExpression
	{
		private List<List<BaseExpression>> indices;
		private List<List<BaseExpression>> Indices
		{
			get
			{
				return indices;
			}
			set
			{
				indices = value;
			}
		}

		public IndexerExpression(List<List<BaseExpression>> indices, string foundExpression)
			: base(foundExpression)
		{
			Indices = indices;
		}

		public static BaseExpression TryParse(string expressionText)
		{
			BaseExpression result = null;

			if (expressionText[0] == '[')
			{
				List<List<BaseExpression>> parameters = new List<List<BaseExpression>>();
				int expressionIndex = 1;
				List<BaseExpression> parameter = null;
				Parser parser = new Parser();

				do
				{
					parameter = parser.Parse(expressionText.Substring(expressionIndex));

					if (parameter != null)
					{
						parameters.Add(parameter);

						foreach (BaseExpression parameterExpression in parameter)
						{
							expressionIndex += parameterExpression.FoundExpressionLength;
						}

						if (expressionText[expressionIndex] == ',')
						{
							expressionIndex++;
						}
					}

				} while (parameter != null && expressionText[expressionIndex] != ']');

				result = new IndexerExpression(parameters, expressionText.Substring(0, expressionIndex + 1));
			}

			return result;
		}

		public override DebugExpressionResult Evaluate(EvaluationContext context, DebugExpressionResult thisValue)
		{
			DebugExpressionResult result = null;

			if ((CorElementType)thisValue.ResultValue.ElementType == CorElementType.ELEMENT_TYPE_ARRAY || (CorElementType)thisValue.ResultValue.ElementType == CorElementType.ELEMENT_TYPE_SZARRAY)
			{
				ArrayValueWrapper arrayWrapper = thisValue.ResultValue.ConvertToArrayValue();
				List<uint> arrayIndices = new List<uint>(Indices.Count);

				for (int index = 0; index < Indices.Count; index++)
				{
					List<BaseExpression> arrayIndex = Indices[index];
					DebugExpressionResult arrayIndexValue = ExecuteExpressionList(context, null, arrayIndex);

					arrayIndices.Add(arrayIndexValue.ResultValue.GetGenericValue<uint>());
				}

				result = new DebugExpressionResult(context, arrayWrapper.GetElement(arrayIndices));
			}
			else
			{
				MemberExpression itemMember = new MemberExpression("get_Item", Indices);
				TypeTreeNodeList typeArguments = GetClassTypeArguments(context, thisValue);

				if (typeArguments != null && typeArguments.TypeTreeNodes.Count > 0)
				{
					context.ClassTypeArguments = typeArguments;
				}

				result = itemMember.Evaluate(context, thisValue);
			}

			return result;
		}
	}
}