package fantasy.jobs;

import fantasy.*;

public class EnvironmentVariablesReader implements ITagValueProvider
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	@Override
	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		
		String rs = "${" + tag + "}"; 
		return SystemUtils.expandEnvironmentVariables(rs);
	}

	@Override
	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		
		return System.getProperties().containsKey(tag.toUpperCase());
		
	}

	public final char getPrefix()
	{
		return '$';
	}



	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return true;
	}
}