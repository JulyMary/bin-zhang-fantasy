package Fantasy.Jobs;

public class JobException extends RuntimeException implements Serializable
{

	protected JobException(SerializationInfo info, StreamingContext context)
	{
		super(info, context);
	}

	public JobException()
	{

	}
	public JobException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public JobException(String message)
	{
		super(message);

	}
}