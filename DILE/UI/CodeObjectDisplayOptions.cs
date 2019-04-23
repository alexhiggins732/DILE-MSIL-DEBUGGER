using System;
using System.Collections.Generic;
using System.Text;

using Dile.Controls;

namespace Dile.UI
{
	public class CodeObjectDisplayOptions
	{
		private List<BaseLineDescriptor> specialLinesToAdd = null;
		public List<BaseLineDescriptor> SpecialLinesToAdd
		{
			get
			{
				return specialLinesToAdd;
			}
			set
			{
				specialLinesToAdd = value;
			}
		}

		private BaseLineDescriptor currentLine = null;
		public BaseLineDescriptor CurrentLine
		{
			get
			{
				return currentLine;
			}
			set
			{
				currentLine = value;
			}
		}

		private uint navigateToOffset = 0;
		public uint NavigateToOffset
		{
			get
			{
				return navigateToOffset;
			}
			set
			{
				navigateToOffset = value;
				IsNavigateSet = true;
			}
		}

		private bool isNavigateSet = false;
		public bool IsNavigateSet
		{
			get
			{
				return isNavigateSet;
			}
			set
			{
				isNavigateSet = value;
			}
		}

		public CodeObjectDisplayOptions()
		{
		}

		public CodeObjectDisplayOptions(int instructionOffset, bool exactLocation)
		{
			if (exactLocation)
			{
				CurrentLine = new CurrentLine(instructionOffset);
			}
			else
			{
				CurrentLine = new CallerLine(instructionOffset);
			}
		}
	}
}