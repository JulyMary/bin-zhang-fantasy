package fantasy.properties;

import org.jdom2.Element;
import org.jdom2.Namespace;

import fantasy.Consts;
import fantasy.configuration.*;

public class Settings extends SettingsBase {
	
	

	
	private static Settings _default;
    public static  Settings getDefault() throws Exception
    {
    	if(_default == null)
    	{
    		_default = (Settings)SettingStorage.load(new Settings());
    	}
    	
    	return _default;
    }
    
    
    
    private Element _addIn = new Element("addIn", Namespace.getNamespace(Consts.SettingsNameSpace));
   
    public Element getAddIn()
    {
    	return _addIn;
    }
   
}
