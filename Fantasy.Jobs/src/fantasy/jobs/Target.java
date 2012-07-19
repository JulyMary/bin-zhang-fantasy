package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.jobs.Properties.*;
import fantasy.servicemodel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("target", NamespaceUri = Consts.XNamespaceURI)]
public class Target extends Sequence implements IConditionalObject
{
	private boolean _executing = false;

	@Override
	public void Execute()
	{

		if (!HasExecuted() && !_executing)
		{
			_executing = true;

			IJob job = (IJob)this.Site.GetService(IJob.class);

			ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
			boolean hasError = false;
			try
			{
				try
				{

					if (logger != null)
					{
						logger.LogMessage(LogCategories.getInstruction(), MessageImportance.Low, "Execute target {0}.", this.getName());
					}

					if (!job.getRuntimeStatus().getLocal().GetValue("target.executingFinally", false))
					{
						if (job.getRuntimeStatus().getLocal().GetValue("target.executingOnFail", false))
						{
							throw new JobException("Resuming onFail operation.");
						}

						IConditionService conditionsvc = (IConditionService)this.Site.GetService(IConditionService.class);
						if (conditionsvc.Evaluate(this))
						{
							ExecuteDependsOnTargets();
							super.ExecuteSequence();

						}
					}

				}
				catch (ThreadAbortException e)
				{

				}
				catch (RuntimeException error)
				{
					logger.SafeLogError(LogCategories.getInstruction(), error, "An error occurs when execute target {0}.", this.getName());
					hasError = true;

				}
				//bool rethrow = false;
				if (hasError)
				{
					try
					{
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", true);
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", OnError());

					}
					catch (ThreadAbortException e2)
					{

					}
					catch (RuntimeException error)
					{
						logger.SafeLogError(LogCategories.getInstruction(), error, "An error occurs when execute onFailed of target {0}.", this.getName());
						job.getRuntimeStatus().getLocal().setItem("target.rethrow", true);
					}
				}


				OnFinal();


				if (job.getRuntimeStatus().getLocal().GetValue("target.rethrow", false))
				{
					throw new JobException(String.format("An error occurs when execute target %1$s.", this.getName()));
				}
			}
			finally
			{

				_executing = false;
			}

		}
	}


	private void OnFinal()
	{
		try
		{
			IJob job = this.getSite().<IJob>GetRequiredService();
			if (!DotNetToJavaStringHelper.isNullOrEmpty(this.Finally))
			{
				job.getRuntimeStatus().getLocal().setItem("target.executingFinally", true);
				job.ExecuteTarget(this.Finally);
			}
		}
		catch (ThreadAbortException e)
		{

		}
		catch (RuntimeException e2)
		{
			this.SetAsExecuted();
			throw e2;
		}
		this.SetAsExecuted();

	}

	private boolean OnError()
	{
		IJob job = this.getSite().<IJob>GetRequiredService();
		ILogger logger = this.getSite().<ILogger>GetService();

		boolean rs = false;
		switch (this._failAction)
		{
			case Terminate:

				if (logger != null)
				{
					logger.LogError(LogCategories.getInstruction(), Properties.Resources.getTargetTermianteText(), this.getName());
				}
				break;
			case Continue:
				if (logger != null)
				{
					logger.LogError(LogCategories.getInstruction(), Properties.Resources.getTargetContinueText(), this.getName());
				}
				break;
		}


		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.OnFail))
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
				IJobEngine engine = this.getSite().<IJobEngine>GetRequiredService();
				engine.Fail();
				break;
		}
		return rs;
	}

	private void ExecuteDependsOnTargets()
	{
		IStringParser parser = (IStringParser)this.Site.GetService(IStringParser.class);
		IJob job = (IJob)this.Site.GetService(IJob.class);
		String[] targets;
		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.DependsOnTargets))
		{
			String s = parser.Parse(this.DependsOnTargets);
			if (!String.IsNullOrWhiteSpace(s))
			{
				targets = parser.Parse(this.DependsOnTargets).split("[;]", -1);
				int index = (int)job.getRuntimeStatus().getLocal().GetValue("target.dependsOnTarget.index", 0);
				while (index < targets.length)
				{
					String name = targets[index];
					if (!String.IsNullOrWhiteSpace(name))
					{
						job.ExecuteTarget(targets[index]);
					}
					index++;
					job.getRuntimeStatus().getLocal().setItem("target.dependsOnTarget.index", index);
				}
			}
		}
	}

	private void SetAsExecuted()
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		String[] oldValue = (String[])job.getRuntimeStatus().getGlobal().GetValue(ExecutedTargetsVarName, new String[0]);
		String[] newValue = new String[oldValue.length + 1];
		System.arraycopy(oldValue, 0, newValue, 0, oldValue.length);
		newValue[newValue.length - 1] = this.getName();
		job.getRuntimeStatus().getGlobal().setItem(ExecutedTargetsVarName, newValue);
	}

	private static final String ExecutedTargetsVarName = "targets.executedTargets";

	private boolean HasExecuted()
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		String[] executed = (String[])job.getRuntimeStatus().getGlobal().getItem(ExecutedTargetsVarName);

		return executed != null ? Array.indexOf(executed, this.getName()) >= 0 : false;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("dependsOnTargets", Order = 10)]
	public String DependsOnTargets = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute(NameAttributeName, Order = 0)]
	public String Name = null;

	public static final String NameAttributeName = "name";

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String _condition = null;
	private String getCondition()
	{
		return this._condition;
	}
	private void setCondition(String value)
	{
		this._condition = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("onFail", Order = 30)]
	public String OnFail = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("failAction", Order = 40)]
	private FailActions _failAction = FailActions.Throw;




//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("finally", Order = 50)]
	public String Finally = null;

}