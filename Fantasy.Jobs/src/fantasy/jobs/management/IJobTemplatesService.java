package fantasy.jobs.management;

import fantasy.jobs.*;




public interface IJobTemplatesService
{
	JobTemplate GetJobTemplateByName(String name) throws Exception;
	JobTemplate GetJobTemplateByPath(String path) throws Exception;;

	JobTemplate[] GetJobTemplates() throws Exception;;
}