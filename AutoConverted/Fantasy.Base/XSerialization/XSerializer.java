package Fantasy.XSerialization;

import Fantasy.Properties.*;

public class XSerializer
{
	public XSerializer(java.lang.Class targetType)
	{
		if (targetType == null)
		{
			throw new ArgumentNullException("type");
		}
		this.setTargetType(targetType);
		this._map = XHelper.getDefault().GetTypeMap(targetType);

	}

	private IServiceProvider privateContext;
	public final IServiceProvider getContext()
	{
		return privateContext;
	}
	public final void setContext(IServiceProvider value)
	{
		privateContext = value;
	}

	private java.lang.Class privateTargetType;
	public final java.lang.Class getTargetType()
	{
		return privateTargetType;
	}
	private void setTargetType(java.lang.Class value)
	{
		privateTargetType = value;
	}

	private XTypeMap _map;

	public final void Serialize(XElement element, Object o)
	{
		this._map.Save(this.getContext(), element, o);
	}

//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public XElement Serialize(object o, XmlSerializerNamespaces namespaces = null)
	public final XElement Serialize(Object o, XmlSerializerNamespaces namespaces)
	{
		if (o == null)
		{
			throw new ArgumentNullException("o");
		}

		String ns = null;
		String name = null;

		name = this._map.getSerializableAttribute().getName();
		ns = this._map.getSerializableAttribute().getNamespaceUri();
		if (DotNetToJavaStringHelper.isNullOrEmpty(name))
		{
			throw new XException(String.format(Resources.getTypeMissingElementNameText(), this.getTargetType()));
		}


		XElement rs = new XElement((XNamespace)ns + name);


		if (namespaces != null)
		{
			for (XmlQualifiedName qn : namespaces.toArray())
			{
				if ( ! qn.getName().equals(""))
				{
					rs.Add(new XAttribute(XNamespace.Xmlns + qn.getName(), qn.Namespace));
				}
				else
				{
					rs.Add(new XAttribute("xmlns", qn.Namespace));
				}

			}
		}
		this._map.Save(getContext(), rs, o);
		return rs;
	}



	public final Object Deserialize(XElement element)
	{
		Object rs = Activator.CreateInstance(this.getTargetType());
		this.Deserialize(element, rs);
		return rs;
	}

	public final void Deserialize(XElement element, Object instance)
	{
		this._map.Load(this.getContext(), element, instance);
	}
}