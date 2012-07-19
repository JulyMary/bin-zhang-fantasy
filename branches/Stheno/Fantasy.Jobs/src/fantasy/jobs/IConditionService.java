package fantasy.jobs;



public interface IConditionService
{
	boolean Evaluate(IConditionalObject obj);

	boolean Evaluate(String expression);
}