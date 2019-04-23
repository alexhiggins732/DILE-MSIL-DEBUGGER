using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class CustomAttributeNamedArgument
	{
		private NamedArgumentType argumentType;
		public NamedArgumentType ArgumentType
		{
			get
			{
				return argumentType;
			}
			set
			{
				argumentType = value;
			}
		}

		private ICustomAttributeElement argumentValue;
		public ICustomAttributeElement ArgumentValue
		{
			get
			{
				return argumentValue;
			}
			set
			{
				argumentValue = value;
			}
		}

		public void AppendString(StringBuilder stringBuilder)
		{
			switch(ArgumentType)
			{
				case NamedArgumentType.Field:
					stringBuilder.Append("field ");
					break;

				case NamedArgumentType.Property:
					stringBuilder.Append("property ");
					break;

				default:
					throw new NotSupportedException("Unknown argument type: " + ArgumentType);
			}

			bool isArgumentValueEnum = ArgumentValue.IsEnum;

			ArgumentValue.AppendElementType(stringBuilder, false);

			if (isArgumentValueEnum)
			{
				ArgumentValue.AppendEnumTypeName(stringBuilder);
			}

			stringBuilder.Append(" '");
			ArgumentValue.AppendName(stringBuilder);
			stringBuilder.Append("' = ");

			if (ArgumentValue.IsBoxed)
			{
				stringBuilder.Append("object(");
			}

			if (isArgumentValueEnum)
			{
				stringBuilder.Append("int32");
			}
			else
			{
				ArgumentValue.AppendElementType(stringBuilder, true);
			}

			if (!ArgumentValue.IsArray)
			{
				stringBuilder.Append("(");
			}

			ArgumentValue.AppendElementValue(stringBuilder);

			if (!ArgumentValue.IsArray)
			{
				stringBuilder.Append(")");
			}

			if (ArgumentValue.IsBoxed)
			{
				stringBuilder.Append(")");
			}
		}
	}
}