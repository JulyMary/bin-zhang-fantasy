package fantasy.jobs.tasks;

import fantasy.io.*;
import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "delete", namespaceUri = Consts.XNamespaceURI, description="Delete files")
public class DeleteTask extends ObjectWithSite implements ITask
{
	@TaskMember(name="source", flags={TaskMemberFlags.Input, TaskMemberFlags.Required},  description="The list of file to delete.")
	private TaskItem[] Source;
	


	public final void Execute() throws Exception
	{
		if (this.Source != null)
		{
			ILogger logger = (ILogger)this.getSite().getService(ILogger.class);
			if (logger != null)
			{
				logger.LogMessage("delete", MessageImportance.Low, "{0} files need to be deleted.", this.Source.length);
			}

			IStatusBarService statusBar = this.getSite().getService(IStatusBarService.class);
			IProgressMonitor progress = this.getSite().getService(IProgressMonitor.class);

			if (progress != null)
			{
				progress.setValue(0);
				progress.setMinimum(0);
				progress.setMaximum(this.Source.length);

			}

			for (TaskItem item : this.Source)
			{


				String file = item.getItem("fullname");
				if (File.exists(file))
				{
					Log.SafeLogMessage(logger, "Delete", "Delete file %1$s", item.getName());
					StatusBar.safeSetStatus(statusBar, "Delete", "Delete file %1$s", item.getName());
					
					File.delete(file);
				}
				else
				{
					Log.SafeLogWarning(logger, "Delete", "File {0} does not exist", item.getName());
					
				}
                if(progress != null)
                {
				   progress.setValue(progress.getValue()+1);
                }
			}
		}
	}

}