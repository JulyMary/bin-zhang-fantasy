package fantasy.servicemodel;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XmlRoot("address")]
public class AddressSetting
{
	public AddressSetting()
	{
		setPort(-1);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlAttribute("host")]
	private String privateHost;
	public final String getHost()
	{
		return privateHost;
	}
	public final void setHost(String value)
	{
		privateHost = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlAttribute("port")]
	private int privatePort;
	public final int getPort()
	{
		return privatePort;
	}
	public final void setPort(int value)
	{
		privatePort = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlArray("contracts"), XmlArrayItem("contract", java.lang.Class = typeof(String))]
	private String[] privateContracts;
	public final String[] getContracts()
	{
		return privateContracts;
	}
	public final void setContracts(String[] value)
	{
		privateContracts = value;
	}
}