package fantasy.servicemodel;

import java.net.URL;
import java.rmi.Naming;
import fantasy.rmi.RmiSettings;


@SuppressWarnings("unchecked")
public final class ClientFactory
{
	public static <T> T Create(Class<T> type) throws Exception
	{
		URL uri = RmiSettings.getDefault().getClient().GetUrl(type);
		T rs = (T)Naming.lookup(uri.toString());
		return rs;
	}
	
	public static <T> T Create(Class<T> type, String url) throws Exception
	{
		T rs = (T)Naming.lookup(url);	
		return rs;
	}

}