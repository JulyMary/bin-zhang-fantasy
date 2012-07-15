package Fantasy.XSerialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Class)]
public class XConverterAttribute extends Attribute
{
	private java.lang.Class privateTargetType;
	public final java.lang.Class getTargetType()
	{
		return privateTargetType;
	}
	public final void setTargetType(java.lang.Class value)
	{
		privateTargetType = value;
	}
}