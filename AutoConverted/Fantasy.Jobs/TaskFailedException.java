package Fantasy.Jobs;

public class TaskFailedException extends RuntimeException implements Serializable
{
   public TaskFailedException(String message, RuntimeException innerException)
	{
	   super(message, innerException);

	}

   public TaskFailedException(String message)
	{
	   super(message);

	}
}