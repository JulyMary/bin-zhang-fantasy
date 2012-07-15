package System.Linq.Dynamic;

public class DynamicProperty
{
	private String name;
	private java.lang.Class type;

	public DynamicProperty(String name, java.lang.Class type)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (type == null)
		{
			throw new ArgumentNullException("type");
		}
		this.name = name;
		this.type = type;
	}

	public final String getName()
	{
		return name;
	}

	public final java.lang.Class getType()
	{
		return type;
	}
}