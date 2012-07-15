package Fantasy.Jobs.Resources;

public class ResourceParameter implements Serializable
{
//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public ResourceParameter(string name, object values = null)
	public ResourceParameter(String name, Object values)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		this.setName(name);


		if (values != null)
		{
			if (values instanceof java.util.Map<String, String>)
			{
				for (java.util.Map.Entry<String, String> kv : (java.util.Map<String, String>)values)
				{
					_values.put(kv.getKey(), kv.getValue());
				}
			}
			else
			{
				java.lang.Class t = values.getClass();
				for (PropertyInfo pi : t.GetProperties())
				{
					Object v = pi.GetValue(values, null);
					String s = v != null ? String.valueOf(v) : null;
					this._values.put(pi.getName(), s);
				}
			}

		}
	}

	public ResourceParameter()
	{

	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}


	private java.util.HashMap<String, String> _values = new java.util.HashMap<String, String>(StringComparer.OrdinalIgnoreCase);

	public final java.util.HashMap<String,String> getValues()
	{
		return _values;
	}

	@Override
	public String toString()
	{
		StringBuilder rs = new StringBuilder();
		rs.append(String.format("name=%1$s", this.getName()));
		for (java.util.Map.Entry<String, String> item : this._values.entrySet())
		{
			rs.append(String.format(";%1$s=%2$s", item.getKey(), item.getValue()));
		}

		return rs.toString();

	}

}