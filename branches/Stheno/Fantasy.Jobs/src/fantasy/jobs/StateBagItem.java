package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.*;
import org.jdom2.*;

@SuppressWarnings({"rawtypes", "unchecked"})
@XSerializable(name = "state", namespaceUri= Consts.XNamespaceURI)
public class StateBagItem implements IXSerializable
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

	private Object _value;

	public final Object getValue()
	{
		return _value;
	}
	public final void setValue(Object value)
	{
		_value = value;
	}


	public void Load(IServiceProvider context, Element element) throws Exception
	{
		this.setName(element.getName());
		Namespace ns = Namespace.getNamespace(Consts.XNamespaceURI);
		
		String typeName = element.getAttributeValue("type", ns);
		if(! StringUtils2.isNullOrEmpty(typeName))
		{

			
			Class t = java.lang.Class.forName(typeName);
			if (t.isAnnotationPresent(XSerializable.class))
			{

				XSerializer ser = new XSerializer(t);
				this.setValue(ser.deserialize(element));
			}
			else
			{
				ITypeConverter convert = XHelper.getDefault().CreateXConverter(t);
				this.setValue(convert.convertTo(element.getValue(), t));
			}
		}

	}

	public void Save(IServiceProvider context, Element element) throws Exception
	{

		if (getValue() != null)
		{
			Namespace ns = Namespace.getNamespace(Consts.XNamespaceURI);
			java.lang.Class t = getValue().getClass();
			element.setAttribute(ns + "type", t.getName(), ns);

			if (t.isAnnotationPresent(XSerializable.class))
			{
				XSerializer ser = new XSerializer(t);
				ser.serialize(element, getValue());
			}
			else
			{
				ITypeConverter convert = XHelper.getDefault().CreateXConverter(t);
				String s = (String)convert.convertFrom(this.getValue());
				element.setText(s);
			}


		}
	}

}