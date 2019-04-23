using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public interface ICustomAttributeElement
	{
		bool IsArray
		{
			get;
		}

		bool IsEnum
		{
			get;
		}

		bool IsBoxed
		{
			get;
			set;
		}

		string Name
		{
			get;
			set;
		}

		string EnumTypeName
		{
			get;
			set;
		}

		CorElementType ElementType
		{
			get;
			set;
		}

		CorElementType ArrayElementType
		{
			get;
			set;
		}

		void AppendName(StringBuilder stringBuilder);

		void AppendEnumTypeName(StringBuilder stringBuilder);

		void AppendElementType(StringBuilder stringBuilder, bool includeArrayLength);

		void AppendElementValue(StringBuilder stringBuilder);
	}
}