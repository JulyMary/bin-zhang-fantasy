package fantasy.jobs;

public class InvalidConditionException extends RuntimeException implements Serializable
{
	public InvalidConditionException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidConditionException(String message)
	{
		super(message);

	}
}