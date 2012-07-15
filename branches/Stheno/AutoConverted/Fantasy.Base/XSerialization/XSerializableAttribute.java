package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
public class XSerializableAttribute extends Attribute
{

	public XSerializableAttribute(String name)
	{
		this.setName(name);
	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	private void setName(String value)
	{
		privateName = value;
	}

	private String privateNamespaceUri;
	public final String getNamespaceUri()
	{
		return privateNamespaceUri;
	}
	public final void setNamespaceUri(String value)
	{
		privateNamespaceUri = value;
	}

	private java.lang.Class privateXConverter;
	public final java.lang.Class getXConverter()
	{
		return privateXConverter;
	}
	public final void setXConverter(java.lang.Class value)
	{
		privateXConverter = value;
	}

}