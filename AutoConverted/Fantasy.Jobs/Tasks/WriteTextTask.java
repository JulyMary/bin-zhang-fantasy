package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("writeText", Consts.XNamespaceURI, Description = "Write text to file")]
public class WriteTextTask extends ObjectWithSite implements ITask
{
	public WriteTextTask()
	{
		this.setAppend(true);

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (!String.IsNullOrWhiteSpace(this.getPath()) && this.getValue() != null)
		{
			System.Text.Encoding encoding = System.Text.Encoding.Default;
			if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getEncoding()))
			{
				encoding = System.Text.Encoding.GetEncoding(this.getEncoding());
			}
			if (this.getAppend())
			{
				LongPathFile.AppendAllText(this.getPath(), this.getValue(), encoding);
			}
			else
			{
				LongPathFile.WriteAllText(this.getPath(), this.getValue(), encoding);
			}
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("path", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The file to write to.")]
	private String privatePath;
	public final String getPath()
	{
		return privatePath;
	}
	public final void setPath(String value)
	{
		privatePath = value;
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("append", Description="true if append text to the end of file if the source file exists; otherwise the write opration overwrite existing file.")]
	 boolean getAppend()
	 void setAppend(boolean value)


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("text", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The content to write to.")]
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("encoding", Flags=TaskMemberFlags.Input, Description="The encoding to apply to the string. ")]
	private String privateEncoding;
	public final String getEncoding()
	{
		return privateEncoding;
	}
	public final void setEncoding(String value)
	{
		privateEncoding = value;
	}
}