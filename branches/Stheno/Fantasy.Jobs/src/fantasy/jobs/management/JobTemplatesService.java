package fantasy.jobs.management;

import java.io.*;
import java.rmi.RemoteException;

import org.apache.commons.io.filefilter.*;
import org.apache.commons.lang3.StringUtils;
import org.jdom2.*;

import fantasy.JDomUtils;
import fantasy.jobs.*;
import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.collections.*;


public class JobTemplatesService extends AbstractService implements IJobTemplatesService
{

	public JobTemplatesService() throws RemoteException {
		super();
		
	}



	/**
	 * 
	 */
	private static final long serialVersionUID = -5787564023533957679L;
	private ILogger _logger;
	public final ILogger getLogger() throws Exception
	{
		if (_logger == null)
		{
			_logger =this.getSite().getService(ILogger.class);
		}
		return _logger;
	}



	private java.util.ArrayList<JobTemplate> _templates = new java.util.ArrayList<JobTemplate>();
	private java.util.ArrayList<JobTemplate> _invalidTemplates = new java.util.ArrayList<JobTemplate>();

	private int _sequence = 0;

	@Override
	public void initializeService() throws Exception
	{
		File dir = new File(JobManagerSettings.getDefault().getJobTemplateDirectoryFullPath());
		if (dir.exists())
		{
			LoadTemplates(dir);
		}
		super.initializeService();
	}

	private void LoadTemplates(File dir) throws Exception
	{
		for(File fi : dir.listFiles((FileFilter)new SuffixFileFilter(".jt")))
		{
			LoadTemplate(fi, false);
		}
		for (File sub : dir.listFiles((FileFilter)DirectoryFileFilter.INSTANCE))
		{
			LoadTemplates(sub);
		}
	}

	private void LoadTemplate(File fi, boolean replace) throws Exception
	{
		final JobTemplate template = new JobTemplate();
		template.setid(this._sequence++);
		template.setLocation(fi.getAbsolutePath());
		template.setValid(false);
		try
		{


			
			String text = fantasy.io.File.readAllText(fi.getAbsolutePath(), "UTF-8");
			template.setContent(text);
			
			Element element =  JDomUtils.parseElement(text);

			
			template.setName(element.getAttributeValue("name"));

			if (!StringUtils2.isNullOrEmpty(template.getName()))
			{

				JobTemplate old = new Enumerable<JobTemplate>(this._templates).firstOrDefault(new Predicate<JobTemplate>(){

					@Override
					public boolean evaluate(JobTemplate t) throws Exception {
						return StringUtils.equalsIgnoreCase(template.getName(), t.getName());
					}});
				if(old != null)
				{
					if(replace)
					{
						this._templates.remove(old);
					}
					else
					{
						throw new IllegalStateException(String.format(Resources.getDulplicateTemplateNamesText(), old.getLocation(), fi.getAbsolutePath(), old.getName()));
					}
				}
				template.setValid(true);
				this._templates.add(template);
				getLogger().LogMessage(LogCategories.getManager(), Resources.getSuccessToLoadTemplateText(), fi.getAbsolutePath(), template.getName());
			}
			else
			{
				template.setValid(true);
				this._templates.add(template);

				getLogger().LogMessage(LogCategories.getManager(), Resources.getSuccessToLoadAnonymousTemplateText(), fi.getAbsolutePath());
			}

		}
		catch(Exception ex)
		{
			getLogger().LogWarning(LogCategories.getManager(), ex, MessageImportance.High, Resources.getFailToLoadTemplateText(), fi.getAbsolutePath());
		}
		if (!template.getValid())
		{
			this._invalidTemplates.add(template);
		}
	}



	public final JobTemplate GetJobTemplateByName(final String name) throws Exception
	{
			
			JobTemplate rs = new Enumerable<JobTemplate>(this._templates).firstOrDefault(new Predicate<JobTemplate>(){

					@Override
					public boolean evaluate(JobTemplate t) throws Exception {
						return StringUtils.equalsIgnoreCase(name, t.getName());
					}});
			if(rs == null)
			{
				throw new JobException(String.format(Resources.getCannotFindTemplateByNameText(), name));
			}
			
			return rs;
		
		

	}

	public final JobTemplate GetJobTemplateByPath(final String path) throws Exception
	{
		JobTemplate rs = new Enumerable<JobTemplate>(this._templates).firstOrDefault(new Predicate<JobTemplate>(){

			@Override
			public boolean evaluate(JobTemplate t) throws Exception {
				return StringUtils.equalsIgnoreCase(path, t.getLocation());
			}});
		if(rs == null)
		{
			throw new JobException(String.format(Resources.getCannotFindTemplateByPathText(), path));
		}

		return rs;
	}


	public final JobTemplate[] GetJobTemplates()
	{
		java.util.ArrayList<JobTemplate> rs = new java.util.ArrayList<JobTemplate>();
		rs.addAll(this._templates);
		rs.addAll(this._invalidTemplates);
		return rs.toArray(new JobTemplate[]{});
	}
}