package Fantasy.Jobs.Tasks;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Task("setMetaData", Consts.XNamespaceURI, Description="Set meta data for input items")]
public class SetMetaDataTask extends ObjectWithSite implements ITask
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITask Members

	public final boolean Execute()
	{
		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getItems()) && getValues() != null)
		{
			IItemParser parser = this.Site.<IItemParser>GetRequiredService();
			TaskItem[] items = parser.ParseItem(this.getItems());

			ILogger logger = this.Site.<ILogger>GetService();
			for (int i = 0; i < items.length && i < this.getValues().length; i++)
			{
				items[i].setItem(this.getName(), getValues()[i]);
				if (logger != null)
				{
					logger.LogMessage("setMetaData", "set item {0}'s meta data {1} as {2}", items[i].getName(), this.getName(), getValues()[i]);
				}
			}
		}

		return true;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("items", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of item to set metadata for.")]
	private String privateItems;
	public final String getItems()
	{
		return privateItems;
	}
	public final void setItems(String value)
	{
		privateItems = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("name", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Name of metadata")]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[TaskMember("values", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of metadata value to set. If values only contains one element, all items will be set with the same value; otherwise values will be set to with coresponding order of items.")]
	 String[] getValues()
	 void setValues(String[] value)

}