package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("xpathReadValue", Consts.XNamespaceURI, Description="Using XPath to read value from xml file")]
public class XPathReadValue extends ObjectWithSite implements ITask
{
	public final boolean Execute()
	{
		java.util.ArrayList<String> values = new java.util.ArrayList<String>();
		if (getInclude() != null && getInclude().length > 0)
		{

			for (TaskItem item : getInclude())
			{
				String file = item.getItem("fullname");
				XDocument doc = LongPathXNode.LoadXDocument(file);




				XmlNamespaceManager mngr = this.getSite().<XmlNamespaceManager>GetRequiredService();
				Iterable targets = (Iterable)doc.XPathEvaluate(getXPath(), mngr);

				for (Object target : targets)
				{
					if (target instanceof XElement)
					{
						values.add((String)(XElement)target);
					}
					else if (target instanceof XAttribute)
					{
						values.add((String)(XAttribute)target);
					}
				}

			}
		}
		this.setValue(values.toArray(new String[]{}));

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items from those to retrive values")]
	private TaskItem[] privateInclude;
	public final TaskItem[] getInclude()
	{
		return privateInclude;
	}
	public final void setInclude(TaskItem[] value)
	{
		privateInclude = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The XPath expression to read value.")]
	private String privateXPath;
	public final String getXPath()
	{
		return privateXPath;
	}
	public final void setXPath(String value)
	{
		privateXPath = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("value", Flags = TaskMemberFlags.Output, Description="A list of string contains the value read form include items using specified XPath.")]
	private String[] privateValue;
	public final String[] getValue()
	{
		return privateValue;
	}
	public final void setValue(String[] value)
	{
		privateValue = value;
	}
}