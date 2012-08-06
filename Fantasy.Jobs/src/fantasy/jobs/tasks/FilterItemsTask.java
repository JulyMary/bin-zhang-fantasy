package fantasy.jobs.tasks;

import java.util.*;

import fantasy.collections.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "filterItems", namespaceUri = Consts.XNamespaceURI, description="Divides exclude items from input items")
public class FilterItemsTask extends ObjectWithSite implements ITask
{
    @TaskMember(name = "input",flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items whose elements are not also in exclude will be returned")
	public TaskItem[] Input;
	

	@TaskMember(name = "exclude", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of items to remove form the source items")
	public TaskItem[] Exclude;
	

	@TaskMember(name = "result", flags = TaskMemberFlags.Output, description="The list of items that contains the set difference of the items of input and exclude items.")
	public TaskItem[] Result;


	public final void Execute()
	{
		if (this.Exclude == null)
		{
			this.Result = this.Input;
		}
		else
		{
			if (this.Input != null)
			{
				TreeSet<String> exclude = new TreeSet<String>(String.CASE_INSENSITIVE_ORDER);
				
				exclude.addAll(new Enumerable<TaskItem>(this.Exclude).select(new Selector<TaskItem, String>(){

					@Override
					public String select(TaskItem item) {
						return item.getName();
					}}).toArrayList());
				
				
			
				java.util.ArrayList<TaskItem> result = new java.util.ArrayList<TaskItem>();
				for (TaskItem item : this.Input)
				{
					if(!exclude.contains(item.getName()))
					
					{
						result.add(item);
					}
				}
				this.Result = result.toArray(new TaskItem[]{});
			}
		}
		
	}

}