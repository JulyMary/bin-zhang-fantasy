package fantasy.jobs.tasks;

import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;


@Task(name = "setMetaData", namespaceUri = Consts.XNamespaceURI, description="Set meta data for input items")
public class SetMetaDataTask extends ObjectWithSite implements ITask
{


	public final void Execute() throws Exception
	{
		if (!StringUtils2.isNullOrEmpty(this.Items) && this.Values != null)
		{
			IItemParser parser = this.getSite().getRequiredService(IItemParser.class);
			TaskItem[] items = parser.ParseItem(this.Items);

			ILogger logger = this.getSite().getService(ILogger.class);
			for (int i = 0; i < items.length && i < this.Values.length; i++)
			{
				items[i].setItem(this.Name, this.Values[i]);
				if (logger != null)
				{
					logger.LogMessage("setMetaData", "set item %1$s's meta data %2$s as %3$s", items[i].getName(), this.Name, this.Values[i]);
				}
			}
		}

	}


	@TaskMember(name = "items", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="The list of item to set metadata for.")
	public String Items;


	@TaskMember(name = "name", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required}, description="Name of metadata")
	public String Name;


	@TaskMember(name = "values", flags = {TaskMemberFlags.Input, TaskMemberFlags.Required} , description="The list of metadata value to set. If values only contains one element, all items will be set with the same value; otherwise values will be set to with coresponding order of items.")
	public String[] Values;


}