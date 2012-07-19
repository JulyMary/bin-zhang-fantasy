package fantasy.jobs;

public class EnvironmentVariablesReader implements ITagValueProvider
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		return Environment.ExpandEnvironmentVariables(String.format("%%%1$s%%", tag));
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		return Environment.GetEnvironmentVariables().Contains(tag);
	}

	public final char getPrefix()
	{
		return '$';
	}



	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}