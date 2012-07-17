package fantasy.properties;

public class Settings {
	
	
	private static Settings _default = new Settings();
    public static  Settings getDefault()
    {
    	return _default;
    }
    
    
    private  String _userSettingsFile;
    public String getUserSettingsFile()
    {
    	return _userSettingsFile;
    }
    
    public void setUserSettingsFile(String value)
    {
    	_userSettingsFile = value;
    	
    	
    }
}
