package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class XArrayItemAttribute extends Attribute
{
	public XArrayItemAttribute()
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
	private String privateNamespaceUri;
	public final String getNamespaceUri()
	{
		return privateNamespaceUri;
	}
	public final void setNamespaceUri(String value)
	{
		privateNamespaceUri = value;
	}
	private java.lang.Class privateType;
	public final java.lang.Class getType()
	{
		return privateType;
	}
	public final void setType(java.lang.Class value)
	{
		privateType = value;
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