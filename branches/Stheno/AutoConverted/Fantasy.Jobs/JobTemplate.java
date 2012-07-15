package Fantasy.Jobs;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract]
public class JobTemplate implements Serializable
{
	public JobTemplate()
	{
		this.setName("");
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private int privateid;
	public final int getid()
	{
		return privateid;
	}
	public final void setid(int value)
	{
		privateid = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateLocation;
	public final String getLocation()
	{
		return privateLocation;
	}
	public final void setLocation(String value)
	{
		privateLocation = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateContent;
	public final String getContent()
	{
		return privateContent;
	}
	public final void setContent(String value)
	{
		privateContent = value;
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
	private boolean privateValid;
	public final boolean getValid()
	{
		return privateValid;
	}
	public final void setValid(boolean value)
	{
		privateValid = value;
	}

	public final String ToAbsolutPath(String path)
	{
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		String rs = path;
		if (!Path.IsPathRooted(rs))
		{
			String dir = Path.GetDirectoryName(this.getLocation());
			rs = dir + Path.DirectorySeparatorChar + rs;

			rs = Path.GetFullPath(path);
		}

		return rs;

	}
}