package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name="writeText", namespaceUri=Consts.XNamespaceURI, description = "Write text to file")
public class WriteTextTask extends ObjectWithSite implements ITask
{
	

	public final void Execute() throws Exception
	{
		if (!StringUtils2.isNullOrWhiteSpace(this.Path) && this.Text != null)
		{
			
			
			if (this.Append)
			{
				File.appendAllText(this.Path, this.Text, this.Encoding);
			}
			else
			{
				File.writeAllText(this.Path, this.Text, this.Encoding);
			}
		}

		
	}


	@TaskMember(name = "path", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required},  description="The file to write to.")
	public String Path;
	

	@TaskMember(name = "append", description="true if append text to the end of file if the source file exists; otherwise the write opration overwrite existing file.")
	public boolean Append = true;

    @TaskMember(name = "text", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The content to write to.")
	public String Text;


	@TaskMember(name = "encoding", flags=TaskMemberFlags.Input, description="The encoding to apply to the string. ")
	public String Encoding = java.nio.charset.Charset.defaultCharset().name();
	
}