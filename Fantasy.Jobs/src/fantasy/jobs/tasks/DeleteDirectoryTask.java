package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "deleteDirectory", namespaceUri = Consts.XNamespaceURI, description="Delete directories")
public class DeleteDirectoryTask extends ObjectWithSite implements ITask
{


	@TaskMember(name = "path", flags = {TaskMemberFlags.Input , TaskMemberFlags.Required}, description="The list of directory path to delete.")
	public String[] Path;
	

	//[TaskMember("recursive", Flags = TaskMemberFlags.Input, Description="ture if delete all child directory in path; otherwise false")]
	 public boolean Recursive = false;
	

	
	public final void Execute() throws Exception
	{
		if (this.Path != null && this.Path.length > 0)
		{
			ILogger logger = this.getSite().getService(ILogger.class);
			for (String path : this.Path)
			{
				if (Directory.exists(path))
				{
					Directory.delete(path,this.Recursive);
					if (logger != null)
					{
						logger.LogMessage("deleteDirectory", "Delete directory {0}.", path);
					}
				}
				else
				{
					logger.LogWarning("deleteDirectory", MessageImportance.Low, "Directory {0} does not exist.", path);
				}
			}

		}

	}

}