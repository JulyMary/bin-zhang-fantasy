package fantasy.configuration;

import java.io.*;

import org.jdom2.*;
import org.jdom2.filter.*;
import org.jdom2.xpath.*;
import java.util.*;

import fantasy.*;

import fantasy.io.Path;
import fantasy.xserialization.*;

public final class SettingStorage
{

	private static Element _appSettingsRoot;
	private static Element _userSettingsRoot;

	private static Namespace _namespace = Namespace.getNamespace(Consts.SettingsNameSpace);

	
	private static String _appSettingsLocation;
	
	public static String getAppSettingsLocation()
	{
		return _appSettingsLocation;
	}
	
	public static void setAppSettingsLocation(String value)
	{
		_appSettingsLocation = value;
	}

	private static String _userSettingslocation = null;
	
    private static void loadAppSettingsRoot() throws Exception
    {
    	if(_appSettingsRoot == null)
    	{
    		_appSettingsRoot = JDomUtils.loadElement(_appSettingsLocation);
    		
    		
    	}
    }
    
    private static void loadUserSettingsRoot() throws Exception
    {
    	if(_userSettingsRoot == null)
    	{
    		_userSettingslocation = _appSettingsRoot.getAttributeValue("userSettingsPath"); 
    		
    		if(StringUtils2.isNullOrEmpty(_userSettingslocation))
    		{
    			_userSettingslocation = Path.changeExtension(_appSettingsLocation, "user.xsettings");
    		}
    		
    		File f = new File (_userSettingslocation);
    		
    		if(f.exists())
    		{
    			_userSettingsRoot = JDomUtils.loadElement(_userSettingslocation);
    		}
    		else
    		{
    			_userSettingsRoot = new Element("settingsFile", _namespace);
    		
    			
    		}
    		
    	}
    }

	public static SettingsBase load(SettingsBase data) throws Exception
	{
		loadAppSettingsRoot();
		loadUserSettingsRoot();
			
		
		HashMap<String, Object> vars = new HashMap<String, Object>();
	    vars.put("class",  data.getClass().getName());
			XPathExpression<Element> xpath = XPathFactory.instance().compile("s:settings[@type=\"$class\"]", new ElementFilter(), vars, Namespace.getNamespace("s", Consts.SettingsNameSpace));
		Element appSettings = xpath.evaluateFirst(_appSettingsRoot);
		
		XSerializer ser = new XSerializer(data.getClass());
		if(appSettings != null)
		{
			ser.deserialize(appSettings, data);
		}
			
		Element userSettings = xpath.evaluateFirst(_userSettingsRoot);
		
		if(userSettings != null)
		{
			ser.deserialize(userSettings, data);
		}
			
			
		return data;
	}

	public static void save(SettingsBase data) throws Exception
	{
		
		HashMap<String, Object> vars = new HashMap<String, Object>();
	    vars.put("class",  data.getClass().getName());
			XPathExpression<Element> xpath = XPathFactory.instance().compile("s:settings[@type=\"$class\"]", new ElementFilter(), vars, Namespace.getNamespace("s", Consts.SettingsNameSpace));
		XSerializer ser = new XSerializer(data.getClass());
		
		
		Element newSettings = new Element("settings", _namespace);
		newSettings.setAttribute("type",data.getClass().getName());
		
		ser.serialize(newSettings, data);
		
		Element oldSettings = xpath.evaluateFirst(_userSettingsRoot);
		

		if (oldSettings != null)
		{
			int index = _userSettingsRoot.indexOf(oldSettings);
			oldSettings.detach();
			_userSettingsRoot.addContent(index, newSettings);
		}
		else
		{
			_userSettingsRoot.addContent(newSettings);
		}

		
		JDomUtils.saveElement(_userSettingsRoot, _userSettingslocation);
		

	}
}