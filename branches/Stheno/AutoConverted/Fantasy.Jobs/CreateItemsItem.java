package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("taskitem", NamespaceUri = Consts.XNamespaceURI)]
public class CreateItemsItem implements IConditionalObject, IXSerializable
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

	private String privateCategory;
	public final String getCategory()
	{
		return privateCategory;
	}
	public final void setCategory(String value)
	{
		privateCategory = value;
	}

	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}

	private NameValueCollection _metaData = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
	public final NameValueCollection getMetaData()
	{
		return _metaData;
	}


	public final void Load(IServiceProvider context, XElement element)
	{
		this.setName((String)element.Attribute("name"));
		this.setCondition((String)element.Attribute("condition"));
		this.setCategory(element.getName().LocalName);
		for (XElement child : element.Elements())
		{
			this._metaData[child.getName().LocalName] = child.getValue();
		}
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		element.SetAttributeValue("name", this.getName());
		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getCondition()))
		{
			element.SetAttributeValue("condition", this.getCondition());
		}
		for (String key : this._metaData.AllKeys)
		{
			XElement child = new XElement(element.getName().Namespace + key, this._metaData[key]);

			element.Add(child);
		}
	}

}