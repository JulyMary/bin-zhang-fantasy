package Fantasy.Jobs.Management;

import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public interface IJobTemplatesService
{
	JobTemplate GetJobTemplateByName(String name);
	JobTemplate GetJobTemplateByPath(String path);

	JobTemplate[] GetJobTemplates();
}