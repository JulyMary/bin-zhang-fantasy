package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public abstract class XMemberAttribute extends Attribute
{
	public XMemberAttribute()
	{
		this.setOrder(Integer.MAX_VALUE);

	}
	private int privateOrder;
	public final int getOrder()
	{
		return privateOrder;
	}
	public final void setOrder(int value)
	{
		privateOrder = value;
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