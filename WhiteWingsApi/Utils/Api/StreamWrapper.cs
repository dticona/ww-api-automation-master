// <copyright file="StreamWrapper.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.Api
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Wrapper class to clone read/write bytes from stream and convert to string.
    /// </summary>
    public class StreamWrapper : Stream
    {
        private Stream wrappedStream;
        private StringBuilder stringValueForStream;

        public string StringValueForStream { get { return stringValueForStream.ToString(); } }

        public StreamWrapper(Stream wrappedStream)
        {
            this.wrappedStream = wrappedStream;
            stringValueForStream = new StringBuilder();
        }

        public override bool CanRead
        {
            get { return wrappedStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return wrappedStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return wrappedStream.CanWrite; }
        }

        public override void Flush()
        {
            wrappedStream.Flush();
        }

        public override long Length
        {
            get { return wrappedStream.Length; }
        }

        public override long Position
        {
            get { return wrappedStream.Position; }
            set { wrappedStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            AppendToStringValue(buffer, offset, count);
            return wrappedStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return wrappedStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            wrappedStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            AppendToStringValue(buffer, offset, count);
            wrappedStream.Write(buffer, offset, count);
        }

        public override void Close()
        {
            wrappedStream.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (wrappedStream != null)
            {
                wrappedStream.Dispose();
                wrappedStream = null;
            }

            base.Dispose(disposing);
        }

        private void AppendToStringValue(byte[] buffer, int offset, int count)
        {
            stringValueForStream.Append(Encoding.UTF8.GetString(buffer, offset, count));
        }
    }
}
