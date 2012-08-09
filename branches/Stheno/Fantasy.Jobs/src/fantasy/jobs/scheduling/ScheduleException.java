package fantasy.jobs.scheduling;

public class ScheduleException extends RuntimeException implements Serializable
{
	public ScheduleException(String message)
	{
		super(message);

	}
}