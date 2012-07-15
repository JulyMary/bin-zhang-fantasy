package Fantasy.Jobs.Tasks;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("changeExtension", Consts.XNamespaceURI, Description="Change file extension for input files. Please note this task does not rename files. It is only for creating items with different extensios.")]
public class ChangeExtensionTask extends ObjectWithSite implements ITask
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags= TaskMemberFlags.Input | TaskMemberFlags.Output | TaskMemberFlags.Required, Description="The items to modify")]
	private TaskItem[] privateInclude;
	public final TaskItem[] getInclude()
	{
		return privateInclude;
	}
	public final void setInclude(TaskItem[] value)
	{
		privateInclude = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("extension", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The new extension (with or without a leading period). Specity empty to remove an existing extension from path")]
	private String privateExtension;
	public final String getExtension()
	{
		return privateExtension;
	}
	public final void setExtension(String value)
	{
		privateExtension = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("preserveExistingMetadata", Description="true if changeExtension task should copy items metadata to new created items; otherwise, false")]
	 boolean getPreserveExistingMetadata()
	 void setPreserveExistingMetadata(boolean value)

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (this.getInclude() != null)
		{
			TaskItem[] items = new TaskItem[getInclude().length];
			for (int i = 0; i < items.length; i++)
			{
				TaskItem tempVar = new TaskItem();
				tempVar.setName(Path.ChangeExtension(getInclude()[i].getName(), this.getExtension()));
				items[i] = tempVar;
				if (this.getPreserveExistingMetadata())
				{
					this.getInclude()[i].CopyMetaDataTo(items[i]);
				}
			}

			this.setInclude(items);
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}