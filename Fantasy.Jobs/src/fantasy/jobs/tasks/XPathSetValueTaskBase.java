package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

public abstract class XPathSetValueTaskBase extends XmlTaskBase
{

	protected abstract void SetValue(XElement element, XmlNamespaceManager nsMgr);

	@Override
	public boolean Execute()
	{
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		if (getInclude() != null && getInclude().length > 0)
		{
			for (TaskItem item : getInclude())
			{


				String file = item.getItem("fullname");
				XDocument doc = LongPathXNode.LoadXDocument(file);
				XmlNamespaceManager mngr = this.getSite().<XmlNamespaceManager>GetRequiredService();

				Iterable<XElement> targets = (Iterable<XElement>)doc.XPathSelectElements(getXPath(), mngr);

				for (XElement target : targets)
				{
					SetValue(target, mngr);
				}

				FileStream fs = LongPathFile.Open(file, FileMode.Create, FileAccess.ReadWrite);
				XmlWriter writer = XmlWriter.Create(fs, this.getFormat());
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

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to set values")]
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
	//[TaskMember("xpath", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The XPath expression indicating the target elements in incude XML files.")]
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
	//[TaskMember("value", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The value to set to.")]
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}
}