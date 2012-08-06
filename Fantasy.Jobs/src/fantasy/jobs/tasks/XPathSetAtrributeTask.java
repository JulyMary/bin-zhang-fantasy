package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("xpathSetAttribute", Consts.XNamespaceURI, Description = "Using XPath to set xml attribute value to xml file")]
public class XPathSetAtrributeTask extends XPathSetValueTaskBase
{
	@Override
	protected void SetValue(System.Xml.Linq.XElement element, XmlNamespaceManager nsMgr)
	{
		String[] names = this.getAttribute().split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
		XName name;
		if (names.length == 2)
		{
			XNamespace ns = nsMgr.LookupNamespace(names[0]);
			name = ns + names[1];
		}
		else
		{
			name = names[0];
		}

		element.SetAttributeValue(name, this.getValue());

	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("attribute", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The attribute name to set value.")]
	private String privateAttribute;
	public final String getAttribute()
	{
		return privateAttribute;
	}
	public final void setAttribute(String value)
	{
		privateAttribute = value;
	}
}