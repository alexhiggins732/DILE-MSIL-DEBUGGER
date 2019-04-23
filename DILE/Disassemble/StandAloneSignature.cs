using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;

namespace Dile.Disassemble
{
	public class StandAloneSignature : TextTokenBase, IHasSignature
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

		private string text = null;
		public string Text
		{
			get
			{
				if (text == null)
				{
					LazyInitialize(Assembly.AllTokens);
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

		private StandAloneSignatureReader signatureReader;
		public BaseSignatureReader SignatureReader
		{
			get
			{
				ReadSignature();

				return signatureReader;
			}
		}

		[ThreadStatic()]
		private static StringBuilder textBuilder;
		private static StringBuilder TextBuilder
		{
			get
			{
				if (textBuilder == null)
				{
					textBuilder = new StringBuilder();
				}

				return textBuilder;
			}
		}

		public StandAloneSignature(Assembly assembly, uint token, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Assembly = assembly;
			Token = token;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
		}

		private void ReadSignature()
		{
			if (signatureReader == null)
			{
				signatureReader = new StandAloneSignatureReader(assembly.AllTokens, SignatureBlob, SignatureBlobLength);
				signatureReader.ReadSignature();
			}
		}

		protected override void CreateText(Dictionary<uint, TokenBase> allTokens)
		{
			base.CreateText(allTokens);
			TextBuilder.Length = 0;
			ReadSignature();

			if (signatureReader.CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_FIELD)
			{
				HelperFunctions.SetSignatureItemToken(allTokens, signatureReader.FieldSignatureItem);
				TextBuilder.Append(signatureReader.FieldSignatureItem);
			}
			else
			{
				string formatString1;
				string formatString2;

				TextBuilder.Append("(");

				if (signatureReader.Parameters != null)
				{
					if (signatureReader.CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_LOCAL_SIG)
					{
						formatString1 = "{0} V_{1},\n";
						formatString2 = "{0} V_{1}";
					}
					else
					{
						formatString1 = "{0}, ";
						formatString2 = "{0}";

						HelperFunctions.SetSignatureItemToken(allTokens, signatureReader.ReturnType);
						TextBuilder.Insert(0, " ");
						TextBuilder.Insert(0, signatureReader.ReturnType);
						TextBuilder.Insert(0, " ");
						TextBuilder.Insert(0, HelperFunctions.GetCallingConventionName(signatureReader.CallingConvention));
					}

					for (int index = 0; index < signatureReader.Parameters.Count; index++)
					{
						BaseSignatureItem signatureItem = signatureReader.Parameters[index];
						HelperFunctions.SetSignatureItemToken(allTokens, signatureItem);

						if (signatureReader.CallingConvention == CorCallingConvention.IMAGE_CEE_CS_CALLCONV_LOCAL_SIG && index > 0)
						{
							TextBuilder.Append("      ");
						}

						if (signatureReader.Parameters.Count > 1 && index < signatureReader.Parameters.Count - 1)
						{
							TextBuilder.AppendFormat(formatString1, signatureItem, index);
						}
						else
						{
							TextBuilder.AppendFormat(formatString2, signatureItem, index);
						}
					}
				}

				TextBuilder.Append(")");
			}

			Text = TextBuilder.ToString();
		}

		public override string ToString()
		{
			LazyInitialize(Assembly.AllTokens);

			return Text;
		}
	}
}