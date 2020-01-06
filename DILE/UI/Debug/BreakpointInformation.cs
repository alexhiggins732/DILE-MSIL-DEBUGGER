using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;

namespace Dile.UI.Debug
{
	public abstract class BreakpointInformation : IComparable
	{
		private BreakpointState state = BreakpointState.Active;
		public virtual BreakpointState State
		{
			get
			{
				return state;
			}
			set
			{
				Activate(value == BreakpointState.Active);
				state = value;
			}
		}

		public abstract string DisplayName
		{
			get;
		}

		public abstract string OffsetValue
		{
			get;
		}

		protected abstract void Activate(bool active);

		public abstract void NavigateTo();

		public abstract void Reset();

		public virtual void Remove()
		{
			Activate(false);
			state = BreakpointState.Removed;
		}

		#region IComparable Members

		public int CompareTo(object obj)
		{
			int result = 0;

			if (obj == null)
			{
				result = 1;
			}
			else
			{
				BreakpointInformation compareTo = obj as BreakpointInformation;

				if (compareTo == null)
				{
					throw new ArgumentException(string.Empty, "obj");
				}
				else
				{
					result = DisplayName.CompareTo(compareTo.DisplayName);

					if (result == 0)
					{
						result = OffsetValue.CompareTo(compareTo.OffsetValue);
					}
				}
			}

			return result;
		}

		public override bool Equals(object obj)
		{
			return (CompareTo(obj) == 0);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}