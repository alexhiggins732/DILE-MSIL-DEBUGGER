using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Metadata.Signature
{
	public class FunctionPointerSignatureItem : BaseSignatureItem
	{
		private string definition = null;
		public string Definition
		{
			get
			{
				return definition;
			}
			private set
			{
				definition = value;
			}
		}

		private MethodRefSignatureReader signatureReader;
		public MethodRefSignatureReader SignatureReader
		{
			get
			{
				return signatureReader;
			}
			private set
			{
				signatureReader = value;
			}
		}

		public FunctionPointerSignatureItem(MethodRefSignatureReader signatureReader)
		{
			SignatureReader = signatureReader;
		}

		public override string ToString()
		{
			if (Definition == null)
			{
				Definition = "method " + SignatureReader.GetDefinition(null, null, true);
			}

			return Definition;
		}
	}
}