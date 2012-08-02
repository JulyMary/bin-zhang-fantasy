package fantasy.xserialization;

import java.util.*;
import java.lang.reflect.*;

import fantasy.*;
import fantasy.collections.*;

import org.jdom2.*;

@SuppressWarnings({"rawtypes", "unchecked"})
public class XTypeMap
{
	public XTypeMap(java.lang.Class t) throws Exception
	{
		this.setTargetType(t);
		this._xSerializableAnnotation = (XSerializable) t.getAnnotation(XSerializable.class);
		this.LoadNamespaceMap();
		this.LoadAttributeMaps();
		this.LoadElementMaps();
		this.LoadArrayMaps();
		this.LoadValueMap();

		Collections.sort(this._attributeMaps, new Comparator<XAttributeMap>(){
			public int compare(XAttributeMap x, XAttributeMap y) {
				if(x.getOrder() > y.getOrder())
				{
					return 1;
				}
				else if(x.getOrder() == y.getOrder())
				{
					return 0;

				}
				else
				{
					return -1;
				}
			}});

		Collections.sort(this._elementMaps, new Comparator<XMemberMap>(){
			public int compare(XMemberMap x, XMemberMap y)
			{
				if(x.getOrder() > y.getOrder())
				{
					return 1;
				}
				else if(x.getOrder() == y.getOrder())
				{
					return 0;

				}
				else
				{
					return -1;
				}
			}
		});



	}




	private void LoadNamespaceMap()
	{

		Field f = new Enumerable<Field>(this.GetAllMembers()).singleOrDefault(new Predicate<Field>(){

			@Override
			public boolean evaluate(Field arg0) {

				return arg0.isAnnotationPresent(XNamespace.class);
			}});

		if(f != null)
		{
			this._namespaceMap = new XNamespaceMap();
			this._namespaceMap.setMember(f);
		}


	}

	private XNamespaceMap _namespaceMap = null;

	private void LoadValueMap() throws Exception
	{

		Field f = new Enumerable<Field>(this.GetAllMembers()).singleOrDefault(new Predicate<Field>(){

			@Override
			public boolean evaluate(Field arg0) {

				return arg0.isAnnotationPresent(XValue.class);
			}});

		if(f != null)
		{
			XValue anno = f.getAnnotation(XValue.class);
			this._valueMap = new XValueMap();
			this._valueMap.setMember(f);
			this._valueMap.setValue(anno);

			if(anno.converter() != Dummy.class)
			{
				this._valueMap.setConverter((ITypeConverter)anno.converter().newInstance());
			}
			else

			{
				this._valueMap.setConverter(XHelper.getDefault().CreateXConverter(f.getType()));
			}

		}



	}


	private List<Field> _allMembers = null;

	private List<Field> GetAllMembers()
	{
		if(_allMembers == null)
		{
			_allMembers = new ArrayList<Field>();
			Class clz = this.getTargetType();
			while(clz != Object.class)
			{
				for(Field f : clz.getDeclaredFields())
				{
					_allMembers.add(f);
				}
                clz = clz.getSuperclass();

			}


		}
		return _allMembers;

	}

	private String GetQualifiedName(String ns, String name)
	{
		return ns + "\n" + name;
	}

	private void LoadAttributeMaps() throws Exception
	{


		for (Field f : this.GetAllMembers())
		{

			if(f.isAnnotationPresent(XAttribute.class))
			{
				XAttributeMap map = new XAttributeMap();
				XAttribute anno = f.getAnnotation(XAttribute.class);
				map.setMember(f);
				map.setAttribute(anno);
				map.setOrder(anno.order());
				Class ft = f.getType();
				if(!f.isAnnotationPresent(XSerializable.class))
				{
					if(anno.converter() != Dummy.class)
					{
						map.setConverter((ITypeConverter)anno.converter().newInstance());
					}
					else
					{
						map.setConverter(XHelper.getDefault().CreateXConverter(ft));
					}
				}


				this.getAttributeMaps().add(map);
				this.getAttributeQNameMaps().put(this.GetQualifiedName(anno.namespaceUri(), anno.name()), map);
			}
		}

	}


