package Fantasy.Configuration;

public class SettingsBase implements INotifyPropertyChanged
{
	private java.util.HashMap<String, Object> _values = new java.util.HashMap<String, Object>();

	private Object _syncRoot = new Object();


	public Object GetValue(String name)
	{
		Object rs = null;
		synchronized (this._syncRoot)
		{
			if (!((rs = this._values.get(name)) != null))
			{
				PropertyInfo prop = this.getClass().GetProperty(name);
				String text = null;
				DefaultSettingValueAttribute attr = prop.<DefaultSettingValueAttribute>GetCustomAttributes(true).SingleOrDefault();
				if (attr != null)
				{
					text = attr.getValue();
				}
				if (DotNetToJavaStringHelper.isNullOrEmpty(text))
				{
					if (IsReadOnlyList(prop))
					{
						rs = Activator.CreateInstance(prop.PropertyType);


					}
					else if (!IsNullableType(prop.PropertyType) && prop.PropertyType.IsValueType)
					{
						rs = Activator.CreateInstance(prop.PropertyType);
					}
				}
				else
				{
					if (prop.PropertyType == XElement.class)
					{
						rs = XElement.Parse(text);
					}
					else if (Array.indexOf(PrimitiveTypes, prop.PropertyType) >= 0)
					{
						rs = Parse(text, prop.PropertyType);
					}
					else
					{
						XmlSerializer ser = new XmlSerializer(prop.PropertyType);
						StringReader reader = new StringReader(text);
						rs = ser.Deserialize(reader);
					}
				}
				this._values.put(name, rs);
			}

		}
		return rs;
	}


	private boolean IsReadOnlyList(PropertyInfo prop)
	{
		return !prop.CanWrite && Iterable.class.IsAssignableFrom(prop.PropertyType);
	}

	public void SetValue(String name, Object value)
	{
		Object old = this.GetValue(name);
		if (!value.equals(old))
		{
			synchronized (this._syncRoot)
			{
				this._values.put(name, value);
			}
			this.OnPropertyChanged(name);
		}
	}


	public void Renew(String xml)
	{
		java.util.ArrayList<String> changed = new java.util.ArrayList<String>();
		synchronized (_syncRoot)
		{

			java.util.HashMap<String, Object> newValues = new java.util.HashMap<String, Object>();
			LoadXmlToDictionary(xml, newValues);

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			var query = from p in this.getClass().GetProperties() where p.DeclaringType.IsSubclassOf(SettingsBase.class) select p;

			for (PropertyInfo prop : query)
			{
				boolean cx = _values.containsKey(prop.getName());
				boolean cy = newValues.containsKey(prop.getName());
				if (cx && cy)
				{
					Object x = _values.get(prop.getName());
					Object y = newValues.get(prop.getName());
					if (x!= null && y != null && Iterable.class.IsAssignableFrom(prop.PropertyType) && prop.PropertyType != String.class)
					{


						java.util.Iterator xe = ((Iterable)x).iterator();
						java.util.Iterator ye = ((Iterable)y).iterator();

						boolean nextX, nextY;
						do
						{
							nextX = xe.hasNext();
							nextY = ye.hasNext();
							if (nextX && nextY)
							{
								if (!xe.next().equals(ye.next()))
								{
									changed.add(prop.getName());
									break;
								}
							}
						} while (nextX && nextY);

						if (nextX ^ nextY)
						{
							changed.add(prop.getName());
						}
					}
					else
					{
						if (!x.equals(y))
						{
							changed.add(prop.getName());
						}
					}
				}
				else if (cx ^ cy)
				{
					changed.add(prop.getName());
				}
			}
			this._values = newValues;
		}

		for (String name : changed)
		{
			this.OnPropertyChanged(name);
		}

	}

	private PropertyInfo GetPropertyInfo(String name)
	{
		PropertyInfo rs = this.getClass().GetProperty(name);
		return rs;
	}

