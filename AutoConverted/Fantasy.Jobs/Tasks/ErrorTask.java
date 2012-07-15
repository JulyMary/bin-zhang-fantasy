package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("error", Consts.XNamespaceURI, Description="Throw an error immediately")]
public class ErrorTask extends ObjectWithSite implements ITask
{

	private String _category = LogCategories.getCustomError();
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("category", Description="Error category")]

	public final String getCategroy()
	{
		return _category;
	}
	public final void setCategroy(String value)
	{
		_category = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("message", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Error message")]
	private String privateMessage;
	public final String getMessage()
	{
		return privateMessage;
	}
	public final void setMessage(String value)
	{
		privateMessage = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		if (logger != null)
		{
			logger.LogError(this.getCategroy(), this.getMessage());
		}

		return false;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}