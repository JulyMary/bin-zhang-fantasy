package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("uncPath", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class UncPath
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("path")]
	private String privatePath;
	public final String getPath()
	{
		return privatePath;
	}
	public final void setPath(String value)
	{
		privatePath = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("username")]
	private String privateUserName;
	public final String getUserName()
	{
		return privateUserName;
	}
	public final void setUserName(String value)
	{
		privateUserName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("password")]
	private String privatePassword;
	public final String getPassword()
	{
		return privatePassword;
	}
	public final void setPassword(String value)
	{
		privatePassword = value;
	}
}