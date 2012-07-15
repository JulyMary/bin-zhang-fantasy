package Fantasy.XSerialization;

public class XException extends RuntimeException implements Serializable
{
	public XException(String message)
	{
		super(message);

	}

	public XException()
	{

	}

	public XException(String message, RuntimeException innerException)

	{
		super(message, innerException);

	}
}