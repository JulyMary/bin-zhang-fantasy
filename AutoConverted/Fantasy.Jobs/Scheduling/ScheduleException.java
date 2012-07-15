package Fantasy.Jobs.Scheduling;

public class ScheduleException extends RuntimeException implements Serializable
{
	public ScheduleException(String message)
	{
		super(message);

	}
}