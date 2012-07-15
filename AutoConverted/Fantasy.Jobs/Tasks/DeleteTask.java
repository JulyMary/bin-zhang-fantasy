package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;
import Fantasy.IO.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("delete", Consts.XNamespaceURI, Description="Delete files")]
public class DeleteTask extends ObjectWithSite implements ITask
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("source", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of file to delete.")]
	private TaskItem[] privateSource;
	public final TaskItem[] getSource()
	{
		return privateSource;
	}
	public final void setSource(TaskItem[] value)
	{
		privateSource = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (getSource() != null)
		{
			ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
			if (logger != null)
			{
				logger.LogMessage("delete", MessageImportance.Low, "{0} files need to be deleted.", this.getSource().length);
			}

			IStatusBarService statusBar = this.Site.<IStatusBarService>GetService();
			IProgressMonitor progress = this.Site.<IProgressMonitor>GetService();

			if (progress != null)
			{
				progress.setValue(0);
				progress.Minimum = 0;
				progress.Maximum = this.getSource().length;

			}

			for (TaskItem item : getSource())
			{


				String file = item.getItem("fullname");
				if (LongPathFile.Exists(file))
				{
					logger.SafeLogMessage("Delete", "Delete file {0}", item.getName());
					LongPathFile.Delete(file);
				}
				else
				{
					logger.SafeLogWarning("Delete", "File {0} does not exist", item.getName());
				}

				progress.getValue()++;
			}
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}