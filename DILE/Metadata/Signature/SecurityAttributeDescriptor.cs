using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class SecurityAttributeDescriptor
	{
		private string attributeName;
		public string AttributeName
		{
			get
			{
				return attributeName;
			}
			set
			{
				attributeName = value;
			}
		}

		private List<CustomAttributeNamedArgument> fieldsAndProperties = new List<CustomAttributeNamedArgument>();
		public List<CustomAttributeNamedArgument> FieldsAndProperties
		{
			get
			{
				return fieldsAndProperties;
			}
			set
			{
				fieldsAndProperties = value;
			}
		}

		public void AppendString(StringBuilder stringBuilder)
		{
			stringBuilder.Append("class '");
			stringBuilder.Append(AttributeName);
			stringBuilder.Append("' = {");

			for (int index = 0; index < FieldsAndProperties.Count; index++)
			{
				FieldsAndProperties[index].AppendString(stringBuilder);

				if (index < FieldsAndProperties.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}

			stringBuilder.Append("}");
		}
	}
}