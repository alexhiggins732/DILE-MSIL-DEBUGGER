using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace Dile.Metadata.Signature
{
	public sealed class SignatureCompression
	{
		private static readonly CorTokenType[] g_tkCorEncodeToken = new CorTokenType[] {CorTokenType.mdtTypeDef, CorTokenType.mdtTypeRef, CorTokenType.mdtTypeSpec, CorTokenType.mdtBaseType};

		private SignatureCompression()
		{
		}

		public static uint CorSigUncompressBigData(IntPtr pData)
		{
			int res;

			// Medium.  
			if ((Marshal.ReadByte(pData) & 0xC0) == 0x80)  // 10?? ????  
			{
				res = (Marshal.ReadByte(pData) & 0x3f) << 8;
				pData = new IntPtr(pData.ToInt32() + 1);
				res |= Marshal.ReadByte(pData);
			}
			else // 110? ???? 
			{
				res = (Marshal.ReadByte(pData) & 0x1f) << 24;
				pData = new IntPtr(pData.ToInt32() + 1);
				res |= Marshal.ReadByte(pData) << 16;
				pData = new IntPtr(pData.ToInt32() + 1);
				res |= Marshal.ReadByte(pData) << 8;
				pData = new IntPtr(pData.ToInt32() + 1);
				res |= Marshal.ReadByte(pData);
			}
			return (uint)res;
		}

		public static uint CorSigUncompressData(IntPtr pData)
		{
			// Handle smallest data inline. 
			if ((Marshal.ReadByte(pData) & 0x80) == 0x00)        // 0??? ????    
				return Marshal.ReadByte(pData);
			return CorSigUncompressBigData(pData);
		}

		public static uint CorSigUncompressData(IntPtr pData, out uint pDataOut)
		{
			unchecked
			{
				uint cb = (uint)-1;
				pDataOut = 0;

				// Smallest.    
				if ((Marshal.ReadByte(pData) & 0x80) == 0x00)       // 0??? ????    
				{
					pDataOut = Marshal.ReadByte(pData);
					cb = 1;
				}
				// Medium.  
				else if ((Marshal.ReadByte(pData) & 0xC0) == 0x80)  // 10?? ????    
				{
					pDataOut = Convert.ToUInt32((Marshal.ReadByte(pData) & 0x3f) << 8);
					pData = new IntPtr(pData.ToInt64() + 1);
					pDataOut |= Marshal.ReadByte(pData);
					cb = 2;
				}
				else if ((Marshal.ReadByte(pData) & 0xE0) == 0xC0)      // 110? ????    
				{
					pDataOut = Convert.ToUInt32((Marshal.ReadByte(pData) & 0x1f) << 24);
					pData = new IntPtr(pData.ToInt32() + 1);
					pDataOut |= Convert.ToUInt32(Marshal.ReadByte(pData) << 16);
					pData = new IntPtr(pData.ToInt32() + 1);
					pDataOut |= Convert.ToUInt32(Marshal.ReadByte(pData) << 8);
					pData = new IntPtr(pData.ToInt32() + 1);
					pDataOut |= Marshal.ReadByte(pData);
					cb = 4;
				}
				return cb;
			}
		}

		public static uint TokenFromRid(uint rid, uint token)
		{
			return (rid | token);
		}

		public static uint CorSigUncompressToken(IntPtr pData)
		{
			uint tk;
			uint tkType;

			tk = CorSigUncompressData(pData);
			tkType = Convert.ToUInt32(g_tkCorEncodeToken[tk & 0x3]);
			tk = TokenFromRid(tk >> 2, tkType);
			return tk;
		}

		public static uint CorSigUncompressToken(uint data)
		{
			uint tk;
			uint tkType;

			tk = data;
			tkType = Convert.ToUInt32(g_tkCorEncodeToken[tk & 0x3]);
			tk = TokenFromRid(tk >> 2, tkType);
			return tk;
		}
	}
}