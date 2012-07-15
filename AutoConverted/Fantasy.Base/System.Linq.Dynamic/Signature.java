package System.Linq.Dynamic;

public class Signature implements IEquatable<Signature>
{
	public DynamicProperty[] properties;
	public int hashCode;

	public Signature(Iterable<DynamicProperty> properties)
	{
		this.properties = properties.toArray();
		hashCode = 0;
		for (DynamicProperty p : properties)
		{
			hashCode ^= p.getName().hashCode() ^ p.getType().hashCode();
		}
	}

	@Override
	public int hashCode()
	{
		return hashCode;
	}

	@Override
	public boolean equals(Object obj)
	{
		return obj instanceof Signature ? equals((Signature)obj) : false;
	}

	public final boolean equals(Signature other)
	{
		if (properties.length != other.properties.length)
		{
			return false;
		}
		for (int i = 0; i < properties.length; i++)
		{
			if (!properties[i].getName().equals(other.properties[i].getName()) || properties[i].getType() != other.properties[i].getType())
			{
				return false;
			}
		}
		return true;
	}
}