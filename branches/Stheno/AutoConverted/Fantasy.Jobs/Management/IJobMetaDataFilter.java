package Fantasy.Jobs.Management;

import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public interface IJobMetaDataFilter
{
	Iterable<JobMetaData> Filter(IQueryable<JobMetaData> source);
}