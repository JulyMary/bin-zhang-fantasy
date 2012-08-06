package fantasy.jobs.tasks;


import org.jdom2.*;

import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "xpathSetElement", namespaceUri= Consts.XNamespaceURI, description="Using XPath to set xml element value to xml file")
public class XPathSetElementTask extends XPathSetValueTaskBase
{
	@Override
	protected void SetValue(Element element, INamespaceManager nsMgr)
	{
		element.setText(this.Value);
	}
}