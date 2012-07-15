package fantasy.xserialization;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public @interface XElement
{
    public int order() default Integer.MAX_VALUE;
	
	public String name();
	
	public String namespaceUri();
	
	@SuppressWarnings("rawtypes")
	public Class converter();

}