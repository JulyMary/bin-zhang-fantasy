package fantasy.jobs;

import fantasy.jobs.Properties.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class TaskMemberAttribute extends Attribute
{
	public TaskMemberAttribute(String name)
	{
		this.setName(name);
		this.setFlags(TaskMemberFlags.Input);
		this.setParseInline(true);
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

	private TaskMemberFlags privateFlags = TaskMemberFlags.forValue(0);
	public final TaskMemberFlags getFlags()
	{
		return privateFlags;
	}
	public final void setFlags(TaskMemberFlags value)
	{
		privateFlags = value;
	}

	private boolean privateParseInline;
	public final boolean getParseInline()
	{
		return privateParseInline;
	}
	public final void setParseInline(boolean value)
	{
		privateParseInline = value;
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