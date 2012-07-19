package fantasy.jobs;

public interface IJobEngineEventHandler
{
	void HandleStart(IJobEngine sender);
	void HandleResume(IJobEngine sender);
	void HandleLoad(IJobEngine sender);
	void HandleExit(IJobEngine sender, JobExitEventArgs e);
}