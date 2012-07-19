package fantasy.jobs;

import Fantasy.IO.*;

public class NameAndCategoryMetaDataProvider implements ITaskItemMetaDataProvider
{

	private static final String[] Names = new String[] { "Name", "Category" };
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITaskItemMetaDataProvider Members

	public final String[] GetNames(TaskItem item)
	{
		return Names;
	}

	public final String GetData(TaskItem item, String name)
	{
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//		switch (name.ToLower())
//ORIGINAL LINE: case "name":
		if (name.toLowerCase().equals("name"))
		{
				return item.getName();
		}
//ORIGINAL LINE: case "category":
		else if (name.toLowerCase().equals("category"))
		{
				return item.getCategory();
		}
		else
		{
				throw new RuntimeException("Absurd!");
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}