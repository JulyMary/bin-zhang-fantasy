package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("createDirectory", Consts.XNamespaceURI, Description="Create directories")]
public class CreateDirectoryTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (getPath() != null && getPath().length > 0)
		{
			ILogger logger = this.Site.<ILogger>GetService();
			for (String path : this.getPath())
			{
				if (!LongPathDirectory.Exists(path))
				{
					LongPathDirectory.Create(path);
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

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("path", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of directory path to create.")]
	private String[] privatePath;
	public final String[] getPath()
	{
		return privatePath;
	}
	public final void setPath(String[] value)
	{
		privatePath = value;
	}
}