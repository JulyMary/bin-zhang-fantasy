package Fantasy.Jobs.Tasks;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("filterItems",Consts.XNamespaceURI, Description="Divides exclude items from input items")]
public class FilterItemsTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("input",Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items whose elements are not also in exclude will be returned")]
	private TaskItem[] privateInput;
	public final TaskItem[] getInput()
	{
		return privateInput;
	}
	public final void setInput(TaskItem[] value)
	{
		privateInput = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("exclude", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to remove form the source items")]
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
	//[TaskMember("result", Flags = TaskMemberFlags.Output, Description="The list of items that contains the set difference of the items of input and exclude items.")]
	private TaskItem[] privateResult;
	public final TaskItem[] getResult()
	{
		return privateResult;
	}
	public final void setResult(TaskItem[] value)
	{
		privateResult = value;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (getExclude() == null)
		{
			setResult(getInput());
		}
		else
		{
			if (getInput() != null)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				var query = from item in getExclude() select item["fullname"];
				java.util.ArrayList<String> exclude = query.ToList();
				exclude.Sort(StringComparer.OrdinalIgnoreCase);
				java.util.ArrayList<TaskItem> result = new java.util.ArrayList<TaskItem>();
				for (TaskItem item : getInput())
				{
					if (exclude.BinarySearch(item.getItem("fullname"), StringComparer.OrdinalIgnoreCase) < 0)
					{
						result.add(item);
					}
				}
				setResult(result.toArray(new TaskItem[]{}));
			}
		}
		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}