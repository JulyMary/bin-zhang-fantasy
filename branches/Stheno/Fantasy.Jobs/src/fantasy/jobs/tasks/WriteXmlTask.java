package fantasy.jobs.tasks;

import org.jdom2.*;

import fantasy.collections.*;
import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name="writeXml", namespaceUri=Consts.XNamespaceURI, description="Write xml to file")
public class WriteXmlTask extends XmlTaskBase
{


	@TaskMember(name = "include", flags = { TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items to write to.")
	public TaskItem[] Include;
	

    @TaskMember(name = "content", flags = {TaskMemberFlags.Input, TaskMemberFlags.Inline, TaskMemberFlags.Required}, description="The XML content to write to.")
	public Element Content;
	



	@Override
	public void Execute() throws Exception
	{
		if (this.Include != null && this.Include.length > 0)
		{

			IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);

			Element root = new Enumerable<Element>(this.Content.getChildren()).first();


			for (TaskItem item : this.Include)
			{

				String path = Path.combine(engine.getJobDirectory(), item.getName());

				JDomUtils.saveElement(root, path, this.getFormat());

			}
		}

		
	}

}