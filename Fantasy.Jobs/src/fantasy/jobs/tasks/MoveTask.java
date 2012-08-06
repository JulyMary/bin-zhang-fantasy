package fantasy.jobs.tasks;

import java.util.Arrays;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "move", namespaceUri = Consts.XNamespaceURI, description="Move files")
public class MoveTask extends ObjectWithSite implements ITask
{
	

	public final void Execute() throws Exception
	{
		ILogger logger = (ILogger)this.getSite().getService(ILogger.class);
		IJobEngine engine = (IJobEngine)this.getSite().getRequiredService(IJobEngine.class);
		IJob job = this.getSite().getRequiredService(IJob.class);

		String targetDir = Path.combine(engine.getJobDirectory(), this.TargetDirectory);

		if (this.Source != null && this.Source.length > 0)
		{
			logger.LogMessage("move", MessageImportance.Low, "{0} files need to be moved.", this.Source.length);


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
			for (int i = 0; i < this.Source.length; i++)
			{
				TaskItem item = this.Source[i];
				String source = item.getName();
				String dest;
				if (i < items.size())
				{
					dest = items.get(i).getName();
					String dir = Path.getDirectoryName(dest);
					if (!Directory.exists(dir))
					{
						Log.SafeLogMessage(logger, "move", MessageImportance.Low, "Create directory %1$s.", dir);
						Directory.create(dir);
					}
				}
				else
				{
					if (!Directory.exists(targetDir))
					{
						Log.SafeLogMessage(logger, "move", MessageImportance.Low, "Create directory %1$s.", targetDir);
						Directory.create(targetDir);
					}

					dest = Path.combine(targetDir, Path.getFileName(source));
					
					TaskItem tempVar = new TaskItem();
					tempVar.setName(dest);
					TaskItem destItem = tempVar;
					items.add(destItem);
				}




				if (job.getRuntimeStatus().getLocal().GetValue("moved.index", -1) < i)
				{
					if (logger != null)
					{
						logger.LogMessage("move", MessageImportance.Low, "Moving file from %1$s to %2$s.", source, dest);
					}
					if (statusBar != null)
					{
						
						statusBar.setStatus(String.format("Moving file from %1$s to %2$s.", source, dest));
					}

					IProgressMonitor subProgress = seqProgress != null ? seqProgress.getItem(i): null;

					File.move(source, dest, subProgress);
					job.getRuntimeStatus().getLocal().setItem("moved.index", i);
				}
				else
				{
					if (logger != null)
					{
						logger.LogMessage("move", MessageImportance.Low, "Moving file from %1$s to %2$s is skipped.", source, dest);
					}
				}



			}

			this.Target = items.toArray(new TaskItem[]{});
		}
		else
		{
			logger.LogMessage("move", MessageImportance.Low, "No file need to be moved.");
		}

		
	}

	@TaskMember(name = "source", description="The files to move.")
	public TaskItem[] Source;
	

	@TaskMember(name = "target", flags = {TaskMemberFlags.Input,  TaskMemberFlags.Output}, description= "The list of files to move the source files to.")
	public TaskItem[] Target;
	

	@TaskMember(name = "targetDirectory", flags = TaskMemberFlags.Input, description="The target directory to move the source files to.")
	public String TargetDirectory = "";
	
}