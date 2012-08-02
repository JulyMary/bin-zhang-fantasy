package fantasy.jobs;

import java.util.Arrays;

import fantasy.xserialization.*;
import fantasy.jobs.properties.*;
import fantasy.*;
import fantasy.servicemodel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
@Instruction
@XSerializable(name = "target", namespaceUri = Consts.XNamespaceURI)
public class Target extends Sequence implements IConditionalObject
{
	private boolean _executing = false;

	@Override
	public void Execute() throws Exception
	{

		if (!HasExecuted() && !_executing)
		{
			_executing = true;

			IJob job = this.getSite().getRequiredService(IJob.class);

			ILogger logger = this.getSite().getService(ILogger.class);
			boolean hasError = false;
			try
			{
				try
				{

					if (logger != null)
					{
						logger.LogMessage(LogCategories.getInstruction(), MessageImportance.Low, Resources.getExecuteTargetMessage(), this.Name);
					}

					if (!job.getRuntimeStatus().getLocal().GetValue("target.executingFinally", false))
					{
						if (job.getRuntimeStatus().getLocal().GetValue("target.executingOnFail", false))
						{
							throw new JobException("Resuming onFail operation.");
						}

						IConditionService conditionsvc = (IConditionService)this.getSite().getRequiredService(IConditionService.class);
						if (conditionsvc.Evaluate(this))
						{
							ExecuteDependsOnTargets();
							super.ExecuteSequence();

						}
					}

				}
				catch (InterruptedException e)
				{
                    throw e;
				}
				catch (Exception error)
				{
					Log.SafeLogError(logger, LogCategories.getInstruction(), error, Resources.getTaskExecuteErrorMessage(), this.Name);
					hasError = true;

				}
				
				if (hasError)
				{
					
					try
					{
						if(Thread.interrupted())
						{
							throw new InterruptedException();
						}
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", true);
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", OnError());

					}
					catch (InterruptedException e2)
					{
                        throw e2;
					}
					catch (Exception error)
					{
						Log.SafeLogError(logger, LogCategories.getInstruction(), error, Resources.getExecuteOnFailedErrorMessage(), this.Name);
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", true);
					}
				}


				OnFinal();


				if (job.getRuntimeStatus().getLocal().GetValue("target.rethrow", false))
				{
					throw new JobException(String.format(Resources.getTaskExecuteErrorMessage(), this.Name));
				}
			}
			finally
			{

				_executing = false;
			}

		}
	}


	private void OnFinal() throws Exception
	{
		try
		{
			IJob job = this.getSite().getRequiredService(IJob.class);
			if (!StringUtils2.isNullOrEmpty(this.Finally))
			{
				job.getRuntimeStatus().getLocal().setItem("target.executingFinally", true);
				job.ExecuteTarget(this.Finally);
			}
		}
		catch (InterruptedException e)
		{
            throw e;
		}
		catch (RuntimeException e2)
		{
			this.SetAsExecuted();
			throw e2;
		}
		this.SetAsExecuted();

	}

	private boolean OnError() throws Exception
	{
		IJob job = this.getSite().getRequiredService(IJob.class);
		ILogger logger = this.getSite().getService(ILogger.class);

		boolean rs = false;
		switch (this._failAction)
		{
			case Terminate:

				if (logger != null)
				{
					logger.LogError(LogCategories.getInstruction(), Resources.getTargetTermianteText(), this.Name);
				}
				break;
			case Continue:
				if (logger != null)
				{
					logger.LogError(LogCategories.getInstruction(), Resources.getTargetContinueText(), this.Name);
				}
				break;
		default:
			break;
		}


		if (!StringUtils2.isNullOrEmpty(this.OnFail))
		{
			job.getRuntimeStatus().getLocal().setItem("target.executingOnFail", true);
			job.ExecuteTarget(this.OnFail);
		}

		switch (this._failAction)
		{
			case Throw:
				rs = true;
				break;
			case Terminate:
				IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
				engine.Fail();
				break;
		default:
			break;
		}
		return rs;
	}

	private void ExecuteDependsOnTargets() throws Exception
	{
		IStringParser parser =this.getSite().getRequiredService(IStringParser.class);
		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);
		String[] targets;
		if (!StringUtils2.isNullOrEmpty(this.DependsOnTargets))
		{
			String s = parser.Parse(this.DependsOnTargets);
			if (!StringUtils2.isNullOrWhiteSpace(s))
			{
				targets = parser.Parse(this.DependsOnTargets).split(";", -1);
				int index = (int)job.getRuntimeStatus().getLocal().GetValue("target.dependsOnTarget.index", 0);
				while (index < targets.length)
				{
					if(Thread.interrupted())
					{
						throw new InterruptedException();
					}
					
					String name = targets[index];
					if (!StringUtils2.isNullOrWhiteSpace(name))
					{
						job.ExecuteTarget(targets[index]);
					}
					index++;
					job.getRuntimeStatus().getLocal().setItem("target.dependsOnTarget.index", index);
				}
			}
		}
	}

	private void SetAsExecuted() throws Exception
	{
		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);
		String[] oldValue = (String[])job.getRuntimeStatus().getGlobal().GetValue(ExecutedTargetsVarName, new String[0]);
		String[] newValue = new String[oldValue.length + 1];
		System.arraycopy(oldValue, 0, newValue, 0, oldValue.length);
		newValue[newValue.length - 1] = this.Name;
		job.getRuntimeStatus().getGlobal().setItem(ExecutedTargetsVarName, newValue);
	}

	private static final String ExecutedTargetsVarName = "targets.executedTargets";

	private boolean HasExecuted() throws Exception
	{
		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);
		String[] executed = (String[])job.getRuntimeStatus().getGlobal().getItem(ExecutedTargetsVarName);

		return executed != null ? Arrays.asList(executed).indexOf(this.Name) >= 0 : false;
	}

	@XAttribute(name = "dependsOnTargets", order = 10)
	public String DependsOnTargets = null;

	@XAttribute(name = "name", order = 0)
	public String Name = null;

	public static final String NameAttributeName = "name";

	@XAttribute(name = "condition")
	private String _condition = null;
	public String getCondition()
	{
		return this._condition;
	}
	public void setCondition(String value)
	{
		this._condition = value;
	}

	@XAttribute(name = "onFail", order = 30)
	public String OnFail = null;

	@XAttribute(name = "failAction", order = 40)
	private FailActions _failAction = FailActions.Throw;

	@XAttribute(name = "finally", order = 50)
	public String Finally = null;

}