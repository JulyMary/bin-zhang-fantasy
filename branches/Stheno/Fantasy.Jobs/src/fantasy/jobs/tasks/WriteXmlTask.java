package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;
public class WriteXmlTask extends XmlTaskBase
{


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to write to.")]
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
	//[TaskMember("content", Flags = TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, Description="The XML content to write to.")]
	private XElement privateContent;
	public final XElement getContent()
	{
		return privateContent;
	}
	public final void setContent(XElement value)
	{
		privateContent = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	@Override
	public boolean Execute()
	{
		if (this.getInclude() != null && this.getInclude().length > 0)
		{


			XElement root = this.getContent().Elements().First();


			for (TaskItem item : this.getInclude())
			{

				String path = item.getItem("fullname");

				XmlWriter writer = XmlWriter.Create(path, this.getFormat());
				try
				{
					root.WriteTo(writer);
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
}