package fantasy.jobs.scheduling;
import fantasy.*;
public interface IScheduleService
{
	long register(java.util.Date timeToExecute, Action action);

	void unregister(long cookie);
}