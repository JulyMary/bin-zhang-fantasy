package fantasy.jobs.management;

import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public interface IJobMetaDataFilter
{
	Iterable<JobMetaData> Filter(IQueryable<JobMetaData> source);
}