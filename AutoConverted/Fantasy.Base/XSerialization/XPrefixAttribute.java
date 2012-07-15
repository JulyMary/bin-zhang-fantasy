package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Assembly,AllowMultiple=true)]
public class XPrefixAttribute extends Attribute
{
	public XPrefixAttribute(String prefix, String namespace)
	{
		this.setPrefix(prefix);
		this.setNamesapce(namespace);
	}
	private String privatePrefix;
	public final String getPrefix()
	{
		return privatePrefix;
	}
	private void setPrefix(String value)
	{
		privatePrefix = value;
	}
	private String privateNamesapce;
	public final String getNamesapce()
	{
		return privateNamesapce;
	}
	private void setNamesapce(String value)
	{
		privateNamesapce = value;
	}
}