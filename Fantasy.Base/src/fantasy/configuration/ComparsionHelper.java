package Fantasy.Configuration;

public final class ComparsionHelper
{
	public static boolean DeepEquals(Object a, Object b)
	{
		if (a != null && b != null)
		{
			if (a.getClass() == b.getClass())
			{
				return CompareProperties(a, b);
			}
			else
			{
				return false;
			}
		}
		else if (a == null && b == null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private static boolean CompareProperties(Object a, Object b)
	{
		java.lang.Class t = a.getClass();
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from prop in t.GetProperties() where prop.GetIndexParameters().getLength() == 0 && ! prop.IsDefined(XmlIgnoreAttribute.class, true) && (prop.CanWrite || java.util.Collection.class.IsAssignableFrom(prop.PropertyType)) select prop;
		for (PropertyInfo prop : query)
		{
			Object v1 = prop.GetValue(a, null);
			Object v2 = prop.GetValue(b, null);
			if (java.util.Collection.class.IsAssignableFrom(prop.PropertyType))
			{
				if (!CollectionEquals((java.util.Collection)v1, (java.util.Collection)v2))
				{
					return false;
				}
			}
			else
			{
				if (!v1.equals(v2))
				{
					return false;
				}
			}
		}

		for (java.lang.reflect.Field field : t.getFields())
		{
			if (!field.IsDefined(XmlIgnoreAttribute.class, true))
			{
				Object v1 = field.GetValue(a);
				Object v2 = field.GetValue(b);
				if (java.util.Collection.class.IsAssignableFrom(field.FieldType))
				{
					if (!CollectionEquals((java.util.Collection)v1, (java.util.Collection)v2))
					{
						return false;
					}
				}
				else
				{
					if (!v1.equals(v2))
					{
						return false;
					}
				}
			}
		}

		return true;

	}

	private static boolean CollectionEquals(java.util.Collection a, java.util.Collection b)
	{
		if (a.getCount() != b.getCount())
		{
			return false;
		}

		java.util.Iterator e1 = a.iterator();
		java.util.Iterator e2 = b.iterator();
		while (e1.hasNext() && e2.hasNext())
		{
			if (!e1.next().equals(e2.next()))
			{
				return false;
			}
		}
		return true;
	}
}