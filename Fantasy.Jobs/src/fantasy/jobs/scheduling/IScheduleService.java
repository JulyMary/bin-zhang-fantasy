package fantasy.jobs.scheduling;
public interface IScheduleService
{
	long register(java.util.Date timeToExecute, fantasy.Action action);

	void unregister(long cookie) throws Exception;
}