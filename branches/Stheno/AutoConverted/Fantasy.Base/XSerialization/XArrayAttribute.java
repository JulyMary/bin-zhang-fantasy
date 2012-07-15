package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class XArrayAttribute extends XMemberAttribute
{
	private java.lang.Class privateSerializer;
	public final java.lang.Class getSerializer()
	{
		return privateSerializer;
	}
	public final void setSerializer(java.lang.Class value)
	{
		privateSerializer = value;
	}
}