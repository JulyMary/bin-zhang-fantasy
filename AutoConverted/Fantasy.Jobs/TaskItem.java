package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[, XSerializable("taskitem",NamespaceUri = Consts.XNamespaceURI)]
public class TaskItem extends IXSerializable implements Cloneable, IConditionalObject, Serializable
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

	private NameValueCollection _metaData = new NameValueCollection(StringComparer.OrdinalIgnoreCase);

	public final String getItem(String index)
	{
		if (index == null)
		{
			throw new ArgumentNullException("index");
		}

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		ITaskItemMetaDataProvider provider = (from p in MetaDataProviders where p.GetNames(this).ToList().Exists(x => StringComparer.OrdinalIgnoreCase.Compare(x, index) == 0) select p).FirstOrDefault();

		if (provider != null)
		{
			return provider.GetData(this, index);
		}
		else
		{
			return this._metaData[index];
		}


	}
	public final void setItem(String index, String value)
	{
		if (index == null)
		{
			throw new ArgumentNullException("index");
		}

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		ITaskItemMetaDataProvider provider = (from p in MetaDataProviders where p.GetNames(this).ToList().Exists(x => StringComparer.OrdinalIgnoreCase.Compare(x, index) == 0) select p).FirstOrDefault();

		if (provider == null)
		{
			this._metaData[index] = value;
		}

	}

	public final int getMetaDataCount()
	{
		return this.getMetaDataNames().length;
	}

	public final String[] getMetaDataNames()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		return (from provider in MetaDataProviders let names = provider.GetNames(this) from name in names select name).Union(this._metaData.AllKeys).toArray();
	}



	public final boolean HasMetaData(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from key in getMetaDataNames() where (StringComparer.OrdinalIgnoreCase.Compare(name, key) == 0) select key;
		return query.Count() > 0;
	}

	public final void RemoveMetaData(String name)
	{
		this._metaData.Remove(name);
	}

	public final void CopyMetaDataTo(TaskItem destinationItem)
	{
		for (String key : this._metaData.AllKeys)
		{
			destinationItem.setItem(key, this._metaData[key]);
		}
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICloneable Members

	public final Object clone()
	{
		TaskItem rs = new TaskItem();
		rs.setName(this.getName());
		rs.setCategory(this.getCategory());
		this.CopyMetaDataTo(rs);
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IConditionalObject Members

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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


	private static ITaskItemMetaDataProvider[] MetaDataProviders = new ITaskItemMetaDataProvider[] { new NameAndCategoryMetaDataProvider(), new FileInfoMetaDataProvider() };



}