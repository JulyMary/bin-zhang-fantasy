using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fantasy.IO
{
    public class TrackableStream : Stream
    {
        private Stream _innerStream;

        public TrackableStream(Stream innerStream)
        {
            if (innerStream == null)
            {
                throw new ArgumentNullException("innerStream"); 
            }
            this._innerStream = innerStream;
        }

        public event EventHandler<EventArgs> PositionChanged;

        protected virtual void OnPositionChanged(EventArgs e)
        {
            if (this.PositionChanged != null)
            {
                this.PositionChanged(this, e);
            }
        }


        public override bool CanRead
        {
            get { return this._innerStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return this._innerStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return this._innerStream.CanWrite; }
        }

        public override void Flush()
        {
            this._innerStream.Flush(); 
        }

        public override long Length
        {
            get { return this._innerStream.Length; }
        }

        public override long Position
        {
            get
            {
                return this._innerStream.Position;
            }
            set
            {
                if (value != this._innerStream.Position)
                {
                    this._innerStream.Position = value;
                    this.OnPositionChanged(EventArgs.Empty);
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int rs = this._innerStream.Read(buffer, offset, count);
            if (rs > 0)
            {
                this.OnPositionChanged(EventArgs.Empty);
            }
            return rs;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long rs = this._innerStream.Seek(offset, origin);
            this.OnPositionChanged(EventArgs.Empty);
            return rs;
        }

        public override void SetLength(long value)
        {
            this._innerStream.SetLength(value);
           
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
          
            this.Write(buffer, offset, count);
            this.OnPositionChanged(EventArgs.Empty);
        }

        public override void Close()
        {
            this._innerStream.Close();
        }
    }
}
