package Fantasy.Jobs.Management;

import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public class JobTemplatesService extends AbstractService implements IJobTemplatesService
{

	private ILogger _logger;
	public final ILogger getLogger()
	{
		if (_logger == null)
		{
			_logger = (ILogger)this.Site.GetService(ILogger.class);
		}
		return _logger;
	}



	private java.util.ArrayList<JobTemplate> _templates = new java.util.ArrayList<JobTemplate>();
	private java.util.ArrayList<JobTemplate> _invalidTemplates = new java.util.ArrayList<JobTemplate>();

	private int _sequence = 0;

	@Override
	public void InitializeService()
	{
		DirectoryInfo dir = new DirectoryInfo(JobManagerSettings.getDefault().getJobTemplateDirectoryFullPath());
		if (dir.Exists)
		{
			LoadTemplates(dir);
		}
		super.InitializeService();
	}

	private void LoadTemplates(DirectoryInfo dir)
	{
		for(FileInfo fi : dir.GetFiles("*.jt"))
		{
			LoadTemplate(fi, false);
		}
		for (DirectoryInfo sub : dir.GetDirectories())
		{
			LoadTemplates(sub);
		}
	}

	private void LoadTemplate(FileInfo fi, boolean replace)
	{
		JobTemplate template = new JobTemplate();
		template.setid(this._sequence++);
		template.setLocation(fi.FullName);
		template.setValid(false);
		try
		{


			XmlDocument doc = new XmlDocument();

			String text = File.ReadAllText(fi.FullName);
			template.setContent(text);
			doc.LoadXml(text);
			template.setName(doc.DocumentElement.GetAttribute("name"));

			if (!DotNetToJavaStringHelper.isNullOrEmpty(template.getName()))
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				JobTemplate old = (from t in this._templates where (StringComparer.OrdinalIgnoreCase.Compare(t.getName(), template.getName()) == 0) select t).SingleOrDefault();
				if(old != null)
				{
					if(replace)
					{
						this._templates.remove(old);
					}
					else
					{
						throw new ApplicationException(String.format(Properties.Resources.getDulplicateTemplateNamesText(), old.getLocation(), fi.FullName, old.getName()));
					}
				}
				template.setValid(true);
				this._templates.add(template);
				getLogger().LogMessage(LogCategories.getManager(), Properties.Resources.getSuccessToLoadTemplateText(), fi.FullName, template.getName());
			}
			else
			{
				template.setValid(true);
				this._templates.add(template);

				getLogger().LogMessage(LogCategories.getManager(), Properties.Resources.getSuccessToLoadAnonymousTemplateText(), fi.FullName);
			}

		}
		catch(RuntimeException ex)
		{
			getLogger().LogWarning(LogCategories.getManager(), ex, MessageImportance.High, Properties.Resources.getFailToLoadTemplateText(), fi.FullName);
		}
		if (!template.getValid())
		{
			this._invalidTemplates.add(template);
		}
	}




//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobTemplatesService Members

	public final JobTemplate GetJobTemplateByName(String name)
	{
		try
		{

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			return (from template in this._templates where StringComparer.OrdinalIgnoreCase.Compare(template.getName(), name) == 0 select template).Single();
		}
		catch (InvalidOperationException e)
		{
			throw new JobException(String.format(Properties.Resources.getCannotFindTemplateByNameText(), name));
		}

	}

	public final JobTemplate GetJobTemplateByPath(String path)
	{
		Uri uri = new Uri(path);
		try
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			return (from template in this._templates where (new Uri(template.getLocation())) == uri select template).Single();
		}
		catch (InvalidOperationException e)
		{
			throw new JobException(String.format(Properties.Resources.getCannotFindTemplateByPathText(), path));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


	public final JobTemplate[] GetJobTemplates()
	{
		java.util.ArrayList<JobTemplate> rs = new java.util.ArrayList<JobTemplate>();
		rs.addAll(this._templates);
		rs.addAll(this._invalidTemplates);
		return rs.toArray(new JobTemplate[]{});
	}
}