package Fantasy.XSerialization;

import Fantasy.*;

public class XTypeMap
{
	public XTypeMap(java.lang.Class t)
	{
		this.setTargetType(t);
		this.setSerializableAttribute((XSerializableAttribute)t.GetCustomAttributes(XSerializableAttribute.class, true).SingleOrDefault());
		this.LoadNamespaceMap();
		this.LoadAttributeMaps();
		this.LoadElementMaps();
		this.LoadArrayMaps();
		this.LoadValueMap();

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this._attributeMaps = new java.util.ArrayList<XAttributeMap>(this._attributeMaps.OrderBy(map => map.Order));
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		this._elementMaps = new java.util.ArrayList<XMemberMap>(this._elementMaps.OrderBy(map => map.Order));
	}

	private void LoadNamespaceMap()
	{
		XNamespaceMap tempVar = new XNamespaceMap();
		tempVar.setMember(mi);
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from mi in this.GetAllMembers() where mi.IsDefined(XNamespaceAttribute.class, true) && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite)) select tempVar;
		this._namespaceMap = query.FirstOrDefault();
	}

	private XNamespaceMap _namespaceMap = null;

	private void LoadValueMap()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from mi in this.GetAllMembers() where mi.IsDefined(XValueAttribute.class, true) && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite)) select new { member = mi, attribute = (XValueAttribute)mi.GetCustomAttributes(XValueAttribute.class, true)[0] };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		var v = query.SingleOrDefault();
		if (v != null)
		{
			this.setValueMap(new XValueMap());
			this.getValueMap().setMember(v.member);
			this.getValueMap().setValue(v.attribute);
			java.lang.Class mt = v.member.MemberType == MemberTypes.Field ? ((java.lang.reflect.Field)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;
			this.getValueMap().setConverter(this.getValueMap().getValue().getXConverter() != null ? (TypeConverter)Activator.CreateInstance(this.getValueMap().getValue().getXConverter()) : XHelper.getDefault().CreateXConverter(mt));
		}

	}

	private Iterable<MemberInfo> GetAllMembers()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		var query = from t in this.getTargetType().Flatten(x=>x.getSuperclass()) let members = t.GetMembers(_memberBindingFlags) from mi in members select mi;
		java.util.ArrayList<String> keys = new java.util.ArrayList<String>();
		for (MemberInfo mi : query)
		{
			int n = keys.BinarySearch(mi.getName());
			if (n < 0)
			{
				keys.add(~n, mi.getName());
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return mi;
			}


		}

	}

	private String GetQualifiedName(String ns, String name)
	{
		return ns + "\n" + name;
	}

	private void LoadAttributeMaps()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from mi in this.GetAllMembers() where mi.IsDefined(XAttributeAttribute.class, true) && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite)) select new { member = mi, attribute = (XAttributeAttribute)mi.GetCustomAttributes(XAttributeAttribute.class, true)[0] };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var v : query)
		{

			XAttributeMap map = new XAttributeMap();
			map.setMember(v.member);
			map.setAttribute(v.attribute);
			map.setOrder(v.attribute.Order);
			java.lang.Class mt = v.member.MemberType == MemberTypes.Field ? ((java.lang.reflect.Field)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;
			if (!mt.IsDefined(XSerializableAttribute.class, false))
			{
				map.setConverter(map.getAttribute().getXConverter() != null ? (TypeConverter)Activator.CreateInstance(map.getAttribute().getXConverter()) : XHelper.getDefault().CreateXConverter(mt));

			}
			this.getAttributeMaps().add(map);
			this.getAttributeQNameMaps().put(this.GetQualifiedName(v.attribute.NamespaceUri, v.attribute.getName()), map);
		}

	}

	private void LoadElementMaps()
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from mi in this.GetAllMembers() where mi.IsDefined(XElementAttribute.class, true) && (mi.MemberType == MemberTypes.Field || (mi.MemberType == MemberTypes.Property && ((PropertyInfo)mi).CanRead && ((PropertyInfo)mi).CanWrite)) select new { member = mi, attribute = (XElementAttribute)mi.GetCustomAttributes(XElementAttribute.class, true)[0] };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var v : query)
		{
			XElementMap map = new XElementMap();
			map.setMember(v.member);
			map.setElement(v.attribute);
			map.setOrder(v.attribute.Order);
			java.lang.Class mt = v.member.MemberType == MemberTypes.Field ? ((java.lang.reflect.Field)v.member).FieldType : ((PropertyInfo)v.member).PropertyType;

			if (!mt.IsDefined(XSerializableAttribute.class, false))
			{
				map.setConverter(map.getElement().getXConverter() != null ? (TypeConverter)Activator.CreateInstance(map.getElement().getXConverter()) : XHelper.getDefault().CreateXConverter(mt));
			}
			this.getElementMaps().add(map);
		}
	}

	private java.lang.Class GetGenericListType(java.lang.Class type)
	{

		if (type.GetGenericTypeDefinition() == java.util.Collection<>.class)
		{
			return type;
		}
		for (java.lang.Class intf : type.GetInterfaces())
		{
			if (intf.IsGenericType)
			{
				if (intf.GetGenericTypeDefinition() == java.util.Collection<>.class)
				{
					// if needed, you can also return the type used as generic argument 
					return intf;
				}
			}
		}
		return null;
	}

	private boolean IsValidCollectionMemeber(MemberInfo mi)
	{
		java.lang.Class memberType = null;
		if (mi instanceof java.lang.reflect.Field)
		{
			java.lang.reflect.Field fi = (java.lang.reflect.Field)mi;
			if (fi.FieldType.IsArray)
			{
				return true;
			}
			else
			{
				memberType = fi.FieldType;
			}

		}
		else
		{
			PropertyInfo pi = (PropertyInfo)mi;
			if (pi.CanRead == false)
			{
				return false;
			}

			if (pi.PropertyType.IsArray)
			{
				return pi.CanWrite;
			}

			memberType = pi.PropertyType;
		}

		return java.util.List.class.IsAssignableFrom(memberType) || GetGenericListType(memberType) != null;
	}

	private static final BindingFlags _memberBindingFlags = BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.DeclaredOnly;

	private void LoadArrayMaps()
	{
		Iterable<MemberInfo> members = this.GetAllMembers();
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from mi in members where mi.IsDefined(XArrayAttribute.class, true) && IsValidCollectionMemeber(mi) select new { member = mi, attribute = (XArrayAttribute)mi.GetCustomAttributes(XArrayAttribute.class, true)[0] };

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var v : query)
		{
			XArrayMap map = new XArrayMap();
			map.setMember(v.member);
			map.setArray(v.attribute);
			map.setOrder(v.attribute.Order);

			for (XArrayItemAttribute item : map.getMember().GetCustomAttributes(XArrayItemAttribute.class, true))
			{
				map.getItems().add(item);
			}

			this.getElementMaps().add(map);
		}

	}

	public final void LoadByXAttributes(IServiceProvider context, XElement element, Object instance)
	{
		this.LoadNamespace(element, instance);
		this.LoadAttributes(element, instance);
		this.LoadChildElements(context, element, instance);
		this.LoadArrays(context, element, instance);
		this.LoadValue(element, instance);
	}

	private void LoadNamespace(XElement element, Object instance)
	{
		if (this._namespaceMap != null)
		{
			XmlNamespaceManager mngr = XHelper.CreateXmlNamespaceManager(element, true);

			this.SetValue(this._namespaceMap.getMember(), instance, mngr);
		}
	}

	private void LoadValue(XElement element, Object instance)
	{
		if (this.getValueMap() != null && ! element.getValue().equals(""))
		{
			java.lang.Class t = this.getValueMap().getMember() instanceof PropertyInfo ? ((PropertyInfo)this.ValueMap.getMember()).PropertyType : ((java.lang.reflect.Field)this.ValueMap.getMember()).FieldType;
			Object value = this.getValueMap().getConverter().ConvertTo(element.getValue(), t);
			this.SetValue(this.getValueMap().getMember(), instance, value);
		}
	}

	public final void Load(IServiceProvider context, XElement element, Object instance)
	{
		if (instance instanceof IXSerializable)
		{
			((IXSerializable)instance).Load(context, element);
		}
		else
		{
			this.LoadByXAttributes(context, element, instance);
		}
	}

	private void LoadArrays(IServiceProvider context, XElement element, Object instance)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (XArrayMap map : this.getElementMaps().Where(m => m instanceof XArrayMap))
		{
			XElement collectionElement = null;

			if (!DotNetToJavaStringHelper.isNullOrEmpty(map.getArray().getName()))
			{
				XNamespace ns = !DotNetToJavaStringHelper.isNullOrEmpty(map.getArray().getNamespaceUri()) ? map.getArray().getNamespaceUri() : element.getName().NamespaceName;
				XName name = ns + map.getArray().getName();
				collectionElement = element.Element(name);
			}
			else
			{
				collectionElement = element;
			}

			if (collectionElement != null)
			{

				java.lang.Class collectionType = map.getMember() instanceof PropertyInfo ? ((PropertyInfo)map.getMember()).PropertyType : ((java.lang.reflect.Field)map.getMember()).FieldType;
				Object list = this.GetValue(map.getMember(), instance);

				if (list!= null || collectionType.IsArray)
				{
					java.util.ArrayList tempList = new java.util.ArrayList();
					if (map.getArray().getSerializer() != null)
					{
						IXCollectionSerializer ser = (IXCollectionSerializer)Activator.CreateInstance(map.getArray().getSerializer());
						for (Object o : ser.Load(context, collectionElement))
						{
							tempList.add(o);
						}
					}
					else
					{



						for (XElement itemElement : collectionElement.Elements())
						{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
							XArrayItemAttribute ia = (from attr in map.getItems() where attr.getName() == itemElement.getName().LocalName && ((attr.NamespaceUri == itemElement.getName().NamespaceName) || ((DotNetToJavaStringHelper.isNullOrEmpty(attr.NamespaceUri) && itemElement.getName().NamespaceName == collectionElement.getName().NamespaceName))) select attr).SingleOrDefault();
							if (ia != null)
							{
								TypeConverter cvt = null;
								if (!ia.getType().IsDefined(XSerializableAttribute.class, false))
								{
									cvt = ia.getXConverter() != null ? (TypeConverter)Activator.CreateInstance(ia.getXConverter()) : XHelper.getDefault().CreateXConverter(ia.getType());
								}
								Object value = this.ValueFromElement(context, itemElement, ia.getType(), cvt);
								tempList.add(value);
							}
						}


					}

					if (collectionType.IsArray)
					{
						Array arr = Array.CreateInstance(collectionType.GetElementType(), tempList.size());
						tempList.CopyTo(arr, 0);
						this.SetValue(map.getMember(), instance, arr);
					}
					else if (java.util.List.class.IsAssignableFrom(collectionType))
					{
						for (Object o : tempList)
						{
							((java.util.List)list).Add(o);
						}
					}
					else
					{
						java.lang.Class t = GetGenericListType(collectionType);
						if (t != null)
						{
							for (Object o : tempList)
							{
								t.InvokeMember("Add", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Public, null, list, new Object[] { o });
							}
						}
					}
				}
			}
		}
	}

	private void LoadChildElements(IServiceProvider context, XElement element, Object instance)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (XElementMap map : this.getElementMaps().Where(m => m instanceof XElementMap))
		{

			XNamespace ns = !DotNetToJavaStringHelper.isNullOrEmpty(map.getElement().getNamespaceUri()) ? map.getElement().getNamespaceUri() : element.getName().NamespaceName;
			XName name = ns + map.getElement().getName();
			XElement childElement = element.Element(name);
			if (childElement != null)
			{
				Object value;
				java.lang.Class t = map.getMember() instanceof PropertyInfo ? ((PropertyInfo)map.getMember()).PropertyType : ((java.lang.reflect.Field)map.getMember()).FieldType;
				value = this.ValueFromElement(context, childElement, t, map.getConverter());
				this.SetValue(map.getMember(), instance, value);
			}
		}
	}

	private Object ValueFromElement(IServiceProvider context, XElement element, java.lang.Class targetType, TypeConverter converter)
	{
		Object value;

		if (targetType.IsDefined(XSerializableAttribute.class, false))
		{
			XTypeMap map = XHelper.getDefault().GetTypeMap(targetType);
			value = Activator.CreateInstance(targetType);
			map.Load(context, element, value);
		}
		else
		{
			value = converter.ConvertTo(element.getValue(), targetType);
		}
		return value;
	}

	private void LoadAttributes(XElement element, Object instance)
	{
		for (XAttributeMap map : this.getAttributeMaps())
		{
			String tempVar = map.getAttribute().getNamespaceUri();
			XNamespace ns = (tempVar != null) ? tempVar : "";
			XAttribute node = element.Attribute(ns + map.getAttribute().getName());
			if (node != null)
			{
				TypeConverter cvt = map.getConverter();
				java.lang.Class t = map.getMember() instanceof PropertyInfo ? ((PropertyInfo)map.getMember()).PropertyType : ((java.lang.reflect.Field)map.getMember()).FieldType;
				Object value = cvt.ConvertTo(node.getValue(), t);
				this.SetValue(map.getMember(), instance, value);
			}
		}
	}

	public final void SaveByXAttributes(IServiceProvider context, XElement element, Object instance)
	{
		this.SaveNamespace(element, instance);

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (XAttributeMap attrMap : this._attributeMaps.OrderBy(m=>m.Order))
		{
			this.SaveAttribute(element, attrMap, instance);
		}
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (XMemberMap memberMap : this._elementMaps.OrderBy(m=>m.Order))
		{
			if (memberMap instanceof XElementMap)
			{
				this.SaveElement(context, element, (XElementMap)memberMap, instance);
			}
			else
			{
				this.SaveArray(context, element, (XArrayMap)memberMap, instance);
			}
		}
		this.SaveValue(element, instance);

	}

	private void SaveNamespace(XElement element, Object instance)
	{
		if (this._namespaceMap != null)
		{
			XmlNamespaceManager mngr = (XmlNamespaceManager)this.GetValue(this._namespaceMap.getMember(), instance);
			if (mngr != null)
			{
				XHelper.AddNamespace(mngr, element);
			}
		}
	}

	private void SaveValue(XElement element, Object instance)
	{
		if (this.getValueMap() != null)
		{
			Object value = this.GetValue(this.getValueMap().getMember(), instance);

			if (value != null)
			{
				String s = (String)this.ValueMap.getConverter().ConvertFrom(value);
				element.setValue(s);
			}
		}
	}

	public final void Save(IServiceProvider context, XElement element, Object instance)
	{
		if (instance instanceof IXSerializable)
		{
			((IXSerializable)instance).Save(context, element);
		}
		else
		{
			this.SaveByXAttributes(context, element, instance);
		}
	}

	private void SaveArray(IServiceProvider context, XElement element, XArrayMap arrMap, Object instance)
	{

		Object tempVar = this.GetValue(arrMap.getMember(), instance);
		Iterable collection = (Iterable)((tempVar instanceof Iterable) ? tempVar : null);
		if (collection != null)
		{
			XElement collectionElement;
			if (!DotNetToJavaStringHelper.isNullOrEmpty(arrMap.getArray().getName()))
			{
				collectionElement = this.CreateElement(element, arrMap.getArray().getNamespaceUri(), arrMap.getArray().getName());
			}
			else
			{
				collectionElement = element;
			}

			if (arrMap.getArray().getSerializer() != null)
			{
				IXCollectionSerializer serializer = (IXCollectionSerializer)Activator.CreateInstance(arrMap.getArray().getSerializer());
				serializer.Save(context, collectionElement, collection);
			}
			else
			{
				for (Object child : collection)
				{
					if (child != null)
					{
						java.lang.Class vt = child.getClass();

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
						XArrayItemAttribute ia = (from attribute in arrMap.getItems() where attribute.Type == vt select attribute).SingleOrDefault();
						if (ia != null)
						{
							XElement childElement = this.CreateElement(collectionElement, ia.getNamespaceUri(), ia.getName());
							TypeConverter cvt = null;
							if (!vt.IsDefined(XSerializableAttribute.class, false))
							{
								cvt = ia.getXConverter() != null ? (TypeConverter)Activator.CreateInstance(ia.getXConverter()) : XHelper.getDefault().CreateXConverter(vt);
							}
							this.SaveValueToElement(context, childElement, child, cvt);
						}
						else
						{

						}
					}
				}
			}

		}
	}

	private XElement CreateElement(XElement parentElement, String namespace, String name)
	{
		XNamespace ns = DotNetToJavaStringHelper.isNullOrEmpty(namespace) ? parentElement.getName().Namespace : (XNamespace)namespace;


		XElement rs = new XElement(ns + name);


		parentElement.Add(rs);

		return rs;
	}



	private void SaveElement(IServiceProvider context, XElement element, XElementMap eleMap, Object instance)
	{
		Object value = this.GetValue(eleMap.getMember(), instance);

		if (value != null)
		{
			XElement valEle = this.CreateElement(element, eleMap.getElement().getNamespaceUri(), eleMap.getElement().getName());
			this.SaveValueToElement(context, valEle, value, eleMap.getConverter());
		}
	}

	private void SaveValueToElement(IServiceProvider context, XElement element, Object value, TypeConverter converter)
	{

		java.lang.Class vt = value.getClass();
		if (vt.IsDefined(XSerializableAttribute.class, false))
		{
			XTypeMap map = XHelper.getDefault().GetTypeMap(vt);
			map.Save(context, element, value);
		}
		else
		{
			String s = (String)converter.ConvertFrom(value);
			element.setValue(s);
		}

	}


	private Object GetValue(MemberInfo mi, Object instance)
	{
		if (mi instanceof PropertyInfo)
		{
			return ((PropertyInfo)mi).GetValue(instance, null);
		}
		else
		{
			return ((java.lang.reflect.Field)mi).GetValue(instance);
		}
	}

	private void SetValue(MemberInfo mi, Object instance, Object value)
	{
		if (mi instanceof PropertyInfo)
		{
			((PropertyInfo)mi).SetValue(instance, value, null);
		}
		else
		{
			((java.lang.reflect.Field)mi).SetValue(instance, value);
		}
	}


	private void SaveAttribute(XElement element, XAttributeMap attrMap, Object instance)
	{
		Object value = this.GetValue(attrMap.getMember(), instance);

		if (value != null)
		{
			String s = (String)attrMap.getConverter().ConvertFrom(value);

			String ns = attrMap.getAttribute().getNamespaceUri();

			XAttribute node = new XAttribute(ns + attrMap.getAttribute().getName(), s);
			element.Add(node);
		}
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

	private XSerializableAttribute privateSerializableAttribute;
	public final XSerializableAttribute getSerializableAttribute()
	{
		return privateSerializableAttribute;
	}
	private void setSerializableAttribute(XSerializableAttribute value)
	{
		privateSerializableAttribute = value;
	}

	private java.util.ArrayList<XMemberMap> _elementMaps = new java.util.ArrayList<XMemberMap>();
	public final java.util.List<XMemberMap> getElementMaps()
	{
		return _elementMaps;
	}

	private java.util.ArrayList<XAttributeMap> _attributeMaps = new java.util.ArrayList<XAttributeMap>();
	public final java.util.List<XAttributeMap> getAttributeMaps()
	{
		return _attributeMaps;
	}

	private XValueMap privateValueMap;
	public final XValueMap getValueMap()
	{
		return privateValueMap;
	}
	private void setValueMap(XValueMap value)
	{
		privateValueMap = value;
	}


	public java.util.Map<String, XMemberMap> _elementQNameMaps = new java.util.HashMap<String, XMemberMap>();

	public final java.util.Map<String, XMemberMap> getElementQNameMaps()
	{
		return _elementQNameMaps;
	}

	public java.util.Map<String, XAttributeMap> _attributeQNameMaps = new java.util.HashMap<String, XAttributeMap>();

	public final java.util.Map<String, XAttributeMap> getAttributeQNameMaps()
	{
		return _attributeQNameMaps;
	}

}