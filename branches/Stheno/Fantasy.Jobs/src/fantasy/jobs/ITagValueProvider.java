package fantasy.jobs;



public interface ITagValueProvider
{
	char getPrefix();
	String GetTagValue(String tag, java.util.Map<String, Object> context) throws Exception;
	boolean HasTag(String tag, java.util.Map<String, Object> context)  throws Exception;
	boolean IsEnabled(java.util.Map<String, Object> context)  throws Exception;
}