	private void LoadElementMaps() throws Exception
	{


		Enumerable<Field> fields = new Enumerable<Field>(this.GetAllMembers()).where(new Predicate<Field>(){

			@Override
			public boolean evaluate(Field arg0) {

				return arg0.isAnnotationPresent(XElement.class);
			}}	);

		for (Field f : fields)
		{
			XElementMap map = new XElementMap();
			XElement anno = f.getAnnotation(XElement.class);
			map.setMember(f);
			map.setElement(anno);
			map.setOrder(anno.order());
			Class ft = f.getType();

			if (!ft.isAnnotationPresent(XSerializable.class))
			{
				if(anno.converter() != Dummy.class)
				{
					map.setConverter((ITypeConverter)anno.converter().newInstance());
				}
				else
				{
					map.setConverter(XHelper.getDefault().CreateXConverter(ft));
				}

			}
			this.getElementMaps().add(map);
		}
	}



	private void LoadArrayMaps()
	{


        try
        {
		Enumerable<Field> fields = new Enumerable<Field>(this.GetAllMembers()).where(new Predicate<Field>(){

			@Override
			public boolean evaluate(Field arg0) {

				return ((Field)arg0).isAnnotationPresent(XArray.class);
			}}	);
		for (Field f : fields)
		{
			XArrayMap map = new XArrayMap();
			XArray anno = f.getAnnotation(XArray.class);
			map.setMember(f);
			map.setArray(anno);
			map.setOrder(anno.order());
			this.getElementMaps().add(map);
		}
        }
        catch(Exception error)
        {
        	
        }

	}

	public final void LoadByXAttributes(IServiceProvider context, Element element, Object instance) throws Exception
	{
		this.LoadNamespace(context, element, instance);
		this.LoadAttributes(context, element, instance);
		this.LoadChildElements(context, element, instance);
		this.LoadArrays(context, element, instance);
		this.LoadValue(context, element, instance);
	}

	private void LoadNamespace(IServiceProvider context,Element element, Object instance) throws Exception
	{
		if (this._namespaceMap != null)
		{
			IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;

			if(filter == null || filter.filter(this._namespaceMap.getMember()) )
			{

				List<Namespace> mngr = element.getNamespacesInScope();


				this.SetValue(this._namespaceMap.getMember(), instance, mngr.toArray(new Namespace[0]));
			}
		}
	}

	private void LoadValue(IServiceProvider context,Element element, Object instance) throws Exception
	{
		if (this._valueMap != null && ! element.getValue().equals(""))
		{
			IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;

			if(filter == null || filter.filter(this._valueMap.getMember()) )
			{
				java.lang.Class t = this._valueMap.getMember().getType();
				Object value = this._valueMap.getConverter().convertTo(element.getValue(), t);
				this.SetValue(this._valueMap.getMember(), instance, value);
			}
		}
	}


	public final void Load(IServiceProvider context, Element element, Object instance) throws Exception
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


	private void LoadArrays(IServiceProvider context, Element element, Object instance) throws Exception
	{

		IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;


		Enumerable<XArrayMap> maps = new Enumerable<XMemberMap>(this.getElementMaps()).ofType(XArrayMap.class);

		for (XArrayMap map : maps)
		{
			if(filter != null && ! filter.filter(map.getMember()) )
			{
				continue;
			}

			Element collectionElement = null;

			if (!StringUtils2.isNullOrEmpty(map.getArray().name()))
			{
				Namespace ns = !StringUtils2.isNullOrEmpty(map.getArray().namespaceUri()) ? Namespace.getNamespace(map.getArray().namespaceUri()) : element.getNamespace();

				collectionElement = element.getChild(map.getArray().name(), ns);
			}
			else
			{
				collectionElement = element;
			}

			if (collectionElement != null)
			{


				java.lang.Class collectionType = map.getMember().getType();
				Object list = this.GetValue(map.getMember(), instance);

				if (list!= null || collectionType.isArray())
				{
					java.util.ArrayList tempList = new java.util.ArrayList();
					if (map.getArray().serializer() != Dummy.class)
					{
						IXCollectionSerializer ser = (IXCollectionSerializer)map.getArray().serializer().newInstance();
						for (Object o : ser.Load(context, collectionElement))
						{
							tempList.add(o);
						}
					}
					else
					{

						for (Element itemElement : collectionElement.getChildren())
						{

							for(XArrayItem ia : map.getArray().items())
							{
								if(ia.name() == itemElement.getName() 
										&& (ia.namespaceUri() == itemElement.getNamespaceURI() || (StringUtils2.isNullOrEmpty(ia.namespaceUri()) && itemElement.getNamespaceURI() == collectionElement.getNamespaceURI())))
								{
									ITypeConverter cvt = null;

									if (!ia.type().isAnnotationPresent(XSerializable.class))
									{
										cvt = ia.converter() != null ? (ITypeConverter)ia.converter().newInstance() : XHelper.getDefault().CreateXConverter(ia.type());
									}
									Object value = this.ValueFromElement(context, itemElement, ia.type(), cvt);
									tempList.add(value);
									break;
								}
							}
						}


					}

					if (collectionType.isArray())
					{
						Object arr = Array.newInstance(collectionType.getComponentType(), tempList.size());
						for(int i = 0; i < tempList.size(); i ++)
						{
							Array.set(arr, i, tempList.get(i));
						}
						this.SetValue(map.getMember(), instance, arr);
					}
					else if (java.util.List.class.isAssignableFrom(collectionType))
					{
						if(map.getArray().clear())
						{
							((java.util.List)list).clear();
						}
						for (Object o : tempList)
						{
							((java.util.List)list).add(o);
						}
					}

				}
			}
		}
	}

