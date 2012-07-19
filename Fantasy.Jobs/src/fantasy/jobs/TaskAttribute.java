package fantasy.jobs;

import fantasy.jobs.Properties.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Class)]
public class TaskAttribute extends Attribute
{
	public TaskAttribute(String name, String namespaceUri)
	{
		this.setName(name);
		this.setNamespaceUri(namespaceUri);
	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	private void setName(String value)
	{
		privateName = value;
	}
	private String privateNamespaceUri;
	public final String getNamespaceUri()
	{
		return privateNamespaceUri;
	}
	private void setNamespaceUri(String value)
	{
		privateNamespaceUri = value;
	}

	private String privateDescription;
	public final String getDescription()
	{
		return privateDescription;
	}
	public final void setDescription(String value)
	{
		privateDescription = value;
	}
}