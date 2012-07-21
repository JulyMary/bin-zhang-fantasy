package fantasy.jobs;

import java.io.Serializable;

public class InvalidJobStartInfoException extends RuntimeException implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 447359590596942020L;

	public InvalidJobStartInfoException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidJobStartInfoException(String message)
	{
		super(message);

	}
}