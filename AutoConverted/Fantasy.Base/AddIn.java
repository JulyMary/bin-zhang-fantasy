package Fantasy;

public class AddIn implements IConfigurationSectionHandler
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IConfigurationSectionHandler Members

	public final Object Create(Object parent, Object configContext, System.Xml.XmlNode section)
	{
		return section;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


	public static <T> T[] CreateObjects(String xpath)
	{
		Object[] temp = CreateObjects(xpath);
		T[] rs = new T[temp.length];
		temp.CopyTo(rs, 0);
		return rs;
	}

	public static java.lang.Class[] GetTypes(String xpath)
	{

		XmlElement root = (XmlElement)ConfigurationManager.GetSection("addin");

		xpath += "/@type";


		java.util.ArrayList<java.lang.Class> rs = new java.util.ArrayList<java.lang.Class>();

		for(XmlNode node : root.SelectNodes(xpath))
		{
			rs.add(java.lang.Class.forName(node.getValue(), true));
		}
		return rs.toArray(new java.lang.Class[]{});
	}

	public static Object[] CreateObjects(String xpath)
	{
		java.lang.Class[] types = GetTypes(xpath);
		Object[] rs;
		if (types != null)
		{
			rs = new Object[types.length];
			for (int i = 0; i < rs.length; i++)
			{
				rs[i] = Activator.CreateInstance(types[i]);
			}
		}
		else
		{
			rs = new Object[0];
		}

		return rs;
	}
}