package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Management.*;

public class TemplateStartupFilter extends ObjectWithSite implements IJobStartupFilter
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source)
	{
		 IJobController controller = this.Site.<IJobController>GetRequiredService();

		 JobMetaData[] running = controller.GetRunningJobs();


		for (JobMetaData job : source)
		{
			String template = job.getTemplate();

			int max = this.GetMaxCount(template);
			int exists = 0;
			if (max < Integer.MAX_VALUE)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				exists = running.Where(j => String.equals(j.Template, template, StringComparison.OrdinalIgnoreCase)).Count();
			}
			if (exists < max)
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return job;
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion




	private int GetMaxCount(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from t in JobManagerSettings.getDefault().getConcurrentTemplateCount().getTemplates() where name.equals(t.getName()) select t;
		TemplateCountSetting setting = query.SingleOrDefault();

		return setting != null ? setting.getCount() : JobManagerSettings.getDefault().getConcurrentTemplateCount().getCount();
	}
}