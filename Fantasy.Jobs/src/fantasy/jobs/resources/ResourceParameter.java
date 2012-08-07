package fantasy.jobs.resources;

import java.io.Serializable;

public class ResourceParameter implements Serializable
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -3823660117179524383L;

	public ResourceParameter(String name, java.util.Map<String, String> values)
	{
		if (name == null)
		{
			throw new IllegalArgumentException("name");
		}
		this.setName(name);


		if (values != null)
		{

			for (java.util.Map.Entry<String, String> kv :  values.entrySet())
			{
				this._values.put(kv.getKey(), kv.getValue());
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


	private java.util.TreeMap<String, String> _values = new java.util.TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);

	public final java.util.Map<String,String> getValues()
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