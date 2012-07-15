package Fantasy.Jobs;

import Microsoft.Win32.*;

public interface ITagValueProvider
{
	char getPrefix();
	String GetTagValue(String tag, java.util.Map<String, Object> context);
	boolean HasTag(String tag, java.util.Map<String, Object> context);
	boolean IsEnabled(java.util.Map<String, Object> context);
}