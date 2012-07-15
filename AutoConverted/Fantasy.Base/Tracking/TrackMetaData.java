package Fantasy.Tracking;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract]
public class TrackMetaData
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private Guid privateId = new Guid();
	public final Guid getId()
	{
		return privateId;
	}
	public final void setId(Guid value)
	{
		privateId = value;
	}

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
	private String privateCategory;
	public final String getCategory()
	{
		return privateCategory;
	}
	public final void setCategory(String value)
	{
		privateCategory = value;
	}
}