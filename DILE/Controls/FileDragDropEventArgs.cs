using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.Controls
{
	public class FileDragDropEventArgs : EventArgs
	{
		public Uri DraggedObjectUri
		{
			get;
			private set;
		}

		public FileDragDropEventArgs(Uri draggedObjectUri)
		{
			DraggedObjectUri = draggedObjectUri;
		}
	}
}