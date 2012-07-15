package System.Linq.Dynamic;

public abstract class DynamicClass
{
	@Override
	public String toString()
	{
		PropertyInfo[] props = this.getClass().GetProperties(BindingFlags.Instance | BindingFlags.Public);
		StringBuilder sb = new StringBuilder();
		sb.append("{");
		for (int i = 0; i < props.length; i++)
		{
			if (i > 0)
			{
				sb.append(", ");
			}
			sb.append(props[i].getName());
			sb.append("=");
			sb.append(props[i].GetValue(this, null));
		}
		sb.append("}");
		return sb.toString();
	}
}