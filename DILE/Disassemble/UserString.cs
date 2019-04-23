using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble
{
	public class UserString : TokenBase
	{
		private static readonly UserString incorrectToken = new UserString(0, Constants.IncorrectTokenName);
		public static UserString IncorrectToken
		{
			get
			{
				return incorrectToken;
			}
		}

		public override string Name
		{
			get;
			set;
		}

		public UserString(uint token, string name)
		{
			Token = token;
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}