package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("property", NamespaceUri=Consts.XNamespaceURI)]
public class JobProperty extends IXSerializable implements Cloneable, IConditionalObject, Serializable
{
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
	//[XValue]
	private String privateValue;
	public final String getValue()
	{
		return privateValue;
	}
	public final void setValue(String value)
	{
		privateValue = value;
	}

	public final Object clone()
	{
		JobProperty tempVar = new JobProperty();
		tempVar.setName(this.getName());
		tempVar.setValue(this.getValue());
		tempVar.setCondition(this.getCondition());
		return tempVar;
	}

	public final void Load(IServiceProvider context, XElement element)
	{
		this.setName(element.getName().LocalName);
		XHelper.Default.LoadByXAttributes(context, element, this);
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		XHelper.Default.SaveByXAttributes(context, element, this);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}

}