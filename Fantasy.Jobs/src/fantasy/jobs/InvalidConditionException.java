package fantasy.jobs;

import java.io.Serializable;

public class InvalidConditionException extends RuntimeException implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -3575876848171398799L;

	public InvalidConditionException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidConditionException(String message)
	{
		super(message);

	}
}