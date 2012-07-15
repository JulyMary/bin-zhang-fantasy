package Fantasy.ServiceModel;

import Fantasy.Configuration.*;

public class ChannelSettings extends SettingsBase
{
	private static ChannelSettings _default = (ChannelSettings)Fantasy.Configuration.SettingStorage.Load(new ChannelSettings());

	public static ChannelSettings getDefault()
	{
		return _default;
	}


	public final java.util.ArrayList<AddressSetting> getAddresses()
	{
		return (((java.util.ArrayList<AddressSetting>)super.GetValue("Addresses")) != null) ? (java.util.ArrayList<AddressSetting>)super.GetValue("Addresses") : new java.util.ArrayList<AddressSetting>();
	}
}