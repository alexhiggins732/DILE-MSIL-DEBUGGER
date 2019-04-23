using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dile.Debug;
using System.Text.RegularExpressions;

namespace Dile.UI
{
	public class RuntimeVersion : IComparable<RuntimeVersion>
	{
		public int Major
		{
			get;
			private set;
		}

		public int Minor
		{
			get;
			private set;
		}

		public int Revision
		{
			get;
			private set;
		}

		public RuntimeInfo RuntimeInfo
		{
			get;
			private set;
		}

		public RuntimeVersion(RuntimeInfo runtimeInfo)
		{
			RuntimeInfo = runtimeInfo;

			Parse(runtimeInfo.GetVersionString());
		}

		public RuntimeVersion(string version)
		{
			Parse(version);
		}

		private void Parse(string version)
		{
			var regex = new Regex(@"v(\d+)\.(\d+)(?>\.?(\d*))");
			var match = regex.Match(version);

			if (match.Length != version.Length)
			{
				throw new NotSupportedException("Failed to parse the following runtime version string: " + version);
			}

			Major = Convert.ToInt32(match.Groups[1].Value);
			Minor = Convert.ToInt32(match.Groups[2].Value);

			Group revisionGroup = match.Groups[3];
			if (revisionGroup.Length > 0)
			{
				Revision = Convert.ToInt32(revisionGroup.Value);
			}
			else
			{
				Revision = 0;
			}
		}

		#region IComparable<RuntimeVersion> Members
		public int CompareTo(RuntimeVersion other)
		{
			int result = 0;

			if (other == null)
			{
				result = 1;
			}
			else
			{
				result = Major.CompareTo(other.Major);

				if (result == 0)
				{
					result = Minor.CompareTo(other.Minor);
				}

				if (result == 0)
				{
					result = Revision.CompareTo(other.Revision);
				}
			}

			return result;
		}
		#endregion
	}
}