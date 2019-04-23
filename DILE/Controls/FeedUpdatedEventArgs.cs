using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.Controls
{
	public class FeedUpdatedEventArgs : EventArgs
	{
		public string FeedHtml
		{
			get;
			private set;
		}

		public FeedUpdatedEventArgs(string feedHtml)
		{
			FeedHtml = feedHtml;
		}
	}
}