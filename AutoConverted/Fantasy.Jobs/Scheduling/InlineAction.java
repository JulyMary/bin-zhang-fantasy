package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("inlineAction")]
public class InlineAction extends Action implements IXSerializable
{

	@Override
	public ActionType getType()
	{
		return ActionType.Inline;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember]
	private String privateXslt;
	public final String getXslt()
	{
		return privateXslt;
	}
	public final void setXslt(String value)
	{
		privateXslt = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	public final void Load(IServiceProvider context, XElement element)
	{
		XHelper.Default.LoadByXAttributes(context, element, this);
		XElement xsltElement = element.Elements().FirstOrDefault();
		this.setXslt(xsltElement != null ? xsltElement.toString() : null);
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		XHelper.Default.SaveByXAttributes(context, element, this);
		if (!DotNetToJavaStringHelper.isNullOrEmpty(getXslt()))
		{
			element.Add(XElement.Parse(getXslt()));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}