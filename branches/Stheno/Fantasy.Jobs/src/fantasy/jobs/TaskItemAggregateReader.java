package fantasy.jobs;
import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;
public class TaskItemAggregateReader extends ObjectWithSite implements ITagValueProvider
{

	public final char getPrefix()
	{
		return '@';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context) throws Exception
	{
		final String[] expr = tag.split("\\.", 2);

		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);

		TaskItem[] items = job.GetEvaluatedItemsByCatetory(expr[0]);

		Iterable<String> values;
		if(expr.length > 1)
		{
			values = new Enumerable<TaskItem>(items).select(new Selector<TaskItem, String>(){

				@Override
				public String select(TaskItem item) {
					return item.getItem(expr[1]);
				}});
		}
		else
		{
			
			values = new Enumerable<TaskItem>(items).select(new Selector<TaskItem, String>(){

				@Override
				public String select(TaskItem item) {
					return item.getName();
				}});
		}
		
		return StringUtils.join(values, ";");

		
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		return true;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context) throws Exception
	{
		return MapUtils.getValueOrDefault(context, "EnableTaskItemReader", true) && this.getSite() != null && this.getSite().getService(IJob.class) != null;
	}

}