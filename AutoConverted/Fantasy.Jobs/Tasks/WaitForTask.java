package Fantasy.Jobs.Tasks;

import Fantasy.Jobs.Resources.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("waitFor", Consts.XNamespaceURI, Description="Suspend current job until specified job terminated")]
public class WaitForTask extends ObjectWithSite implements ITask
{
	public WaitForTask()
	{
		this.setMode(WaitForMode.All);
	}

	public final boolean Execute()
	{

		IResourceService ressvc = this.Site.<IResourceService>GetService();
		ILogger logger = this.Site.<ILogger>GetService();
		if (this.getJobs() != null)
		{
			if (ressvc != null)
			{
				String jobs = DotNetToJavaStringHelper.join(";", this.getJobs());
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
				ResourceParameter res = new ResourceParameter("WaitFor", new { mode = this.getMode(), jobs=jobs});
				if (logger != null)
				{
					if (this.getMode() == WaitForMode.All)
					{
						logger.LogMessage("WaitFor", "Wait for all of following jobs terminated {0}", jobs);
					}
					else
					{
						logger.LogMessage("WaitFor", "Wait for any of following jobs terminated {0}", jobs);
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

		return true;

	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("jobs", Flags=TaskMemberFlags.Input | TaskMemberFlags.Output | TaskMemberFlags.Required, Description="The list of Job Id to wait.")]
	private Guid[] privateJobs;
	public final Guid[] getJobs()
	{
		return privateJobs;
	}
	public final void setJobs(Guid[] value)
	{
		privateJobs = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("mode", Description="Mode of WaitFor task. All for wait until all job to termiated in jobs; Any for wait until any of job termianted in jobs.")]
	 WaitForMode getMode()
	 void setMode(WaitForMode value)
}