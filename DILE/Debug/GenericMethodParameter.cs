using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug.Expressions;
using Dile.Disassemble;
using Dile.Metadata;

namespace Dile.Debug
{
	public class GenericMethodParameter : IMethodParameter
	{
		#region IMethodParameter Members
		private CorElementType elementType;
		public CorElementType ElementType
		{
			get
			{
				return elementType;
			}
			set
			{
				elementType = value;
			}
		}

		private IMethodParameter arrayElementType;
		public IMethodParameter ArrayElementType
		{
			get
			{
				return arrayElementType;
			}
			set
			{
				arrayElementType = value;
			}
		}

		private TokenBase tokenObject;
		public TokenBase TokenObject
		{
			get
			{
				return tokenObject;
			}
			set
			{
				tokenObject = value;
			}
		}

		public GenericMethodParameter()
		{
		}

		public string GetTokenObjectName(bool fullName)
		{
			return HelperFunctions.GetTokenObjectName(TokenObject, fullName);
		}
		#endregion
	}
}