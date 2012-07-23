package fantasy.jobs;

import java.io.*;

public class TaskFailedException extends RuntimeException implements Serializable
{
   /**
	 * 
	 */
	private static final long serialVersionUID = 8741706106655930035L;

    public TaskFailedException(String message, RuntimeException innerException)
	{
	   super(message, innerException);

	}

   public TaskFailedException(String message)
	{
	   super(message);

	}
}