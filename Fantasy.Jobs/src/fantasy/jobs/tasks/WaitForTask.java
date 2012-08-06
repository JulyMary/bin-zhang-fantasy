package fantasy.jobs.tasks;

import java.util.*;

import fantasy.collections.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;
import fantasy.jobs.resources.*;


@Task(name = "waitFor", namespaceUri = Consts.XNamespaceURI, description="Suspend current job until specified job terminated")
public class WaitForTask extends ObjectWithSite implements ITask
{
	
	public final void Execute() throws Exception
	{

		IResourceService ressvc = this.getSite().getService(IResourceService.class);
		ILogger logger = this.getSite().getService(ILogger.class);
		if (this.Jobs != null)
		{
			if (ressvc != null)
			{
				String jobs = StringUtils2.join(";", new Enumerable<UUID>(this.Jobs));
				TreeMap<String, String> args = new TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);
				args.put("mode", this.Mode.name());
				args.put("jobs", jobs);
				ResourceParameter res = new ResourceParameter("WaitFor",args);
				if (logger != null)
				{
					if (this.Mode == WaitForMode.All)
					{
						logger.LogMessage("WaitFor", "Wait for all of following jobs terminated %1$s", jobs);
					}
					else
					{
						logger.LogMessage("WaitFor", "Wait for any of following jobs terminated %2$s", jobs);
					}
				}

				IResourceHandle handler = ressvc.Request(new ResourceParameter[] { res });
				// if request succeed, release resource immediately, otherwise, job engine will block executing.
				handler.dispose();

			}
			else
			{
				if (logger != null)
				{
					logger.LogMessage("WaitFor", "IResourceService is not available on this system, this task is skipped.");
				}
			}
		}

	

	}

    @TaskMember(name = "jobs", flags= {TaskMemberFlags.Input, TaskMemberFlags.Output, TaskMemberFlags.Required}, description="The list of Job Id to wait.")
	public UUID[] Jobs;
	
	 @TaskMember(name = "mode", description="Mode of WaitFor task. All for wait until all job to termiated in jobs; Any for wait until any of job termianted in jobs.")
	 public WaitForMode Mode = WaitForMode.All;
	
}