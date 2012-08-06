package fantasy.jobs.tasks;

import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "error", namespaceUri=Consts.XNamespaceURI, description="Throw an error immediately")
public class ErrorTask extends ObjectWithSite implements ITask
{

	@TaskMember(name="category", description = "Category log to.")
	public String Category = LogCategories.getCustomError();


	@TaskMember(name = "message", flags = {TaskMemberFlags.Input,TaskMemberFlags.Required}, description="Error message")
	public String Message;
	
	public final void Execute() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		if (logger != null)
		{
			logger.LogError(this.Category, this.Message);
		}

		throw new JobException(this.Message);
	}
}