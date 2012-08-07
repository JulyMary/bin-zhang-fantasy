package fantasy;

import java.util.Properties;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public final class SystemUtils {
    
	
	private SystemUtils()
	{
		
	}
	
	public static String expandEnvironmentVariables(String name)
	{
		
		StringBuilder rs = new StringBuilder(name);
		Properties properties = System.getProperties(); 
		String pattern = "\\$\\{([A-Za-z0-9]+)\\}"; 
		Pattern expr = Pattern.compile(pattern); 
		Matcher matcher = expr.matcher(rs); 
		while (matcher.find()) { 
		    Object envValue = properties.get(matcher.group(1).toUpperCase()); 
		    if (envValue == null) { 
		        envValue = ""; 
		     
		    } 
		    rs.delete(matcher.start(), matcher.end());
		    rs.insert(matcher.start(), envValue);
		    matcher = expr.matcher(rs);
		    
		} 
		
		return rs.toString();
	}
}
