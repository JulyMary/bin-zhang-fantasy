package fantasy.jobs;

import java.io.Serializable;

public class InvalidJobTemplateException extends RuntimeException implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 6281942850486118334L;

	public InvalidJobTemplateException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidJobTemplateException(String message)
	{
		super(message);

	}
}