package fantasy.jobs;



public class NameAndCategoryMetaDataProvider implements ITaskItemMetaDataProvider
{

	private static final String[] Names = new String[] { "Name", "Category" };
	

	public final String[] GetNames(TaskItem item)
	{
		return Names;
	}

	public final String GetData(TaskItem item, String name)
	{
		if (name.toLowerCase().equals("name"))
		{
			return item.getName();
		}
		else if (name.toLowerCase().equals("category"))
		{
			return item.getCategory();
		}
		else
		{
			throw new RuntimeException("Absurd!");
		}
	}
}