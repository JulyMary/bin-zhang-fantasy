package fantasy.jobs.management;

import java.io.Serializable;
import java.util.*;
import java.util.regex.Pattern;

import org.apache.commons.lang3.StringUtils;
import org.jdom2.Element;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.*;


public class JobMetaData implements Serializable
{


	/**
	 * 
	 */
	private static final long serialVersionUID = -6391831625988567054L;

	/** 
	 Create a new JobMetaData object.
	 
	 @param id Initial value of the Id property.
	 @param template Initial value of the Template property.
	 @param name Initial value of the Name property.
	 @param state Initial value of the State property.
	 @param creationTime Initial value of the CreationTime property.
	 @param application Initial value of the Application property.
	 @param user Initial value of the User property.
	*/
	public static JobMetaData CreateJobMetaData(UUID id, String template, String name, int state, java.util.Date creationTime, String application, String user)
	{
		JobMetaData jobMetaData = new JobMetaData();
		jobMetaData.setId(id);
		jobMetaData.setTemplate(template);
		jobMetaData.setName(name);
		jobMetaData.setState(state);
		jobMetaData.setCreationTime(creationTime);
		jobMetaData.setApplication(application);
		jobMetaData.setUser(user);
		return jobMetaData;
	}

	public final UUID getId()
	{
		return _Id;
	}
	public final void setId(UUID value)
	{
		if (!_Id.equals(value))
		{
			_Id = value;	
		}
	}
	private UUID _Id = UUID.randomUUID();

	public final UUID getParentId()
	{
		return _ParentId;
	}
	public final void setParentId(UUID value)
	{
		_ParentId = value;
	}
	private UUID _ParentId;
	public final String getTemplate()
	{
		return _Template;
	}
	public final void setTemplate(String value)
	{
		_Template = value;
	}
	private String _Template;

	public final String getName()
	{
		return _Name;
	}
	public final void setName(String value)
	{
		_Name = value;
		
	}
	private String _Name;

	public final int getState()
	{
		return _State;
	}
	public final void setState(int value)
	{
		_State = value;
	}
	private int _State;

	public final int getPriority()
	{
		return _Priority;
	}
	public final void setPriority(int value)
	{
		
		_Priority = value;
		
	}
	private int _Priority = 0;
	public final java.util.Date getStartTime()
	{
		return _StartTime;
	}
	public final void setStartTime(java.util.Date value)
	{
		
		_StartTime = value;
		
	}
	private java.util.Date _StartTime;

	public final java.util.Date getEndTime()
	{
		return _EndTime;
	}
	public final void setEndTime(java.util.Date value)
	{
	
		_EndTime = value;
			}
	private java.util.Date _EndTime;

	public final java.util.Date getCreationTime()
	{
		return _CreationTime;
	}
	public final void setCreationTime(java.util.Date value)
	{
		
		
		_CreationTime = value;
	
		
	}
	private java.util.Date _CreationTime = new java.util.Date(0);

	public final String getApplication()
	{
		return _Application;
	}
	public final void setApplication(String value)
	{
		
		_Application = value;
		
	}
	private String _Application;

	public final String getUser()
	{
		return _User;
	}
	public final void setUser(String value)
	{
		_User = value;
	}
	private String _User;

	public final String getStartInfo()
	{
		return _StartInfo;
	}
	public final void setStartInfo(String value)
	{
		
		_StartInfo = value;
		
	}
	private String _StartInfo;
	public final String getTag()
	{
		return _Tag;
	}
	public final void setTag(String value)
	{
		
		_Tag = value;
		
	}
	private String _Tag;



	public final boolean getIsTerminated()
	{
		return (this.getState() & JobState.Terminated) == JobState.Terminated;
	}

	private String GetStartInfoProperty(Element root, String name) throws Exception
	{
		
		for(Element group : new Enumerable<Element>(root.getChildren()).where(new Predicate<Element>(){

			@Override
			public boolean evaluate(Element obj) throws Exception {
				return obj.getName() == "properties";
			}}))
		{
			for(Element prop : group.getChildren())
			{
			    if(StringUtils.equalsIgnoreCase(prop.getName(), name))
			    {
			    	return prop.getText();
			    }
			}
		}
		
		return null;

	
	}


	public final void LoadXml(String xml) throws Exception
	{
		Element root = JDomUtils.parseElement(xml);


		UUID id = (!StringUtils2.isNullOrEmpty(root.getAttributeValue("id"))) ? UUID.fromString(root.getAttributeValue("id")) : UUID.randomUUID();
		
		this.setId(id);
		this.setTemplate(root.getAttributeValue("template"));

		if (StringUtils2.isNullOrWhiteSpace(this.getTemplate()))
		{
			throw new JobException("Missing job template");
		}

		String name = this.GetStartInfoProperty(root, "name");

		this.setName((name != null) ? name : this.getTemplate());

		try
		{

			this.setApplication(this.GetStartInfoProperty(root, "application"));
		}
		catch (java.lang.Exception e)
		{
			throw new JobException("Missing job application");
		}


		this.setCreationTime(new java.util.Date());
		this.setState(JobState.Unstarted);


		try
		{
			this.setUser(this.GetStartInfoProperty(root, "user"));
		}
		catch (java.lang.Exception e2)
		{
			throw new JobException("Missing job user");
		}

		String sp = this.GetStartInfoProperty(root, "priority");
		int priority = 0;
		if(sp != null && Pattern.matches("^\\d+$", sp))
		{
			priority = Integer.parseInt(sp);
		}
		
	    this.setPriority(priority);
		

		this.setStartInfo(xml);

		this.setTag(this.GetStartInfoProperty(root, "tag"));
	}

}