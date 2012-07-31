package fantasy.jobs.management;

public interface IJobQueueListener {
    void Changed(JobMetaData job) throws Exception;
    void Added(JobMetaData job) throws Exception;
    void RequestCancel(JobMetaData job) throws Exception;
    void RequestSuspend(JobMetaData job) throws Exception;
    void RequestUserPause(JobMetaData job) throws Exception;
}
