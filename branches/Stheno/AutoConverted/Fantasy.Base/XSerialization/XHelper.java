package Fantasy.XSerialization;

import Fantasy.Properties.*;

public class XHelper
{
	private XHelper()
	{
		synchronized (_syncRoot)
		{
			for (Assembly asm : AppDomain.CurrentDomain.GetAssemblies())
			{
				this.LoadXConverters(asm);
			}
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
		}
	}

	private void CurrentDomain_AssemblyLoad(Object sender, AssemblyLoadEventArgs args)
	{
		synchronized (_syncRoot)
		{
			this.LoadXConverters(args.LoadedAssembly);
		}
	}

	private void LoadXConverters(Assembly assembly)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from type in assembly.GetTypes() where type.IsDefined(XConverterAttribute.class, false) select type;
		for (java.lang.Class t : query)
		{
			XConverterAttribute attr = (XConverterAttribute)t.GetCustomAttributes(XConverterAttribute.class, false)[0];
			this._converters.put(attr.getTargetType(), t);
		}
	}

	private Object _syncRoot = new Object();


	private void LoadXPrefixes(Assembly assembly)
	{
		for(XPrefixAttribute attribute : assembly.GetCustomAttributes(XPrefixAttribute.class, true))
		{
			this._prefixes.put(attribute.getNamesapce(), attribute.getPrefix());
		}
	}

	private java.util.HashMap<String, String> _prefixes = new java.util.HashMap<String, String>();

	private java.util.HashMap<java.lang.Class, XTypeMap> _maps = new java.util.HashMap<java.lang.Class, XTypeMap>();
	private java.util.HashMap<java.lang.Class, java.lang.Class> _converters = new java.util.HashMap<java.lang.Class, java.lang.Class>();


	public final XTypeMap GetTypeMap(java.lang.Class t)
	{
		synchronized (_syncRoot)
		{
			XTypeMap rs = null;
			if ((rs = this._maps.get(t)) != null)
			{
				return rs;

			}

			else if (t.IsDefined(XSerializableAttribute.class, false))
			{
				rs = new XTypeMap(t);
				this._maps.put(t, rs);
				return rs;
			}
			else
			{

				throw new XException(String.format(Resources.getTypeIsNotXSerializableText(), t));
			}
		}
	}


	public final void LoadByXAttributes(IServiceProvider context, XElement element, Object instance)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		XTypeMap map = this.GetTypeMap(instance.getClass());
		map.LoadByXAttributes(context, element, instance);
	}

	public final void SaveByXAttributes(IServiceProvider context, XElement element, Object instance)
	{
		if (element == null)
		{
			throw new ArgumentNullException("element");
		}
		if (instance == null)
		{
			throw new ArgumentNullException("instance");
		}
		XTypeMap map = this.GetTypeMap(instance.getClass());
		map.SaveByXAttributes(context, element, instance);
	}


	private boolean IsNullableType(java.lang.Class theType)
	{
		return (theType.IsGenericType && theType.GetGenericTypeDefinition().equals(.class));
	}

	private NullableConverter _nullableConvert = new NullableConverter();

	public final TypeConverter CreateXConverter(java.lang.Class t)
	{
		if (IsNullableType(t))
		{
			return _nullableConvert;
		}

		java.lang.Class converterType = null;
		do
		{
			converterType = this._converters.get(t);
			t = t.getSuperclass();
		} while (converterType == null);

		return (TypeConverter)Activator.CreateInstance(converterType);
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

	public static XmlNamespaceManager CreateXmlNamespaceManager(XElement element, boolean includeAncestor)
	{
		XmlNamespaceManager rs = new XmlNamespaceManager(new NameTable());
		java.util.ArrayList<String> added = new java.util.ArrayList<String>();
		do
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			for (XAttribute attr : element.Attributes().Where(a => a.IsNamespaceDeclaration))
			{

				String prefix = null;

				if (attr.getName().LocalName.equals("xmlns") && attr.getName().NamespaceName.equals(""))
				{
					prefix = "";

				}
				else if (attr.getName().Namespace == XNamespace.Xmlns)
				{
					prefix = attr.getName().LocalName;
				}

				if (prefix != null && added.indexOf(prefix) < 0)
				{
					added.add(prefix);
					rs.AddNamespace(prefix, attr.getValue());
				}

			}

			element = element.Parent;
		} while (element != null && includeAncestor);
		return rs;
	}

	public static void AddNamespace(XmlNamespaceManager mngr, XElement element)
	{
		for (String prefix : mngr)
		{
			XNamespace ns = mngr.LookupNamespace(prefix);
			if (prefix.equals(""))
			{
				element.Add(new XAttribute("xmlns", ns));
			}

			else if (ns != XNamespace.Xmlns && ns != XNamespace.Xml)
			{
				element.Add(new XAttribute(XNamespace.Xmlns + prefix, mngr.LookupNamespace(prefix)));
			}
		}
	}

}