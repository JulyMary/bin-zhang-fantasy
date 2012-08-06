package fantasy.xserialization;
import java.util.*;


import org.jdom2.*;
import org.joda.time.*;

import fantasy.IServiceProvider;
import fantasy.ITypeConverter;
@SuppressWarnings("rawtypes")
public class XHelper
{
	
	private java.util.HashMap<java.lang.Class, XTypeMap> _maps = new java.util.HashMap<java.lang.Class, XTypeMap>();
	private java.util.ArrayList<Type2Converter> _converters = new java.util.ArrayList<Type2Converter>();
	private PrimitiveConverter _defaultConverter = new PrimitiveConverter();
	private XArrayConverter _arrayConverter = new XArrayConverter();


	private Object _syncRoot = new Object();
	

	private XHelper()
	{
		
		this._converters.add(new Type2Converter(Date.class, new XDateTimeConverter()));
		this._converters.add(new Type2Converter(UUID.class, new XUUIDConverter()));
		
		this._converters.add(new Type2Converter(Interval.class, new DurationConverter()));
		

	}
	
	
	@SuppressWarnings("unchecked")
	public final XTypeMap GetTypeMap(Class t) throws Exception
	{
		synchronized (_syncRoot)
		{
			XTypeMap rs = null;
			if ((rs = this._maps.get(t)) != null)
			{
				return rs;

			}
			

			else if (t.isAnnotationPresent(XSerializable.class))
			{
				rs = new XTypeMap(t);
				this._maps.put(t, rs);
				return rs;
			}
			else
			{

				throw new XException(String.format("Type %1s is not marked as XSerializable.", t.getName() ));
			}
		}
	}


	public final void LoadByXAttributes(IServiceProvider context, Element element, Object instance) throws Exception
	{
		if (element == null)
		{
			throw new IllegalArgumentException("element");
		}
		if (instance == null)
		{
			throw new IllegalArgumentException("instance");
		}
		XTypeMap map = this.GetTypeMap(instance.getClass());
		map.LoadByXAttributes(context, element, instance);
	}

	public final void SaveByXAttributes(IServiceProvider context, Element element, Object instance) throws Exception
	{
		if (element == null)
		{
			throw new IllegalArgumentException("element");
		}
		if (instance == null)
		{
			throw new IllegalArgumentException("instance");
		}
		XTypeMap map = this.GetTypeMap(instance.getClass());
		map.SaveByXAttributes(context, element, instance);
	}


	
	
	
	private class Type2Converter 
	{
		
		public Type2Converter(Class t, ITypeConverter c)
		{
			this.Type = t;
			this.Converter = c;
		}
		
		Class Type;
		ITypeConverter Converter;
	}

	public final ITypeConverter CreateXConverter(java.lang.Class t)
	{

		ITypeConverter rs = null;
		java.lang.Class t2 = t;
		do
		{
			
			for(Type2Converter tc : _converters)
			{
				if(t2 == tc.Type)
				{
					rs = tc.Converter;
					break;
				}
			}
			t2 = t2.getSuperclass();
		} while (rs == null && t2 != null);
		
		if(rs == null)
		{
			if(t.isArray())
			{
				rs =  this._arrayConverter;
			}
			else
			{
				rs = this._defaultConverter;
			}
		}

		return rs;
	}

	private static XHelper _default = null;

	public static XHelper getDefault()
	{
		if (_default == null)
		{
			_default = new XHelper();
		}
		return _default;
	}

	public static List<Namespace> CreateXmlNamespaceManager(Element element, boolean includeAncestor)
	{
		return includeAncestor ? element.getNamespacesInScope() : element.getNamespacesIntroduced();
	}

	public static void AddNamespace(Namespace[] namespaces, Element element)
	{
		for (Namespace ns  : namespaces)
		{
			element.addNamespaceDeclaration(ns);
		}
	}

}