package Fantasy.Jobs;

import Fantasy.Jobs.Expressions.*;
import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public interface IConditionService
{
	boolean Evaluate(IConditionalObject obj);

	boolean Evaluate(String expression);
}