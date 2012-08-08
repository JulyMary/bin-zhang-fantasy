package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;

public class TemplateStartupFilter extends ObjectWithSite implements IJobStartupFilter
{


	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source) throws Exception
	{
		 IJobController controller = this.getSite().getRequiredService(IJobController.class);

		 final JobMetaData[] running = controller.GetRunningJobs();
		 
		 return new Enumerable<JobMetaData>(source).where(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				final String template = job.getTemplate();

				int max = TemplateStartupFilter.this.getMaxCount(template);
				int exists = 0;
				if (max < Integer.MAX_VALUE)
				{
					exists = new Enumerable<JobMetaData>(running).where(new Predicate<JobMetaData>(){

						@Override
						public boolean evaluate(JobMetaData j) throws Exception {
							return StringUtils.equalsIgnoreCase(j.getTemplate(), template);
						}}).count(); 
				}
				
				return exists < max;
			}});

	}





	private int getMaxCount(final String key) throws Exception
	{
		TemplateCapacitySetting setting = new Enumerable<TemplateCapacitySetting>(JobManagerSettings.getDefault().getTemplateCapacity().getTemplates()).firstOrDefault(new Predicate<TemplateCapacitySetting>(){

			@Override
			public boolean evaluate(TemplateCapacitySetting s) throws Exception {
				
				return StringUtils.equals(s.getName() ,key);
			}});

		return setting != null ? setting.getCapacity() : JobManagerSettings.getDefault().getTemplateCapacity().getCapacity();

	}
}