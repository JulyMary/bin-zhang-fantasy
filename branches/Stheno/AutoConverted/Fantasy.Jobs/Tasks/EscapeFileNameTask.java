package Fantasy.Jobs.Tasks;

import Fantasy.IO.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("escapeFileName", Consts.XNamespaceURI, Description="Replace invalid charactors of file name with underline(_)")]
public class EscapeFileNameTask extends ObjectWithSite implements ITask
{
	public EscapeFileNameTask()
	{
		this.setReplacement("_");
	}

	private Regex regex = new Regex("[\\\\\\/\\:\\*\\?\\\"\\<\\>\\|]");

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (this.getSource() != null)
		{
			this.setResult(LongPath.EscapeFileName(getSource(), this.getReplacement()).trim());
		}
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("source", Flags= TaskMemberFlags.Input | TaskMemberFlags.Required, Description="File name to escape")]
	private String privateSource;
	public final String getSource()
	{
		return privateSource;
	}
	public final void setSource(String value)
	{
		privateSource = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("result", Flags=TaskMemberFlags.Output, Description="Escaped file name")]
	private String privateResult;
	public final String getResult()
	{
		return privateResult;
	}
	public final void setResult(String value)
	{
		privateResult = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("replacement", Description="The string to replace all invalid characters in source string.")]
	private String privateReplacement;
	public final String getReplacement()
	{
		return privateReplacement;
	}
	public final void setReplacement(String value)
	{
		privateReplacement = value;
	}
}