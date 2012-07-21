package fantasy.jobs;

import java.util.Properties;
import java.util.regex.*;

public class EnvironmentVariablesReader implements ITagValueProvider
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	@Override
	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		
		String rs = "${" + tag + "}"; 
		Properties properties = System.getProperties(); 
		String pattern = "\\$\\{([A-Za-z0-9]+)\\}"; 
		Pattern expr = Pattern.compile(pattern); 
		Matcher matcher = expr.matcher(rs); 
		while (matcher.find()) { 
		    Object envValue = properties.get(matcher.group(1).toUpperCase()); 
		    if (envValue == null) { 
		        envValue = ""; 
		    } else { 
		        envValue = envValue.toString().replace("\\", "\\\\"); 
		    } 
		    Pattern subexpr = Pattern.compile(Pattern.quote(matcher.group(0))); 
		    rs = subexpr.matcher(rs).replaceAll((String)envValue); 
		} 

		
		return rs;
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}