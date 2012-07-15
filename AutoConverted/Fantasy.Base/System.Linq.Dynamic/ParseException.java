package System.Linq.Dynamic;

public final class ParseException extends RuntimeException
{
	private int position;

	public ParseException(String message, int position)
	{
		super(message);
		this.position = position;
	}

	public int getPosition()
	{
		return position;
	}

	@Override
	public String toString()
	{
		return String.format(Res.ParseExceptionFormat, Message, position);
	}
}