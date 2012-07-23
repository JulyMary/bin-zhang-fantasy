package fantasy.jobs;


public interface IStringParser
{
	String Parse(String value, java.util.Map<String, Object> context) throws Exception;
	String Parse(String value) throws Exception;
	
	
}