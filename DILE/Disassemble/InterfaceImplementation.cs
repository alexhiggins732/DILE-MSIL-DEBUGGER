using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.Disassemble
{
	public class InterfaceImplementation : TokenBase
	{
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

		private uint interfaceToken;
		public uint InterfaceToken
		{
			get
			{
				return interfaceToken;
			}
			private set
			{
				interfaceToken = value;
			}
		}

		public InterfaceImplementation(IMetaDataImport2 import, uint token, TypeDefinition typeDefinition, uint interfaceToken)
		{
			Token = token;
			TypeDefinition = typeDefinition;
			InterfaceToken = interfaceToken;
		}
	}
}