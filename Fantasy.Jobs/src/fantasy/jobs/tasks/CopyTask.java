package fantasy.jobs.tasks;

import java.util.Arrays;

import fantasy.servicemodel.*;
import fantasy.io.*;

import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "copy", namespaceUri = Consts.XNamespaceURI, description = "Copy files")
public class CopyTask extends ObjectWithSite implements ITask
{
	public CopyTask()
	{
		this.TargetDirectory = "";
	}


	public final void Execute() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		IJob job = this.getSite().getRequiredService(IJob.class);

		String targetDir = Path.combine(engine.getJobDirectory(), this.TargetDirectory);

		if (this.Source != null && this.Source.length > 0)
		{
			logger.LogMessage("copy", MessageImportance.Low, "%1$d files need to be copied.", this.Source.length);

			IStatusBarService statusBar = this.getSite().getService(IStatusBarService.class);
			IProgressMonitor progress = this.getSite().getService(IProgressMonitor.class);
			SequenceProgressMonitor seqProgress = null;
			if (progress != null)
			{
				seqProgress = new SequenceProgressMonitor(progress, this.Source.length);
			}

			java.util.ArrayList<TaskItem> items = new java.util.ArrayList<TaskItem>();
			if (this.Target != null)
			{
				items.addAll(Arrays.asList(this.Target));
			}
			for(int i = 0; i < this.Source.length; i ++)
			{
				TaskItem item = this.Source[i];
				String source = item.getName();
				String dest;
				if (i < items.size())
				{
					dest = items.get(i).getName();
				}
				else
				{
					if (!Directory.exists(targetDir))
					{
						logger.LogMessage("copy", MessageImportance.Low, "Create directory {0}.", targetDir);
						Directory.create(targetDir);
					}

					dest = Path.combine(targetDir, Path.getFileName(source));
					
					TaskItem tempVar = new TaskItem();
					tempVar.setName(dest);
					TaskItem destItem = tempVar;
					items.add(destItem);
				}

				if (job.getRuntimeStatus().getLocal().GetValue("copied.index", -1) < i)
				{
					if (logger != null)
					{
						logger.LogMessage("copy", MessageImportance.Low, "Copying file from %1$s to %2$s.", source, dest);
					}
					if (statusBar != null)
					{
						statusBar.setStatus(String.format("Copying file from %1$s to %2$s.", source, dest));
					}

					IProgressMonitor subProgress = seqProgress != null ? seqProgress.getItem(i): null;

					File.copy(source, dest, subProgress);
					job.getRuntimeStatus().getLocal().setItem("copied.index", i);
				}
				else
				{
					if (logger != null)
					{
						logger.LogMessage("copy", MessageImportance.Low, "Copying file from {0} to {1} is skipped.", source, dest);
					}
				}



			}

			this.Target = items.toArray(new TaskItem[]{});
		}
		else
		{
			logger.LogMessage("copy", MessageImportance.Low, "No file need to be copy.");
		}

		
	}


	@TaskMember(name = "source", description="The files to copy")
	public TaskItem[] Source;
	

	@TaskMember(name = "target", flags = {TaskMemberFlags.Input , TaskMemberFlags.Output }, description="The list of files to copy the source files to.")
	public TaskItem[] Target;
	

	@TaskMember(name = "targetDirectory", flags= TaskMemberFlags.Input, description="The target directory to copy the source files to.")
	public String TargetDirectory;
	
}