	private void LoadChildElements(IServiceProvider context, Element element, Object instance) throws Exception
	{
		IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;


		Enumerable<XElementMap> maps = new Enumerable<XMemberMap>(this.getElementMaps()).ofType(XElementMap.class);
		for (XElementMap map : maps)
		{

			if(filter == null || filter.filter(map.getMember()) )
			{
				Namespace ns = !StringUtils2.isNullOrEmpty(map.getElement().namespaceUri()) ? Namespace.getNamespace(map.getElement().namespaceUri()) : element.getNamespace();

				Element childElement = element.getChild(map.getElement().name(), ns);
				if (childElement != null)
				{
					Object value;
					java.lang.Class t = map.getMember().getType();
					value = this.ValueFromElement(context, childElement, t, map.getConverter());
					this.SetValue(map.getMember(), instance, value);
				}
			}
		}
	}

	private Object ValueFromElement(IServiceProvider context, Element element, java.lang.Class targetType, ITypeConverter converter) throws Exception
	{
		Object value;

		if(targetType == Element.class)
		{
			return element.clone();
		}

		else if (targetType.isAnnotationPresent(XSerializer.class))
		{
			XTypeMap map = XHelper.getDefault().GetTypeMap(targetType);
			value = targetType.newInstance();
			map.Load(context, element, value);
		}
		else
		{
			value = converter.convertTo(element.getValue(), targetType);
		}
		return value;
	}

	private void LoadAttributes(IServiceProvider context,Element element, Object instance) throws Exception
	{

		IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;



		for (XAttributeMap map : this.getAttributeMaps())
		{
			if(filter == null || filter.filter(map.getMember()) )
			{
				String tempVar = map.getAttribute().namespaceUri();
				Namespace ns = Namespace.getNamespace((tempVar != null) ? tempVar : "");
				Attribute node = element.getAttribute(map.getAttribute().name(), ns);
				if (node != null)
				{
					ITypeConverter cvt = map.getConverter();
					java.lang.Class t = map.getMember().getType();
					Object value = cvt.convertTo(node.getValue(), t);
					this.SetValue(map.getMember(), instance, value);
				}
			}
		}
	}

	public final void SaveByXAttributes(IServiceProvider context, Element element, Object instance) throws Exception
	{
		IFieldFilter filter = context != null ? (IFieldFilter)context.getService2(IFieldFilter.class) : null;

		if(this._namespaceMap != null)
		{
			if(filter == null || filter.filter(_namespaceMap.getMember()))
			{
				this.SaveNamespace(element, instance);
			}
		}

		for (XAttributeMap attrMap : this._attributeMaps)
		{

			if(filter == null || filter.filter(attrMap.getMember()))
			{
				this.SaveAttribute(element, attrMap, instance);
			}


		}


		for (XMemberMap memberMap : this._elementMaps)
		{

			if(filter == null || filter.filter(memberMap.getMember()))
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
		}

		if(this._valueMap != null )
		{
			if(filter == null || filter.filter(_valueMap.getMember()))
			{
				this.SaveValue(element, instance);
			}
		}

	}

	private void SaveNamespace(Element element, Object instance) throws Exception
	{
		if (this._namespaceMap != null)
		{
			Namespace[] namespaces = (Namespace[])this.GetValue(this._namespaceMap.getMember(), instance);
			if (namespaces != null)
			{
				XHelper.AddNamespace(namespaces, element);
			}
		}
	}

