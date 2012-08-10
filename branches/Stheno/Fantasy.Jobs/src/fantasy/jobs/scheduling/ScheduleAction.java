package fantasy.jobs.scheduling;

import java.io.*;


public abstract class ScheduleAction implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 7109434522736215478L;

	public abstract ActionType getType();
}