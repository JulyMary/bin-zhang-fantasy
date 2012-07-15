package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("searchFiles", Consts.XNamespaceURI, Description="Search files in directory")]
public class SearchFilesTask extends ObjectWithSite implements ITask
{
	public SearchFilesTask()
	{

		setRecursive(false);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("path", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The directory from those to retrive the files.")]
	private String privatePath;
	public final String getPath()
	{
		return privatePath;
	}
	public final void setPath(String value)
	{
		privatePath = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("pattern", Description="The search string to match against the names of files in path.")]
	private String privatePattern;
	public final String getPattern()
	{
		return privatePattern;
	}
	public final void setPattern(String value)
	{
		privatePattern = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("result", Flags=TaskMemberFlags.Output, Description="The list contains items in specified directory that match the specified search pattern.")]
	private TaskItem[] privateResult;
	public final TaskItem[] getResult()
	{
		return privateResult;
	}
	public final void setResult(TaskItem[] value)
	{
		privateResult = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("recursive", Description="true if includes the current directory and all the subdirectories in a search operation; otherwise only top directory in a search.")]
	 boolean getRecursive()
	 void setRecursive(boolean value)

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (this.getPath() != null && LongPathDirectory.Exists(this.getPath()))
		{
			IJobEngine engine = this.Site.<IJobEngine>GetRequiredService();
			java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
			Iterable<String> files = this.getRecursive() ? LongPathDirectory.EnumerateAllFiles(this.getPath(), getPattern()) : LongPathDirectory.EnumerateFiles(this.getPath(), getPattern());
			for (String f : files)
			{
				String name = LongPath.GetRelativePath(engine.getJobDirectory() + "\\", f);
				TaskItem tempVar = new TaskItem();
				tempVar.setName(name);
				TaskItem item = tempVar;
				rs.add(item);
			}
			this.setResult(rs.toArray(new TaskItem[]{}));

		}
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}