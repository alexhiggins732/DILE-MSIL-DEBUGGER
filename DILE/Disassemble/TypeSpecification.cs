using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class TypeSpecification : TextTokenBase, IHasSignature
	{
		private IntPtr signatureBlob;
		public IntPtr SignatureBlob
		{
			get
			{
				return signatureBlob;
			}
			private set
			{
				signatureBlob = value;
			}
		}

		private uint signatureBlobLength;
		public uint SignatureBlobLength
		{
			get
			{
				return signatureBlobLength;
			}
			private set
			{
				signatureBlobLength = value;
			}
		}

		private TypeSpecSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				LazyInitialize(Assembly.AllTokens);

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

		public BaseSignatureItem Type
		{
			get
			{
				LazyInitialize(Assembly.AllTokens);

				return signatureReader.Type;
			}
		}

		public IList<BaseSignatureItem> GenericParameters
		{
			get
			{
				LazyInitialize(Assembly.AllTokens);

				return signatureReader.GenericParameters;
			}
		}

		public TypeSpecification(Assembly assembly, uint token, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Token = token;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
			Assembly = assembly;

			HelperFunctions.GetMemberReferences(Assembly, Token);
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new TypeSpecSignatureReader(Assembly.AllTokens, SignatureBlob, SignatureBlobLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
			ReadSignature();
			HelperFunctions.SetSignatureItemToken(allTokens, signatureReader.Type);
			Name = signatureReader.Type.ToString();

			if (signatureReader.GenericParameters != null)
			{
				StringBuilder nameBuilder = new StringBuilder(Name);
				nameBuilder.Append("<");

				for (int index = 0; index < signatureReader.GenericParameters.Count; index++)
				{
					BaseSignatureItem genericParameter = signatureReader.GenericParameters[index];

					HelperFunctions.SetSignatureItemToken(allTokens, genericParameter);
					nameBuilder.Append(genericParameter);

					if (index < signatureReader.GenericParameters.Count - 1)
					{
						nameBuilder.Append(", ");
					}
				}

				nameBuilder.Append(">");
				Name = nameBuilder.ToString();
			}
		}

		public override string ToString()
		{
			LazyInitialize(Assembly.AllTokens);

			return Name;
		}
	}
}