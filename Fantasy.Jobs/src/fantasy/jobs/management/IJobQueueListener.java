package fantasy.jobs.management;

public interface IJobQueueListener {
    void Changed(JobMetaData job);
    void Added(JobMetaData job);
    void RequestCancel(JobMetaData job);
    void RequestSuspend(JobMetaData job);
    void RequestUserPause(JobMetaData job);
}
