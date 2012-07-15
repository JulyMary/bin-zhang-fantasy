package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("deleteDirectory", Consts.XNamespaceURI, Description="Delete directories")]
public class DeleteDirectoryTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("path", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of directory path to delete.")]
	private String[] privatePath;
	public final String[] getPath()
	{
		return privatePath;
	}
	public final void setPath(String[] value)
	{
		privatePath = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("recursive", Flags = TaskMemberFlags.Input, Description="ture if delete all child directory in path; otherwise false")]
	 boolean getRecursive()
	 void setRecursive(boolean value)

	public DeleteDirectoryTask()
	{
		setRecursive(false);
	}

	public final boolean Execute()
	{
		if (getPath() != null && getPath().length > 0)
		{
			ILogger logger = this.Site.<ILogger>GetService();
			for (String path : this.getPath())
			{
				if (LongPathDirectory.Exists(path))
				{
					LongPathDirectory.Delete(path,this.getRecursive());
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

		return true;

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}