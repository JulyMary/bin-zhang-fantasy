package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("state", NamespaceUri= Consts.XNamespaceURI)]
public class StateBagItem extends IXSerializable
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


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	private void Load(IServiceProvider context, XElement element)
	{
		this.setName(element.getName().LocalName);
		XNamespace ns = Consts.XNamespaceURI;
		if(! DotNetToJavaStringHelper.isNullOrEmpty((String)element.Attribute(ns + "type")))
		{

			java.lang.Class t = java.lang.Class.forName((String)element.Attribute(ns + "type"), true);
			if (t.IsDefined(XSerializableAttribute.class, false))
			{

				XSerializer ser = new XSerializer(t);
				this.setValue(ser.Deserialize(element));
			}
			else
			{
				TypeConverter convert = XHelper.Default.CreateXConverter(t);
				this.setValue(convert.ConvertTo(element.getValue(), t));
			}
		}

	}

	private void Save(IServiceProvider context, XElement element)
	{

		if (getValue() != null)
		{
			XNamespace ns = Consts.XNamespaceURI;
			java.lang.Class t = getValue().getClass();
			element.SetAttributeValue(ns + "type", String.format("%1$s, %2$s", t.FullName, t.Assembly.GetName().getName()));

			if (t.IsDefined(XSerializableAttribute.class, false))
			{
				XSerializer ser = new XSerializer(t);
				ser.Serialize(element, getValue());
			}
			else
			{
				TypeConverter convert = XHelper.Default.CreateXConverter(t);
				String s = (String)convert.ConvertFrom(this.getValue());
				element.setValue(s);
			}


		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}