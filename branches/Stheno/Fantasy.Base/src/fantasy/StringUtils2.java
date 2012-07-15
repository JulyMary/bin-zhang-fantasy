package fantasy;

import org.apache.commons.lang3.StringUtils;

public class StringUtils2 {
    private StringUtils2()
    {
    	
    }
    
    public static boolean isNullOrEmpty(String s)
    {
    	return s == null || s.isEmpty();
    }
    
    public static boolean isNullOrWhitespace(String s)
    {
    	return s == null || StringUtils.isBlank(s);
    }
}
