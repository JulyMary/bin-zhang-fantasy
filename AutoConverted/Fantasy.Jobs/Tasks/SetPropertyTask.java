package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("property", Consts.XNamespaceURI, Description="Set property value. This task is deprecated. Please use 'properties' element in instruction to set property value")]
public class SetPropertyTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		IStringParser parser = this.Site.<IStringParser>GetRequiredService();
		ILogger logger = this.Site.<ILogger>GetService();
		IJob job = this.Site.<IJob>GetRequiredService();

		String name = parser.Parse(this.getName());
		String value = parser.Parse(this.getValue());

		job.SetProperty(name, value);
		if (logger != null)
		{
			logger.LogMessage("property", MessageImportance.Low, "set property {0} as {1}", name, value);
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("name", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required)]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("value", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required)]
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}
}