package Fantasy.Jobs;

public class InvalidJobTemplateException extends RuntimeException implements Serializable
{
	public InvalidJobTemplateException(String message, RuntimeException innerException)
	{
		super(message, innerException);

	}

	public InvalidJobTemplateException(String message)
	{
		super(message);

	}
}