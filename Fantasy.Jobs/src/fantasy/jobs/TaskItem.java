package fantasy.jobs;

import java.io.Serializable;
import java.util.*;

import org.apache.commons.lang3.StringUtils;

import fantasy.xserialization.*;
import fantasy.*;
import org.jdom2.*;

@XSerializable(name = "taskitem", namespaceUri = Consts.XNamespaceURI)
public class TaskItem implements IXSerializable, Cloneable, IConditionalObject, Serializable
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -8092952054534622690L;
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
	

	private TreeMap<String, String> _metaData = new TreeMap<String, String>(String.CASE_INSENSITIVE_ORDER);

	public final String getItem(String index)
	{
		if (index == null)
		{
			throw new IllegalArgumentException("index");
		}

		ITaskItemMetaDataProvider provider = providerForName(index);

		if (provider != null)
		{
			return provider.GetData(this, index);
		}
		else
		{
			return this._metaData.get(index);
		}


	}
	
	
	private ITaskItemMetaDataProvider providerForName(String name)
	{
		
		for(ITaskItemMetaDataProvider p : MetaDataProviders)
		{
			for(String s : p.GetNames(this))
			{
				if(StringUtils.equalsIgnoreCase(s, name))
				{
					return p;
				}
			}
		}
		
		return null;
	}
	
	public final void setItem(String index, String value)
	{
		if (index == null)
		{
			throw new IllegalArgumentException("index");
		}

		ITaskItemMetaDataProvider provider = providerForName(index);

		if (provider == null)
		{
			this._metaData.put(index, value);
		}

	}

	public final int getMetaDataCount()
	{
		return this.getMetaDataNames().length;
	}

	public final String[] getMetaDataNames()
	{
		
		ArrayList<String> rs = new ArrayList<String>();
		
		for(ITaskItemMetaDataProvider p : MetaDataProviders)
		{
			for(String s : p.GetNames(this))
			{
				rs.add(s);
			}
		}
		
		rs.addAll(this._metaData.keySet());
		
		
		return rs.toArray(new String[0]);
	}



	public final boolean HasMetaData(String name)
	{
		return this._metaData.containsKey(name) || this.providerForName(name) != null;

	}

	public final void RemoveMetaData(String name)
	{
		this._metaData.remove(name);
	}

	public final void CopyMetaDataTo(TaskItem destinationItem)
	{
		for (String key : this._metaData.keySet())
		{
			destinationItem.setItem(key, this._metaData.get(key));
		}
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
			Element child = new Element(key,element.getNamespace());
			child.setText(this._metaData.get(key));

			element.addContent(child);
		}
	}

	public final Object clone()
	{
		TaskItem rs = new TaskItem();
		rs.setName(this.getName());
		rs.setCategory(this.getCategory());
		this.CopyMetaDataTo(rs);
		return rs;
	}

	@XAttribute(name = "condition")
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}



	private static ITaskItemMetaDataProvider[] MetaDataProviders = new ITaskItemMetaDataProvider[] { new NameAndCategoryMetaDataProvider(), new FileInfoMetaDataProvider() };



}