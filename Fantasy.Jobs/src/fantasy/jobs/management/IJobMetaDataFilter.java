package fantasy.jobs.management;

import fantasy.jobs.properties.*;
import Fantasy.ServiceModel.*;

public interface IJobMetaDataFilter
{
	Iterable<JobMetaData> Filter(IQueryable<JobMetaData> source);
}