using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;
using Dile.Metadata.Signature;
using System.Runtime.InteropServices;

namespace Dile.Disassemble
{
	public class PermissionSet : TokenBase
	{
		private CorDeclSecurity securityFlag;
		public CorDeclSecurity SecurityFlag
		{
			get
			{
				return securityFlag;
			}
			private set
			{
				securityFlag = value;
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

		[ThreadStatic()]
		private static StringBuilder nameBuilder;
		private static StringBuilder NameBuilder
		{
			get
			{
				if (nameBuilder == null)
				{
					nameBuilder = new StringBuilder();
				}

				return nameBuilder;
			}
		}

		public PermissionSet(IMetaDataImport2 import, Assembly assembly, uint token, uint securityFlag, IntPtr signatureBlob, uint signatureBlobLength)
		{
			Token = Token;
			SecurityFlag = (CorDeclSecurity)securityFlag;
			SignatureBlob = signatureBlob;
			SignatureBlobLength = signatureBlobLength;
		}

		private string SecurityFlagAsString()
		{
			string result = string.Empty;
			SecurityFlag = SecurityFlag & CorDeclSecurity.dclActionMask;

			switch (SecurityFlag)
			{
				case CorDeclSecurity.dclAssert:
					result = "assert";
					break;

				case CorDeclSecurity.dclDemand:
					result = "demand";
					break;

				case CorDeclSecurity.dclDeny:
					result = "deny";
					break;

				case CorDeclSecurity.dclNonCasDemand:
					result = "noncasdemand";
					break;

				case CorDeclSecurity.dclNonCasInheritance:
					result = "noncasinheritance";
					break;

				case CorDeclSecurity.dclNonCasLinkDemand:
					result = "noncaslinkdemand";
					break;

				case CorDeclSecurity.dclPermitOnly:
					result = "permitonly";
					break;

				case CorDeclSecurity.dclPrejitDenied:
					result = "prejitdeny";
					break;

				case CorDeclSecurity.dclPrejitGrant:
					result = "prejitgrant";
					break;

				case CorDeclSecurity.dclRequest:
					result = "request";
					break;

				case CorDeclSecurity.dclRequestMinimum:
					result = "reqmin";
					break;

				case CorDeclSecurity.dclRequestOptional:
					result = "reqopt";
					break;

				case CorDeclSecurity.dclRequestRefuse:
					result = "reqrefuse";
					break;

				case CorDeclSecurity.dclLinktimeCheck:
					result = "linkcheck";
					break;

				case CorDeclSecurity.dclInheritanceCheck:
					result = "inheritcheck";
					break;

				default:
					throw new NotImplementedException(string.Format("Unknown security flag ('{0}').", SecurityFlag));
			}

			return result;
		}

		public void SetText(Dictionary<uint, TokenBase> allTokens)
		{
			if (Marshal.ReadByte(signatureBlob) == (byte)'.')
			{
				//.NET v2.0 permission set format.
				SecuritySignatureReader signatureReader = new SecuritySignatureReader(allTokens, SignatureBlob, SignatureBlobLength);

				NameBuilder.Length = 0;
				NameBuilder.Append(".permissionset ");
				NameBuilder.Append(SecurityFlagAsString());
				NameBuilder.Append(" ");
				signatureReader.AppendString(NameBuilder);
			}
			else
			{
				//Old .NET v1.0/v1.1 XML format permission set.
				byte[] description = new byte[signatureBlobLength];
				for (int offset = 0; offset < signatureBlobLength; offset++)
				{
					description[offset] = Marshal.ReadByte(signatureBlob, offset);
				}

				UnicodeEncoding encoding = new UnicodeEncoding();
				NameBuilder.AppendFormat(".permissionset {0} \"{1}\"", SecurityFlagAsString(), HelperFunctions.ShowEscapeCharacters(encoding.GetString(description, 0, description.Length)));
			}

			Name = NameBuilder.ToString();
		}
	}
}