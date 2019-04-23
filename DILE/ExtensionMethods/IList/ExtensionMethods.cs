using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dile.ExtensionMethods.IList
{
	public static class ExtensionMethods
	{
		public static void AddRange<TItem>(this IList<TItem> obj, IEnumerable<TItem> itemsToAdd)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}

			foreach (TItem itemToAdd in itemsToAdd)
			{
				obj.Add(itemToAdd);
			}
		}
	}
}