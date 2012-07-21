package fantasy.jobs;



public interface IConditionService
{
	boolean Evaluate(IConditionalObject obj) throws Exception;

	boolean Evaluate(String expression) throws Exception;
}