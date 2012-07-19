package fantasy.jobs;

import Fantasy.IO.*;
import fantasy.jobs.Management.*;

public class ReservedPropertiesTagProvider extends ObjectWithSite implements ITagValueProvider
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final char getPrefix()
	{
		return '$';
	}


	private IJobEngine _engine;

	private IJobEngine getEngine()
	{
		if (_engine == null)
		{
			_engine = this.Site.<IJobEngine>GetRequiredService();

		}
		return _engine;
	}




	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//		switch (tag.ToLower())
//ORIGINAL LINE: case "jobdir":
		if (tag.toLowerCase().equals("jobdir"))
		{
				return this.getEngine().getJobDirectory();
		}
//ORIGINAL LINE: case "intermediatedir":
		else if (tag.toLowerCase().equals("intermediatedir"))
		{
				return this.getEngine().getJobDirectory();
		}
//ORIGINAL LINE: case "jobhostdir":
		else if (tag.toLowerCase().equals("jobhostdir"))
		{
				return LongPath.GetDirectoryName(Assembly.GetEntryAssembly().getLocation());
		}
//ORIGINAL LINE: case "templatedir":
		else if (tag.toLowerCase().equals("templatedir"))
		{
				return this.Site.<IJobManagerSettingsReader>GetRequiredService().<String>GetSetting("JobTemplateDirectoryFullPath");
		}
//ORIGINAL LINE: case "jobid":
		else if (tag.toLowerCase().equals("jobid"))
		{
				return this.getEngine().getJobId().toString();
		}
//ORIGINAL LINE: case "template":
		else if (tag.toLowerCase().equals("template"))
		{
				IJob job = this.Site.<IJob>GetRequiredService();
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
		return Array.indexOf(tags, tag.toLowerCase()) >= 0;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}