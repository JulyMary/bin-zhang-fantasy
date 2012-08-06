package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("xpathWriteElement", Consts.XNamespaceURI, Description="Using XPath to write xml element to xml file")]
public class XPathWriteElementTask extends XmlTaskBase
{
	public XPathWriteElementTask()
	{
		this.setMode(XPathWriteElementMode.Add);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	@Override
	public boolean Execute()
	{
		if (getInclude() != null && getInclude().length > 0)
		{

			XmlNamespaceManager mngr = this.getSite().<XmlNamespaceManager>GetRequiredService();
			for (TaskItem item : getInclude())
			{
				String file = item.getItem("fullname");
				XDocument doc = LongPathXNode.LoadXDocument(file);


				XElement[] targets = doc.XPathSelectElements(getXPath(), mngr).toArray();

				for (XElement target : targets)
				{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
					XElement[] newElements = (from e in this.getContent().Elements() select new XElement(e)).toArray();

					switch (this.getMode())
					{
						case AddAfterSelf:
							target.AddAfterSelf(newElements);
							break;
						case AddBeforeSelf:
							target.AddBeforeSelf(newElements);
							break;
						case AddFirst:
							target.AddFirst(newElements);
							break;
						case Add:
							target.Add(newElements);
							break;
						case ReplaceWith:
							target.ReplaceWith(newElements);
							break;
						case ReplaceAll:
							target.ReplaceAll(newElements);
							break;
					}
				}

				XmlWriter writer = XmlWriter.Create(file, this.getXmlWriterSettings());
				try
				{
					doc.WriteTo(writer);
				}
				finally
				{
					writer.Close();
				}

			}
		}
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to write.")]
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
	//[TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="A XPath expression indicating the target elements to write.")]
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
	//[TaskMember("content", Flags = TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, ParseInline=true, Description="The XML content to write.")]
	private XElement privateContent;
	public final XElement getContent()
	{
		return privateContent;
	}
	public final void setContent(XElement value)
	{
		privateContent = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("mode", Description="A value indicating how the conent write to target elements. Default is Add for adding new a child element in the end of target elements.")]
	private XPathWriteElementMode privateMode = XPathWriteElementMode.forValue(0);
	private XPathWriteElementMode getMode()
	{
		return privateMode;
	}
	private void setMode(XPathWriteElementMode value)
	{
		privateMode = value;
	}
}