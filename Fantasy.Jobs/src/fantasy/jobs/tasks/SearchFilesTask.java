package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "searchFiles", namespaceUri = Consts.XNamespaceURI, description="Search files in directory")
public class SearchFilesTask extends ObjectWithSite implements ITask
{
	
	
	@TaskMember(name = "path", flags={TaskMemberFlags.Input , TaskMemberFlags.Required}, description="The directory from those to retrive the files.")
	public String Path;
	

	@TaskMember(name = "pattern", description="The search string to match against the names of files in path.")
	public String Pattern = "*.*";
	

	@TaskMember(name = "result", flags=TaskMemberFlags.Output, description="The list contains items in specified directory that match the specified search pattern.")
	public TaskItem[] Result;
	

	

	@TaskMember(name = "recursive", description="true if includes the current directory and all the subdirectories in a search operation; otherwise only top directory in a search.")
	public boolean Recursive = false;

	public final void Execute() throws Exception
	{
		if (this.Path != null && Directory.exists(this.Path))
		{
			
			java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
			Iterable<String> files = Directory.enumerateFiles(this.Path, this.Pattern, this.Recursive);
			for (String f : files)
			{
				
				TaskItem tempVar = new TaskItem();
				tempVar.setName(f);
				TaskItem item = tempVar;
				rs.add(item);
			}
			this.Result = rs.toArray(new TaskItem[]{});

		}
		
	}


}