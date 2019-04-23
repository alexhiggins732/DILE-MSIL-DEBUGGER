using System;
using System.Collections.Generic;
using System.Text;

namespace Dile
{
	public class Constants
	{
		public const int WM_USER = 0x0400;
		public const int WM_KEYDOWN = 0x100;
		public const int WM_SYSKEYDOWN = 0x104;
		public const int CFM_COLOR = 0x40000000;
		public const int CFM_BACKCOLOR = 0x04000000;
		public const int EM_SETCHARFORMAT = WM_USER + 68;
		public const int EM_GETSCROLLPOS = WM_USER + 221;
		public const int EM_SETSCROLLPOS = WM_USER + 222;
		public const int SCF_SELECTION = 0x0001;

		public const string DecimalTypeName = "System.Decimal";
		public const string BooleanTypeName = "System.Boolean";
		public const string StringTypeName = "System.String";
		public const string CharTypeName = "System.Char";
		public const string Int32TypeName = "System.Int32";
		public const string Int64TypeName = "System.Int64";
		public const string FloatTypeName = "System.Single";
		public const string DoubleTypeName = "System.Double";
		public const string ObjectTypeName = "System.Object";
		public const string ValueTypeName = "System.ValueType";
		public const string EnumTypeName = "System.Enum";

		public const string ToStringMethodName = "ToString";
		public const string MscorlibName = "mscorlib";

		public const string ConstructorMethodName = ".ctor";
		public const string ImplicitOperatorMethodName = "op_Implicit";
		public const string ExplicitOperatorMethodName = "op_Explicit";

		public const string CurrentExceptionName = "{exception}";

		public const string DefaultNamespaceName = "<no namespace>";
	}
}