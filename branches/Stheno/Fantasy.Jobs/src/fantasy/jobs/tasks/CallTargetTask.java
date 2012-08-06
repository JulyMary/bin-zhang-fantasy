package fantasy.jobs.tasks;

import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;



@Task(name="callTarget", namespaceUri= Consts.XNamespaceURI, description = "Invoke specified target immediately")
public class CallTargetTask extends ObjectWithSite implements ITask
{

	public final void Execute() throws Exception
	{
		IJob job = this.getSite().getRequiredService(IJob.class);
		if(this.Targets != null)
		{
			for(String target : this.Targets)
			{
				if(!StringUtils2.isNullOrEmpty(target))
				{
					job.ExecuteTarget(target);
				}
			}
		}

		

	}

	@TaskMember(name="target", description = "The target or targets to execute")
	public String[] Targets;
	
}