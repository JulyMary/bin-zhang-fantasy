package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "createDirectory", namespaceUri = Consts.XNamespaceURI, description="Create directories")
public class CreateDirectoryTask extends ObjectWithSite implements ITask
{


	public final void Execute() throws Exception
	{
		if (this.Path != null && this.Path.length > 0)
		{
			ILogger logger = this.getSite().getService(ILogger.class);
			for (String path : this.Path)
			{
				if (!Directory.exists(path))
				{
					Directory.create(path);
					if (logger != null)
					{
						logger.LogMessage("createDirectory", "Create directory {0}.", path);
					}

				}
				else
				{
					logger.LogWarning("createDirectory", MessageImportance.Low, "Directory {0} already exists.", path);
				}
			}

		}
	}

	@TaskMember(name = "path", flags={TaskMemberFlags.Input , TaskMemberFlags.Required}, description="The list of directory path to create.")
	public String[] Path;
	
}