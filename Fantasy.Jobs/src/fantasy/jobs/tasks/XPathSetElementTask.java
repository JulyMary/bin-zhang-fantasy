package fantasy.jobs.tasks;


import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("xpathSetElement", Consts.XNamespaceURI, Description="Using XPath to set xml element value to xml file")]
public class XPathSetElementTask extends XPathSetValueTaskBase
{
	@Override
	protected void SetValue(XElement element, XmlNamespaceManager nsMgr)
	{
		element.setValue(this.getValue());
	}
}