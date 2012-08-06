package fantasy.jobs.tasks;

import org.jdom2.*;
import org.jdom2.filter.Filters;
import org.jdom2.xpath.XPathExpression;
import org.jdom2.xpath.XPathFactory;

import fantasy.io.*;
import fantasy.*;
import fantasy.jobs.*;



public abstract class XPathSetValueTaskBase extends XmlTaskBase
{

	protected abstract void SetValue(Element element, INamespaceManager nsMgr) throws Exception;

	@Override
	public void Execute() throws Exception
	{
		
		if (this.Include != null && this.Include.length > 0)
		{
			
			IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
			INamespaceManager mngr = this.getSite().getRequiredService(INamespaceManager.class);
			XPathExpression<Element> expression = XPathFactory.instance().compile(this.XPath, Filters.element(), null, mngr.getNamespaces());

			
			for (TaskItem item : this.Include)
			{


				String file = Path.combine(engine.getJobDirectory(), item.getName());
				Document doc = JDomUtils.loadDocument(file);
				
				Iterable<Element> targets = expression.evaluate(doc);

				for (Element target : targets)
				{
					SetValue(target, mngr);
				}

				JDomUtils.saveDocument(doc, file);
			}
		}

		
	}

	@TaskMember(name = "include", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items to set values")
	public TaskItem[] Include;
	

	@TaskMember(name = "xpath", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The XPath expression indicating the target elements in incude XML files.")
	public String XPath;
	

    @TaskMember(name = "value", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The value to set to.")
	public String Value;
	
}