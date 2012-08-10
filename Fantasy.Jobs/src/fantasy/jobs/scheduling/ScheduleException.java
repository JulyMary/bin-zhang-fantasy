package fantasy.jobs.scheduling;

import java.io.Serializable;

public class ScheduleException extends RuntimeException implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -4104173359836136233L;

	public ScheduleException(String message)
	{
		super(message);

	}
}