package Fantasy.Jobs.Tasks;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("createItem", Consts.XNamespaceURI, Description="Create job service items. This task is deprecated. Please use 'items' element in instructions to create new items")]
public class CreateItemTask extends ObjectWithSite implements ITask
{
	public CreateItemTask()
	{
		this.setPreserveExistingMetadata(true);

	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		java.util.ArrayList<TaskItem> items = new java.util.ArrayList<TaskItem>();
		java.util.ArrayList<String[]> additional = new java.util.ArrayList<String[]>();
		if (this.getAdditionalMetadata() != null)
		{
			for (String expr : this.getAdditionalMetadata())
			{
				String[] kv = expr.split(new char[] {'='}, 2);
				if (kv.length == 1)
				{
					kv = new String[] { kv[0], "" };
				}
				additional.add(kv);
			}
		}

		for (TaskItem src : this.getInclude())
		{
			if (!IsExclude(src.getName()))
			{
				TaskItem tempVar = new TaskItem();
				tempVar.setName(src.getName());
				TaskItem newItem = tempVar;
				if (this.getPreserveExistingMetadata())
				{
					src.CopyMetaDataTo(newItem);
				}

				for (String[] kv : additional)
				{
					src.setItem(kv[0], src.getItem(kv[1]));
				}

			}
		}

		this.setInclude(items.toArray(new TaskItem[]{}));

		return true;
	}


	private boolean IsExclude(String name)
	{
		if (getExclude() != null)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			var query = from item in getExclude() where StringComparer.OrdinalIgnoreCase.Compare(item.getName(), name) == 0 select item;
			return query.Any();
		}
		return false;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required | TaskMemberFlags.Output)]
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
	//[TaskMember("exclude")]
	private TaskItem[] privateExclude;
	public final TaskItem[] getExclude()
	{
		return privateExclude;
	}
	public final void setExclude(TaskItem[] value)
	{
		privateExclude = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("preserveExistingMetadata")]
	private boolean privatePreserveExistingMetadata;
	public final boolean getPreserveExistingMetadata()
	{
		return privatePreserveExistingMetadata;
	}
	public final void setPreserveExistingMetadata(boolean value)
	{
		privatePreserveExistingMetadata = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("additionalMetadata")]
	private String[] privateAdditionalMetadata;
	public final String[] getAdditionalMetadata()
	{
		return privateAdditionalMetadata;
	}
	public final void setAdditionalMetadata(String[] value)
	{
		privateAdditionalMetadata = value;
	}
}