using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble.ILCodes
{
	public class LocationILCode <LocationType> : BaseParameterILCode<LocationType, LocationType>
	{
		public LocationILCode()
		{	
		}

		public override void DecodeParameter()
		{
			int targetAddress = Offset + (typeof(LocationType) == typeof(sbyte) ? 2 : 5);

			Text = string.Format("{0} IL_{1}", OpCode.Name, HelperFunctions.FormatAsHexNumber(Convert.ToInt32(DecodedParameter) + targetAddress, 4));
		}
	}
}
