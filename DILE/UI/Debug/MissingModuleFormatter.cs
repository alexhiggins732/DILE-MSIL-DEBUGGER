using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class MissingModuleFormatter : BaseValueFormatter
	{
		private const string FieldName = "Missing module";

		private MissingModule missingModule;
		public MissingModule MissingModule
		{
			get
			{
				return missingModule;
			}
			private set
			{
				missingModule = value;
			}
		}

		public MissingModuleFormatter(MissingModule missingModule)
		{
			MissingModule = missingModule;
			Name = FieldName;

			FieldGroup = ValueFieldGroup.MissingModule;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			string result = string.Empty;

			if (MissingModule.IsInMemoryModule)
			{
				result = string.Format("The '{0}' in-memory module is not added to the project.", MissingModule.MissingModuleName);
			}
			else
			{
				result = string.Format("The '{0}' module is not added to the project.", MissingModule.MissingModuleName);
			}

			return result;
		}

		public override bool Equals(object obj)
		{
			bool result = false;
			MissingModuleFormatter other = obj as MissingModuleFormatter;

			if (other != null)
			{
				result = (MissingModule.MissingModuleName == other.MissingModule.MissingModuleName);
			}

			return result;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}