	private void SaveValue(Element element, Object instance) throws Exception
	{
		if (this._valueMap!= null)
		{
			Object value = this.GetValue(this._valueMap.getMember(), instance);

			if (value != null)
			{
				String s = (String)this._valueMap.getConverter().convertFrom(value);
				element.setText(s);
			}
		}
	}

	public final void Save(IServiceProvider context, Element element, Object instance) throws Exception
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

	private void SaveArray(IServiceProvider context, Element element, XArrayMap arrMap, Object instance) throws Exception
	{

		Object tempVar = this.GetValue(arrMap.getMember(), instance);
		
		if(tempVar == null)
		{
			return;
		}
		
		Iterable collection = null;
		if (tempVar instanceof Iterable)
		{
			collection = (Iterable)tempVar;
		}
		else if(tempVar.getClass().isArray())
		{
			collection = Arrays.asList(tempVar);
		}
		if (collection != null)
		{
			Element collectionElement;
			if (!StringUtils2.isNullOrEmpty(arrMap.getArray().name()))
			{
				collectionElement = this.CreateElement(element, arrMap.getArray().namespaceUri(), arrMap.getArray().name());
			}
			else
			{
				collectionElement = element;
			}

			if (arrMap.getArray().serializer() != Dummy.class)
			{
				IXCollectionSerializer serializer = (IXCollectionSerializer)arrMap.getArray().serializer().newInstance();
				serializer.Save(context, collectionElement, collection);
			}
			else
			{
				for (Object child : collection)
				{
					if (child != null)
					{
						java.lang.Class vt = child.getClass();

						for(XArrayItem ia : arrMap.getArray().items())
						{

							if (ia.type().isInstance(child))
							{
								Element childElement = this.CreateElement(collectionElement, ia.namespaceUri(), ia.name());
								ITypeConverter cvt = null;
								if (!vt.isAnnotationPresent(XSerializable.class))
								{
									cvt = ia.converter() != Dummy.class ? (ITypeConverter)ia.converter().newInstance() : XHelper.getDefault().CreateXConverter(vt);
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
	}

	private Element CreateElement(Element parentElement, String namespace, String name)
	{
		Namespace ns = StringUtils2.isNullOrEmpty(namespace) ? parentElement.getNamespace() : Namespace.getNamespace(namespace);


		Element rs = new Element(name, ns);


		parentElement.addContent(rs);

		return rs;
	}



	private void SaveElement(IServiceProvider context, Element element, XElementMap eleMap, Object instance) throws Exception
	{
		Object value = this.GetValue(eleMap.getMember(), instance);



		if (value != null)
		{
			if(eleMap.getMember().getType() == Element.class)
			{
				Element valEle = (Element)value;
				element.addContent(valEle.clone());
			}
			else
			{

				Element valEle = this.CreateElement(element, eleMap.getElement().namespaceUri(), eleMap.getElement().name());
				this.SaveValueToElement(context, valEle, value, eleMap.getConverter());
			}
		}
	}

	private void SaveValueToElement(IServiceProvider context, Element element, Object value, ITypeConverter converter) throws Exception
	{

		java.lang.Class vt = value.getClass();
		if (vt.isAnnotationPresent(XSerializable.class))
		{
			XTypeMap map = XHelper.getDefault().GetTypeMap(vt);
			map.Save(context, element, value);
		}
		else
		{
			String s = (String)converter.convertFrom(value);
			element.setText(s);

		}

	}


	private Object GetValue(Field fi, Object instance) throws Exception
	{

		
		return fi.get(instance);


	}

	private void SetValue(Field fi, Object instance, Object value) throws Exception
	{
		((java.lang.reflect.Field)fi).set(instance, value);
	}


	private void SaveAttribute(Element element, XAttributeMap attrMap, Object instance) throws Exception
	{
		Object value = this.GetValue(attrMap.getMember(), instance);

		if (value != null)
		{
			String s = (String)attrMap.getConverter().convertFrom(value);

			String ns = attrMap.getAttribute().namespaceUri();



			Attribute node = new Attribute(attrMap.getAttribute().name(), s, Namespace.getNamespace(ns));
			element.setAttribute(node);

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

	private XSerializable _xSerializableAnnotation;

	public XSerializable getXSerializableAnnotation()
	{
		return this._xSerializableAnnotation;
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

	private XValueMap _valueMap;



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