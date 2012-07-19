package fantasy.jobs;

public class InvalidJobStartInfoException extends RuntimeException implements Serializable
{
	public InvalidJobStartInfoException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidJobStartInfoException(String message)
	{
		super(message);

	}
}