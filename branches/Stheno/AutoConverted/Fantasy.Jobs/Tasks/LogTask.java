package Fantasy.Jobs.Tasks;

import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("log", Consts.XNamespaceURI, Description="Write log")]
public class LogTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
		if (getMessage() == null)
		{
			setMessage("");
		}
		if (logger != null)
		{
			logger.LogMessage(this.getCategroy(), this.getImportance(), getMessage());
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("message", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Log message.")]
	private String privateMessage;
	public final String getMessage()
	{
		return privateMessage;
	}
	public final void setMessage(String value)
	{
		privateMessage = value;
	}


	private String _category = LogCategories.getLogTask();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("category", Description="Log category.")]

	public final String getCategroy()
	{
		return _category;
	}
	public final void setCategroy(String value)
	{
		_category = value;
	}


	private MessageImportance _importance = MessageImportance.Normal;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("importance", Description="Importance level of log message.")]
	public final MessageImportance getImportance()
	{
		return _importance;
	}
	public final void setImportance(MessageImportance value)
	{
		_importance = value;
	}



}