package Fantasy.IO;

public class TrackableStream extends Stream
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

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<EventArgs> PositionChanged;

	protected void OnPositionChanged(EventArgs e)
	{
		if (this.PositionChanged != null)
		{
			this.PositionChanged(this, e);
		}
	}


	@Override
	public boolean getCanRead()
	{
		return this._innerStream.CanRead;
	}

	@Override
	public boolean getCanSeek()
	{
		return this._innerStream.CanSeek;
	}

	@Override
	public boolean getCanWrite()
	{
		return this._innerStream.CanWrite;
	}

	@Override
	public void Flush()
	{
		this._innerStream.Flush();
	}

	@Override
	public long getLength()
	{
		return this._innerStream.getLength();
	}

	@Override
	public long getPosition()
	{
		return this._innerStream.Position;
	}
	@Override
	public void setPosition(long value)
	{
		if (value != this._innerStream.Position)
		{
			this._innerStream.Position = value;
			this.OnPositionChanged(EventArgs.Empty);
		}
	}

	@Override
	public int Read(byte[] buffer, int offset, int count)
	{
		int rs = this._innerStream.Read(buffer, offset, count);
		if (rs > 0)
		{
			this.OnPositionChanged(EventArgs.Empty);
		}
		return rs;
	}

	@Override
	public long Seek(long offset, SeekOrigin origin)
	{
		long rs = this._innerStream.Seek(offset, origin);
		this.OnPositionChanged(EventArgs.Empty);
		return rs;
	}

	@Override
	public void SetLength(long value)
	{
		this._innerStream.SetLength(value);

	}

	@Override
	public void Write(byte[] buffer, int offset, int count)
	{

		this.Write(buffer, offset, count);
		this.OnPositionChanged(EventArgs.Empty);
	}

	@Override
	public void Close()
	{
		this._innerStream.Close();
	}
}