using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.Configuration
{
	[Serializable()]
	public class ExceptionInformation : IComparable
	{
		private bool skip = true;
		public bool Skip
		{
			get
			{
				return skip;
			}
			set
			{
				skip = value;
			}
		}

		private string assemblyPath;
		public string AssemblyPath
		{
			get
			{
				return assemblyPath;
			}
			set
			{
				assemblyPath = value;
			}
		}

		private uint exceptionClassToken;
		public uint ExceptionClassToken
		{
			get
			{
				return exceptionClassToken;
			}
			set
			{
				exceptionClassToken = value;
			}
		}

		public string ExceptionClassTokenString
		{
			get
			{
				return string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(ExceptionClassToken, 8));
			}
		}

		public string ExceptionClassName
		{
			get
			{
				string result = string.Empty;
				TypeDefinition exceptionTypeDefinition = HelperFunctions.FindObjectByToken(ExceptionClassToken, AssemblyPath, false) as TypeDefinition;

				if (exceptionTypeDefinition == null)
				{
					result = "<Unknown class>";
				}
				else
				{
					result = exceptionTypeDefinition.FullName;
				}

				return result;
			}
		}

		private uint throwingMethodToken;
		public uint ThrowingMethodToken
		{
			get
			{
				return throwingMethodToken;
			}
			set
			{
				throwingMethodToken = value;
			}
		}

		public string ThrowingMethodTokenString
		{
			get
			{
				return string.Format("0x{0}", HelperFunctions.FormatAsHexNumber(ThrowingMethodToken, 8));
			}
		}

		public string ThrowingMethodName
		{
			get
			{
				string result = string.Empty;
				MethodDefinition throwingMethodDefinition = HelperFunctions.FindObjectByToken(ThrowingMethodToken, AssemblyPath, false) as MethodDefinition;

				if (throwingMethodDefinition == null)
				{
					result = "<Unknown method>";
				}
				else
				{
					result = throwingMethodDefinition.Text;
				}

				return result;
			}
		}

		private uint? ip;
		public uint? IP
		{
			get
			{
				return ip;
			}
			set
			{
				ip = value;
			}
		}

		public string IPAsString
		{
			get
			{
				string result = string.Empty;

				if (IP.HasValue)
				{
					result = string.Format("IL_{0}", HelperFunctions.FormatAsHexNumber(IP.Value, 4));
				}

				return result;
			}
		}

		public ExceptionInformation()
		{
		}

		public ExceptionInformation(string assemblyName, uint exceptionClassToken)
		{
			AssemblyPath = assemblyName;
			ExceptionClassToken = exceptionClassToken;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			int result = 0;

			if (obj == null)
			{
				result = 1;
			}
			else
			{
				ExceptionInformation otherExceptionInformation = obj as ExceptionInformation;

				if (obj == null)
				{
					throw new ArgumentException("Incorrect argument type: " + obj.GetType().FullName, "obj");
				}
				else
				{
					result = AssemblyPath.CompareTo(otherExceptionInformation.AssemblyPath);

					if (result == 0)
					{
						result = ExceptionClassToken.CompareTo(otherExceptionInformation.ExceptionClassToken);
					}
				}
			}

			return result;
		}

		#endregion

		public override bool Equals(object obj)
		{
			return (CompareTo(obj) == 0);
		}

		public bool Equals(string assemblyPath, uint exceptionClassToken, uint throwingMethodToken, uint? ip)
		{
			bool result = false;

			if (Skip && AssemblyPath.Equals(assemblyPath, StringComparison.Ordinal) && ExceptionClassToken == exceptionClassToken)
			{
				if (IP.HasValue)
				{
					result = (ThrowingMethodToken == throwingMethodToken && IP.Value == ip);
				}
				else
				{
					result = true;
				}
			}

			return result;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}