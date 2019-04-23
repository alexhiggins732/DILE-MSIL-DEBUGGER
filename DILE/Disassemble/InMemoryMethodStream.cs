using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using System.IO;

namespace Dile.Disassemble
{
	public class InMemoryMethodStream : Stream
	{
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		public override bool CanSeek
		{
			get
			{
				return true;
			}
		}

		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		public override void Flush()
		{
			throw new NotSupportedException();
		}

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				return Convert.ToInt64(CurrentAddress - StartingAddress);
			}
			set
			{
				CurrentAddress = StartingAddress + Convert.ToUInt64(value);
			}
		}

		private ProcessWrapper debuggedProcess;
		private ProcessWrapper DebuggedProcess
		{
			get
			{
				return debuggedProcess;
			}
			set
			{
				debuggedProcess = value;
			}
		}

		private ulong startingAddress;
		private ulong StartingAddress
		{
			get
			{
				return startingAddress;
			}
			set
			{
				startingAddress = value;
			}
		}

		private ulong currentAddress;
		private ulong CurrentAddress
		{
			get
			{
				return currentAddress;
			}
			set
			{
				currentAddress = value;
			}
		}

		public InMemoryMethodStream(ProcessWrapper debuggedProcess, ulong startingAddress)
		{
			DebuggedProcess = debuggedProcess;
			StartingAddress = startingAddress;
			CurrentAddress = StartingAddress;
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			byte[] tempBuffer = DebuggedProcess.ReadMemory(CurrentAddress + Convert.ToUInt64(offset), Convert.ToUInt32(count));
			int read = Math.Min(tempBuffer.Length, buffer.Length);

			for (int index = 0; index < read; index++)
			{
				buffer[index] = tempBuffer[index];
			}

			CurrentAddress += Convert.ToUInt64(read);
			return read;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			switch(origin)
			{
				case SeekOrigin.Begin:
					CurrentAddress = StartingAddress + Convert.ToUInt64(offset);
					break;

				case SeekOrigin.Current:
					CurrentAddress += Convert.ToUInt64(offset);
					break;

				case SeekOrigin.End:
					throw new NotSupportedException();
			}

			return Convert.ToInt64(CurrentAddress);
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}
	}
}