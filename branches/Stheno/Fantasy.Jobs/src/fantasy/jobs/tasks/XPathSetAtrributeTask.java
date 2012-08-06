package fantasy.jobs.tasks;

import org.jdom2.*;

import fantasy.collections.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "xpathSetAttribute", namespaceUri = Consts.XNamespaceURI, description = "Using XPath to set xml attribute value to xml file")
public class XPathSetAtrributeTask extends XPathSetValueTaskBase
{
	@Override
	protected void SetValue(Element element, INamespaceManager nsMgr) throws Exception
	{
		final String[] names = StringUtils2.split(this.Attribute, ":", 2, true);
		
		if (names.length == 2)
		{
			Namespace ns = new Enumerable<Namespace>(nsMgr.getNamespaces()).single(new Predicate<Namespace>(){

				@Override
				public boolean evaluate(Namespace obj) throws Exception {
					return obj.getPrefix() == names[0];
				}});
			
			element.setAttribute(names[1], this.Value, ns);
		}
		else
		{
			element.setAttribute(names[0], this.Value);
		}

		

	}


    @TaskMember(name = "attribute", flags= {TaskMemberFlags.Input, TaskMemberFlags.Required }, description="The attribute name to set value.")
	public String Attribute = "";
	
}