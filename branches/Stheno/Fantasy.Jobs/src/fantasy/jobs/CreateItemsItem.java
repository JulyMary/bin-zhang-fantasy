package fantasy.jobs;

import java.util.*;

import fantasy.xserialization.*;
import fantasy.*;
import org.jdom2.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
@XSerializable(name = "taskitem", namespaceUri = Consts.XNamespaceURI)
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

	private TreeMap<String, String> _metaData = new TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);
	public final TreeMap<String, String> getMetaData()
	{
		return _metaData;
	}


	public final void Load(IServiceProvider context, Element element)
	{
		this.setName((String)element.getAttributeValue("name"));
		this.setCondition((String)element.getAttributeValue("condition"));
		this.setCategory(element.getName());
		for (Element child : element.getChildren())
		{
			this._metaData.put(child.getName() ,child.getValue());
		}
	}

	public final void Save(IServiceProvider context, Element element)
	{
		element.setAttribute("name", this.getName());
		if (!StringUtils2.isNullOrEmpty(this.getCondition()))
		{
			element.setAttribute("condition", this.getCondition());
		}
		for (String key : this._metaData.keySet())
		{
			Element child = new Element(key, element.getNamespace()).addContent(this._metaData.get(key));

			element.addContent(child);
		}
	}

}