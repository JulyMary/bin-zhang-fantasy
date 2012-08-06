package fantasy.jobs.tasks;

import org.jdom2.*;
import org.jdom2.filter.*;
import org.jdom2.xpath.*;


import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "xpathReadValue", namespaceUri = Consts.XNamespaceURI, description="Using XPath to read value from xml file")
public class XPathReadValue extends ObjectWithSite implements ITask
{
	public final void Execute() throws Exception
	{
		java.util.ArrayList<String> values = new java.util.ArrayList<String>();
		if (this.Include != null && this.Include.length > 0)
		{
			IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
			INamespaceManager mngr = this.getSite().getRequiredService(INamespaceManager.class);
			XPathExpression<?> expression = XPathFactory.instance().compile(this.XPath, Filters.fpassthrough(), null, mngr.getNamespaces());

			for (TaskItem item : this.Include)
			{
				String file = Path.combine(engine.getJobDirectory(), item.getName());
				Document doc = JDomUtils.loadDocument(file);

				Iterable<?> targets = expression.evaluate(doc);

				for (Object target : targets)
				{
					if (target instanceof Element)
					{
						values.add(((Element)target).getTextTrim());
					}
					else if (target instanceof Attribute)
					{
						values.add(((Attribute)target).getValue());
					}
				}

			}
		}
		this.Value = values.toArray(new String[]{});

	}

	@TaskMember(name = "include", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items from those to retrive values")
	public TaskItem[] Include;
	

	@TaskMember(name = "xpath", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The XPath expression to read value.")
	public String XPath;
	

    @TaskMember(name = "value", flags = TaskMemberFlags.Output, description="A list of string contains the value read form include items using specified XPath.")
	private String[] Value;
	
}