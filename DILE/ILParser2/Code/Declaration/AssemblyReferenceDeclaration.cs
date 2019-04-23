using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.ILParser2.Grammar;

namespace Dile.ILParser2.Code.Declaration
{
	public class AssemblyReferenceDeclaration : DeclarationBase
	{
		public AssemblyReferenceDeclaration(LanguageElement declLanguageElement)
			: base(declLanguageElement, "assemblyRefHead")
		{
		}
	}
}