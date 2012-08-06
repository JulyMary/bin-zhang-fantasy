package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "escapeFileName", namespaceUri = Consts.XNamespaceURI, description="Replace invalid charactors of file name with empty string (\"\")")
public class EscapeFileNameTask extends ObjectWithSite implements ITask
{

	public final void Execute()
	{
		if (this.Source != null)
		{
			this.Result = Path.escapeFileName(this.Source, this.Replacement).trim();
		}
		
	}

	@TaskMember(name = "source", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="File name to escape")
	public String Source;
	
	@TaskMember(name = "result", flags=TaskMemberFlags.Output, description="Escaped file name")
	public String Result;

	@TaskMember(name = "replacement", description="The string to replace all invalid characters in source string.")
	public String Replacement;
	
}