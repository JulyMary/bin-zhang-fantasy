package fantasy.jobs.tasks;

import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;
import fantasy.io.*;

@Task(name = "changeExtension", namespaceUri= Consts.XNamespaceURI, description="Change file extension for input files. Please note this task does not rename files. It is only for creating items with different extensios.")
public class ChangeExtensionTask extends ObjectWithSite implements ITask
{
	@TaskMember(name = "include", flags= {TaskMemberFlags.Input, TaskMemberFlags.Output,  TaskMemberFlags.Required}, description="The items to modify")
	public TaskItem[] Include;
	

	@TaskMember(name = "extension", flags={TaskMemberFlags.Input , TaskMemberFlags.Required}, description="The new extension (with or without a leading period). Specity empty to remove an existing extension from path")
	public String Extension;
	

	@TaskMember(name = "preserveExistingMetadata", description="true if changeExtension task should copy items metadata to new created items; otherwise, false")
	public boolean PreserveExistingMetadata = false;
	


	public final void Execute()
	{
		if (this.Include != null)
		{
			TaskItem[] items = new TaskItem[Include.length];
			for (int i = 0; i < items.length; i++)
			{
				TaskItem tempVar = new TaskItem();
				tempVar.setName(Path.changeExtension(Include[i].getName(), this.Extension));
				items[i] = tempVar;
				if (this.PreserveExistingMetadata)
				{
					this.Include[i].CopyMetaDataTo(items[i]);
				}
			}

			this.Include = items;
		}
	}

}