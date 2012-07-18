package fantasy.servicemodel;

import fantasy.configuration.*;

public class ChannelSettings extends SettingsBase
{
	private static ChannelSettings _default = null;

	public static ChannelSettings getDefault() throws Exception
	{
		if(_default == null)
		{
			 _default = (ChannelSettings)fantasy.configuration.SettingStorage.load(new ChannelSettings());
		}
		
		return _default;
	}


	private java.util.ArrayList<AddressSetting> _addresses = new java.util.ArrayList<AddressSetting>();
	
	public final java.util.ArrayList<AddressSetting> getAddresses()
	{
		return _addresses;
	}
}