package fantasy.configuration;

import java.io.*;
import java.lang.reflect.Field;

import org.jdom2.*;
import fantasy.*;

import fantasy.io.Path;
import fantasy.servicemodel.ServiceContainer;
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
    		if(_appSettingsLocation == null)
    		{
    			_appSettingsLocation = JavaLibraryUtils.getEntryLibrary().getAbsolutePath() + ".xsettings";
    		}
    		
    		File f = new File(_appSettingsLocation);
    		
    		if(f.exists())
    		{
    		    _appSettingsRoot = JDomUtils.loadElement(_appSettingsLocation);
    		}
    		else
    		{
    			_appSettingsRoot = new Element("settingsFile", _namespace);
    		}
    		
    		
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
    
    
    private static class UserScopeFilter  implements IFieldFilter
    {

    	public static final UserScopeFilter Instance = new UserScopeFilter();
    	
		@Override
		public boolean filter(Field field) {
			
			Scope scope = field.getAnnotation(Scope.class);
			return scope == null || scope.value() == SettingScope.Users; 
			
		}
    	
    }

	public static SettingsBase load(SettingsBase data) throws Exception
	{
		loadAppSettingsRoot();
		loadUserSettingsRoot();
			
		XSerializable anno = data.getClass().getAnnotation(XSerializable.class);
		
	    Element appSettings = _appSettingsRoot.getChild(anno.name(), Namespace.getNamespace(anno.namespaceUri()));
		
		
		XSerializer ser = new XSerializer(data.getClass());
		if(appSettings != null)
		{
			ser.deserialize(appSettings, data);
		}
			
		Element userSettings =  _userSettingsRoot.getChild(anno.name(), Namespace.getNamespace(anno.namespaceUri()));
		
		if(userSettings != null)
		{
			
			ServiceContainer context = new ServiceContainer();
			context.initializeServices(new Object[]{UserScopeFilter.Instance});
			ser.setContext(context);
			ser.deserialize(userSettings, data);
		}
			
			
		return data;
	}

	public static void save(SettingsBase data) throws Exception
	{
		
		
		XSerializable anno = data.getClass().getAnnotation(XSerializable.class);
		
		
		
		
		Element newSettings = new Element(anno.name(), Namespace.getNamespace(anno.namespaceUri()));
		
		
		XSerializer ser = new XSerializer(data.getClass());
		
		ServiceContainer context = new ServiceContainer();
		context.initializeServices(new Object[]{UserScopeFilter.Instance});
		ser.setContext(context);
		
		ser.serialize(newSettings, data);
		
		Element oldSettings = _userSettingsRoot.getChild(anno.name(), Namespace.getNamespace(anno.namespaceUri()));
		

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