	private void LoadXmlToDictionary(String xml, java.util.HashMap<String, Object> dict)
	{
		XmlDocument doc = new XmlDocument();
		doc.LoadXml(xml);
		for (XmlElement element : doc.DocumentElement.SelectNodes("*"))
		{
			PropertyInfo prop = this.GetPropertyInfo(element.LocalName);
			if (prop != null)
			{
				java.lang.Class t = prop.PropertyType;
				Object value = LoadValueFromElement(t, element);
				dict.put(prop.getName(), value);
			}
		}
	}

	private static final String xsiUrl = "http://www.w3.org/2001/XMLSchema-instance";
	private static final java.lang.Class[] PrimitiveTypes = new java.lang.Class[] {Boolean.class, Byte.class, Character.class, java.math.BigDecimal.class, Double.class, Float.class, Integer.class, Long.class, Byte.class, Short.class, String.class, Color.class, java.util.Date.class, Font.class, Point.class, Size.class, Guid.class, TimeSpan.class, Short.class, Integer.class, Long.class, XElement.class};


	private Object Parse(String text, java.lang.Class conversionType)
	{
		TypeConverterAttribute a = conversionType.<TypeConverterAttribute>GetCustomAttributes(true).FirstOrDefault();
		if (a == null)
		{

			java.lang.reflect.Method method = conversionType.getMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new java.lang.Class[] { String.class }, null);
			if (method != null)
			{
				return method.invoke(null, new Object[] { text });
			}
			else
			{
				return Convert.ChangeType(text, conversionType);
			}
		}
		else
		{
			TypeConverter cvt = (TypeConverter)Activator.CreateInstance(java.lang.Class.forName(a.ConverterTypeName));
			return cvt.ConvertFrom(text);
		}

	}

	private boolean IsList(java.lang.Class t, RefObject<java.lang.Class> elementType)
	{

		elementType.argvalue = null;
		if (t.IsGenericType && t.GetGenericTypeDefinition()== java.util.ArrayList<>.class)
		{
			 elementType.argvalue = t.GetGenericArguments()[0];
			 return true;
		}
		return false;
	}

	private Object LoadValueFromElement(java.lang.Class t, XmlElement element)
	{

		if (element.GetAttribute("nil", xsiUrl).equals("true"))
		{
			return Convert.ChangeType(null, t);
		}

		if (IsNullableType(t))
		{
			t = Nullable.GetUnderlyingType(t);
		}

		if (t == String.class)
		{
			return element.InnerText;
		}
		if (t == XElement.class)
		{
		   XmlElement child = (XmlElement)element.SelectSingleNode("*");
			if (child != null)
			{
				return child.ToXElement();
			}
			else
			{
				return null;
			}
		}

		if (Array.indexOf(PrimitiveTypes, t) >= 0)
		{
			String s = element.InnerText;
			return Parse(s, t);
		}

		java.lang.Class elementType = null;
		RefObject<java.lang.Class> tempRef_elementType = new RefObject<java.lang.Class>(elementType);
		boolean tempVar = this.IsList(t, tempRef_elementType);
			elementType = tempRef_elementType.argvalue;
		if (tempVar)
		{
			XmlSerializer ser = new XmlSerializer(elementType);
			java.util.List rs = (java.util.List)Activator.CreateInstance(t);
			for (XmlElement child : element.SelectNodes("*"))
			{
				XmlNodeReader reader = new XmlNodeReader(child);
				try
				{
					Object o = ser.Deserialize(reader);
					rs.add(o);
				}
				finally
				{
					reader.Close();
				}

			}
			return rs;
		}
		else
		{
			XmlRootAttribute root = new XmlRootAttribute(element.LocalName);
			root.Namespace = element.NamespaceURI;
			XmlSerializer ser = new XmlSerializer(t, root);
			XmlNodeReader reader = new XmlNodeReader(element);
			try
			{
				return ser.Deserialize(reader);
			}
			finally
			{
				reader.Close();
			}
		}
	}

	public void Load(String xml)
	{
		synchronized (this._syncRoot)
		{
			this.LoadXmlToDictionary(xml, this._values);
		}
	}

	public String ToXml()
	{
		XmlDocument doc = new XmlDocument();
		doc.AppendChild(doc.CreateElement("settings"));
		doc.DocumentElement.SetAttribute("type", this.getClass().FullName);
		doc.DocumentElement.SetAttribute("xmlns:xsi", xsiUrl);
		synchronized (this._syncRoot)
		{
			for (java.util.Map.Entry<String, Object> kv : this._values.entrySet())
			{
				PropertyInfo prop = this.getClass().GetProperty(kv.getKey());
				if (prop != null)
				{
					XmlElement element = this.SaveValueToElement(prop.getName(), prop.PropertyType, kv.getValue(), doc);
					doc.DocumentElement.AppendChild(element);
				}
			}
		}
		return doc.OuterXml;

	}

	private XmlElement SaveValueToElement(String name, java.lang.Class t, Object value, XmlDocument doc)
	{
		if (value == null)
		{
			XmlElement rs = doc.CreateElement(name);
			XmlAttribute nil = doc.CreateAttribute("xsi:nil", xsiUrl);
			nil.setValue("true");
			rs.Attributes.Append(nil);
			return rs;
		}

		if (IsNullableType(t))
		{
			t = Nullable.GetUnderlyingType(t);
		}

		if (Array.indexOf(PrimitiveTypes, t) >= 0)
		{
			XmlElement rs = doc.CreateElement(name);
			if (t == XElement.class)
			{
				XElement x = (XElement)value;
				XmlElement child = x.ToXmlElement(doc);
				rs.AppendChild(child);
			}
			else
			{
				TypeConverterAttribute a = t.<TypeConverterAttribute>GetCustomAttributes(true).FirstOrDefault();
				String text;
				if (a == null)
				{
					text = String.valueOf(value);
				}
				else
				{
					TypeConverter cvt = (TypeConverter)Activator.CreateInstance(java.lang.Class.forName(a.ConverterTypeName));
					text = cvt.ConvertToString(value);
				}


				rs.InnerText = text;
			}
			return rs;

		}
		else
		{
			java.lang.Class elementType = null;
			RefObject<java.lang.Class> tempRef_elementType = new RefObject<java.lang.Class>(elementType);
			boolean tempVar = IsList(t, tempRef_elementType);
				elementType = tempRef_elementType.argvalue;
			if (tempVar)
			{
				XmlElement rs = doc.CreateElement(name);
				XmlSerializer ser = new XmlSerializer(elementType);

				for (Object o : (Iterable)value)
				{
					StringWriter w = new StringWriter ();
					ser.Serialize(w, o);
					XmlDocument doc2 = new XmlDocument();
					doc2.LoadXml(w.GetStringBuilder().toString());
					XmlNode child = doc.ImportNode(doc2.DocumentElement, true);
					rs.AppendChild(child);
				}
				return rs;
			}
			else
			{

				XmlRootAttribute root = new XmlRootAttribute(name);
				root.Namespace = doc.DocumentElement.NamespaceURI;
				XmlSerializer ser = new XmlSerializer(t, root);
				StringWriter w = new StringWriter();
				ser.Serialize(w, value);

				XmlDocument tempDoc = new XmlDocument();
				tempDoc.LoadXml(w.toString());
				XmlElement rs = (XmlElement)doc.ImportNode(tempDoc.DocumentElement, true);
				return rs;
			}
		}

	}

	private static boolean IsNullableType(java.lang.Class t)
	{
		return t.IsGenericType && t.GetGenericTypeDefinition() == .class;
	}



	protected void OnPropertyChanged(String name)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region INotifyPropertyChanged Members

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event PropertyChangedEventHandler PropertyChanged;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion



	public final void Save()
	{
		SettingStorage.Save(this);
	}
}