package Fantasy.Jobs.Management;

import Fantasy.ServiceModel.*;

public interface IJobManagerSettingsReader
{
	Object GetSetting(String name);
	<T> T GetSetting(String name);
}