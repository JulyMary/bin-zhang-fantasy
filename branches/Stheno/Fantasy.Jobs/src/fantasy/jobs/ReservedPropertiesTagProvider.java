package fantasy.jobs;

import java.util.*;

import fantasy.jobs.management.*;
import fantasy.*;

public class ReservedPropertiesTagProvider extends ObjectWithSite implements ITagValueProvider
{

	public final char getPrefix()
	{
		return '$';
	}


	private IJobEngine _engine;

	private IJobEngine getEngine() throws Exception
	{
		if (_engine == null)
		{
			_engine = this.getSite().getRequiredService(IJobEngine.class);

		}
		return _engine;
	}




	public final String GetTagValue(String tag, java.util.Map<String, Object> context) throws Exception
	{

		if (tag.toLowerCase().equals("jobdir"))
		{
				return this.getEngine().getJobDirectory();
		}
		else if (tag.toLowerCase().equals("intermediatedir"))
		{
				return this.getEngine().getJobDirectory();
		}
		else if (tag.toLowerCase().equals("jobhostdir"))
		{
				return JavaLibraryUtils.getEntryLibrary().getParent();
		}
		else if (tag.toLowerCase().equals("templatedir"))
		{
				return this.getSite().getRequiredService(IJobManagerSettingsReader.class).getSetting(String.class, "JobTemplateDirectoryFullPath");
		}
		else if (tag.toLowerCase().equals("jobid"))
		{
				return this.getEngine().getJobId().toString();
		}
		else if (tag.toLowerCase().equals("template"))
		{
				IJob job = this.getSite().getRequiredService(IJob.class);
				return job.getTemplateName();
		}
		else
		{
				return "";
		}
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		String[] tags = new String[] { "jobdir", "intermediatedir", "jobhostdir", "templatedir", "jobid", "templatename"};
		return Arrays.asList(tags).indexOf(tag.toLowerCase()) >= 0;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return true;
	}
}