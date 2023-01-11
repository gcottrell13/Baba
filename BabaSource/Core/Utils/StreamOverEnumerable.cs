using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class StreamOverEnumerable : Stream
    {
        private IEnumerator<byte> bytes;

        public StreamOverEnumerable(IEnumerable<byte> bytes)
        {
            this.bytes = bytes.GetEnumerator();
        }

        public StreamOverEnumerable(IEnumerable<byte[]> bytes) : this(bytes.SelectMany(x => x))
        {
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotImplementedException();

        public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var copied = 0;
            while (count-- > 0)
            {
                if (bytes.MoveNext())
                {

                    buffer[offset++] = bytes.Current;
                    copied ++;
                }
                else
                    break;
            }
            return copied;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
