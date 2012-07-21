package fantasy;

import java.lang.reflect.*;
import java.util.*;


import org.jdom2.*;

import org.jdom2.filter.*;

import org.jdom2.xpath.*;

import fantasy.configuration.*;
import fantasy.xserialization.*;

@SuppressWarnings({"rawtypes", "unchecked"})
@XSerializable(name = "addIn", namespaceUri = Consts.AddInNameSpace)
public final class AddIn extends SettingsBase implements IXSerializable {

	private static AddIn _instance;
	
	private AddIn()
	{
		
	}
	
	
	
	private Element _element;

	@Override
	public void Load(IServiceProvider context, Element element) {
		this._element = element;
		if(this._element == null)
		{
			this._element = new Element("addIn", Namespace.getNamespace(Consts.AddInNameSpace));
		}
		
	}

	@Override
	public void Save(IServiceProvider context, Element element) throws Exception {
		throw new NotImplementedException();
		
	}

	private static AddIn getInstance() throws Exception {
		if(_instance == null)
		{
			_instance = (AddIn)SettingStorage.load(new AddIn());
		} 
		return _instance;
	}
	
	
	public static <T> T[] CreateObjects(Class<T> type, String xpath) throws Exception
	{
		Object[] temp = CreateObjects(xpath);
		
		T[] rs = (T[]) Array.newInstance(type, temp.length);
		for(int i = 0; i < temp.length; i ++)
		{
			Array.set(rs, i, temp[i]);
		}
		return rs;
	}

	public static java.lang.Class[] GetTypes(String path) throws Exception
	{

		Element root = getInstance()._element;
		
		String[] segs = path.split("/");
		
		StringBuilder xpath = new StringBuilder(path);
		for(String seg : segs)
		{
			if(xpath.length() > 0)
			{
				xpath.append("/");
			}
			xpath.append("a:" + seg);
			
		}

		xpath.append("/@type");

		
		XPathExpression<Attribute> expression = XPathFactory.instance().compile(xpath.toString(), new AttributeFilter(), null, Namespace.getNamespace("a", Consts.AddInNameSpace));

		

		ArrayList<java.lang.Class> rs = new java.util.ArrayList<java.lang.Class>();

		for(Attribute node :  expression.evaluate(root))
		{
			rs.add(java.lang.Class.forName(node.getValue()));
		}
		return rs.toArray(new java.lang.Class[0]);
	}

	public static Object[] CreateObjects(String xpath) throws Exception
	{
		
		java.lang.Class[] types = GetTypes(xpath);
		Object[] rs;
		if (types != null)
		{
			rs = new Object[types.length];
			for (int i = 0; i < rs.length; i++)
			{
				rs[i] = types[i].newInstance();
			}
		}
		else
		{
			rs = new Object[0];
		}

		return rs;
	}

	
}
