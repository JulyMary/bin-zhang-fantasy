package Fantasy.Jobs.Scheduling;

public interface IScheduleService
{
	long Register(java.util.Date timeToExecute, System.Action action);

	void Unregister(long cookie);
}