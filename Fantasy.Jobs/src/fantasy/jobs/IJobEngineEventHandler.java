package fantasy.jobs;

public interface IJobEngineEventHandler
{
	void HandleStart(IJobEngine sender) throws Exception;
	void HandleResume(IJobEngine sender) throws Exception;
	void HandleLoad(IJobEngine sender) throws Exception;
	void HandleExit(IJobEngine sender, JobExitEventArgs e) throws Exception;
}