using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.Metadata.Signature
{
	public interface ILocalVarSignatureItem
	{
		bool ByRef
		{
			get;
			set;
		}

		bool Pinned
		{
			get;
			set;
		}
	}
}
