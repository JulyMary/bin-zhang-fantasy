package Fantasy.Tracking;

// Use a data contract as illustrated in the sample below to add composite types to service operations
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract]
public class TrackProperty
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
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
	//[DataMember]
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateTypeName;
	public final String getTypeName()
	{
		return privateTypeName;
	}
	public final void setTypeName(String value)
	{
		privateTypeName = value;
	}


	public static TrackProperty Create(String name, Object value)
	{
		TrackProperty tempVar = new TrackProperty();
		tempVar.setName(name);
		TrackProperty rs = tempVar;
		if(value != null)
		{
			rs.setValue(value.toString());
			rs.setTypeName(value.getClass().FullName);
		}
		return rs;

	}

	public static Object ToObject(TrackProperty property)
	{
		if (property.getTypeName() != null)
		{
			java.lang.Class t = java.lang.Class.forName(property.getTypeName());
			return Convert.ChangeType(property.getValue(), t);
		}
		else
		{
			return null;
		}
	}
}