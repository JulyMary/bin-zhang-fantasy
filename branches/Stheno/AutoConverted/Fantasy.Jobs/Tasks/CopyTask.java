﻿package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;
import Fantasy.IO.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("copy", Consts.XNamespaceURI, Description = "Copy files")]
public class CopyTask extends ObjectWithSite implements ITask
{
	public CopyTask()
	{
		this.setTargetDirectory("");
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
		IJobEngine engine = (IJobEngine)this.Site.GetService(IJobEngine.class);
		IJob job = this.Site.<IJob>GetRequiredService();

		String targetDir = Fantasy.IO.LongPath.Combine(engine.getJobDirectory(), this.getTargetDirectory());

		if (this.getSource() != null && this.getSource().length > 0)
		{
			logger.LogMessage("copy", MessageImportance.Low, "{0} files need to be copied.", this.getSource().length);

			IStatusBarService statusBar = this.Site.<IStatusBarService>GetService();
			IProgressMonitor progress = this.Site.<IProgressMonitor>GetService();
			SequenceProgressMonitor seqProgress = null;
			if (progress != null)
			{
				seqProgress = new SequenceProgressMonitor(progress, this.getSource().length);
			}

			java.util.ArrayList<TaskItem> items = new java.util.ArrayList<TaskItem>();
			if (this.getTarget() != null)
			{
				items.addAll(this.getTarget());
			}
			for(int i = 0; i < this.getSource().length; i ++)
			{
				TaskItem item = this.getSource()[i];
				String source = item.getItem("FullName");
				String dest;
				if (i < items.size())
				{
					dest = items.get(i).getItem("fullname");
				}
				else
				{
					if (!LongPathDirectory.Exists(targetDir))
					{
						logger.LogMessage("copy", MessageImportance.Low, "Create directory {0}.", targetDir);
						LongPathDirectory.Create(targetDir);
					}

					dest = Fantasy.IO.LongPath.Combine(targetDir, Path.GetFileName(source));
					String destName = Fantasy.IO.LongPath.GetRelativePath(engine.getJobDirectory() + "\\", dest);
					TaskItem tempVar = new TaskItem();
					tempVar.setName(destName);
					TaskItem destItem = tempVar;
					items.add(destItem);
				}

				if (job.getRuntimeStatus().getLocal().GetValue("copied.index", -1) < i)
				{
					if (logger != null)
					{
						logger.LogMessage("copy", MessageImportance.Low, "Copying file from {0} to {1}.", source, dest);
					}
					if (statusBar != null)
					{
						statusBar.SetStatus(String.format("Copying file from %1$s to %2$s.", source, dest));
					}

					IProgressMonitor subProgress = seqProgress != null ? seqProgress[i] : null;

					LongPathFile.Copy(source, dest, true, subProgress);
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

			this.setTarget(items.toArray(new TaskItem[]{}));
		}
		else
		{
			logger.LogMessage("copy", MessageImportance.Low, "No file need to be copy.");
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("source", Description="The files to copy")]
	private TaskItem[] privateSource;
	public final TaskItem[] getSource()
	{
		return privateSource;
	}
	public final void setSource(TaskItem[] value)
	{
		privateSource = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("target", Flags = TaskMemberFlags.Input | TaskMemberFlags.Output, Description="The list of files to copy the source files to.")]
	private TaskItem[] privateTarget;
	public final TaskItem[] getTarget()
	{
		return privateTarget;
	}
	public final void setTarget(TaskItem[] value)
	{
		privateTarget = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("targetDirectory", Flags= TaskMemberFlags.Input, Description="The target directory to copy the source files to.")]
	private String privateTargetDirectory;
	public final String getTargetDirectory()
	{
		return privateTargetDirectory;
	}
	public final void setTargetDirectory(String value)
	{
		privateTargetDirectory = value;
	}
}