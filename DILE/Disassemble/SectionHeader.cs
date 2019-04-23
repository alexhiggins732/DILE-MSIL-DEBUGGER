using System;
using System.Collections.Generic;
using System.Text;

namespace Dile.Disassemble
{
	public class SectionHeader
	{
		private uint virtualAddress;
		public uint VirtualAddress
		{
			get
			{
				return virtualAddress;
			}
			set
			{
				virtualAddress = value;
			}
		}

		private uint pointerToRawData;
		public uint PointerToRawData
		{
			get
			{
				return pointerToRawData;
			}
			set
			{
				pointerToRawData = value;
			}
		}

		private uint sizeOfRawData;
		public uint SizeOfRawData
		{
			get
			{
				return sizeOfRawData;
			}
			set
			{
				sizeOfRawData = value;
			}
		}

		public SectionHeader()
		{
		}

		public bool ContainsRvaAddress(uint rva)
		{
			return (rva >= VirtualAddress && rva < VirtualAddress + SizeOfRawData);
		}
	}
}
