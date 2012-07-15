package Fantasy.Jobs;

import Fantasy.Jobs.Expressions.*;
import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;

public class ConditionService extends AbstractService implements IConditionService
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IConditionService Members
	public final boolean Evaluate(String condition)
	{
		if (String.IsNullOrWhiteSpace(condition))
		{
			throw new IllegalArgumentException(condition);
		}

		IStringParser parser = this.Site.<IStringParser>GetRequiredService();
		String parsed = condition;
		ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
		java.util.HashMap<String, Object> ctx = new java.util.HashMap<String, Object>();
		ctx.put("c#-style-string", true);
		parsed = parser.Parse(parsed, ctx);

		if (!String.IsNullOrWhiteSpace(parsed))
		{
			Expression expr = new Expression(parsed);

			if (expr.getSuccess())
			{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
				expr.InvokeFunction += new EventHandler<InvokeFunctionEventArgs>(ExpressionInvokeFunction);

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
					throw new InvalidConditionException(String.format(Properties.Resources.getConditionNotBoolText(), condition, parsed));
				}
			}
			else
			{
				throw new InvalidConditionException(String.format(Properties.Resources.getParseConditionFailedText(), condition, parsed));
			}


		}
		else
		{
			return true;
		}
	}



	public final boolean Evaluate(IConditionalObject obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}

		if (!DotNetToJavaStringHelper.isNullOrEmpty(obj.getCondition()))
		{
			return this.Evaluate(obj.getCondition());
		}
		else
		{
			return true;
		}

	}

	private void ExpressionInvokeFunction(Object sender, InvokeFunctionEventArgs e)
	{
		BindingFlags flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.IgnoreCase;
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from method in e.getType().getMethods(flags) where StringComparer.OrdinalIgnoreCase.Compare(method.getName(), e.getFunctionName()) == 0 select method;
		if (query.Any())
		{
			try
			{
				Object o = Activator.CreateInstance(e.getType());
				if (o instanceof IObjectWithSite)
				{
					((IObjectWithSite)o).Site = this.Site;
				}
				e.setResult(e.getType().InvokeMember(e.getFunctionName(), flags, null, o, e.getArguments()));
				e.setHandled(true);
			}
			catch (NoSuchMethodError e)
			{
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}