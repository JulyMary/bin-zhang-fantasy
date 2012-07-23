package fantasy.jobs;

import java.rmi.RemoteException;

import fantasy.jobs.expressions.*;
import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;
import fantasy.*;
public class ConditionService extends AbstractService implements IConditionService
{

    /**
	 * 
	 */
	private static final long serialVersionUID = -8396161868800467194L;

	public ConditionService() throws RemoteException {
		super();
		
	}


	public final boolean Evaluate(String condition) throws Exception
	{
		if (StringUtils2.isNullOrWhiteSpace(condition))
		{
			throw new IllegalArgumentException("condition");
		}

		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);
		String parsed = condition;
		ILogger logger = this.getSite().getService(ILogger.class);
		java.util.HashMap<String, Object> ctx = new java.util.HashMap<String, Object>();
		ctx.put("c#-style-string", true);
		parsed = parser.Parse(parsed, ctx);

		if (!StringUtils2.isNullOrWhiteSpace(parsed))
		{
			Expression expr = new Expression(parsed);

			if (expr.getSuccess())
			{

				Object rs = expr.Eval();
				if (rs instanceof Boolean)
				{
					if (logger != null)
					{
						logger.LogMessage("condition", MessageImportance.Low, "Source: {0}, Parsed: {1}, Value: {2}", condition, parsed, rs);
					}
					return ((Boolean)rs).booleanValue();
				}
				else
				{
					throw new InvalidConditionException(String.format(Resources.getConditionNotBoolText(), condition, parsed));
				}
			}
			else
			{
				throw new InvalidConditionException(String.format(Resources.getParseConditionFailedText(), condition, parsed));
			}


		}
		else
		{
			return true;
		}
	}



	public final boolean Evaluate(IConditionalObject obj) throws Exception
	{
		if (obj == null)
		{
			throw new IllegalArgumentException("obj");
		}

		if (!StringUtils2.isNullOrEmpty(obj.getCondition()))
		{
			return this.Evaluate(obj.getCondition());
		}
		else
		{
			return true;
		}

	}

	
}