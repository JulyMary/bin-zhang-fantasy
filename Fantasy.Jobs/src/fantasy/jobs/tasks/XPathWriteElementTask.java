package fantasy.jobs.tasks;

import java.util.*;

import org.jdom2.*;
import org.jdom2.filter.Filters;
import org.jdom2.xpath.*;


import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "xpathWriteElement", namespaceUri= Consts.XNamespaceURI, description="Using XPath to write xml element to xml file")
public class XPathWriteElementTask extends XmlTaskBase
{
	

	@Override
	public void Execute() throws Exception
	{
		if (this.Include != null && this.Include.length > 0)
		{

			INamespaceManager mngr = this.getSite().getRequiredService(INamespaceManager.class);
			IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
			
			XPathExpression<Element> expression = XPathFactory.instance().compile(this.XPath, Filters.element(), null, mngr.getNamespaces());

			for (TaskItem item : this.Include)
			{
				String file = Path.combine(engine.getJobDirectory(), item.getName());
				Document doc = JDomUtils.loadDocument(file);


				Iterable<Element> targets = expression.evaluate(doc);

				for (Element target : targets)
				{
					List<Content> newElements = this.Content.cloneContent();

					switch (this.Mode)
					{
						case AddAfterSelf:
						{
							int index = target.getParentElement().indexOf(target);
							target.getParentElement().addContent(index + 1, newElements);
						}
							break;
						case AddBeforeSelf:
						{
							int index = target.getParentElement().indexOf(target);
							target.getParentElement().addContent(index, newElements);
						}
							break;
						case AddFirst:
							target.addContent(0, newElements);
							break;
						case Add:
							target.addContent(newElements);
							break;
						case ReplaceWith:
							{
								int index = target.getParentElement().indexOf(target);
								Element parent = target.getParentElement();
								target.detach();
								parent.addContent(index, newElements);
							}
							break;
						case ReplaceAll:
							target.setContent(newElements);
							break;
					}
				}

				JDomUtils.saveDocument(doc, file, this.getFormat());

			}
		}
		
	}




	@TaskMember(name = "include", flags = { TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items to write to.")
	public TaskItem[] Include;
	

    @TaskMember(name = "content", flags = {TaskMemberFlags.Input, TaskMemberFlags.Inline, TaskMemberFlags.Required}, description="The XML content to write to.")
	public Element Content;

    @TaskMember(name = "xpath", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The XPath expression indicating the target elements in incude XML files.")
	public String XPath;


    @TaskMember(name = "mode", description="A value indicating how the conent write to target elements. Default is Add for adding new a child element in the end of target elements.")
	public XPathWriteElementMode Mode = XPathWriteElementMode.Add;
	
}