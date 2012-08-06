package fantasy.jobs.tasks;

import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "log", namespaceUri = Consts.XNamespaceURI, description="Write log")
public class LogTask extends ObjectWithSite implements ITask
{

	public final void Execute() throws Exception
	{
		ILogger logger = (ILogger)this.getSite().getService(ILogger.class);
		
		if (logger != null)
		{
			logger.LogMessage(this.Category, this.Importance, this.Message);
		}

		
	}

	@TaskMember(name = "message", flags= {TaskMemberFlags.Input , TaskMemberFlags.Required }, description="Log message.")
	public String Message = "";
	

	@TaskMember(name = "category", description="Log category.")
	public String Category = LogCategories.getLogTask();

	
	@TaskMember(name = "importance", description="Importance level of log message.")
	public MessageImportance Importance = MessageImportance.Normal;

}