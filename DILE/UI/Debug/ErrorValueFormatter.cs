using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.UI.Debug
{
	public class ErrorValueFormatter : BaseValueFormatter
	{
		private string message;
		public string Message
		{
			get
			{
				return message;
			}
			set
			{
				message = value;
			}
		}

		public ErrorValueFormatter(string message)
			: this("Evaluation error", message)
		{
		}

		public ErrorValueFormatter(string name, string message)
		{
			Name = name;
			Message = message;

			FieldGroup = ValueFieldGroup.EvaluationException;
		}

		public override string GetFormattedString(bool useHexaFormatting)
		{
			return Message;
		}
	}
}