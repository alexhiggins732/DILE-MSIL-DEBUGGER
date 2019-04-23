using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public class MethodSpec : TextTokenBase, IHasSignature
	{
		private IntPtr signature;
		public IntPtr Signature
		{
			get
			{
				return signature;
			}
			private set
			{
				signature = value;
			}
		}

		private uint signatureLength;
		public uint SignatureLength
		{
			get
			{
				return signatureLength;
			}
			private set
			{
				signatureLength = value;
			}
		}

		private TokenBase parent;
		public TokenBase Parent
		{
			get
			{
				return parent;
			}
			set
			{
				parent = value;
			}
		}

		private MethodSpecSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		private Assembly assembly;
		private Assembly Assembly
		{
			get
			{
				return assembly;
			}
			set
			{
				assembly = value;
			}
		}

		public override string Name
		{
			get
			{
				LazyInitialize(Assembly.AllTokens);

				return base.Name;
			}
			set
			{
				base.Name = value;
			}
		}

		public MethodSpec(uint token, TokenBase parent, IntPtr signature, uint signatureLength, Assembly assembly)
		{
			Assembly = assembly;
			Token = token;
			Parent = parent;
			Signature = signature;
			SignatureLength = signatureLength;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new MethodSpecSignatureReader(Assembly.AllTokens, Signature, SignatureLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
			StringBuilder nameBuilder = null;

			if (string.IsNullOrEmpty(base.Name))
			{
				nameBuilder = new StringBuilder();
				nameBuilder.Append("<");
				ReadSignature();

				if (signatureReader.Arguments != null)
				{
					for (int index = 0; index < signatureReader.Arguments.Count; index++)
					{
						BaseSignatureItem argument = signatureReader.Arguments[index];
						HelperFunctions.SetSignatureItemToken(allTokens, argument);
						nameBuilder.Append(argument);

						if (index < signatureReader.Arguments.Count - 1)
						{
							nameBuilder.Append(", ");
						}
					}
				}

				nameBuilder.Append(">(");

				string parentAsString = Parent.ToString();
				Name = parentAsString.Replace("(", nameBuilder.ToString());
			}
		}
	}
}