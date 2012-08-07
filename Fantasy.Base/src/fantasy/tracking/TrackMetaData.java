package fantasy.tracking;
import java.util.*;
import java.io.*;

import fantasy.UUIDUtils;
public class TrackMetaData implements Serializable
{
/**
	 * 
	 */
	private static final long serialVersionUID = -4832311918518984032L;
	//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private UUID privateId = UUIDUtils.Empty;
	public final UUID getId()
	{
		return privateId;
	}
	public final void setId(UUID value)
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