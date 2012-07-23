package fantasy.jobs.management;

import fantasy.jobs.*;




public interface IJobTemplatesService
{
	JobTemplate GetJobTemplateByName(String name);
	JobTemplate GetJobTemplateByPath(String path);

	JobTemplate[] GetJobTemplates();
}