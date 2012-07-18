package fantasy.xserialization;

import org.jdom2.*;

import fantasy.*;

public class XSerializer
{
	public XSerializer(@SuppressWarnings("rawtypes") java.lang.Class targetType) throws Exception
	{
		if (targetType == null)
		{
			throw new IllegalArgumentException("type");
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

	@SuppressWarnings("rawtypes")
	private Class privateTargetType;
	@SuppressWarnings("rawtypes")
	public final Class getTargetType()
	{
		return privateTargetType;
	}
	
	private void setTargetType(@SuppressWarnings("rawtypes") java.lang.Class value)
	{
		privateTargetType = value;
	}

	private XTypeMap _map;

	public final void serialize(Element element, Object o) throws Exception
	{
		this._map.Save(this.getContext(), element, o);
	}

	public final Element serialize(Object o) throws Exception
	{
		return serialize(o, null);
	}

	public final Element serialize(Object o, Namespace[] namespaces) throws Exception
	{
		if (o == null)
		{
			throw new IllegalArgumentException("o");
		}

		String ns = null;
		String name = null;

		name = this._map.getXSerializableAnnotation().name();
		ns = this._map.getXSerializableAnnotation().namespaceUri();
		if (StringUtils2.isNullOrEmpty(name))
		{
			throw new XException(String.format("%s does not declare XSerializable", this.getTargetType()));
		}


		Element rs = new Element(name, Namespace.getNamespace(ns));


		if (namespaces != null)
		{
			for (Namespace ns2 : namespaces)
			{
				rs.addNamespaceDeclaration(ns2);

			}
		}
		this._map.Save(getContext(), rs, o);
		return rs;
	}



	public final Object deserialize(Element element) throws Exception
	{
		Object rs = this.getTargetType().newInstance();
		this.deserialize(element, rs);
		return rs;
	}

	public final void deserialize(Element element, Object instance) throws Exception
	{
		this._map.Load(this.getContext(), element, instance);
	}
}