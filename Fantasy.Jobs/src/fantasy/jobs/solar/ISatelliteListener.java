package fantasy.jobs.solar;

import fantasy.jobs.management.JobMetaData;

public interface ISatelliteListener {
	void Changed(JobMetaData job) throws Exception;
    void Added(JobMetaData job) throws Exception;
}
