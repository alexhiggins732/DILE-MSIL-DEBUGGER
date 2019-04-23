using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public class MemberReference : TextTokenBase, IHasSignature
	{
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = HelperFunctions.QuoteMethodName(value);
			}
		}

		private uint typeDefToken;
		public uint TypeDefToken
		{
			get
			{
				return typeDefToken;
			}
			private set
			{
				typeDefToken = value;
			}
		}

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

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					text = string.Empty;

					if (Assembly.AllTokens.ContainsKey(TypeDefToken))
					{
						TokenBase tokenObject = Assembly.AllTokens[TypeDefToken];
						Type tokenObjectType = tokenObject.GetType();

						if (tokenObjectType.IsSubclassOf(typeof(TextTokenBase)))
						{
							TextTokenBase textToken = (TextTokenBase)tokenObject;

							textToken.LazyInitialize(Assembly.AllTokens);
						}

						if (tokenObjectType == typeof(TypeDefinition))
						{
							ClassName = ((TypeDefinition)tokenObject).FullName;
						}
						else if (tokenObjectType == typeof(TypeReference))
						{
							TypeReference typeRef = (TypeReference)tokenObject;

							ClassName = typeRef.FullName;
						}
						else if (tokenObjectType == typeof(ModuleReference) || tokenObjectType == typeof(TypeSpecification))
						{
							ClassName = tokenObject.Name;
						}
						else if (tokenObjectType == typeof(MethodDefinition))
						{
							MethodDefinition methodDef = (MethodDefinition)tokenObject;

							ClassName = methodDef.BaseTypeDefinition.FullName;
						}

						text = signatureReader.GetDefinition(ClassName, Name, false);
					}
				}

				return text;
			}
			private set
			{
				text = value;
			}
		}

		private Assembly assembly;
		public Assembly Assembly
		{
			get
			{
				return assembly;
			}
			private set
			{
				assembly = value;
			}
		}

		private string className;
		public string ClassName
		{
			get
			{
				return className;
			}
			private set
			{
				className = value;
			}
		}

		private List<MethodSpec> methodSpecs;
		public List<MethodSpec> MethodSpecs
		{
			get
			{
				return methodSpecs;
			}
			private set
			{
				methodSpecs = value;
			}
		}

		private MethodRefSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				return signatureReader;
			}
		}

		public MemberReference(Assembly assembly, string name, uint token, uint typeDefToken, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Assembly = assembly;
			Name = name;
			Token = token;
			TypeDefToken = typeDefToken;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;

			signatureReader = new MethodRefSignatureReader(assembly.AllTokens, SignatureBlob, SignatureBlobLength);
			signatureReader.ReadSignature();
			MethodSpecs = HelperFunctions.EnumMethodSpecs(assembly.Import, assembly, this);
		}

		public override string ToString()
		{
			LazyInitialize(Assembly.AllTokens);

			return Text;
		}
	}
}