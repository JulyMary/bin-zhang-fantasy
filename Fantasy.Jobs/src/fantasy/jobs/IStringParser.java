package fantasy.jobs;


public interface IStringParser
{
	String Parse(String value, java.util.Map<String, Object> context);
	String Parse(String value);

}