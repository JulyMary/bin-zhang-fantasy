package Fantasy.Jobs.Tasks;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("callTarget", Consts.XNamespaceURI, Description = "Invoke specified target immediately")]
public class CallTargetTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		IJob job = this.Site.<IJob>GetRequiredService();
		if(this.getTargets() != null)
		{
			for(String target : this.getTargets())
			{
				if(!DotNetToJavaStringHelper.isNullOrEmpty(target))
				{
					job.ExecuteTarget(target);
				}
			}
		}

		return true;

	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("target", Description = "The target or targets to execute")]
	private String[] privateTargets;
	public final String[] getTargets()
	{
		return privateTargets;
	}
	public final void setTargets(String[] value)
	{
		privateTargets = value;
	}
}