package fantasy.jobs;

import java.io.Serializable;

public class JobException extends RuntimeException implements Serializable
{

	

	/**
	 * 
	 */
	private static final long serialVersionUID = -7812130909730383738L;

	public JobException()
	{

	}
	public JobException(String message, Throwable cause)
	{
		super(message, cause);

	}

	public JobException(String message)
	{
		super(message);

	}
}